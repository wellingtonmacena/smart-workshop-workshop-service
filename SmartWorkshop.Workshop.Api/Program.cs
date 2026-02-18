using SmartWorkshop.Shared.EventBus.MassTransit;
using SmartWorkshop.Workshop.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Configure PostgreSQL Database for relational data
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
//     ?? "Host=localhost;Port=5433;Database=workshop_db;Username=workshop_user;Password=workshop_pass";
// builder.Services.AddDbContext<WorkshopDbContext>(options =>
//     options.UseNpgsql(connectionString));

// TODO: Configure MongoDB for diagnostic and repair logs
// var mongoConnectionString = builder.Configuration.GetValue<string>("MongoDB:ConnectionString")
//     ?? "mongodb://admin:admin123@localhost:27017";
// var mongoDatabaseName = builder.Configuration.GetValue<string>("MongoDB:DatabaseName")
//     ?? "workshop_logs_db";
// builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));
// builder.Services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

// Register MassTransit with RabbitMQ and Consumers
builder.Services.AddMassTransitWithRabbitMQ(builder.Configuration, x =>
{
    // Register consumers for events from Billing Service
    x.AddConsumer<QuoteApprovedConsumer>();
    x.AddConsumer<PaymentConfirmedConsumer>();
    
    // Workshop Service publishes:
    // - ServiceOrderCreatedIntegrationEvent (to Billing)
    // - WorkCompletedIntegrationEvent (to Billing)
});

// Add Health Checks
builder.Services.AddHealthChecks();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
