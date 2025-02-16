namespace SurveyBasket.Contracts.Polls
{
    public class LoginRequestValidator : AbstractValidator<PollRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Title)
                 .NotEmpty()
                 .Length(3, 100);

            RuleFor(x => x.Summary)
             .NotEmpty()
             .Length(3, 1500);


            RuleFor(P => P.StartsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(P => P.EndsAt).NotEmpty();

            RuleFor(p => p).Must(HasValidDate).WithName(nameof(PollRequest.EndsAt))
                .WithMessage("{PropertyName} should be greater than or equals Start Date");

        }
        private bool HasValidDate(PollRequest pollRequest)
        {

            return pollRequest.EndsAt >= pollRequest.StartsAt;
        }
    }
}
