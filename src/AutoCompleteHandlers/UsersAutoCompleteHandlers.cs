using Discord;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using pelican.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.AutoCompleteHandlers
{
   /// <summary>
   /// Handles autocompletes for online pterodactyl servers
   /// </summary>
   public class UsersAutoCompleteHandler : AutocompleteHandler
   {
      /// <inheritdoc/>
      public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
      {
         var logger = services.GetRequiredService<ILogger<UsersAutoCompleteHandler>>();
         
         try
         {
            logger.LogInformation("UsersAutoCompleteHandler called");
            
            var database = PterodactylDatabase.Instance;
            var userTypingString = autocompleteInteraction.Data.Options.FirstOrDefault()?.Value?.ToString() ?? string.Empty;
            
            logger.LogInformation($"User typed: '{userTypingString}'");
            
            var allUsers = database.GetUsers();
            logger.LogInformation($"Found {allUsers.Count()} users in database");
            
            // Create a collection with suggestions for autocomplete
            var results = new List<AutocompleteResult>();

            foreach (var user in allUsers)
            {
               try
               {
                  var foundUser = await context.Guild.GetUserAsync((ulong)user.DiscordID);

                  if(foundUser != null)
                  {
                     var displayName = foundUser.Nickname ?? foundUser.Username;
                     results.Add(new AutocompleteResult(displayName, user.Id));
                     logger.LogInformation($"Added user: {displayName} (ID: {user.Id})");
                  }
                  else
                  {
                     results.Add(new AutocompleteResult(user.DiscordID.ToString(), user.Id));
                     logger.LogInformation($"Added user by Discord ID: {user.DiscordID} (DB ID: {user.Id})");
                  }
               }
               catch (Exception userEx)
               {
                  logger.LogWarning($"Failed to get Discord user {user.DiscordID}: {userEx.Message}");
               }
            }

            if (!string.IsNullOrEmpty(userTypingString))
            {
               results = results.Where(res => res.Name.ToLower().Contains(userTypingString.ToLower())).ToList();
               logger.LogInformation($"Filtered to {results.Count} results");
            }

            logger.LogInformation($"Returning {results.Count} autocomplete results");
            return AutocompletionResult.FromSuccess(results.OrderBy(x => x.Name).Take(25)); // max 25 suggestions at a time (API limit)
         }
         catch (Exception ex)
         {
            logger.LogError(ex, $"Error in UsersAutoCompleteHandler: {ex.Message}");
            return AutocompletionResult.FromError(InteractionCommandError.Unsuccessful, "Failed to load users");
         }
      }
   }
}
