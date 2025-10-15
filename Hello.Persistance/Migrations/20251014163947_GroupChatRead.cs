using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hello.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GroupChatRead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReadAt",
                table: "Chats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupsChatsRead",
                columns: table => new
                {
                    GroupChatId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsChatsRead", x => new { x.MemberId, x.GroupChatId });
                    table.ForeignKey(
                        name: "FK_GroupsChatsRead_Accounts_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupsChatsRead_GroupChats_GroupChatId",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupsChatsRead_GroupChatId",
                table: "GroupsChatsRead",
                column: "GroupChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupsChatsRead");

            migrationBuilder.DropColumn(
                name: "ReadAt",
                table: "Chats");
        }
    }
}
