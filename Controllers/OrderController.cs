// File : OrderController.cs
// Date : 08/10/2024
// The OrderController class is responsible for handling HTTP requests related to order management in the WebApi application. This   includes operations for cart items, order statuses, and customer orders. The controller utilizes the IOrderService interface to   perform the required business logic.

using Microsoft.AspNetCore.Mvc;
using WebApi.Constant;
using WebApi.Dto;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpGet]
        [Route("get-cart-items")]
        public async Task<IActionResult> GetCartItemsByCustomerId(Guid id)
        {
            try
            {
                var cartItemList = await _orderService.GetCartItemsByCustomerId(id);

                if (cartItemList != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgCartItemsRetrievalSuccess,
                        Data = cartItemList
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCartItemsNotFound
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
        [Route("add-item-to-cart")]
        public async Task<IActionResult> CreateCartItem(CartItem cartItem)
        {
            try
            {
                await _orderService.CreateCartItem(cartItem);

                var allCartItems = await _orderService.GetCartItemsByCustomerId(cartItem.CustomerId);

                return Ok(new ResponseDto
                {
                    Code = StatusCodes.Status200OK,
                    Status = ApiConstant.Success,
                    Message = ApiConstant.MsgAddItemToCartSuccess,
                    Data = allCartItems
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

        [HttpPost]
        [Route("remove-item-from-cart")]
        public async Task<IActionResult> RemoveCartItem(Guid customerId, string cartItemId)
        {
            try
            {
                await _orderService.RemoveCartItem(customerId, cartItemId);

                var allCartItems = await _orderService.GetCartItemsByCustomerId(customerId);

                return Ok(new ResponseDto
                {
                    Code = StatusCodes.Status200OK,
                    Status = ApiConstant.Success,
                    Message = ApiConstant.MsgCartItemRemovalSuccess,
                    Data = allCartItems
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

        [HttpPost]
        [Route("create-status")]
        public async Task<IActionResult> CreateStatus(Status status)
        {
            try
            {
                await _orderService.CreateOrderStatus(status);

                return Ok(new ResponseDto
                {
                    Code = StatusCodes.Status200OK,
                    Status = ApiConstant.Success,
                    Message = ApiConstant.MsgStatusCreationSuccess,
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

        [HttpGet]
        [Route("active-statuses")]
        public async Task<IActionResult> GetActiveStatuses()
        {
            try
            {
                var statuses = await _orderService.GetActiveStatuses();

                if (statuses != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgStatusRetrievalSuccess,
                        Data = statuses
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgStatusDoesNotExist
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
        [Route("create-order")]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            try
            {
                var result = await _orderService.CreateOrder(order);


                if (result == "order-created-successfully")
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgOrderCreationSuccess,
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgOrderCreationFailed
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
        [Route("get-customer-orders")]
        public async Task<IActionResult> GetCustomerOrdersById(Guid id)
        {
            try
            {
                var cartItemList = await _orderService.GetCustomerOrdersById(id);

                if (cartItemList != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgCartItemsRetrievalSuccess,
                        Data = cartItemList
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCartItemsNotFound
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
        [Route("cancel-order")]
        public async Task<IActionResult> DeleteOrder(Guid customerId, string orderId)
        {
            try
            {
                await _orderService.DeleteOrder(customerId, orderId);

                var allCartItems = await _orderService.GetCustomerOrdersById(customerId);

                return Ok(new ResponseDto
                {
                    Code = StatusCodes.Status200OK,
                    Status = ApiConstant.Success,
                    Message = ApiConstant.MsgCartItemRemovalSuccess,
                    Data = allCartItems
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
    }
}
