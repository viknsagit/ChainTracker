using Nethereum.Web3;
using Transactions.Data;
using Transactions.Repository;

namespace Transactions
{
    public class TransactionsIndexer(IConfiguration config, TransactionsRepositoryFactory repoFactory)
    {
        private readonly Web3 _web3 = new(config["rpc"]);

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
        }


        /// <summary>
        /// Создает новый контракт
        /// </summary>
        /// <param name="hash"></param>
        private void ProccesingContractFromTransaction(string hash)
        {

        }

        /// <summary>
        /// Обновляет данные адресов учавствующих в транзакции
        /// </summary>
        /// <param name="hash"></param>
        private void ProcessAddressesFromTransaction(string hash)
        {

        }
    }
}
 