namespace pelican.DataObjects
{
   public record UserDto
   {
      public int Id { get; set; }
      public long DiscordID { get; set; }
      public string? PelicanApiKey { get; set; }
   }
}
