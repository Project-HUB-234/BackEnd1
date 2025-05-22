using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Hub.Migrations
{
    /// <inheritdoc />
    public partial class addbackgroundpicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ContactTitle = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ContactMessage = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ContactU__5C6625BB1B9DB066", x => x.ContactID);
                });

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PostCate__19093A2B0A775DD9", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__8AFACE1A1F8D427E", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ProfilePicture = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    BackgroundPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateJoined = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    PhoneNumber = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    QuickAccessQRCode = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCAC10BC468C", x => x.UserID);
                    table.ForeignKey(
                        name: "FK__Users__RoleId__3429BB53",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    LoginID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Login__4DDA2838E409ACDF", x => x.LoginID);
                    table.ForeignKey(
                        name: "FK__Login__UserId__37FA4C37",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "varchar(5000)", unicode: false, maxLength: 5000, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Posts__AA12603859E989D5", x => x.PostID);
                    table.ForeignKey(
                        name: "FK__Posts__CategoryI__3EA749C6",
                        column: x => x.CategoryId,
                        principalTable: "PostCategory",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Posts__UserID__3DB3258D",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "varchar(5000)", unicode: false, maxLength: 5000, nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comment__C3B4DFAADD23DEF4", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK__Comment__PostID__4277DAAA",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "PostID");
                    table.ForeignKey(
                        name: "FK__Comment__UserID__436BFEE3",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    LikeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    DateLiked = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PostLike__A2922CF4B53B50B0", x => x.LikeID);
                    table.ForeignKey(
                        name: "FK__PostLikes__PostI__473C8FC7",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "PostID");
                    table.ForeignKey(
                        name: "FK__PostLikes__UserI__4830B400",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentPath = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    PostID = table.Column<int>(type: "int", nullable: true),
                    CommentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Attachme__442C64DE85606C18", x => x.AttachmentID);
                    table.ForeignKey(
                        name: "FK__Attachmen__Comme__50C5FA01",
                        column: x => x.CommentID,
                        principalTable: "Comment",
                        principalColumn: "CommentID");
                    table.ForeignKey(
                        name: "FK__Attachmen__PostI__51BA1E3A",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "PostID");
                });

            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    LikeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    DateLiked = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CommentL__A2922CF4AAFDF86F", x => x.LikeID);
                    table.ForeignKey(
                        name: "FK__CommentLi__Comme__4C0144E4",
                        column: x => x.CommentID,
                        principalTable: "Comment",
                        principalColumn: "CommentID");
                    table.ForeignKey(
                        name: "FK__CommentLi__UserI__4CF5691D",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CommentID",
                table: "Attachments",
                column: "CommentID");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_PostID",
                table: "Attachments",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostID",
                table: "Comment",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserID",
                table: "Comment",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_CommentID",
                table: "CommentLikes",
                column: "CommentID");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_UserID",
                table: "CommentLikes",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Login_UserId",
                table: "Login",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Login__536C85E41BB14B9E",
                table: "Login",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_PostID",
                table: "PostLikes",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_UserID",
                table: "PostLikes",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserID",
                table: "Posts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D10534F89EB249",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
