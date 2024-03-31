using books452.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using books452;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        //fetch connstring info
        var connString = builder.Configuration.GetConnectionString("DefaultConnection");

        // add context class to set of services, define option to use sql server on the fetched connection string 
        builder.Services.AddDbContext<BooksDBContext>(options => options.UseSqlServer(connString));
        
        builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<BooksDBContext>().AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options => 
        { options.LoginPath = "/Identity/Account/Login";
            options.LogoutPath = "/Identity/Account/Logout";
            options.AccessDeniedPath = "/Identity/Account/Access";
            
        }); // add cookies, initialize path

        builder.Services.AddRazorPages();

        builder.Services.AddScoped<IEmailSender, EmailSender>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/ aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllerRoute(
            name: "default",
            pattern: "{Area=Customer}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}


    
