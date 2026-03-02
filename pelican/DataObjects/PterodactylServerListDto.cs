#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace pelican.DataObjects
{
   /// <summary>
   /// Dataobject for retrieving pelican server lists
   /// </summary>
   public record PterodactylServerListDto
   {
      public PterodactylServerListServerDto[] data { get; set; }
   }

   /// <summary>
   /// Dataobject for retrieving pelican servers
   /// </summary>
   public record PterodactylServerListServerDto
   {
      public string _object { get; set; }
      public PterodactylServerDtoAttributes attributes { get; set; }
   }

   /// <summary>
   /// Dataobject for retrieving pelican server attributes
   /// </summary>
   public record PterodactylServerDtoAttributes
   {
      public bool server_owner { get; set; }
      public string identifier { get; set; }
      public string uuid { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public bool is_suspended { get; set; }
      public bool is_installing { get; set; }
   }
}
