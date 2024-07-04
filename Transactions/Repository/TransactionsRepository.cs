using Microsoft.EntityFrameworkCore;
using Transactions.Data;

namespace Transactions.Repository
{
    public class TransactionsRepository : DbContext
    {
        private DbSet<Transaction> Transactions => Set<Transaction>();

        public TransactionsRepository(DbContextOptions<TransactionsRepository> options) 
            : base(options)
        { }

        public async Task<Transaction?> FindAsync(string hash)
        {
            return await Transactions.FindAsync(hash);
        }

        public async Task CreateAsync(Transaction tx)
        {
            var finded = await FindAsync(tx.Hash);
            if (finded is null)
            {
                await Transactions.AddAsync(tx);
                await SaveChangesAsync();
            }
        }
    }
}
