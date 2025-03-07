namespace SurveyBasket.Contracts.Votes
{
    public class VoteRequestValidator : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidator()
        {

            RuleFor(v => v.Answers).NotEmpty();

            RuleForEach(v => v.Answers)
          .SetInheritanceValidator(va => va.Add(new VoteAnswerRequestValidator()));
        }
    }
}
