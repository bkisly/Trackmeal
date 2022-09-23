using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackmeal.Data.Migrations
{
    public partial class SeedOrderStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO OrderStatus (Id, Name) VALUES " +
                                 "(1, 'Submitted'), (2, 'In preparation'), (3, 'Ready to collect'), (4, 'Completed')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM OrderStatus");
        }
    }
}
