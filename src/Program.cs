using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using pelican.DataProviders;
using pelican.Services;
using System;
using discord.Services;
using pelican.Utility;

Console.WriteLine("----------------------------------------------------------------------");
Console.WriteLine(" _  __          _             ____  _                 ____        _   ");
Console.WriteLine("| |/ /_____   _(_)_ __  ___  |  _ \\| |_ ___ _ __ ___ | __ )  ___ | |_ ");
Console.WriteLine("| ' // _ \\ \\ / / | '_ \\/ __| | |_) | __/ _ \\ '__/ _ \\|  _ \\ / _ \\| __|");
Console.WriteLine("| . \\  __/\\ V /| | | | \\__ \\ |  __/| ||  __/ | | (_) | |_) | (_) | |_ ");
Console.WriteLine("|_|\\_\\___| \\_/ |_|_| |_|___/ |_|    \\__\\___|_|  \\___/|____/ \\___/ \\__|");
Console.WriteLine("----------------------------------------------------------------------");

var discordToken = Settings.DiscordToken;
var url = Settings.PelicanUrl;
var authGroup = Settings.DiscordAuthGroup;
var globalKey = Settings.GlobalPelicanKey;

if (string.IsNullOrEmpty(authGroup))
   Console.WriteLine("Currently not adding users to a special discord group on authentication.");
else
   Console.WriteLine("Currently adding users to a special discord group on authentication. (Group ID "+ authGroup + ")");

if (string.IsNullOrEmpty(globalKey))
   Console.WriteLine("Currently not using a global API key that everyone can use.");

using IHost host = Host.CreateDefaultBuilder(args)
.ConfigureServices(services =>
{
   var discordConfig = new DiscordSocketConfig()
   {
      GatewayIntents = Discord.GatewayIntents.Guilds,
      AlwaysDownloadUsers = false,
      MessageCacheSize = 100,
      ConnectionTimeout = 30000,
      DefaultRetryMode = Discord.RetryMode.AlwaysRetry,
      LogGatewayIntentWarnings = false
   };

   services.AddSingleton(new DiscordSocketClient(discordConfig));
       services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
       services.AddScoped<IPterodactylModuleDataProvider, PterodactylModuleDataProvider>();
       services.AddHostedService<InteractionHandlingService>();
       services.AddHostedService<DiscordStartupService>();
       services.AddHttpClient<IPterodactylHttpService, PterodactylHttpService>(client =>
       {
          var address = Settings.PelicanUrl;
          client.BaseAddress = new Uri(address);
       });

    })
    .Build();

await host.RunAsync();