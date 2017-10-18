using System.Collections.Generic;
using System.Web.Http;
using System.Web.Services.Description;
using DemoServices.DataObjects;
using Microsoft.Azure.Mobile.Server.Config;

namespace DemoServices.Controllers
{

    [MobileAppController]
    [RoutePrefix("api/spatialtexts")]
    public class SpatialTextController : ApiController
    {
        [HttpGet]
        [Route("horizontal")]
        public SpatialTextList GetHorizontal()
        {
            return new SpatialTextList
            {
                texts =
                    new List<SpatialText>
                    {
                        new SpatialText("Center", 0f, 0f, 5.0f),
                        new SpatialText("Left", -0.7f, 0f, 5.0f),
                        new SpatialText("Right", 0.7f, 0f, 5.0f)
                    }
            };
        }

        [HttpGet]
        [Route("vertical")]
        public SpatialTextList GetVertical()
        {
            return new SpatialTextList
            {
                texts =
                    new List<SpatialText>
                    {
                        new SpatialText("Center", 0f, 0f, 5.0f),
                        new SpatialText("Top", 0f, 0.5f, 5.0f),
                        new SpatialText("Bottom", 0f, -0.5f, 5.0f)
                    }
            };
        }
    }
}
