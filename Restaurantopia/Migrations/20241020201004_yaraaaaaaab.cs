using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurantopia.Migrations
{
    /// <inheritdoc />
    public partial class yaraaaaaaab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Customers_CustomerId",
                table: "Review");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83f30b7f-d053-4f34-a11b-8a8d63752847");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fefd4923-df71-4fbb-9c8a-2a08b476b808");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Review",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0818110f-79c0-4c5f-8137-735b47d0a969", "44f88fa0-17ef-4aca-b29a-bf049f2514fe", "Customer", "customer" },
                    { "d9a6088f-539e-4466-9494-2f81a13eae99", "24f1c26e-76b3-4beb-b7e3-7e6ce831c404", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Customers_CustomerId",
                table: "Review",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Customers_CustomerId",
                table: "Review");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0818110f-79c0-4c5f-8137-735b47d0a969");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9a6088f-539e-4466-9494-2f81a13eae99");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "83f30b7f-d053-4f34-a11b-8a8d63752847", "b32ae175-6b91-4680-9674-30075525bf83", "Admin", "ADMIN" },
                    { "fefd4923-df71-4fbb-9c8a-2a08b476b808", "f847c305-0ba5-4c31-85a4-b690697e2830", "Customer", "customer" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Customers_CustomerId",
                table: "Review",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
