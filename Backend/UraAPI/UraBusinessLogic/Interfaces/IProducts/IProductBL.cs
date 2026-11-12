using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraDataModel.Models.Products;
using UraDataModel.Response;
using UraDataModel.Response.Products;

namespace UraBusinessLogic.Interfaces.IProducts
{
    public interface IProductBL : IBaseBL<Product>
    {
        /// <summary>
        /// thêm mới sản phẩm
        /// </summary>
        /// <param name="product"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<ResultModel<ProductResponseDto>> InsertProductAsync(ProductCreateDto product,string userId);
        /// <summary>
        /// cập nhật sản phẩm
        /// </summary>
        /// <param name="product"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<ResultModel<ProductResponseDto>> UpdateProductAsync(ProductUpdateDto product, string userId);
    }
}
