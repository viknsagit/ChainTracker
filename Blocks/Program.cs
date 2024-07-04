using Blocks;
using Blocks.Repository;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BlockRepository>(options => options.UseNpgsql(builder.Configuration["dbString"]));
builder.Services.AddSingleton<BlocksRepositoryFactory>();
builder.Services.AddSingleton(new ProducerBuilder<string, string>(new ProducerConfig()
{
    BootstrapServers = builder.Configuration["kafkaBootstrap"],
}).Build());
builder.Services.AddHostedService<BlocksIndexer>();

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
