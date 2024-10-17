// File : CategoryController.cs
// Date : 08/10/2024
// This controller handles HTTP requests related to categories in the WebApi application. It includes actions for retrieving,        creating, updating, and deleting categories, with appropriate status codes and response messages.

using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Dto;
using WebApi.Constant;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();

                if (categories != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgCategoriesRetrievalSuccess,
                        Data = categories
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCategoryDoesNotExist
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
        [Route("active-categories")]
        public async Task<IActionResult> GetActiveCategories()
        {
            try
            {
                var categories = await _categoryService.GetActiveCategories();

                if (categories != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgCategoriesRetrievalSuccess,
                        Data = categories
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCategoryDoesNotExist
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(string id)
        {
            try
            {
                var categoryDetails = await _categoryService.GetCategoryById(id);

                if (categoryDetails != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgCategoryRetrievalSuccess,
                        Data = categoryDetails
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCategoryDoesNotExist
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
        public async Task<IActionResult> AddCategory(Category product)
        {
            try
            {
                await _categoryService.CreateCategory(product);

                var allCategories = await _categoryService.GetActiveCategories();

                return Ok(new ResponseDto
                {
                    Code = StatusCodes.Status200OK,
                    Status = ApiConstant.Success,
                    Message = ApiConstant.MsgCategoryCreationSuccess,
                    Data = allCategories
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
        [Route("update-category")]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            try
            {
                var result = await _categoryService.UpdateCategory(category);

                if (result == "success")
                {
                    var allCategories = await _categoryService.GetActiveCategories();

                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgCategoryUpdateSuccess,
                        Data = allCategories
                    });
                }
                else if (result == "not-found")
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCategoryDoesNotExist
                    });
                }
                else
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCategoryUpdateFailed
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

        [HttpPut]
        [Route("update-category-status")]
        public async Task<IActionResult> UpdateCategoryStatus(Category category)
        {
            try
            {
                var result = await _categoryService.UpdateCategoryStatus(category);

                if (result == "success")
                {
                    var allCategories = await _categoryService.GetActiveCategories();

                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgCategoryStatusUpdateSuccess,
                        Data = allCategories
                    });
                }
                else if (result == "not-found")
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCategoryDoesNotExist
                    });
                }
                else
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCategoryStatusUpdateFailed
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
