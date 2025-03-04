using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addIsDisabledColumnToApplicationUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01954f55-a4b1-738b-ac61-e881cbb746b6",
                columns: new[] { "IsDisabled", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAELwGcml23aLkGLnACwYFOy8gUrq+2nUijOaZt7SPa5fREU1KB35hNXUP0LcDHbiFIw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01954f55-a4b1-738b-ac61-e881cbb746b6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPTa94yZt2/5ae836KVaDVUotkpSpSl+qwIIk/ic+Jnx81DMJ8wlwTmi+NOJgmyqww==");
        }
    }
}
