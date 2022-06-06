using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialLurker.Logic.CQRS.ScraperTest.Queries;
using System.Threading.Tasks;

namespace SocialLurker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScraperTestController : ControllerBase
    {
        private readonly ILogger<ScraperTestController> _logger;
        private IMediator _mediator;

        public ScraperTestController(ILogger<ScraperTestController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("Get")]
        public async Task<Get.Result> Get()
        {
            var result = await _mediator.Send(new Get.Query());
            return result;
        }

        [HttpGet("GrommrTest")]
        public async Task<GrommrTest.Result> GrommrTest()
        {
            var result = await _mediator.Send(new GrommrTest.Query());
            return result;
        }
    }
}
