namespace SurveyBasket.Contracts.Votes
{
    public sealed record VoteRequest(IEnumerable<VoteAnswerRequest> Answers);


}
