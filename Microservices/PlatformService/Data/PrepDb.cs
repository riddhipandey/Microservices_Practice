using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data{
    public static class PrepDb{
        public static void PrepPopulation(IApplicationBuilder app, bool isDev){
            using(var serviceScope = app.ApplicationServices.CreateScope()){
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isDev);
            }
        }
 
        public static void SeedData(AppDbContext context, bool isDev){
            if(isDev){
                Console.WriteLine("Trying to apply Migration...");{
                    try{
                        context.Database.Migrate();
                    }
                    catch(Exception ex){
                        Console.WriteLine("Could not run migration ..." + ex);
                    }
                }
            }
            if(!context.Platforms.Any()){
                Console.WriteLine("--- Seeding Data ---");
                context.Platforms.AddRange(
                    new Platform() {Name="Dot Net", Publisher = "Microsoft", Cost = "Free"},
                    new Platform() {Name="SQL Server", Publisher = "Microsoft", Cost = "Free"},
                    new Platform() {Name="Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"}
                );
                context.SaveChanges();
            }else{
                Console.WriteLine("--- We already have data ---");
            }
        }
    }
}