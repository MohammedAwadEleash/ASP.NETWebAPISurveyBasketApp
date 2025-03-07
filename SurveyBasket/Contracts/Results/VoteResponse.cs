namespace SurveyBasket.Contracts.Results
{
    public record VoteResponse(string voterName, DateTime VoteDate,
       IEnumerable<QuestionAnswerResponse> SelectedAnswers
        )
    {
    }
}
