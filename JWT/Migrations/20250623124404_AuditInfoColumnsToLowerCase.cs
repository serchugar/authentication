using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWT.Migrations
{
    /// <inheritdoc />
    public partial class AuditInfoColumnsToLowerCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuditInfo_UpdatedAt",
                schema: "authentication",
                table: "users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "AuditInfo_CreatedAt",
                schema: "authentication",
                table: "users",
                newName: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "authentication",
                table: "users",
                newName: "AuditInfo_UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "authentication",
                table: "users",
                newName: "AuditInfo_CreatedAt");
        }
    }
}
