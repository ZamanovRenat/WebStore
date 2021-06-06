using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;

namespace WebStore.DAL.Context
{
    public class WebStoreDB : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public WebStoreDB(DbContextOptions<WebStoreDB> options) : base(options)
        {
            
        }
    }
}
