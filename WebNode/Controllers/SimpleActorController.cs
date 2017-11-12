using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebNode.Services;

namespace WebNode.Controllers
{
    public class SimpleActorController
    {
        private readonly IActorSystemService myActorSystemService;

        public SimpleActorController(IActorSystemService myActorSystemService)
        {
            this.myActorSystemService = myActorSystemService;
        }

        [HttpGet]
        [Route("api/testactor")]
        public async Task<string> GetResponseFromSimpleActor(string message)
        {
            return await myActorSystemService.GetResponseFromSimpleActor(message);
        }
    }
}
