using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PractiseTest1.Migrations
{
    /// <inheritdoc />
    public partial class newImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UploadImg",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadImg",
                table: "Books");
        }
    }
}
