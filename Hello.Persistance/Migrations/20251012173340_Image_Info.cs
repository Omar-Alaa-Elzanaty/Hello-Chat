using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hello.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Image_Info : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "GroupChats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "GroupChats");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Chats");
        }
    }
}
