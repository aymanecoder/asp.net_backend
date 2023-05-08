  using System;
using System.Collections.Generic;
using MongoDB.Bson;
  public class User
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? EmailVerified { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> FavoriteIds { get; set; }

        // public List<Account> Accounts { get; set; }
        public List<Listing> Listings { get; set; }
        // public List<Reservation> Reservations { get; set; }
    }

    
