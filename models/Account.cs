

// namespace appLogement.models
// {
  
// public class Account
// {
//     [Key]
//     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//     [BsonId]
//     [BsonRepresentation(BsonType.ObjectId)]
//     public string Id { get; set; }

//     [Required]
//     [ForeignKey("User")]
//     [BsonRepresentation(BsonType.ObjectId)]
//     public string UserId { get; set; }

//     [Required]
//     public string Type { get; set; }

//     [Required]
//     public string Provider { get; set; }

//     [Required]
//     public string ProviderAccountId { get; set; }

//     public string RefreshToken { get; set; }

//     public string AccessToken { get; set; }

//     public int? ExpiresAt { get; set; }

//     public string TokenType { get; set; }

//     public string Scope { get; set; }

//     public string IdToken { get; set; }

//     public string SessionState { get; set; }

//     [ForeignKey("UserId")]
//     public virtual User User { get; set; }

//     [Index("IX_Account_Provider_ProviderAccountId", 1, IsUnique = true)]
//     public string ProviderAndProviderAccountId
//     {
//         get { return Provider + ProviderAccountId; }
//         set { }
//     }
// }
// }