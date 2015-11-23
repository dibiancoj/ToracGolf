using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace ToracGolf.Migrations
{
    public partial class BlankDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    OnlyAllow18Holes = table.Column<bool>(nullable: false),
                    Pending = table.Column<bool>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    UserIdThatCreatedCourse = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                });
            migrationBuilder.CreateTable(
                name: "CourseImages",
                columns: table => new
                {
                    CourseId = table.Column<int>(nullable: false),
                    CourseImage = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseImages", x => x.CourseId);
                });
            migrationBuilder.CreateTable(
                name: "Ref_Season",
                columns: table => new
                {
                    SeasonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    SeasonText = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ref_Season", x => x.SeasonId);
                });
            migrationBuilder.CreateTable(
                name: "Ref_State",
                columns: table => new
                {
                    StateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ref_State", x => x.StateId);
                });
            migrationBuilder.CreateTable(
                name: "RoundHandicap",
                columns: table => new
                {
                    RoundId = table.Column<int>(nullable: false),
                    HandicapAfterRound = table.Column<double>(nullable: false),
                    HandicapBeforeRound = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundHandicap", x => x.RoundId);
                });
            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CurrentSeasonId = table.Column<int>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    StateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.UserId);
                });
            migrationBuilder.CreateTable(
                name: "UserSeason",
                columns: table => new
                {
                    SeasonId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSeason", x => new { x.SeasonId, x.UserId });
                });
            migrationBuilder.CreateTable(
                name: "CourseTeeLocations",
                columns: table => new
                {
                    CourseTeeLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Back9Par = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Front9Par = table.Column<int>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    Slope = table.Column<double>(nullable: false),
                    TeeLocationSortOrderId = table.Column<int>(nullable: false),
                    Yardage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTeeLocations", x => x.CourseTeeLocationId);
                    table.ForeignKey(
                        name: "FK_CourseTeeLocations_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Round",
                columns: table => new
                {
                    RoundId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(nullable: false),
                    CourseTeeLocationId = table.Column<int>(nullable: false),
                    FairwaysHit = table.Column<int>(nullable: true),
                    FairwaysHitPossible = table.Column<int>(nullable: true),
                    GreensInRegulation = table.Column<int>(nullable: true),
                    Is9HoleScore = table.Column<bool>(nullable: false),
                    Putts = table.Column<int>(nullable: true),
                    RoundDate = table.Column<DateTime>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    SeasonId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.RoundId);
                    table.ForeignKey(
                        name: "FK_Round_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Round_CourseTeeLocations_CourseTeeLocationId",
                        column: x => x.CourseTeeLocationId,
                        principalTable: "CourseTeeLocations",
                        principalColumn: "CourseTeeLocationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("CourseImages");
            migrationBuilder.DropTable("Ref_Season");
            migrationBuilder.DropTable("Ref_State");
            migrationBuilder.DropTable("Round");
            migrationBuilder.DropTable("RoundHandicap");
            migrationBuilder.DropTable("UserAccounts");
            migrationBuilder.DropTable("UserSeason");
            migrationBuilder.DropTable("CourseTeeLocations");
            migrationBuilder.DropTable("Course");
        }

    }
}
