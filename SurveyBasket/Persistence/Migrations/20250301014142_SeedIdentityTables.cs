using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01954f57-5085-7246-9551-8e731e692951", "01954f57-5085-7246-9551-8e773045a01d", false, false, "Admin", "ADMIN" },
                    { "01954f58-6c58-7b84-912a-9212ed5a42c3", "01954f58-a423-79c6-aceb-50377fb9e627", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "01954f55-a4b1-738b-ac61-e881cbb746b6", 0, "01954f55-a4b1-738b-ac61-e8847c075173", "admin@survey-basket.com", true, "Mohammed", "Awad", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEPTa94yZt2/5ae836KVaDVUotkpSpSl+qwIIk/ic+Jnx81DMJ8wlwTmi+NOJgmyqww==", null, false, "55BF92C9EF0249CDA210D85D1A851BC9", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "polls:read", "01954f57-5085-7246-9551-8e731e692951" },
                    { 2, "permissions", "polls:add", "01954f57-5085-7246-9551-8e731e692951" },
                    { 3, "permissions", "polls:update", "01954f57-5085-7246-9551-8e731e692951" },
                    { 4, "permissions", "polls:delete", "01954f57-5085-7246-9551-8e731e692951" },
                    { 5, "permissions", "questions:read", "01954f57-5085-7246-9551-8e731e692951" },
                    { 6, "permissions", "questions:add", "01954f57-5085-7246-9551-8e731e692951" },
                    { 7, "permissions", "questions:update", "01954f57-5085-7246-9551-8e731e692951" },
                    { 8, "permissions", "users:read", "01954f57-5085-7246-9551-8e731e692951" },
                    { 9, "permissions", "users:add", "01954f57-5085-7246-9551-8e731e692951" },
                    { 10, "permissions", "users:update", "01954f57-5085-7246-9551-8e731e692951" },
                    { 11, "permissions", "roles:read", "01954f57-5085-7246-9551-8e731e692951" },
                    { 12, "permissions", "roles:add", "01954f57-5085-7246-9551-8e731e692951" },
                    { 13, "permissions", "roles:update", "01954f57-5085-7246-9551-8e731e692951" },
                    { 14, "permissions", "results:read", "01954f57-5085-7246-9551-8e731e692951" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "01954f57-5085-7246-9551-8e731e692951", "01954f55-a4b1-738b-ac61-e881cbb746b6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01954f58-6c58-7b84-912a-9212ed5a42c3");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "01954f57-5085-7246-9551-8e731e692951", "01954f55-a4b1-738b-ac61-e881cbb746b6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01954f57-5085-7246-9551-8e731e692951");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01954f55-a4b1-738b-ac61-e881cbb746b6");
        }
    }
}
