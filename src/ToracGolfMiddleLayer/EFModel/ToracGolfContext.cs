using System;
using System.Collections.Generic;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSeason>().ToTable("UserSeason");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Handicap>().ToTable("Handicap");

            modelBuilder.Entity<Round>().ToTable("Round");//.HasRequired(x => x.CourseTeeLocationId);

            modelBuilder.Entity<Round>()
                .HasRequired(x => x.Handicap);
                //.HasForeignKey(x => x.RoundId);



            //modelBuilder.Entity<CourseTeeLocations>().HasKey(e => e.CourseTeeLocationId);

            //modelBuilder.Entity<Course>().HasOptional(x => x.CourseImage).WithRequired(x => x.Course);

            modelBuilder.Entity<Ref_State>().HasKey(x => x.StateId);
            modelBuilder.Entity<Course>().HasRequired(x => x.State).WithMany(x => x.Courses);
            
        }

    }
}
