using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechaSoft.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MechaSoftCS");

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    CitizenCard = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Parish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Municipality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Complement = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Specialties = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HourlyRateAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HourlyRateCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "EUR"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CanPerformInspections = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    InspectionLicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Part",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    MinStockLevel = table.Column<int>(type: "int", nullable: false),
                    UnitCostAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitCostCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    SalePriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalePriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SupplierContact = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Part", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EstimatedHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PricePerHourAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerHourCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    FixedPriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FixedPriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "EUR"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RequiresInspection = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: true),
                    ChassisNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EngineNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    EngineDisplacement = table.Column<int>(type: "int", nullable: true),
                    EnginePower = table.Column<int>(type: "int", nullable: true),
                    InspectionExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicle_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceOrder",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EstimatedCostAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedCostCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    FinalCostAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalCostCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "EUR"),
                    EstimatedDelivery = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDelivery = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MechanicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActualHours = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    RequiresInspection = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceOrder_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceOrder_Employee_MechanicId",
                        column: x => x.MechanicId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceOrder_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inspection",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CostAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    CertificateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InspectionCenter = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VehicleMileage = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspection_ServiceOrder_ServiceOrderId",
                        column: x => x.ServiceOrderId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "ServiceOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspection_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartItem",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    ServiceOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    TotalPriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "EUR")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartItem", x => new { x.ServiceOrderId, x.PartId });
                    table.ForeignKey(
                        name: "FK_PartItem_Part_PartId",
                        column: x => x.PartId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Part",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartItem_ServiceOrder_ServiceOrderId",
                        column: x => x.ServiceOrderId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "ServiceOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceItem",
                schema: "MechaSoftCS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    EstimatedHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ActualHours = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    UnitPriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    TotalPriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "EUR"),
                    Status = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MechanicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceItem_Employee_MechanicId",
                        column: x => x.MechanicId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceItem_ServiceOrder_ServiceOrderId",
                        column: x => x.ServiceOrderId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "ServiceOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceItem_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalSchema: "MechaSoftCS",
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Email",
                schema: "MechaSoftCS",
                table: "Customer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Nif",
                schema: "MechaSoftCS",
                table: "Customer",
                column: "Nif",
                unique: true,
                filter: "[Nif] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Phone",
                schema: "MechaSoftCS",
                table: "Customer",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Email",
                schema: "MechaSoftCS",
                table: "Employee",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_InspectionLicenseNumber",
                schema: "MechaSoftCS",
                table: "Employee",
                column: "InspectionLicenseNumber",
                unique: true,
                filter: "[InspectionLicenseNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_IsActive",
                schema: "MechaSoftCS",
                table: "Employee",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Role",
                schema: "MechaSoftCS",
                table: "Employee",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_CertificateNumber",
                schema: "MechaSoftCS",
                table: "Inspection",
                column: "CertificateNumber",
                unique: true,
                filter: "[CertificateNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Expiry_Result",
                schema: "MechaSoftCS",
                table: "Inspection",
                columns: new[] { "ExpiryDate", "Result" },
                filter: "[Result] = 'Approved'");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_ExpiryDate",
                schema: "MechaSoftCS",
                table: "Inspection",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_InspectionCenter",
                schema: "MechaSoftCS",
                table: "Inspection",
                column: "InspectionCenter");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_InspectionDate",
                schema: "MechaSoftCS",
                table: "Inspection",
                column: "InspectionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Result",
                schema: "MechaSoftCS",
                table: "Inspection",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Result_Expiry",
                schema: "MechaSoftCS",
                table: "Inspection",
                columns: new[] { "Result", "ExpiryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_ServiceOrderId",
                schema: "MechaSoftCS",
                table: "Inspection",
                column: "ServiceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Type",
                schema: "MechaSoftCS",
                table: "Inspection",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Vehicle_Date",
                schema: "MechaSoftCS",
                table: "Inspection",
                columns: new[] { "VehicleId", "InspectionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Vehicle_Type",
                schema: "MechaSoftCS",
                table: "Inspection",
                columns: new[] { "VehicleId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_Vehicle_Type_Date",
                schema: "MechaSoftCS",
                table: "Inspection",
                columns: new[] { "VehicleId", "Type", "InspectionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_VehicleId",
                schema: "MechaSoftCS",
                table: "Inspection",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Part_Brand",
                schema: "MechaSoftCS",
                table: "Part",
                column: "Brand",
                filter: "[Brand] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Part_Category",
                schema: "MechaSoftCS",
                table: "Part",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Part_Category_IsActive",
                schema: "MechaSoftCS",
                table: "Part",
                columns: new[] { "Category", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Part_Code",
                schema: "MechaSoftCS",
                table: "Part",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Part_IsActive",
                schema: "MechaSoftCS",
                table: "Part",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Part_Name",
                schema: "MechaSoftCS",
                table: "Part",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Part_Stock_Levels",
                schema: "MechaSoftCS",
                table: "Part",
                columns: new[] { "StockQuantity", "MinStockLevel" });

            migrationBuilder.CreateIndex(
                name: "IX_Part_SupplierName",
                schema: "MechaSoftCS",
                table: "Part",
                column: "SupplierName",
                filter: "[SupplierName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PartItem_PartId",
                schema: "MechaSoftCS",
                table: "PartItem",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_PartItem_ServiceOrderId",
                schema: "MechaSoftCS",
                table: "PartItem",
                column: "ServiceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_Category",
                schema: "MechaSoftCS",
                table: "Service",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Service_Category_IsActive",
                schema: "MechaSoftCS",
                table: "Service",
                columns: new[] { "Category", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Service_IsActive",
                schema: "MechaSoftCS",
                table: "Service",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Service_Name",
                schema: "MechaSoftCS",
                table: "Service",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItem_CompletedAt",
                schema: "MechaSoftCS",
                table: "ServiceItem",
                column: "CompletedAt",
                filter: "[CompletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItem_Mechanic_Status",
                schema: "MechaSoftCS",
                table: "ServiceItem",
                columns: new[] { "MechanicId", "Status" },
                filter: "[MechanicId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItem_MechanicId",
                schema: "MechaSoftCS",
                table: "ServiceItem",
                column: "MechanicId",
                filter: "[MechanicId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItem_ServiceId",
                schema: "MechaSoftCS",
                table: "ServiceItem",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItem_ServiceOrder_Status",
                schema: "MechaSoftCS",
                table: "ServiceItem",
                columns: new[] { "ServiceOrderId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItem_ServiceOrderId",
                schema: "MechaSoftCS",
                table: "ServiceItem",
                column: "ServiceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItem_StartedAt",
                schema: "MechaSoftCS",
                table: "ServiceItem",
                column: "StartedAt",
                filter: "[StartedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceItem_Status",
                schema: "MechaSoftCS",
                table: "ServiceItem",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_ActualDelivery",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "ActualDelivery",
                filter: "[ActualDelivery] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_CreatedAt",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_CustomerId",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_EstimatedDelivery",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "EstimatedDelivery",
                filter: "[EstimatedDelivery] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_MechanicId",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "MechanicId",
                filter: "[MechanicId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_OrderNumber",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_Priority",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_RequiresInspection",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "RequiresInspection");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_Status",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_Status_Priority",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                columns: new[] { "Status", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_VehicleId",
                schema: "MechaSoftCS",
                table: "ServiceOrder",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Brand_Model",
                schema: "MechaSoftCS",
                table: "Vehicle",
                columns: new[] { "Brand", "Model" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_ChassisNumber",
                schema: "MechaSoftCS",
                table: "Vehicle",
                column: "ChassisNumber",
                unique: true,
                filter: "[ChassisNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CustomerId",
                schema: "MechaSoftCS",
                table: "Vehicle",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_FuelType",
                schema: "MechaSoftCS",
                table: "Vehicle",
                column: "FuelType");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_InspectionExpiryDate",
                schema: "MechaSoftCS",
                table: "Vehicle",
                column: "InspectionExpiryDate",
                filter: "[InspectionExpiryDate] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_LicensePlate",
                schema: "MechaSoftCS",
                table: "Vehicle",
                column: "LicensePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Year",
                schema: "MechaSoftCS",
                table: "Vehicle",
                column: "Year");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inspection",
                schema: "MechaSoftCS");

            migrationBuilder.DropTable(
                name: "PartItem",
                schema: "MechaSoftCS");

            migrationBuilder.DropTable(
                name: "ServiceItem",
                schema: "MechaSoftCS");

            migrationBuilder.DropTable(
                name: "Part",
                schema: "MechaSoftCS");

            migrationBuilder.DropTable(
                name: "ServiceOrder",
                schema: "MechaSoftCS");

            migrationBuilder.DropTable(
                name: "Service",
                schema: "MechaSoftCS");

            migrationBuilder.DropTable(
                name: "Employee",
                schema: "MechaSoftCS");

            migrationBuilder.DropTable(
                name: "Vehicle",
                schema: "MechaSoftCS");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "MechaSoftCS");
        }
    }
}
