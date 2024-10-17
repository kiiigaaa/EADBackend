// File : OrderController.cs
// Date : 08/10/2024
// This controller handles all HTTP requests related to product management in the WebApi application. It provides endpoints for retrieving, adding, updating, and deleting products, as well as handling responses in a consistent format using ResponseDto.

using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Dto;
using WebApi.Constant;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();

                if (products != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgProductsRetrievalSuccess,
                        Data = products
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgProductDoesNotExist
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpGet]
        [Route("get-vendor-products")]
        public async Task<IActionResult> GetProductsByVendorID(Guid id)
        {
            try
            {
                var products = await _productService.GetProductsByVendorID(id);

                if (products != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgProductsRetrievalSuccess,
                        Data = products
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgProductDoesNotExist
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpGet]
        [Route("get-product")]
        public async Task<IActionResult> GetProduct(string id)
        {
            try
            {
                var productDetails = await _productService.GetProductById(id);

                if (productDetails != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgProductRetrievalSuccess,
                        Data = productDetails
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgProductDoesNotExist
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            try
            {
                await _productService.CreateProduct(product);

                var allProducts = await _productService.GetAllProducts();

                return Ok(new ResponseDto
                {
                    Code = StatusCodes.Status200OK,
                    Status = ApiConstant.Success,
                    Message = ApiConstant.MsgProductCreationSuccess,
                    Data = allProducts
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpPut]
        [Route("update-product")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            try
            {
                var result = await _productService.UpdateProduct(product);

                if (result == "success")
                {
                    var allProducts = await _productService.GetAllProducts();

                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgProductUpdateSuccess,
                        Data = allProducts
                    });
                }
                else if (result == "not-found")
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgProductDoesNotExist
                    });
                }
                else
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgProductUpdateFailed
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id, Guid userId)
        {
            try
            {
                var result = await _productService.DeleteProduct(id, userId);

                if (result == "success")
                {
                    var allProducts = await _productService.GetAllProducts();

                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgProductRemoveSuccess,
                        Data = allProducts
                    });
                }
                else if (result == "not-found")
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgProductDoesNotExist
                    });
                }
                else
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgProductRemoveFailed
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }
    }
}