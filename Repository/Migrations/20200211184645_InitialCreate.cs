using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductCatalogue.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Title = table.Column<string>(nullable: false),
                    BusinessName = table.Column<string>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    TotalCost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => new { x.Title, x.BusinessName });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
