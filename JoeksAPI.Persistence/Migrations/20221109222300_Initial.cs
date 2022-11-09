using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JokesAPI.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jokes",
                columns: table => new
                {
                    JokeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jokes", x => x.JokeId);
                });

            migrationBuilder.InsertData(
                table: "Jokes",
                columns: new[] { "JokeId", "Answer", "Created", "Modified", "Question" },
                values: new object[] { 1, "To get to the other side", new DateTime(2022, 11, 10, 8, 23, 0, 63, DateTimeKind.Local).AddTicks(8093), null, "Why did the chicken cross the road" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jokes");
        }
    }
}
