using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.Migrations
{
    [DbContext(typeof(ToracGolfContext))]
    [Migration("20151123193922_BlankDatabase")]
    partial class BlankDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 75);

                    b.Property<bool>("OnlyAllow18Holes");

                    b.Property<bool>("Pending");

                    b.Property<int>("StateId");

                    b.Property<int>("UserIdThatCreatedCourse");

                    b.HasKey("CourseId");

                    b.HasAnnotation("Relational:TableName", "Course");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.CourseImages", b =>
                {
                    b.Property<int>("CourseId");

                    b.Property<byte[]>("CourseImage");

                    b.HasKey("CourseId");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.CourseTeeLocations", b =>
                {
                    b.Property<int>("CourseTeeLocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Back9Par");

                    b.Property<int>("CourseId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int>("Front9Par");

                    b.Property<double>("Rating");

                    b.Property<double>("Slope");

                    b.Property<int>("TeeLocationSortOrderId");

                    b.Property<int>("Yardage");

                    b.HasKey("CourseTeeLocationId");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.Ref_Season", b =>
                {
                    b.Property<int>("SeasonId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("SeasonText")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("SeasonId");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.Ref_State", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("StateId");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.Round", b =>
                {
                    b.Property<int>("RoundId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseId");

                    b.Property<int>("CourseTeeLocationId");

                    b.Property<int?>("FairwaysHit");

                    b.Property<int?>("FairwaysHitPossible");

                    b.Property<int?>("GreensInRegulation");

                    b.Property<bool>("Is9HoleScore");

                    b.Property<int?>("Putts");

                    b.Property<DateTime>("RoundDate");

                    b.Property<int>("Score");

                    b.Property<int>("SeasonId");

                    b.Property<int>("UserId");

                    b.HasKey("RoundId");

                    b.HasAnnotation("Relational:TableName", "Round");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.RoundHandicap", b =>
                {
                    b.Property<int>("RoundId");

                    b.Property<double>("HandicapAfterRound");

                    b.Property<double>("HandicapBeforeRound");

                    b.HasKey("RoundId");

                    b.HasAnnotation("Relational:TableName", "RoundHandicap");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.UserAccounts", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("CurrentSeasonId");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("StateId");

                    b.HasKey("UserId");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.UserSeason", b =>
                {
                    b.Property<int>("SeasonId");

                    b.Property<int>("UserId");

                    b.Property<DateTime>("CreatedDate");

                    b.HasKey("SeasonId", "UserId");

                    b.HasAnnotation("Relational:TableName", "UserSeason");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.CourseTeeLocations", b =>
                {
                    b.HasOne("ToracGolf.MiddleLayer.EFModel.Tables.Course")
                        .WithMany()
                        .HasForeignKey("CourseId");
                });

            modelBuilder.Entity("ToracGolf.MiddleLayer.EFModel.Tables.Round", b =>
                {
                    b.HasOne("ToracGolf.MiddleLayer.EFModel.Tables.Course")
                        .WithMany()
                        .HasForeignKey("CourseId");

                    b.HasOne("ToracGolf.MiddleLayer.EFModel.Tables.CourseTeeLocations")
                        .WithMany()
                        .HasForeignKey("CourseTeeLocationId");
                });
        }
    }
}
