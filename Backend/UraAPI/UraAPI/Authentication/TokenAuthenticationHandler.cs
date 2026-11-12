using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using UraBusinessLogic.Interfaces.IUsers;
using UraDataModel.Models.Users;

namespace UraAPI.Authentication
{
    public class TokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserTokenBL _userTokenBL;

        public TokenAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IUserTokenBL userTokenBL) : base(options, logger, encoder)
        {
            _userTokenBL = userTokenBL;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorization = Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authorization) ||
                !authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var token = authorization["Bearer ".Length..].Trim();

            var userToken = await _userTokenBL.GetByToken(token);
            if (userToken == null)
                return AuthenticateResult.Fail("Token không hợp lệ hoặc đã hết hạn.");
            //
            var identity = new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.NameIdentifier, userToken.UserId) },
                Scheme.Name);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
