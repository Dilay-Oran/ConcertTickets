using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcertTickets.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceCapacityToConcert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Concerts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Concerts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "Concerts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Concerts");
        }
    }
}