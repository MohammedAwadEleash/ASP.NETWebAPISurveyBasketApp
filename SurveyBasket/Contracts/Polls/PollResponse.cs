namespace SurveyBasket.Contracts.Polls
{
    public record PollResponse
    (
        int id,
         string Title,
         string Summary,
         DateOnly StartsAt,
         DateOnly EndsAt

    );
}
