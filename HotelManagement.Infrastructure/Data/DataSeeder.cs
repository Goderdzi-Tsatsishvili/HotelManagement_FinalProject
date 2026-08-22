
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Domain.Entities;

namespace HotelManagement.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static void EnsureRefreshTokenIsUnique(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>(entitiy => entitiy.HasIndex(x => x.Token).IsUnique());
        }

        public static void EnsurePhoneNumberIsUnique(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity => entity.HasIndex(x => x.PhoneNumber).IsUnique());
        }

        public static void EnsurePersonalNumberIsUnique(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity => entity.HasIndex(x => x.PersonalNumber).IsUnique());
        }
        public static void NormalizeIdentityTableNames(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));
            modelBuilder.Entity<IdentityRole>(entity => entity.ToTable(name: "IdentityRoles"));
            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.ToTable(name: "UserRoles"));
            modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable(name: "UserClaims"));
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable(name: "UserLogins"));
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable(name: "RoleClaims"));
            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.ToTable(name: "UserTokens"));
        }

        public static void SeedData(this ModelBuilder builder)
        {
            SeedHotels(builder);
            SeedRooms(builder);
        }

        private static class SeedIds
        {
            //Hotels
            public static readonly int Hotel1 = 1;
            public static readonly int Hotel2 = 2;
            public static readonly int Hotel3 = 3;

            //Rooms
            public static readonly int Room1 = 1;
            public static readonly int Room2 = 2;
            public static readonly int Room3 = 3;
        }

        private static void SeedHotels(this ModelBuilder builder)
        {
            builder.Entity<Hotel>().HasData(
                new Hotel { Id = SeedIds.Hotel1, Name = "Hotel 1", Country = "Georgia", City = "Tbilisi", Address = "Hotel1Address", Rating = 5},
                new Hotel { Id = SeedIds.Hotel2, Name = "Hotel 2", Country = "Germany", City = "Berlin", Address = "Hotel2Address", Rating = 3},
                new Hotel { Id = SeedIds.Hotel3, Name = "Hotel 3", Country = "USA", City = "Washington D.C", Address = "Hotel3Address", Rating = 4}
            );
        }

        private static void SeedRooms(this ModelBuilder builder)
        {
            builder.Entity<Room>().HasData(
                new Room { Id = SeedIds.Room1, Name = "Room 1", Price = 299.99m, HotelId = 1},
                new Room { Id = SeedIds.Room2, Name = "Room 2", Price = 150.99m, HotelId = 2},
                new Room { Id = SeedIds.Room3, Name = "Room 3", Price = 160.50m, HotelId = 3}
            );
        }
    }
}
