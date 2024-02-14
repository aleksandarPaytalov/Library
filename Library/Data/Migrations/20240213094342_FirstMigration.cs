using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Category identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Category name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                },
                comment: "Books categories in the library");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Book identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Book Title"),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Author of the Book"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false, comment: "Book Description"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Book ImageUrl path"),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Rating of the book"),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "Category identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Books for the library");

            migrationBuilder.CreateTable(
                name: "IdentityUsersBooks",
                columns: table => new
                {
                    CollectorId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Collector identifier"),
                    BookId = table.Column<int>(type: "int", nullable: false, comment: "Book identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUsersBooks", x => new { x.BookId, x.CollectorId });
                    table.ForeignKey(
                        name: "FK_IdentityUsersBooks_AspNetUsers_CollectorId",
                        column: x => x.CollectorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityUsersBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Biography" },
                    { 3, "Children" },
                    { 4, "Crime" },
                    { 5, "Fantasy" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ImageUrl", "Rating", "Title" },
                values: new object[] { 5, "Dolor Sit", 1, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "https://img.freepik.com/free-psd/book-cover-mock-up-arrangement_23-2148622888.jpg?w=826&t=st=1666106877~exp=1666107477~hmac=5dea3e5634804683bccfebeffdbde98371db37bc2d1a208f074292c862775e1b", 9.5m, "Lorem Ipsum" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUsersBooks_CollectorId",
                table: "IdentityUsersBooks",
                column: "CollectorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUsersBooks");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
