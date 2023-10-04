namespace KafkaFlow1.Messages.Customers
{
    using KafkaFlow.TypedHandler;
    using KafkaFlow;

    public class NotificationSentMessageCustomer : IMessageHandler<NotificationSentMessage>
    {
        private readonly ILogger _logger;

        public NotificationSentMessageCustomer(ILogger<NotificationSentMessageCustomer> logger)
        {
            _logger = logger;
        }

        public Task Handle(IMessageContext context, NotificationSentMessage message)
        {
            _logger.LogInformation(
                "Partition: {Partition} | Offset: {Offset} | Worker ID: {WorkerId} | Group ID: {GroupId} | Message: {Message}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                context.ConsumerContext.WorkerId,
                context.ConsumerContext.GroupId,
                message.Text);

            return Task.CompletedTask;
        }
    }
}
