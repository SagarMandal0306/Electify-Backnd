using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "options",
                columns: table => new
                {
                    optionid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pollid = table.Column<int>(type: "int", nullable: false),
                    option = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_options", x => x.optionid);
                });

            migrationBuilder.CreateTable(
                name: "polls",
                columns: table => new
                {
                    pollid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roomid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_polls", x => x.pollid);
                });

            migrationBuilder.CreateTable(
                name: "room",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    roomid = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    voteid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(type: "int", nullable: false),
                    optionid = table.Column<int>(type: "int", nullable: false),
                    pollid = table.Column<int>(type: "int", nullable: false),
                    votedat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_votes", x => x.voteid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "options");

            migrationBuilder.DropTable(
                name: "polls");

            migrationBuilder.DropTable(
                name: "room");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "votes");
        }
    }
}
