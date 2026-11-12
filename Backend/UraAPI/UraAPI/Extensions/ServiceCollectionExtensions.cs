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

namespace UraAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUraServices(this IServiceCollection services)
        {
            // --- Đăng ký các tầng Data Layer (DL) ---
            services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
            services.AddScoped<IUserDL, UserDL>();
            services.AddScoped<IRoleDL, RoleDL>();
            services.AddScoped<IUserRoleDL, UserRoleDL>();
            services.AddScoped<IUserTokenDL, UserTokenDL>();
            services.AddScoped<IProductDL, ProductDL>();
            services.AddScoped<ICategoryDL, CategoryDL>();
            services.AddScoped<IOrderDL, OrderDL>();
            services.AddScoped<IOrderDetailDL, OrderDetailDL>();
            services.AddScoped<IDebtDL, DebtDL>();
            services.AddScoped<IDebtDetailDL, DebtDetailDL>();

            // --- Đăng ký các tầng Business Logic (BL) ---
            services.AddScoped(typeof(IBaseBL<>), typeof(BaseBl<>));
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<IRoleBL, RoleBL>();
            services.AddScoped<IUserRoleBL, UserRoleBL>();
            services.AddScoped<IUserTokenBL, UserTokenBL>();
            services.AddScoped<IProductBL, ProductBL>();
            services.AddScoped<ICategoryBL, CategoryBL>();
            services.AddScoped<IOrderBL, OrderBL>();
            services.AddScoped<IOrderDetailBL, OrderDetailBL>();
            services.AddScoped<IDebtBL, DebtBL>();
            services.AddScoped<IDebtDetailBL, DebtDetailBL>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            return services;
        }
    }
}
