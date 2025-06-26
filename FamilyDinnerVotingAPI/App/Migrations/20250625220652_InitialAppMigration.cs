using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyDinnerVotingAPI.App.Migrations
{
    /// <inheritdoc />
    public partial class InitialAppMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoteSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealVoteSession",
                columns: table => new
                {
                    MealsId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoteSessionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealVoteSession", x => new { x.MealsId, x.VoteSessionsId });
                    table.ForeignKey(
                        name: "FK_MealVoteSession_Meals_MealsId",
                        column: x => x.MealsId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealVoteSession_VoteSessions_VoteSessionsId",
                        column: x => x.VoteSessionsId,
                        principalTable: "VoteSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealVoteSessions",
                columns: table => new
                {
                    MealId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoteSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MealVoteSessionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealVoteSessions", x => new { x.MealId, x.VoteSessionId });
                    table.ForeignKey(
                        name: "FK_MealVoteSessions_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealVoteSessions_VoteSessions_VoteSessionId",
                        column: x => x.VoteSessionId,
                        principalTable: "VoteSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MealId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoteSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_VoteSessions_VoteSessionId",
                        column: x => x.VoteSessionId,
                        principalTable: "VoteSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealVoteSession_VoteSessionsId",
                table: "MealVoteSession",
                column: "VoteSessionsId");

            migrationBuilder.CreateIndex(
                name: "IX_MealVoteSessions_VoteSessionId",
                table: "MealVoteSessions",
                column: "VoteSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_MealId",
                table: "Votes",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId_VoteSessionId",
                table: "Votes",
                columns: new[] { "UserId", "VoteSessionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VoteSessionId",
                table: "Votes",
                column: "VoteSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealVoteSession");

            migrationBuilder.DropTable(
                name: "MealVoteSessions");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "VoteSessions");
        }
    }
}
