namespace SurveyBasket.Contracts.Authentication
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(l => l.Email).NotEmpty().EmailAddress();


            RuleFor(l => l.Password).NotEmpty();
        }
        
    }
}
