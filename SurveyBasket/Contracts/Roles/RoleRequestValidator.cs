namespace SurveyBasket.Contracts.Roles
{
    public class  RoleRequestValidator : AbstractValidator<RoleRequest>
    {
        public RoleRequestValidator()
        {
            RuleFor(r => r.Name)
               .NotEmpty()
               .Length(3, 250);


            RuleFor(r => r.Permissions).NotNull().NotEmpty();



            RuleFor(role => role.Permissions)
                .Must(pr => pr.Distinct().Count() == pr.Count)
                .WithMessage("You cannot add duplicated permissions for the same role")
                .When(r => r.Permissions != null);

        }


    }
}
