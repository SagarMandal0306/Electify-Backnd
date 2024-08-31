using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class addroomtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userinroom",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roomid = table.Column<int>(type: "int", nullable: false),
                    userid = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userinroom", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userinroom");
        }
    }
}
