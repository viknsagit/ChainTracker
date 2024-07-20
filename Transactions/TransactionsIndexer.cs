using Confluent.Kafka;

using Nethereum.Web3;
using Transactions.Data;
using Transactions.Repository;

namespace Transactions
{
    public class TransactionsIndexer(IConfiguration config, TransactionsRepositoryFactory repoFactory, IProducer<string, string> producer)
    {
        private readonly Web3 _web3 = new(config["rpc"]);
        private readonly CancellationTokenRegistration _tokenRegistration = new();

        public async Task ProccessTransaction(string hash)
        {
            var receipt = 
                await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(hash);
            var tx = 
                new Transaction(await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(hash),
                    receipt.HasErrors()!.Value,
                    string.IsNullOrEmpty(receipt.To));
            await using var repo = repoFactory.Create();
            await repo.CreateAsync(tx);
            await ProcessAddressesFromTransaction(tx.Hash);

            if (string.IsNullOrEmpty(tx.ToAddress))
            {
               await ProcessContractFromTransaction(tx.Hash);
            }
        }

        /// <summary>
        /// Создает новый контракт
        /// </summary>
        /// <param name="hash">Хеш транзакции</param>
        private async Task ProcessContractFromTransaction(string hash)
        {
            var msg = new Message<string, string>
            {
                Value = hash
            };
            await producer.ProduceAsync("contracts", msg, _tokenRegistration.Token);
        }

        /// <summary>
        /// Обновляет данные адресов учавствующих в транзакции
        /// </summary>
        /// <param name="hash">Хеш транзакции</param>
        private async Task ProcessAddressesFromTransaction(string hash)
        {
            var msg = new Message<string, string>
            {
                Value = hash
            };
            await producer.ProduceAsync("addresses", msg, _tokenRegistration.Token);
        }
    }
}
 