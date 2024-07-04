using Blocks.Repository;
using Confluent.Kafka;

using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.Web3;

namespace Blocks
{
    public class BlocksIndexer : IHostedService
    {
        private readonly ILogger<BlocksIndexer> _logger;
        private readonly IConfiguration _configuration;
        private readonly Web3 _web3;
        private readonly IProducer<string, string> _producer;
        private readonly CancellationTokenRegistration _tokenRegistration = new();
        private readonly BlocksRepositoryFactory _blocksRepositoryFactory;

        public delegate Task NewBlock(Nethereum.RPC.Eth.DTOs.Block block);
        public static event NewBlock? OnNewBlockCreated;
        public BlocksIndexer(ILogger<BlocksIndexer> logger, IConfiguration configuration,BlocksRepositoryFactory repoFactory, IProducer<string, string> producer)
        {
            _logger = logger;
            _configuration = configuration;
            _web3 = new Web3(configuration["rpc"]);
            _producer = producer;
            _blocksRepositoryFactory = repoFactory;
            OnNewBlockCreated += IndexNewBlock;
        }

        private async Task IndexNewBlock(Nethereum.RPC.Eth.DTOs.Block block)
        {
            var blockWithTransactions =
                await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new BlockParameter(block.Number));

            _logger.LogInformation($"New block, hash: {block.BlockHash}");

            await using var repo = _blocksRepositoryFactory.Create();
            await repo.CreateAsync(blockWithTransactions);
            if (blockWithTransactions.TransactionCount() > 0)
            {
                foreach (var tx in blockWithTransactions.Transactions)
                {
                    var msg = new Message<string,string>
                    {
                        Value = tx.TransactionHash
                    };
                    await _producer.ProduceAsync("transactions", msg,cancellationToken:_tokenRegistration.Token);
                }
                _logger.LogInformation($"Transactions added: {blockWithTransactions.TransactionCount()}");
            }
        }

        private async Task ListeningNewBlocks()
        {
            using var client = new StreamingWebSocketClient(_configuration["ws"]);
            // create the subscription
            // (it won't start receiving data until Subscribe is called)
            var subscription = new EthNewBlockHeadersObservableSubscription(client);

            // attach a handler for when the subscription is first created (optional)
            // this will occur once after Subscribe has been called
            subscription.GetSubscribeResponseAsObservable().Subscribe(subscriptionId =>
                _logger.LogInformation("Block Header subscription Id: " + subscriptionId));

            // attach a handler for each block
            // put your logic here
            subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(block =>
            {
                _logger.LogInformation("New Block: " + block.Number.Value.ToString());
                OnNewBlockCreated?.Invoke(block);
            });

            var subscribed = true;

            // handle unsubscription
            // optional - but may be important depending on your use case
            subscription.GetUnsubscribeResponseAsObservable().Subscribe(response =>
            {
                subscribed = false;
                _logger.LogInformation($"Block Header unsubscribe result: {response}");
            });

            // open the websocket connection
            await client.StartAsync();

            // start the subscription
            // this will only block long enough to register the subscription with the client
            // once running - it won't block whilst waiting for blocks
            // blocks will be delivered to our handler on another thread
            await subscription.SubscribeAsync();

            // run for a minute before unsubscribing
            //await Task.Delay(TimeSpan.FromMinutes(1));

            // unsubscribe
            //await subscription.UnsubscribeAsync();

            //allow time to unsubscribe
            while (subscribed) await Task.Delay(TimeSpan.FromSeconds(1));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ListeningNewBlocks();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
