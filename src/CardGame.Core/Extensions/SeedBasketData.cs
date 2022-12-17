using CardGame.Core.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CardGame.Core.Extensions
{
    public static class SeedBasketData
    {
        public static async Task InitializeBasketDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var serverDbContext = serviceScope.ServiceProvider.GetRequiredService<CardGameContext>();
            await serverDbContext.Database.MigrateAsync();
           
        }
    }
}