using FluentValidation.AspNetCore;

namespace SurveyBasket.Contracts.Questions
{
    public class QuestionRequestValidator: AbstractValidator<QuestionRequest>
    {

        public QuestionRequestValidator()
        {

            RuleFor(q => q.Content).NotEmpty().Length(3, 1000);


            RuleFor(q => q.Answers)
                .NotNull();

            RuleFor(q => q.Answers)
                .Must(a => a.Count > 1)
                .WithMessage("Question should has at least 2 answers")
                .When(q => q.Answers != null);

            RuleFor(q => q.Answers)
                .Must(a => a.Distinct().Count() == a.Count)
                .WithMessage("You cannot add dublicated answers for the same question")
                      .When(q => q.Answers != null);



        }
    }
}
