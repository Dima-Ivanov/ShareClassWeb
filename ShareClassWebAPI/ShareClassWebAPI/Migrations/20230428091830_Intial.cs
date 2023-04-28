using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareClassWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassRoom",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvitationCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teacher_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Students_Count = table.Column<int>(type: "int", nullable: false),
                    Creation_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Administrator_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRoom", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReactionType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password_Hash = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HomeTask",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creation_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClassRoom_ID = table.Column<int>(type: "int", nullable: false),
                    ClassRoomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeTask", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HomeTask_ClassRoom_ClassRoomID",
                        column: x => x.ClassRoomID,
                        principalTable: "ClassRoom",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassRoomsUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    ClassRoom_ID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    ClassRoomID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRoomsUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClassRoomsUsers_ClassRoom_ClassRoomID",
                        column: x => x.ClassRoomID,
                        principalTable: "ClassRoom",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassRoomsUsers_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeTaskFile",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    File_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeTask_ID = table.Column<int>(type: "int", nullable: false),
                    HomeTaskID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeTaskFile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HomeTaskFile_HomeTask_HomeTaskID",
                        column: x => x.HomeTaskID,
                        principalTable: "HomeTask",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Solution",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Solution_Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeTask_ID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    HomeTaskID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solution", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Solution_HomeTask_HomeTaskID",
                        column: x => x.HomeTaskID,
                        principalTable: "HomeTask",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reaction",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reaction_Type = table.Column<int>(type: "int", nullable: false),
                    Solution_ID = table.Column<int>(type: "int", nullable: false),
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    ReactionTypeID = table.Column<int>(type: "int", nullable: false),
                    SolutionID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reaction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reaction_ReactionType_ReactionTypeID",
                        column: x => x.ReactionTypeID,
                        principalTable: "ReactionType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reaction_Solution_SolutionID",
                        column: x => x.SolutionID,
                        principalTable: "Solution",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reaction_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolutionFile",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    File_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Solution_ID = table.Column<int>(type: "int", nullable: false),
                    SolutionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionFile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SolutionFile_Solution_SolutionID",
                        column: x => x.SolutionID,
                        principalTable: "Solution",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRoomsUsers_ClassRoomID",
                table: "ClassRoomsUsers",
                column: "ClassRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRoomsUsers_UserID",
                table: "ClassRoomsUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_HomeTask_ClassRoomID",
                table: "HomeTask",
                column: "ClassRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_HomeTaskFile_HomeTaskID",
                table: "HomeTaskFile",
                column: "HomeTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_Reaction_ReactionTypeID",
                table: "Reaction",
                column: "ReactionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Reaction_SolutionID",
                table: "Reaction",
                column: "SolutionID");

            migrationBuilder.CreateIndex(
                name: "IX_Reaction_UserID",
                table: "Reaction",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Solution_HomeTaskID",
                table: "Solution",
                column: "HomeTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_SolutionFile_SolutionID",
                table: "SolutionFile",
                column: "SolutionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassRoomsUsers");

            migrationBuilder.DropTable(
                name: "HomeTaskFile");

            migrationBuilder.DropTable(
                name: "Reaction");

            migrationBuilder.DropTable(
                name: "SolutionFile");

            migrationBuilder.DropTable(
                name: "ReactionType");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Solution");

            migrationBuilder.DropTable(
                name: "HomeTask");

            migrationBuilder.DropTable(
                name: "ClassRoom");
        }
    }
}
