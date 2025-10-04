using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Task_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ✅ UpdatedAt: TEXT → timestamp with time zone
            migrationBuilder.Sql(
                "ALTER TABLE \"Tasks\" ALTER COLUMN \"UpdatedAt\" TYPE timestamp with time zone USING \"UpdatedAt\"::timestamp with time zone;");

            // ✅ Title: TEXT → varchar(200)
            migrationBuilder.Sql(
                "ALTER TABLE \"Tasks\" ALTER COLUMN \"Title\" TYPE character varying(200);");

            // ✅ Priority: TEXT → integer (or integer → integer just to be safe)
            migrationBuilder.Sql(
                "ALTER TABLE \"Tasks\" ALTER COLUMN \"Priority\" TYPE integer USING \"Priority\"::integer;");

            // ✅ IsCompleted: INTEGER → boolean
            // Assuming old 0/1 values stored as integers
            migrationBuilder.Sql(
                "ALTER TABLE \"Tasks\" ALTER COLUMN \"IsCompleted\" TYPE boolean USING (\"IsCompleted\" = 1);");

            // ✅ DueDate: TEXT → timestamp with time zone (nullable)
            migrationBuilder.Sql(
                "ALTER TABLE \"Tasks\" ALTER COLUMN \"DueDate\" TYPE timestamp with time zone USING NULLIF(\"DueDate\", '')::timestamp with time zone;");

            // ✅ Description: TEXT → varchar(1000)
            migrationBuilder.Sql(
                "ALTER TABLE \"Tasks\" ALTER COLUMN \"Description\" TYPE character varying(1000);");

            // ✅ CreatedAt: TEXT → timestamp with time zone
            migrationBuilder.Sql(
                "ALTER TABLE \"Tasks\" ALTER COLUMN \"CreatedAt\" TYPE timestamp with time zone USING \"CreatedAt\"::timestamp with time zone;");

            // ✅ Id: INTEGER → integer identity (just reapply annotation if needed)
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Tasks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UpdatedAt",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tasks",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "IsCompleted",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DueDate",
                table: "Tasks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "TEXT",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
