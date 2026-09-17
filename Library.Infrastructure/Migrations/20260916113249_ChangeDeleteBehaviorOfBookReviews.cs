using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehaviorOfBookReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reviews_books_book_id",
                table: "reviews");

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_books_book_id",
                table: "reviews",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reviews_books_book_id",
                table: "reviews");

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_books_book_id",
                table: "reviews",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
