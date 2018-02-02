using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GLAA.Domain.Migrations
{
    public partial class initial_create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licence_Multiple_MultipleId",
                table: "Licence");

            migrationBuilder.DropForeignKey(
                name: "FK_LicenceStatus_LicenceStatus_LicenceStatusId",
                table: "LicenceStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_PreviousTradingName_PrincipalAuthority_PrincipalAuthorityId",
                table: "PreviousTradingName");

            migrationBuilder.DropForeignKey(
                name: "FK_PrincipalAuthority_DirectorOrPartner_DirectoryOrPartnerId",
                table: "PrincipalAuthority");

            migrationBuilder.DropIndex(
                name: "IX_LicenceStatus_LicenceStatusId",
                table: "LicenceStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LicenceSector",
                table: "LicenceSector");

            migrationBuilder.DropIndex(
                name: "IX_LicenceSector_LicenceId",
                table: "LicenceSector");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LicenceMultiple",
                table: "LicenceMultiple");

            migrationBuilder.DropIndex(
                name: "IX_LicenceMultiple_LicenceId",
                table: "LicenceMultiple");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LicenceIndustry",
                table: "LicenceIndustry");

            migrationBuilder.DropIndex(
                name: "IX_LicenceIndustry_LicenceId",
                table: "LicenceIndustry");

            migrationBuilder.DropIndex(
                name: "IX_Licence_MultipleId",
                table: "Licence");

            migrationBuilder.DropColumn(
                name: "HasPreviousTradingNames",
                table: "PrincipalAuthority");

            migrationBuilder.DropColumn(
                name: "LicenceStatusId",
                table: "LicenceStatus");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LicenceSector");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LicenceMultiple");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LicenceIndustry");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LicenceCountry");

            migrationBuilder.DropColumn(
                name: "MultipleId",
                table: "Licence");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "DirectoryOrPartnerId",
                table: "PrincipalAuthority",
                newName: "DirectorOrPartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PrincipalAuthority_DirectoryOrPartnerId",
                table: "PrincipalAuthority",
                newName: "IX_PrincipalAuthority_DirectorOrPartnerId");

            migrationBuilder.RenameColumn(
                name: "PrincipalAuthorityId",
                table: "PreviousTradingName",
                newName: "LicenceId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousTradingName_PrincipalAuthorityId",
                table: "PreviousTradingName",
                newName: "IX_PreviousTradingName_LicenceId");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "dbo",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminCategory",
                table: "LicenceStatus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CssClassStem",
                table: "LicenceStatus",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasPreviousTradingName",
                table: "Licence",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasTradingName",
                table: "Licence",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LicenceSector",
                table: "LicenceSector",
                columns: new[] { "LicenceId", "SectorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LicenceMultiple",
                table: "LicenceMultiple",
                columns: new[] { "LicenceId", "MultipleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LicenceIndustry",
                table: "LicenceIndustry",
                columns: new[] { "LicenceId", "IndustryId" });

            migrationBuilder.CreateTable(
                name: "LicenceStatusNextStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NextStatusId = table.Column<int>(nullable: false),
                    NextStatusId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceStatusNextStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenceStatusNextStatus_LicenceStatus_NextStatusId",
                        column: x => x.NextStatusId,
                        principalTable: "LicenceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenceStatusNextStatus_LicenceStatus_NextStatusId1",
                        column: x => x.NextStatusId1,
                        principalTable: "LicenceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatusNextStatus_NextStatusId",
                table: "LicenceStatusNextStatus",
                column: "NextStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatusNextStatus_NextStatusId1",
                table: "LicenceStatusNextStatus",
                column: "NextStatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousTradingName_Licence_LicenceId",
                table: "PreviousTradingName",
                column: "LicenceId",
                principalTable: "Licence",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrincipalAuthority_DirectorOrPartner_DirectorOrPartnerId",
                table: "PrincipalAuthority",
                column: "DirectorOrPartnerId",
                principalTable: "DirectorOrPartner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreviousTradingName_Licence_LicenceId",
                table: "PreviousTradingName");

            migrationBuilder.DropForeignKey(
                name: "FK_PrincipalAuthority_DirectorOrPartner_DirectorOrPartnerId",
                table: "PrincipalAuthority");

            migrationBuilder.DropTable(
                name: "LicenceStatusNextStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LicenceSector",
                table: "LicenceSector");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LicenceMultiple",
                table: "LicenceMultiple");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LicenceIndustry",
                table: "LicenceIndustry");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "dbo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdminCategory",
                table: "LicenceStatus");

            migrationBuilder.DropColumn(
                name: "CssClassStem",
                table: "LicenceStatus");

            migrationBuilder.DropColumn(
                name: "HasPreviousTradingName",
                table: "Licence");

            migrationBuilder.DropColumn(
                name: "HasTradingName",
                table: "Licence");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "dbo",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "DirectorOrPartnerId",
                table: "PrincipalAuthority",
                newName: "DirectoryOrPartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PrincipalAuthority_DirectorOrPartnerId",
                table: "PrincipalAuthority",
                newName: "IX_PrincipalAuthority_DirectoryOrPartnerId");

            migrationBuilder.RenameColumn(
                name: "LicenceId",
                table: "PreviousTradingName",
                newName: "PrincipalAuthorityId");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousTradingName_LicenceId",
                table: "PreviousTradingName",
                newName: "IX_PreviousTradingName_PrincipalAuthorityId");

            migrationBuilder.AddColumn<bool>(
                name: "HasPreviousTradingNames",
                table: "PrincipalAuthority",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LicenceStatusId",
                table: "LicenceStatus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LicenceSector",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LicenceMultiple",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LicenceIndustry",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LicenceCountry",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MultipleId",
                table: "Licence",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LicenceSector",
                table: "LicenceSector",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LicenceMultiple",
                table: "LicenceMultiple",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LicenceIndustry",
                table: "LicenceIndustry",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceStatus_LicenceStatusId",
                table: "LicenceStatus",
                column: "LicenceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceSector_LicenceId",
                table: "LicenceSector",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceMultiple_LicenceId",
                table: "LicenceMultiple",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceIndustry_LicenceId",
                table: "LicenceIndustry",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Licence_MultipleId",
                table: "Licence",
                column: "MultipleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Licence_Multiple_MultipleId",
                table: "Licence",
                column: "MultipleId",
                principalTable: "Multiple",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LicenceStatus_LicenceStatus_LicenceStatusId",
                table: "LicenceStatus",
                column: "LicenceStatusId",
                principalTable: "LicenceStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousTradingName_PrincipalAuthority_PrincipalAuthorityId",
                table: "PreviousTradingName",
                column: "PrincipalAuthorityId",
                principalTable: "PrincipalAuthority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrincipalAuthority_DirectorOrPartner_DirectoryOrPartnerId",
                table: "PrincipalAuthority",
                column: "DirectoryOrPartnerId",
                principalTable: "DirectorOrPartner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
