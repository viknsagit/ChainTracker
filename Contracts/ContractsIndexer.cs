using Contracts.Repo;
using Nethereum.Web3;

namespace Contracts
{
    public class ContractsIndexer(ContractsRepositoryFactory repoFactory,IConfiguration config)
    {
        private Web3 _web3 = new();

        public async Task IndexContract(string hash)
        {
            var contract = GetContractInDeployTransaction(hash);
        }

        public async Task GetContractInDeployTransaction(string hash)
        {

        }
    }
}
