using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechaSoft.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "MechaSoftCS",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LockedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CK_Users_CustomerOrEmployee", "([CustomerId] IS NULL AND [EmployeeId] IS NOT NULL) OR ([CustomerId] IS NOT NULL AND [EmployeeId] IS NULL) OR ([CustomerId] IS NULL AND [EmployeeId] IS NULL)");
                    table.ForeignKey(
                        name: "FK_Users_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomerId",
                schema: "MechaSoftCS",
                table: "Users",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "MechaSoftCS",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                schema: "MechaSoftCS",
                table: "Users",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RefreshToken",
                schema: "MechaSoftCS",
                table: "Users",
                column: "RefreshToken",
                unique: true,
                filter: "[RefreshToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "MechaSoftCS",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "MechaSoftCS");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "MechaSoftCS",
                table: "Customer");
        }
    }
}
