using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Months",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Months", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoneyTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Money = table.Column<float>(type: "real", nullable: false),
                    IsThisIncome = table.Column<bool>(type: "bit", nullable: false),
                    ReocurringMonth = table.Column<bool>(type: "bit", nullable: false),
                    ReocurringYear = table.Column<bool>(type: "bit", nullable: false),
                    IsThisSalary = table.Column<bool>(type: "bit", nullable: false),
                    SickDays = table.Column<int>(type: "int", nullable: false),
                    CalculatedValue = table.Column<float>(type: "real", nullable: false),
                    MonthId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyTransactions_Months_MonthId",
                        column: x => x.MonthId,
                        principalTable: "Months",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactions_MonthId",
                table: "MoneyTransactions",
                column: "MonthId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoneyTransactions");

            migrationBuilder.DropTable(
                name: "Months");
        }
    }
}
