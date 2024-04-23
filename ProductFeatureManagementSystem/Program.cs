using MongoDB.Driver;
using ProductFeatureManagementSystem.Models;
using ProductFeatureManagementSystem.Repositories;
using ProductFeatureManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductManagementRepo, ProductManagementRepo>();
builder.Services.AddScoped<IProductManagementService, ProductManagementService>();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>
    new MongoClient(builder.Configuration.GetValue<string>("MongoDB:ConnectionString")));
builder.Services.AddSingleton(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase(builder.Configuration.GetValue<string>("MongoDB:DatabaseName"));
    return database.GetCollection<ProductFeature>(builder.Configuration.GetValue<string>("MongoDB:CollectionName"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000") // Adjust this if your React app is served from a different URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); // Only set this if your client needs to send credentials like cookies or authentication headers
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();