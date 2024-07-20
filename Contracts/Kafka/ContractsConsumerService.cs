using Confluent.Kafka;

using static Confluent.Kafka.ConfigPropertyNames;

namespace Contracts.Kafka
{
    public class ContractsConsumerService(IConsumer<string,string> contractsConsumer,ContractsIndexer indexer) : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            contractsConsumer.Subscribe("transactions");
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = contractsConsumer.Consume(cancellationToken);
                    if (consumeResult is null)
                        return;
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
