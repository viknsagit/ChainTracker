using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Transactions;
using Transactions.Kafka;
using Transactions.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults(); // Add services to the container.
builder.Services
    .AddControllers(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TransactionsRepository>(options => options.UseNpgsql(builder.Configuration["dbString"]));
builder.Services.AddSingleton<TransactionsRepositoryFactory>();
builder.Services.AddSingleton(new ConsumerBuilder<string, string>(new ConsumerConfig
{
    BootstrapServers = builder.Configuration["kafkaBootstrap"],
    GroupId = builder.Configuration["kafkaGroupId"]
}).Build());
builder.Services.AddSingleton(new ProducerBuilder<string, string>(new ProducerConfig
{
    BootstrapServers = builder.Configuration["kafkaBootstrap"]
}).Build());

builder.Services.AddSingleton<TransactionsIndexer>();
builder.Services.AddHostedService<TransactionsConsumerService>();

var app = builder.Build();
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();