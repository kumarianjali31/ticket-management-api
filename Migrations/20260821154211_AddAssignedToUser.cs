using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "Tickets",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "Tickets");
        }
    }
}
