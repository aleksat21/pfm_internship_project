using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceManagement.API.Migrations
{
    public partial class FixedPkFroSplitTrasactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SplitTransactions",
                table: "SplitTransactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SplitTransactions",
                table: "SplitTransactions",
                columns: new[] { "Id", "Catcode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SplitTransactions",
                table: "SplitTransactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SplitTransactions",
                table: "SplitTransactions",
                columns: new[] { "Id", "Catcode", "Amount" });
        }
    }
}
