using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.EFModel
{
    public class ToracGolfContext : DbContext
    {
        public ToracGolfContext(string connectionString) : base(connectionString)
        {
        }

        public virtual DbSet<UserAccounts> Users { get; set; }
        public virtual DbSet<Ref_State> Ref_State { get; set; }
        public virtual DbSet<Ref_Season> Ref_Season { get; set; }
        public virtual DbSet<UserSeason> UserSeason { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseTeeLocations> CourseTeeLocations { get; set; }
        public virtual DbSet<Round> Rounds { get; set; }
        public virtual DbSet<Handicap> Handicap { get; set; }
        public virtual DbSet<NewsFeedLike> NewsFeedLike { get; set; }
        public virtual DbSet<NewsFeedComment> NewsFeedComment { get; set; }
        public virtual DbSet<Friends> Friends { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSeason>().ToTable("UserSeason").HasKey(x => new { x.UserId, x.SeasonId });
            modelBuilder.Entity<Handicap>().ToTable("Handicap").HasKey(x => x.RoundId);

            modelBuilder.Entity<NewsFeedLike>().ToTable("NewsFeedLike").HasKey(x => new { x.AreaId, x.NewsFeedTypeId, x.UserIdThatLikedItem });

            modelBuilder.Entity<NewsFeedComment>().ToTable("NewsFeedComment")
                .HasKey(x => x.CommentId)
                .Property(x => x.CommentId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<NewsFeedComment>()
                .HasRequired(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserIdThatCommented);

            modelBuilder.Entity<Course>().ToTable("Course")
                .HasKey(x => x.CourseId)
                .Property(x => x.CourseId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Course>().HasRequired(x => x.State).WithMany(x => x.Courses);

            //need this otherwise when we go to insert a round it blows up 

            modelBuilder.Entity<Round>().ToTable("Round")
                .HasKey(x => x.RoundId)
                .Property(x => x.RoundId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Round>()
             .HasRequired(x => x.Handicap)
             .WithRequiredPrincipal(c2 => c2.Round);

            modelBuilder.Entity<UserAccounts>()
                .HasKey(x => x.UserId)
                .Property(x => x.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Ref_State>().HasKey(x => x.StateId);

            modelBuilder.Entity<CourseTeeLocations>()
                .HasKey(x => x.CourseTeeLocationId)
                .Property(x => x.CourseTeeLocationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Ref_State>().HasKey(x => x.StateId);

            modelBuilder.Entity<Ref_Season>()
                .HasKey(x => x.SeasonId)
                .Property(x => x.SeasonId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Friends>()
                .HasKey(x => new { x.UserId, x.FriendId });
        }

    }
}
