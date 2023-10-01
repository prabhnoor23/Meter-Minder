using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pspcl.DBConnect.Migrations
{
    /// <inheritdoc />
    public partial class inittables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Make",
                table: "StockIssueBook");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "MaterialType");

            migrationBuilder.AddColumn<string>(
                name: "Make",
                table: "StockBookMaterial",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TestingCharges",
                table: "Material",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationCode",
                table: "Division",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RatingMaterialTypeMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialTypeId = table.Column<int>(type: "int", nullable: false),
                    RatingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingMaterialTypeMapping", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "RatingMaterialTypeMapping");

            migrationBuilder.DropColumn(
                name: "Make",
                table: "StockBookMaterial");

            migrationBuilder.DropColumn(
                name: "TestingCharges",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "LocationCode",
                table: "Division");

            migrationBuilder.AddColumn<string>(
                name: "Make",
                table: "StockIssueBook",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "MaterialType",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
