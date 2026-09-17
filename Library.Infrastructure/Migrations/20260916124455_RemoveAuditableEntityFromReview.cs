using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAuditableEntityFromReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "last_modified_by_user_id",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "last_modified_date",
                table: "reviews");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "reviews",
                newName: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "reviews",
                newName: "created_date");

            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "last_modified_by_user_id",
                table: "reviews",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modified_date",
                table: "reviews",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
