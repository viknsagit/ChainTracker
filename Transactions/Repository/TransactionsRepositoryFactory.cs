namespace Transactions.Repository
{
    public class TransactionsRepositoryFactory(IServiceScopeFactory scopeFactory)
    {
        public TransactionsRepository Create()
        {
            var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<TransactionsRepository>();
        }
    }
}
