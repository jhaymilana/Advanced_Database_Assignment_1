using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApplication2.Models;
using static WebApplication2.Models.Store;

namespace WebApplication2.Data
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            LaptopStoreContext db = new LaptopStoreContext(serviceProvider.GetRequiredService<DbContextOptions<LaptopStoreContext>>());

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            // Stores
            Store firstStore = new Store("Store 1 Street", "123", CanadianProvince.Ontario);
            Store secondStore = new Store("Store 2 Avenue", "456", CanadianProvince.Alberta);
            Store thirdStore = new Store("Store 3 Road", "789", CanadianProvince.Quebec);

            if (!db.Stores.Any())
            {
                db.Add(firstStore);
                db.Add(secondStore);
                db.Add(thirdStore);
                db.SaveChanges();
            }

            // Laptops
            Laptop firstLaptop = new Laptop
            {
                Id = Guid.NewGuid(),
                Model = "Laptop Model A",
                Price = 1000,
                Condition = LaptopCondition.New,
                BrandId = 1,
                StoreId = Guid.NewGuid(),
                Quantity = 10
            };
            Laptop secondLaptop = new Laptop
            {
                Id = Guid.NewGuid(),
                Model = "Laptop Model B",
                Price = 800,
                Condition = LaptopCondition.Refurbished,
                BrandId = 1,
                StoreId = Guid.NewGuid(),
                Quantity = 5
            };
            Laptop thirdLaptop = new Laptop
            {
                Id = Guid.NewGuid(),
                Model = "Laptop Model C",
                Price = 1200,
                Condition = LaptopCondition.New,
                BrandId = 2,
                StoreId = Guid.NewGuid(),
                Quantity = 15
            };

            if (!db.Laptops.Any())
            {
                db.Add(firstLaptop);
                db.Add(secondLaptop);
                db.Add(thirdLaptop);
                db.SaveChanges();
            }

            // Brands
            Brand firstBrand = new Brand { Id = 1, Name = "Lenovo" };
            Brand secondBrand = new Brand { Id = 2, Name = "Apple" };
            Brand thirdBrand = new Brand { Id = 3, Name = "Asus" };

            if (!db.Brands.Any())
            {
                db.Add(firstBrand);
                db.Add(secondBrand);
                db.Add(thirdBrand);
                db.SaveChanges();
            }
        }
    }
}
