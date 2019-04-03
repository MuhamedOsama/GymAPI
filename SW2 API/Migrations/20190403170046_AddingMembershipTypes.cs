using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sw2API.Migrations
{
    public partial class AddingMembershipTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MembershipTypeId",
                table: "Customers",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "MembershipTypes",
                columns: table => new
                {
                    MembershipTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    SignUpFee = table.Column<int>(nullable: false),
                    DurationInMonths = table.Column<byte>(nullable: false),
                    SaunaAccess = table.Column<bool>(nullable: false),
                    CrossFitAccess = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipTypes", x => x.MembershipTypeId);
                });

            migrationBuilder.InsertData(
                table: "MembershipTypes",
                columns: new[] { "MembershipTypeId", "CrossFitAccess", "DurationInMonths", "Name", "SaunaAccess", "SignUpFee" },
                values: new object[,]
                {
                    { 1, false, (byte)1, "1 Month", false, 200 },
                    { 2, false, (byte)3, "3 Month with 10% Discount", false, 540 },
                    { 3, true, (byte)6, "6 Month with 20% Discount and CrossFit", false, 640 },
                    { 4, true, (byte)12, "1 Year with 30% Discount, CrossFit and Sauna", true, 840 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_MembershipTypeId",
                table: "Customers",
                column: "MembershipTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_MembershipTypes_MembershipTypeId",
                table: "Customers",
                column: "MembershipTypeId",
                principalTable: "MembershipTypes",
                principalColumn: "MembershipTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_MembershipTypes_MembershipTypeId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "MembershipTypes");

            migrationBuilder.DropIndex(
                name: "IX_Customers_MembershipTypeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MembershipTypeId",
                table: "Customers");
        }
    }
}
