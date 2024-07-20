using Confluent.Kafka;
using Contracts.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new ConsumerBuilder<string, string>(new ConsumerConfig
{
    BootstrapServers = builder.Configuration["kafkaBootstrap"],
    GroupId = builder.Configuration["kafkaGroupId"]
}).Build());
builder.Services.AddHostedService<ContractsConsumerService>();

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
