using Addresses.Models;
using Microsoft.EntityFrameworkCore;

namespace Addresses.Repo
{
    public class AddressRepository : DbContext
    {
        private DbSet<Address> _addresses => Set<Address>();

        public AddressRepository(DbContextOptions<AddressRepository> options)
            : base(options)
        { }

        public async Task<Address?> FindAsync(string hash)
        {
            return await _addresses.FindAsync(hash);
        }

        public async Task CreateAsync(Address address)
        {
            var finded = await FindAsync(address.Hash);
            if (finded is null)
            {
                await _addresses.AddAsync(address);
                await SaveChangesAsync();
            }
        }

        public async Task UpdateLastBlockBalanceAsync(string hash,int block)
        {
            var finded = await FindAsync(hash);
            if (finded != null)
            {
                var edited = finded;
                edited.UpdateLastBlock(block);
                _addresses.Entry(finded).CurrentValues.SetValues(edited);
                await SaveChangesAsync();
            }
        }

        public async Task UpdateUsedGas(string hash, decimal gas)
        {
            var finded = await FindAsync(hash);
            if (finded != null)
            {
                var edited = finded;
                edited.UpdateUsedGas(gas);
                _addresses.Entry(finded).CurrentValues.SetValues(edited);
                await SaveChangesAsync();
            }
        }

        public async Task UpdateTransactionsCount(string hash, int txs = 1)
        {
            var finded = await FindAsync(hash);
            if (finded != null)
            {
                var edited = finded;
                edited.UpdateTransactionsCount(txs);
                _addresses.Entry(finded).CurrentValues.SetValues(edited);
                await SaveChangesAsync();
            }
        }
    }
}
