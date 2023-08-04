using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using WebApplication2.Data;
using WebApplication2.Models;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("AdvancedDatabaseA1Connection");

builder.Services.AddDbContext<LaptopStoreContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;

    await SeedData.Initialize(serviceProvider);
}

// Endpoints
app.MapGet("/laptops/search", (LaptopStoreContext 
    db, decimal priceAbove, decimal priceBelow, int storeStock, 
    string province, LaptopCondition condition, int brandId, string searchPhrase) =>
{
    try
    {
        if (priceAbove > 0)
        {
            return Results.Ok(db.Laptops.Where(l => l.Price > priceAbove));
        }
        else if (priceBelow > 0)
        {
            return Results.Ok(db.Laptops.Where(l => l.Price < priceBelow));
        }
        else if (storeStock > 0)
        {
            return Results.Ok(db.Stores.Where(s => s.Stock.Count > 0));
        }
        else if (condition == LaptopCondition.New || condition == LaptopCondition.Refurbished ||
            condition == LaptopCondition.Rental)
        {
            return Results.Ok(db.Laptops.Where(l => l.Condition == condition));
        }
        else if (brandId > 0)
        {
            return Results.Ok(db.Laptops.Where(l => l.BrandId == brandId));
        }
        else if (searchPhrase.Length > 0)
        {
            return Results.Ok(db.Laptops.Where(l => l.Model.Contains(searchPhrase)));
        }
        else
        {
            return Results.Ok(db.Laptops.ToList());
        }
    }
    catch (ArgumentException)
    {
        throw new ArgumentException("Invalid values");
    }
});

app.MapGet("/stores/{storeNumber}/inventory", (LaptopStoreContext db, Guid storeNumber) =>
{
    try
    {
        Store store = db.Stores.FirstOrDefault(s => s.Id == storeNumber);

        if (store == null)
        {
            return Results.NotFound($"Store with number {storeNumber} not found.");
        }

        List<Laptop> laptopsInStore = db.Laptops
            .Where(laptop => laptop.StoreId == store.Id && laptop.Quantity > 0)
            .ToList();

        return Results.Ok(laptopsInStore);
    }
    catch (ArgumentException)
    {
        throw new ArgumentException("Invalid values");
    }
});

app.MapPost("/{storeNumber}/{laptopNumber}/changeQuantity?amount", (LaptopStoreContext db,
            Guid storeNumber, Guid laptopNumber, int amount) =>
{
    try
    {
        Store store = db.Stores.FirstOrDefault(s => s.Id == storeNumber);
        if (store == null)
        {
            return Results.NotFound($"Store with number {storeNumber} not found.");
        }

        Laptop laptop = db.Laptops.FirstOrDefault(l => l.Id == laptopNumber && l.StoreId == store.Id);
        if (laptop == null)
        {
            return Results.NotFound($"Laptop with number {laptopNumber} not found at store {storeNumber}.");
        }

        laptop.Quantity += amount;
        db.SaveChanges();

        return Results.Ok($"Laptop quantity updated successfully. New quantity: {laptop.Quantity}");
    }
    catch (ArgumentException)
    {
        throw new ArgumentException("Invalid values");
    }
});

app.MapGet("/laptops/brand/average", (LaptopStoreContext db, int brandId) =>
{
    try
    {
        List<Laptop> laptopsByBrand = db.Laptops.Where(laptop => laptop.BrandId == brandId).ToList();
        if (laptopsByBrand.Count == 0)
        {
            return Results.NotFound($"No laptops found for the brand with ID {brandId}.");
        }

        int laptopCount = laptopsByBrand.Count;
        decimal averagePrice = laptopsByBrand.Average(laptop => laptop.Price);

        return Results.Ok(new { LaptopCount = laptopCount, AveragePrice = averagePrice });
    }
    catch (ArgumentException)
    {
        throw new ArgumentException("Invalid values");
    }
});

app.MapGet("/province/stores", (LaptopStoreContext db) =>
{
    try
    {
        var storeByProvince = db.Stores
            .GroupBy(store => store.Province)
            .Where(group => group.Any())
            .Select(group => new
            {
                Province = group.Key,
                Stores = group.Select(store => new
                {
                    store.Id,
                    store.StreetName,
                    store.StreetNumber
                })
            })
            .ToList();

        return Results.Ok(storeByProvince);
    }
    catch (ArgumentException)
    {
        throw new ArgumentException("Invalid values");
    }
});

app.Run();
