using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceManagement.API.Migrations
{
    public partial class AddedCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Catcode",
                table: "transactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_Catcode",
                table: "transactions",
                column: "Catcode");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions",
                column: "Catcode",
                principalTable: "categories",
                principalColumn: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_Catcode",
                table: "transactions");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropIndex(
                name: "IX_transactions_Catcode",
                table: "transactions");

            migrationBuilder.AlterColumn<string>(
                name: "Catcode",
                table: "transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
