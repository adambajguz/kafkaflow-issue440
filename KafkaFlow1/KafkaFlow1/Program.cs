namespace KafkaFlow1
{
    using KafkaFlow;
    using KafkaFlow.Consumers.DistributionStrategies;
    using KafkaFlow.Serializer;
    using KafkaFlow.TypedHandler;
    using KafkaFlow1.Messages.Producers;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            ConfigureServices(builder.Services);

            WebApplication app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INotificationMessageProducer, NotificationMessageProducer>();

            services.AddHostedService<PopulateTopicBeforeStarting>();

            services.AddKafkaFlowHostedService(kafka =>
            {
                kafka.UseMicrosoftLog();

                kafka.AddCluster(cluster =>
                {
                    cluster.WithBrokers(new[] { "localhost:9092" });

                    cluster.WithName("Example Bus Cluster");

                    cluster
                        .CreateTopicIfNotExists("notification-events", 10, 1)
                        .AddProducer<NotificationMessageProducer>(producer =>
                        {
                            producer
                                .DefaultTopic("notification-events")
                                .WithAcks(Acks.Leader)
                                .WithCompression(Confluent.Kafka.CompressionType.Lz4)
                                .AddMiddlewares(middlewares =>
                                {
                                    middlewares
                                        .AddSerializer<NewtonsoftJsonSerializer>();
                                });
                        })
                        .AddConsumer(costumer =>
                        {
                            costumer
                                .Topic("notification-events")
                                .WithGroupId("notification-events")
                                .WithBufferSize(100)
                                .WithWorkersCount(10)
                                .WithWorkDistributionStrategy<BytesSumDistributionStrategy>()
                                .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                                .AddMiddlewares(middlewares =>
                                {
                                    middlewares
                                        .AddSerializer<NewtonsoftJsonSerializer>()
                                        .AddTypedHandlers(handlers =>
                                        {
                                            handlers.AddHandlersFromAssemblyOf<Program>();
                                        });
                                });
                        });
                });
            });
        }
    }
}