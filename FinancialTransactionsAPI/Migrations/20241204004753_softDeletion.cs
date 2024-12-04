using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialTransactionsAPI.Migrations
{
    /// <inheritdoc />
    public partial class softDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Transactions",
                newName: "TransactionUpdateDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TransactionCreateDate",
                table: "Transactions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionCreateDate",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TransactionUpdateDate",
                table: "Transactions",
                newName: "TransactionDate");
        }
    }
}
