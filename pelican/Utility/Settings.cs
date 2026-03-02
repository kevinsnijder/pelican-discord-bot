namespace pelican.Utility
{
   public static class Settings
   {
      public static string? GlobalPelicanKey
      {
         get
         {
            return Environment.GetEnvironmentVariable("PELICAN_GLOBAL_API_KEY");
         }
      }

      public static string DiscordToken
      {
         get
         {
            var discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if(discordToken == null)
               throw new ArgumentNullException("DISCORD_TOKEN environment variable is not set!");
            return discordToken;
         }
      }

      public static string PelicanUrl
      {
         get
         {
            var url = Environment.GetEnvironmentVariable("PELICAN_API_URL");
            if (url == null)
               throw new ArgumentNullException("PELICAN_API_URL environment variable is not set!");
            return url.TrimEnd('/');
         }
      }

      public static string? DiscordAuthGroup
      {
         get
         {
            return Environment.GetEnvironmentVariable("DISCORD_LOGIN_GROUP_ID");
         }
      }
   }
}
