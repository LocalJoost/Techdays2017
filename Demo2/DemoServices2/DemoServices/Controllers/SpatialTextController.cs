using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Services.Description;
using DemoServices.DataObjects;
using Microsoft.Azure.Mobile.Server.Config;

namespace DemoServices.Controllers
{

    [MobileAppController]
    public class SpatialTextController : ApiController
    {
        [HttpGet]
        public async Task<List<SpatialText>> Get()
        {
            return new List<SpatialText>
            {
                new SpatialText("Center", 0f, 0f, 1.7f),
                new SpatialText("Left", -0.7f, 0f, 1.6f),
                new SpatialText("Right", 0.7f, 0f, 1.5f)
            };
        }
    }
}
