using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameVoteAnswersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voteAnswers_Answers_AnswerId",
                table: "voteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_voteAnswers_Questions_QuestionId",
                table: "voteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_voteAnswers_Votes_VoteId",
                table: "voteAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_voteAnswers",
                table: "voteAnswers");

            migrationBuilder.RenameTable(
                name: "voteAnswers",
                newName: "VoteAnswers");

            migrationBuilder.RenameIndex(
                name: "IX_voteAnswers_VoteId_QuestionId",
                table: "VoteAnswers",
                newName: "IX_VoteAnswers_VoteId_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_voteAnswers_QuestionId",
                table: "VoteAnswers",
                newName: "IX_VoteAnswers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_voteAnswers_AnswerId",
                table: "VoteAnswers",
                newName: "IX_VoteAnswers_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoteAnswers",
                table: "VoteAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswers_Answers_AnswerId",
                table: "VoteAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswers_Questions_QuestionId",
                table: "VoteAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VoteAnswers_Votes_VoteId",
                table: "VoteAnswers",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswers_Answers_AnswerId",
                table: "VoteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswers_Questions_QuestionId",
                table: "VoteAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteAnswers_Votes_VoteId",
                table: "VoteAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoteAnswers",
                table: "VoteAnswers");

            migrationBuilder.RenameTable(
                name: "VoteAnswers",
                newName: "voteAnswers");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswers_VoteId_QuestionId",
                table: "voteAnswers",
                newName: "IX_voteAnswers_VoteId_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswers_QuestionId",
                table: "voteAnswers",
                newName: "IX_voteAnswers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_VoteAnswers_AnswerId",
                table: "voteAnswers",
                newName: "IX_voteAnswers_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_voteAnswers",
                table: "voteAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_voteAnswers_Answers_AnswerId",
                table: "voteAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_voteAnswers_Questions_QuestionId",
                table: "voteAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_voteAnswers_Votes_VoteId",
                table: "voteAnswers",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
