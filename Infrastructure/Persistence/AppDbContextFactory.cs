using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Client_ConceitoCadastro.Infrastructure.Persistence
{
    // Infrastructure/Persistence/AppDbContextFactory.cs
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseSqlite($"Data Source={SqlitePaths.GetDbPath()}") // usa o MESMO path
           .Options;

            return new AppDbContext(options);
        }
    }

}