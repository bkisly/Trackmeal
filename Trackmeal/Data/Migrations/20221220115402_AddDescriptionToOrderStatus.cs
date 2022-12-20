using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackmeal.Data.Migrations
{
    public partial class AddDescriptionToOrderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderStatus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                "UPDATE OrderStatus SET Description = 'Your order has been send to the restaurant. It is awaiting for the confirmation.' " +
                "WHERE Id = 1");
            migrationBuilder.Sql(
                "UPDATE OrderStatus SET Description = 'The order was confirmed by the restaurant and now is being prepared. You will get notified when it is ready to collect!' " +
                "WHERE Id = 2");
            migrationBuilder.Sql(
                "UPDATE OrderStatus SET Description = 'Your order is ready. Good food is wating to be collected by you! You can approach to the collect zone and get your order.' " +
                "WHERE Id = 3");
            migrationBuilder.Sql(
                "UPDATE OrderStatus SET Description = 'The order has been collected, the process is finished. Enjoy your meal!' " +
                "WHERE Id = 4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderStatus");
        }
    }
}
