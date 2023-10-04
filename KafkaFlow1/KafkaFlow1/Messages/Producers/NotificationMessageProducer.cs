namespace KafkaFlow1.Messages.Producers
{
    using KafkaFlow;
    using KafkaFlow1.Messages;

    public class NotificationMessageProducer : INotificationMessageProducer
    {
        private readonly IMessageProducer<NotificationMessageProducer> _producer;

        public NotificationMessageProducer(IMessageProducer<NotificationMessageProducer> producer)
        {
            _producer = producer;
        }

        public async Task ProduceNotificationSentAsync(string text, CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < 50; i++)
            {
                await _producer.ProduceAsync(
                Guid.NewGuid().ToString(),
                new NotificationSentMessage { Text = $"[{i}] {text}" },
                partition: 3);

                Console.WriteLine("Send: {0}", i);
            }
        }
    }
}
