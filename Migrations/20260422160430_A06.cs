using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudySync.Migrations
{
    /// <inheritdoc />
    public partial class A06 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Friends",
                table: "TblUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "IncomingRequests",
                table: "TblUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "OutgoingRequests",
                table: "TblUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Friends",
                table: "TblUser");

            migrationBuilder.DropColumn(
                name: "IncomingRequests",
                table: "TblUser");

            migrationBuilder.DropColumn(
                name: "OutgoingRequests",
                table: "TblUser");
        }
    }
}
