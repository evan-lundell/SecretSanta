using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SecretSanta.Migrations
{
    public partial class GiftPairRework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Giver");

            migrationBuilder.DropTable(
                name: "Recipient");

            migrationBuilder.CreateIndex(
                name: "IX_GiftPair_GiverId",
                table: "GiftPair",
                column: "GiverId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftPair_RecipientId",
                table: "GiftPair",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftPair_Person_GiverId",
                table: "GiftPair",
                column: "GiverId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftPair_Person_RecipientId",
                table: "GiftPair",
                column: "RecipientId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftPair_Person_GiverId",
                table: "GiftPair");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftPair_Person_RecipientId",
                table: "GiftPair");

            migrationBuilder.DropIndex(
                name: "IX_GiftPair_GiverId",
                table: "GiftPair");

            migrationBuilder.DropIndex(
                name: "IX_GiftPair_RecipientId",
                table: "GiftPair");

            migrationBuilder.CreateTable(
                name: "Giver",
                columns: table => new
                {
                    GiftPairId = table.Column<int>(nullable: false),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giver", x => new { x.GiftPairId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_Giver_GiftPair_GiftPairId",
                        column: x => x.GiftPairId,
                        principalTable: "GiftPair",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Giver_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipient",
                columns: table => new
                {
                    GiftPairId = table.Column<int>(nullable: false),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipient", x => new { x.GiftPairId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_Recipient_GiftPair_GiftPairId",
                        column: x => x.GiftPairId,
                        principalTable: "GiftPair",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipient_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Giver_GiftPairId",
                table: "Giver",
                column: "GiftPairId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Giver_PersonId",
                table: "Giver",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_GiftPairId",
                table: "Recipient",
                column: "GiftPairId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_PersonId",
                table: "Recipient",
                column: "PersonId");
        }
    }
}
