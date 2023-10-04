namespace KafkaFlow1
{
    using System.Threading;
    using System.Threading.Tasks;
    using KafkaFlow1.Messages.Producers;

    internal class PopulateTopicBeforeStarting : IHostedService
    {
        private readonly INotificationMessageProducer _producer;

        public PopulateTopicBeforeStarting(INotificationMessageProducer producer)
        {
            _producer = producer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _producer.ProduceNotificationSentAsync("test");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}