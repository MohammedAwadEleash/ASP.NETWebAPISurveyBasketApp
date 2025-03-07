namespace SurveyBasket.Contracts.Authentication
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>


    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(r => r.Email)
                       .NotEmpty()
                       .EmailAddress();

            RuleFor(r => r.Code)
           .NotEmpty();


            RuleFor(r => r.NewPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");


        }


    }
}
