using MassTransit;
using Microsoft.AspNetCore.Mvc;
using order_ms.Models;
using rabbitmq;
using rabbitmq_message;

namespace order_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public OrderController(
          ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        [Route("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderModel)
        {
            try
            {
                var endPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:" + BusConstants.OrderQueue));
                await endPoint.Send<IOrderMessage>(new
                {
                    OrderId = orderModel.OrderId,
                    ProductName = orderModel.ProductName,
                    PaymentCardNumber = orderModel.CardNumber
                });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            return Ok("Success");
        }
    }

}
