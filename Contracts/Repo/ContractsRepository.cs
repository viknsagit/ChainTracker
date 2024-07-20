using Microsoft.EntityFrameworkCore;
using Contract = Contracts.Data.Contract;

namespace Contracts.Repo;

public class ContractsRepository : DbContext
{
    private DbSet<Contract> Contracts => Set<Contract>();

    public async Task<bool> Exist(string address)
    {
        var result = await Contracts.FindAsync(address);
        return result is not null;
    }

    public async Task Add(Contract contract)
    {
        var finded = await Contracts.FindAsync(contract.Address);
        if (finded is null)
        {
            await Contracts.AddAsync(contract);
            await SaveChangesAsync();
        }
    }

    public async Task<Contract?> Get(string hash)
    {
        var finded = await Contracts.FindAsync(hash);
        return finded;
    }
}