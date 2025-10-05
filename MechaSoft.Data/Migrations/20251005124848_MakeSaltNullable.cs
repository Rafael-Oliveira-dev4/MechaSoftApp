using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechaSoft.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeSaltNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, make the column nullable
            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                schema: "MechaSoftCS",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            // Then, convert empty strings to NULL for BCrypt users
            migrationBuilder.Sql(
                "UPDATE [MechaSoftCS].[Users] SET [Salt] = NULL WHERE [Salt] = ''",
                suppressTransaction: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                schema: "MechaSoftCS",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
