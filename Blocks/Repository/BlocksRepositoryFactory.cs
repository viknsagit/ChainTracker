using Nethereum.BlockchainProcessing.BlockStorage.Repositories;

namespace Blocks.Repository
{
    public class BlocksRepositoryFactory(IServiceScopeFactory scopeFactory)
    {
        public BlockRepository Create()
        {
            var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<BlockRepository>();
        }
    }
}
