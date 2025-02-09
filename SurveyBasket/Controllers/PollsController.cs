using Microsoft.AspNetCore.Authorization.Policy;

namespace SurveyBasket.Controllers

{
    [Route("api/[controller]")]   ///api/Pools
    [ApiController]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()  //api/polls

        {

            return Ok(_pollService.GetAll());
        }

        [HttpGet("{id}")]   //api/polls/1

        public IActionResult Get(int id)  
        {
            var poll = _pollService.Get(id);
            if (poll is null)
                return NotFound();

            return Ok(poll);
        }
        [HttpPost("")]
        public IActionResult Add(Poll request)

        {

            var newPoll = _pollService.Add(request);
            return CreatedAtAction(nameof(Get), new {id= newPoll.Id }, newPoll);
                

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id ,Poll request)

        {

            var isUpdated = _pollService.Update(id,request);
            if (!isUpdated)
                return NotFound();
            return NoContent();


        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)

        {

            var isDeleted = _pollService.Delete(id);
            if (!isDeleted)
                return NotFound();
            return NoContent();


        }
    }
}
