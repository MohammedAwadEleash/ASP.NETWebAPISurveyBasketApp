
using Azure.Core;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace SurveyBasket.Services
{

    public class PollService(ApplicationDbContext context) : IPollService
    {

        private readonly ApplicationDbContext _context = context;
      


        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var pollResponse = await _context.Polls.AsNoTracking()
                .ProjectToType<PollResponse>()
                .ToListAsync(cancellationToken);



            return Result.Success<IEnumerable<PollResponse>>(pollResponse);


        }        
        
        // this function => gets only available Polls
        public async Task<Result<IEnumerable<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default)
        {
            var pollResponse = await _context.Polls.Where(p=>p.IsPublished && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && p.EndsAt>= DateOnly.FromDateTime(DateTime.UtcNow))
                .AsNoTracking()
              .ProjectToType<PollResponse>()
              .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<PollResponse>>(pollResponse);

        }

        public async  Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)

        {
           var poll=  await _context.Polls.FindAsync(id, cancellationToken);
            if (poll is null)
                return Result.Failure<PollResponse>(PollErrors.PollNotFound);


            return Result.Success<PollResponse>(poll.Adapt<PollResponse>());
        }
        public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
        {

            var isExistingTitle = await _context.Polls.AnyAsync(p => p.Title == request.Title, cancellationToken: cancellationToken);

            if(isExistingTitle)
                return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

            var poll = request.Adapt<Poll>();


            await _context.Polls.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(poll.Adapt<PollResponse>());
        }

        public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default)
        {


            var isExistingTitle = await _context.Polls.AnyAsync(p => p.Title == request.Title && p.Id != id, cancellationToken: cancellationToken);

            if (isExistingTitle)
                return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);


            var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);

            if (currentPoll is null)
                return Result.Failure(PollErrors.PollNotFound);



            currentPoll.Title = request.Title;
            currentPoll.Summary = request.Summary;
            currentPoll.StartsAt = request.StartsAt;
            currentPoll.EndsAt = request.EndsAt;

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
        public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken =default)
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
