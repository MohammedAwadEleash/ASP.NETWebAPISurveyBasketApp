namespace SurveyBasket.Contracts.Users
{
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {


            RuleFor(u => u.FirstName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(u => u.LastName)
                .NotEmpty()
                .Length(3, 100);
        }
    }
}



