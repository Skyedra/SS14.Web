using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SS14.ServerHub.Shared.Data;

[UsedImplicitly]
public sealed class HubDesignTimeDbContextFactory : IDesignTimeDbContextFactory<HubDbContext>
{
    public HubDbContext CreateDbContext(string[] args)
    {
        string configPath = "/home/ss14/ss14web/SS14.ServerHub/";  // ugh >.<        
        // I really hate putting an absolute path here, but relative path wasn't working for me.
        // (I don't think anyone else will ever be using this code, but if you are, then sorry)
        //string configPath = "../SS14.Auth/";

        IConfigurationRoot configuration = new ConfigurationBuilder()        
            .AddYamlFile(configPath + "appsettings.yml", false, true)
        //    .AddYamlFile($"appsettings.{env.EnvironmentName}.yml", true, true)
            .AddYamlFile(configPath + "appsettings.Secret.yml", true, true)
            .Build();
        
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<HubDbContext>();

        optionsBuilder.UseMySql(
            connectionString, 
            ServerVersion.AutoDetect(connectionString),
            mysqlOptions => mysqlOptions.UseMicrosoftJson());

        return new HubDbContext(optionsBuilder.Options);
    }
}
