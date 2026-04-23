using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudySync.Migrations
{
    /// <inheritdoc />
    public partial class A02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Goal",
                table: "TblTimeRecord",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TblTimeRecord",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Goal",
                table: "TblTimeRecord");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TblTimeRecord");
        }
    }
}
