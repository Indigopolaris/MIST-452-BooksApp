using books452.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace books452.Data
{
    public class BooksDBContext : IdentityDbContext<IdentityUser>
    {
        public BooksDBContext(DbContextOptions<BooksDBContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; } //corresponds to sql table in DB, each row is a category of book, table is called Categories
        public DbSet<Book> Books { get; set; }//add book table

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>().HasData(

                new Category
                {
                    CategoryId = 1,
                    Name = "Travel",
                    Description = "This is the description for the Travel category"
                },

                new Category
                {
                    CategoryId = 2,
                    Name = "Fiction",
                    Description = "This is the description for the Fiction category"
                },

                new Category
                {
                    CategoryId = 3,
                    Name = "Technology",
                    Description = "This is the description for the Technology category"
                }
                );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    BookTitle = "the Wager",
                    Author = "David Grann",
                    Description = "A tale of Shipwreck, mutiny, and murder",
                    Price = 19.99m,
                    CategoryId = 1,
                    ImgUrl= ""
                },
                new Book
                {
                    BookId = 2,
                    BookTitle = "Midnight",
                    Author = "Amy McCulloch",
                    Description = "In this pulse-pounding thriller, a once-in-a-lifetime trip to Antarctica",
                    Price = 15.99m,
                    CategoryId = 2,
                    ImgUrl = ""
                },
                new Book
                {
                    BookId = 3,
                    BookTitle = "The Tusks Of Extinction",
                    Author = "Ray Naylor",
                    Description = "Moscow has resurrected the mammoth. But someone must teach them how to be mammoths, or they are doomed to die out again.",
                    Price = 25.99m,
                    CategoryId = 3,
                    ImgUrl = ""
                } 
                );  
        }
    }
}
