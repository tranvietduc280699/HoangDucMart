using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UraBusinessLogic.Interfaces.Cloudinary;
using UraBusinessLogic.Interfaces.IProducts;
using UraDataLayer.Interfaces;
using UraDataLayer.Interfaces.IProducts;
using UraDataModel.Model.Users;
using UraDataModel.Models;
using UraDataModel.Models.Products;
using UraDataModel.Response;
using UraDataModel.Response.Products;
using UraDataModel.Response.Users;

namespace UraBusinessLogic.BusinessLogics.Products
{
    public class ProductBL : BaseBl<Product>, IProductBL
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IProductDL _productDL;
        private readonly ICategoryDL _categoryDL;
        public ProductBL(IProductDL productDL, ICloudinaryService cloudinaryService, ICategoryDL categoryDL) : base(productDL, "ura_products")
        {
            _cloudinaryService = cloudinaryService;
            _productDL = productDL;
            _categoryDL = categoryDL;
        }
        /// <summary>
        /// thêm mới sản phẩm
        /// </summary>
        /// <param name="product"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResultModel<ProductResponseDto>> InsertProductAsync(ProductCreateDto product, string userId)
        {
            // kiểm tra tồn tại chưa
            var filter = new BaseFilter
            {
                Conditions = new Dictionary<string, string?> { { "Sku", product.Sku } },
                LogicalOperator = ""
            };
            var found = await _productDL.GetByConditionAsync(_tableName, filter);
            var existing = found?.FirstOrDefault();
            if (existing == null)
                return ResultHelper.Fail<ProductResponseDto>(ResponseCode.AlreadyExists);
            // đối tượng
            Product prd = new Product
            {
                ProductId = Guid.NewGuid().ToString(),
                CategoryId = product.CategoryId,
                ProductName = product.ProductName,
                Sku = product.Sku,
                QrCode = product.QrCode,
                Description = product.Description,
                Unit = product.Unit,
                SellPrice = product.SellPrice,
                CostPrice = product.CostPrice,
                WeightGrams = product.WeightGrams,
                IsActive = true,     
                IsFeatured = false, 
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            // thêm mới ảnh nếu có
            if (product.Image != null && product.Image.Length > 0)
            {
                var avata = await _cloudinaryService.UploadImageAsync(product.Image, prd.ProductId);
                prd.ImageUrl = avata;
            }
            product.Image = null;

            var result = await base.Insert(prd);

            ProductResponseDto prdReponse = new ProductResponseDto
            {
                ProductId = prd.ProductId!,
                CategoryId = prd.CategoryId,
                CategoryName = null,   
                ProductName = prd.ProductName!,
                Sku = prd.Sku,
                QrCode = prd.QrCode,
                ImageUrl = prd.ImageUrl,
                Description = prd.Description,
                Unit = prd.Unit,
                UnitName = product.Unit.ToString(),
                SellPrice = prd.SellPrice,
                CostPrice = prd.CostPrice,
                WeightGrams = prd.WeightGrams,
                IsActive = prd.IsActive,
                IsFeatured = prd.IsFeatured,
                CreatedAt = prd.CreatedAt,
                CreatedBy = prd.CreatedBy
            };
            if (result == null)
            {
                return ResultHelper.Fail<ProductResponseDto>(ResponseCode.InternalServerError);

            }
            return ResultHelper.Success<ProductResponseDto>(prdReponse);

        }
        /// <summary>
        /// cập nhật sản phẩm
        /// </summary>
        /// <param name="product"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResultModel<ProductResponseDto>> UpdateProductAsync(ProductUpdateDto product, string userId)
        {
            // lấy sản phẩm hiện tại
            var filter = new BaseFilter
            {
                Conditions = new Dictionary<string, string?> { { "ProductId", product.ProductId } },
                LogicalOperator = ""
            };
            var found = await _productDL.GetByConditionAsync(_tableName, filter);
            var existing = found?.FirstOrDefault();
            if (existing == null)
                return ResultHelper.Fail<ProductResponseDto>(ResponseCode.NotFound);

            // cập nhật ảnh nếu có
            if (product.Image != null && product.Image.Length > 0)
            {
                var avata = await _cloudinaryService.UploadImageAsync(product.Image, existing.ProductId);
                existing.ImageUrl = avata;
            }
            product.Image = null;

            // máp
            existing.CategoryId = product.CategoryId;
            existing.ProductName = product.ProductName;
            existing.Sku = product.Sku;
            existing.QrCode = product.QrCode;
            existing.Description = product.Description;
            existing.Unit = product.Unit;
            existing.SellPrice = product.SellPrice;
            existing.CostPrice = product.CostPrice;
            existing.WeightGrams = product.WeightGrams;
            existing.IsActive = product.IsActive;
            existing.IsFeatured = product.IsFeatured;
            existing.UpdatedBy = userId;
            existing.UpdatedAt = DateTime.UtcNow;

            var result = await base.Update(existing);
            if (result == null)
            {
                return ResultHelper.Fail<ProductResponseDto>(ResponseCode.InternalServerError);

            }
            // dữ liệu trả về
            ProductResponseDto prdReponse = new ProductResponseDto
            {
                ProductId = existing.ProductId!,
                CategoryId = existing.CategoryId,
                CategoryName = null,
                ProductName = existing.ProductName!,
                Sku = existing.Sku,
                QrCode = existing.QrCode,
                ImageUrl = existing.ImageUrl,
                Description = existing.Description,
                Unit = existing.Unit,
                UnitName = product.Unit.ToString(),
                SellPrice = existing.SellPrice,
                CostPrice = existing.CostPrice,
                WeightGrams = existing.WeightGrams,
                IsActive = existing.IsActive,
                IsFeatured = existing.IsFeatured,
                CreatedAt = existing.CreatedAt,
                CreatedBy = existing.CreatedBy
            };
            return ResultHelper.Success<ProductResponseDto>(prdReponse);
        }
    }
}
