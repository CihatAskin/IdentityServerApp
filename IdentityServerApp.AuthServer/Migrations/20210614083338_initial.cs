using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerApp.AuthServer.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUser", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CustomUser",
                columns: new[] { "Id", "City", "Email", "Password", "UserName" },
                values: new object[,]
                {
                    { 1, "İzmir", "yan@askin.com", "Pass123", "Yan" },
                    { 2, "Ankara", "can@askin.com", "Pass123", "Can" },
                    { 3, "Bursa", "dan@askin.com", "Pass123", "Dan" },
                    { 4, "Van", "man@askin.com", "Pass123", "Man" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUser");
        }
    }
}
