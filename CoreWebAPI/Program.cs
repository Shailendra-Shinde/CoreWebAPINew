using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace CoreWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();

            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            var builtConfig = config.Build();
            var keyVaultEndpoint = builtConfig["VaultURL"];
            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));
                config.AddAzureKeyVault(
                    keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
            }
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Trace);
        })
        .UseNLog();

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)

        //        .ConfigureLogging(logging =>
        //        {
        //            logging.ClearProviders();
        //            logging.SetMinimumLevel(LogLevel.Trace);
        //        })
        //        .UseNLog()  // NLog: Setup NLog for Dependency injection
        //        .ConfigureAppConfiguration((context, config) =>
        //        {
        //            var buildConfig = config.Build();
        //            var vaultName = buildConfig["VaultURL"];

        //            var keyVaultClient = new KeyVaultClient(async (authority, resource, scope) =>
        //            {
        //                var credential = new DefaultAzureCredential(false);
        //                var token = credential.GetToken(
        //                    new Azure.Core.TokenRequestContext(new[] { "https://vault.azure.net/.default" }));
        //                return token.Token;
        //            });
        //            config.AddAzureKeyVault(vaultName, keyVaultClient, new DefaultKeyVaultSecretManager());
        //        })
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //    .ConfigureAppConfiguration((context, config) =>
        //    {
        //        var builtConfig = config.Build();
        //        var vaultName = builtConfig["VaultURL"];
        //        var keyVaultClient = new KeyVaultClient(async (authority, resource, scope) =>
        //        {
        //            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeSharedTokenCacheCredential = true });
        //            var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://vault.azure.net/.default" }));
        //            return token.Token;
        //        });
        //        config.AddAzureKeyVault(vaultName, keyVaultClient, new DefaultKeyVaultSecretManager());
        //    })
        //    .ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        webBuilder.UseStartup<Startup>();
        //    })
        //    .ConfigureLogging(logging =>
        //    {
        //        logging.ClearProviders();
        //        logging.SetMinimumLevel(LogLevel.Trace);
        //    })
        //    .UseNLog();


    }
}
