using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceManagement.API.Migrations
{
    public partial class SplitTransaction_Category_Relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SplitTransactions_Catcode",
                table: "SplitTransactions",
                column: "Catcode");

            migrationBuilder.AddForeignKey(
                name: "FK_SplitTransactions_categories_Catcode",
                table: "SplitTransactions",
                column: "Catcode",
                principalTable: "categories",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SplitTransactions_categories_Catcode",
                table: "SplitTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SplitTransactions_Catcode",
                table: "SplitTransactions");
        }
    }
}
