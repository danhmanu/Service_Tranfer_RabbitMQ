using MassTransit;
using rabbitmq_message;

namespace card_ms
{
    public class OrderCardNumberValidateConsumer :
         IConsumer<IOrderMessage>
    {
        public async Task Consume(ConsumeContext<IOrderMessage> context)
        {
            var data = context.Message;
            if (data.PaymentCardNumber != "test")
            {
                // invalid
            }
            throw new Exception("Lỗi");
        }
    }
}
