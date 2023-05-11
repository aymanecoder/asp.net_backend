  using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;
  public class User 
    {
        public ObjectId Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime? EmailVerified { get; set; }
        public string Image { get; set; }
        public string password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> FavoriteIds { get; set; }

        // public List<Account> Accounts { get; set; }
        public List<Listing> Listings { get; set; }
        // public List<Reservation> Reservations { get; set; }
    }

    
