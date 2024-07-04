using Microsoft.EntityFrameworkCore;

using Nethereum.RPC.Eth.DTOs;
using Block = Blocks.Data.Block;

namespace Blocks.Repository
{
    public class BlockRepository : DbContext
    {
        private DbSet<Block> Blocks => Set<Block>();

        public BlockRepository(DbContextOptions<BlockRepository> options)
            : base(options)
        {
        }

        public async Task<Block?> FindAsync(long id)
        {
            return await Blocks.FindAsync(id);
        }

        public async Task CreateAsync(BlockWithTransactions block)
        {
                var newBlock = new Block(block);
                var exists = await FindAsync(newBlock.BlockNumber);
                if (exists is null)
                {
                    await Blocks.AddAsync(newBlock);
                    await SaveChangesAsync();
                }
        }

        public async Task UpdateAsync(BlockWithTransactions block)
        {
            var newBlock = new Block(block);
            var oldBlock = await FindAsync(newBlock.BlockNumber);
            if (oldBlock != null)
            {
                Blocks.Entry(oldBlock!).CurrentValues.SetValues(newBlock);
                await SaveChangesAsync();
            }
        }
    }
}
