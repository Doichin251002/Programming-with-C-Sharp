using BookStore.Models.Serialization;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.DL.Kafka.KafkaCache
{
    public class KafkaCache<TKey, TValue> : BackgroundService//, IKafkaCache<TKey, TValue> where TKey : notnull where TValue : class
    {
        private readonly ConsumerConfig _config;

        public KafkaCache()
        {
            _config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "default-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => ConsumeMessages(stoppingToken), stoppingToken);

            return Task.CompletedTask;
        }

        private void ConsumeMessages(CancellationToken stoppingToken)
        {
            using (var consumer = new ConsumerBuilder<TKey, TValue>(_config)
              .SetValueDeserializer(new MessagePackDeserializer<TValue>())
              .Build())
            {
                consumer.Subscribe("cache-events");

                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume();

                    if (consumeResult.IsPartitionEOF)
                    {
                        continue;
                    }

                    if (consumeResult != null)
                    {
                        Console.WriteLine($"Error: {consumeResult.Message.Key}");
                    }
                }
            }
        }

    }
}