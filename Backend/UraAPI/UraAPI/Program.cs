using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using UraAPI.Authentication;
using UraAPI.Extensions;
using UraBusinessLogic.BusinessLogics;
using UraBusinessLogic.BusinessLogics.Cloudinary;
using UraBusinessLogic.BusinessLogics.Debts;
using UraBusinessLogic.BusinessLogics.Products;
using UraBusinessLogic.BusinessLogics.Sales;
using UraBusinessLogic.BusinessLogics.Users;
using UraBusinessLogic.Interfaces;
using UraBusinessLogic.Interfaces.Cloudinary;
using UraBusinessLogic.Interfaces.IDebts;
using UraBusinessLogic.Interfaces.IProducts;
using UraBusinessLogic.Interfaces.ISales;
using UraBusinessLogic.Interfaces.IUsers;
using UraBusinessLogic.Interfaces.Users;
using UraDataLayer;
using UraDataLayer.Database;
using UraDataLayer.DataLayer.Users;
using UraDataLayer.DataLayers;
using UraDataLayer.DataLayers.Debts;
using UraDataLayer.DataLayers.Products;
using UraDataLayer.DataLayers.Sales;
using UraDataLayer.DataLayers.Users;
using UraDataLayer.Interfaces;
using UraDataLayer.Interfaces.IDebts;
using UraDataLayer.Interfaces.IProducts;
using UraDataLayer.Interfaces.IUsers;
using UraDataLayer.Interfaces.Sales;


namespace UraAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // ánh xạ db tra ra USER_ID → UserId
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            // connect
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddScoped<MySqlContext>(provider => new MySqlContext(connectionString));

            // --- Đăng ký Cloudinary SDK ---
            var cloudinaryAccount = new CloudinaryDotNet.Account(
                builder.Configuration["CloudinarySettings:CloudName"],
                builder.Configuration["CloudinarySettings:ApiKey"],
                builder.Configuration["CloudinarySettings:ApiSecret"]
            );
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(cloudinaryAccount);
            builder.Services.AddSingleton<CloudinaryDotNet.ICloudinary>(cloudinary);

            // collection
            builder.Services.AddUraServices();


            // Đăng ký xác thực
            builder.Services.AddAuthentication("Token")
                .AddScheme<AuthenticationSchemeOptions, TokenAuthenticationHandler>("Token", null);



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Swagger có nút Authorize để gửi token
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    Description = "Dán token nhận được khi đăng nhập"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

           app.UseHttpsRedirection();

           app.UseAuthentication(); // Xác định người dùng
          
           app.UseAuthorization();  // Kiểm tra quyền truy cập

            app.MapControllers();

            app.Run();
        }
    }
}
