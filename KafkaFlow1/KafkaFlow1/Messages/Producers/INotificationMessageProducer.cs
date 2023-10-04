namespace KafkaFlow1.Messages.Producers
{
    using KafkaFlow;

    public interface INotificationMessageProducer
    {
        Task ProduceNotificationSentAsync(string text, CancellationToken cancellationToken = default);
    }
}