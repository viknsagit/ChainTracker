using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Nethereum.ABI;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Transactions.Data
{
    [Index("Block", "Number", "Timestamp")]
    public class Transaction()
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; }
        [Key] public string Hash { get; private set; }
        public string Input { get; private set; }
        public string FromAddress { get; private set; }
        public string? ToAddress { get; private set; }
        public int Block { get; private set; }
        public long Timestamp { get; private set; }
        public decimal Value { get; private set; }
        public decimal Gas { get; private set; }
        public decimal GasPrice { get; private set; }
        public bool IsReverted { get; private set; }
        public bool IsContractDeployment { get; private set; }
        public bool IsContractInteraction { get; private set; }

        public Transaction(Nethereum.RPC.Eth.DTOs.Transaction tx, bool reverted = false, bool isContractDeployment = false,
            bool isContractInteraction = false) : this()
        {
            Hash = tx.TransactionHash;
            Block = int.Parse(tx.BlockNumber.ToString());
            Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            FromAddress = tx.From;
            ToAddress = tx.To;
            Input = tx.Input;
            Gas = Web3.Convert.FromWei(tx.Gas);
            GasPrice = Web3.Convert.FromWei(tx.GasPrice);
            IsReverted = reverted;
            IsContractDeployment = isContractDeployment;
            IsContractInteraction = isContractInteraction;
            Value = Web3.Convert.FromWei(tx.Value);
        }

        public static async Task<bool> IsContract(string address, Web3 web3)
        {
            var result = await web3.Eth.GetCode.SendRequestAsync(address);
            return result.Length > 2;
        }
    }
}
