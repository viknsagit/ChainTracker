using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System.ComponentModel.DataAnnotations;

namespace Blocks.Data
{
    public class Block
    {
        [Key]
        public long BlockNumber { get; private set; }
        public long TransactionsNumber { get; private set; }
        public decimal GasLimit { get; private set; }
        public decimal GasUsed { get; private set; }
        public long Timestamp { get; private set; }

        public Block()
        {
            
        }

        public Block(BlockWithTransactions block)
        {
            if (!int.TryParse(block.Number.ToString(), out var blockNumber))
            {
                throw new ArgumentException("Invalid block number format.");
            }
            BlockNumber = blockNumber;

            TransactionsNumber = block.TransactionCount();
            GasLimit = Web3.Convert.FromWei(block.GasLimit);
            GasUsed = Web3.Convert.FromWei(block.GasUsed);

            if (!long.TryParse(block.Timestamp.ToString(), out var timestamp))
            {
                throw new ArgumentException("Invalid timestamp format.");
            }
            Timestamp = timestamp;
        }
    }
}
