using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Data
{
    public class Contract()
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Key] public string Address { get; private set; }

        public string? Hash { get; private set; }

        public string? CreatorAddress { get; private set; }

        public int TransactionsCount { get; set; }


        public Contract(string address, string creator, string hash) : this()
        {
            Address = address;
            CreatorAddress = creator;
            Hash = hash;
            TransactionsCount = 0;
        }

        public Contract(string address) : this()
        {
            Address = address;
            TransactionsCount = 0;
        }
    }
}
