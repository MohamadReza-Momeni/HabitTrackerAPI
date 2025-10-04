using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "Priority",
            //    table: "Tasks",
            //    type: "INTEGER",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");
            migrationBuilder.Sql("ALTER TABLE \"Tasks\" ALTER COLUMN \"Priority\" TYPE integer USING \"Priority\"::integer;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
