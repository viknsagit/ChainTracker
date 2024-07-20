using Confluent.Kafka;

namespace Transactions.Kafka
{
    public class TransactionsConsumerService(IConsumer<string, string> consumer,ILogger<TransactionsConsumerService> logger,TransactionsIndexer indexer) : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            consumer.Subscribe("transactions");
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    if (consumeResult is null)
                        return;
                    await indexer.ProccessTransaction(consumeResult.Message.Value);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
