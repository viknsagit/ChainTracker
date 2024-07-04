using System.ComponentModel.DataAnnotations;

namespace Addresses.Models
{
    public class Address
    {
        public int Id { get; private set; }
        [Key] public string Hash { get; private set; }
        public int TransactionsCount { get; private set; }
        public decimal GasUsed { get; private set; }
        public int LastBalanceUpdate { get; private set; }

        public void UpdateTransactionsCount(int countToAdd)
        {
            TransactionsCount += countToAdd;
        }

        public void UpdateUsedGas(decimal gasToAdd)
        {
            GasUsed += gasToAdd;
        }

        public void UpdateLastBlock(int block)
        {
            LastBalanceUpdate = block;
        }
    }
}
