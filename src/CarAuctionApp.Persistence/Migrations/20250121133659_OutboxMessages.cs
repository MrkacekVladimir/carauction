using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAuctionApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "OutboxMessages",
                newName: "Payload");

            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "OutboxMessages",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "OutboxMessages");

            migrationBuilder.RenameColumn(
                name: "Payload",
                table: "OutboxMessages",
                newName: "Data");
        }
    }
}
