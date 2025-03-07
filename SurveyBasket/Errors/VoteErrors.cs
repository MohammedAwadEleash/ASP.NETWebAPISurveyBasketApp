namespace SurveyBasket.Errors
{
    public static class VoteErrors
    {

        public static readonly Error InvalidQuestions = new Error("Vote.InvalidQuestions ", "Invalid questions", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedVote = new Error("Vote.DuplicatedVote", "This user has already voted in this poll before", StatusCodes.Status409Conflict);
    }
}

