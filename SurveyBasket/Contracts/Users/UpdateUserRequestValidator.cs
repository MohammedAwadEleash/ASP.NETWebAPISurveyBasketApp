namespace SurveyBasket.Contracts.Users
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Email)
         .NotEmpty()
         .EmailAddress();

            RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .Length(3, 100);

            RuleFor(x => x.LastName)
                    .NotEmpty()
                    .Length(3, 100);

            RuleFor(u => u.Roles)
                    .NotEmpty()
                    .NotNull();

            RuleFor(u => u.Roles)
                    .Must(r => r.Distinct().Count() == r.Count)
                .WithMessage("You cannot add duplicated roles for the same user")
                    .When(u => u.Roles != null);
        }
    }
}
