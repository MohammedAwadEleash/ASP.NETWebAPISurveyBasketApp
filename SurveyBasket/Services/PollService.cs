
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace SurveyBasket.Services
{

    public class PollService(ApplicationDbContext context) : IPollService
    {

        private readonly ApplicationDbContext _context = context;
        private static readonly List<Poll> _polls = [
            new Poll
            {
            Id=1,
            Title= "Poll 1",
            Summary="My First Pool"
            }
            ];



        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var polls = await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);

          var pollResponse = polls.Adapt<IEnumerable<PollResponse>>();
            return Result.Success(pollResponse);


        }
       public  async  Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)

        {
           var poll=  await _context.Polls.FindAsync(id, cancellationToken);
            if (poll is null)
                return Result.Failure<PollResponse>(PollErrors.PollNotFound);


            return Result.Success<PollResponse>(poll.Adapt<PollResponse>());
        }
        public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
        {
            var poll = request.Adapt<Poll>();


            await _context.Polls.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(poll.Adapt<PollResponse>());
        }

        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default)
        {
            var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);

            if (currentPoll is null)
                return Result.Failure(PollErrors.PollNotFound);

            currentPoll.Title = poll.Title;
            currentPoll.Summary = poll.Summary;
            currentPoll.StartsAt = poll.StartsAt;
            currentPoll.EndsAt = poll.EndsAt;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
        public async Task<Result> DeleteAsync(int id,CancellationToken cancellationToken=default)
        
            {
              var poll = await _context.Polls.FindAsync(id, cancellationToken);

                if (poll is null)
                    return Result.Failure(PollErrors.PollNotFound);

            _context.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
        public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken )
        {

            var poll = await _context.Polls.FindAsync(id, cancellationToken);

            if (poll is null)
                return Result.Failure(PollErrors.PollNotFound);

            poll.IsPublished = !poll.IsPublished;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }



    }
}
