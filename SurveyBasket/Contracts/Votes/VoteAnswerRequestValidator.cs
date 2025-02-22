namespace SurveyBasket.Contracts.Votes
{
    public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
    {
        public VoteAnswerRequestValidator()
        {

            RuleFor(v => v.QuestionId).GreaterThanOrEqualTo(1);
            RuleFor(v => v.AnswerId).GreaterThanOrEqualTo(1);

        }
    }
}
