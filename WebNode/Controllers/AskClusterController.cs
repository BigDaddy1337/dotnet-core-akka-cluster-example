using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebNode.Services;

namespace WebNode.Controllers
{
    public class AskClusterController
    {
        private readonly IClusterActorSystemService myClusterActorSystemService;

        public AskClusterController(IClusterActorSystemService myClusterActorSystemService)
        {
            this.myClusterActorSystemService = myClusterActorSystemService;
        }

        [HttpGet]
        [Route("api/testcluster")]
        public async Task<string> GetAnswerFromCluster(string message)
        {
            return await myClusterActorSystemService.GetAnswerFromCluster(message);
        }
    }
}
