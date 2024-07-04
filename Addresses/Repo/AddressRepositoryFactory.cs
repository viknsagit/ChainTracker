namespace Addresses.Repo
{
    public class AddressRepositoryFactory(IServiceScopeFactory scopeFactory)
    {
        public AddressRepository Create()
        {
            var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<AddressRepository>();
        }
    }
}
