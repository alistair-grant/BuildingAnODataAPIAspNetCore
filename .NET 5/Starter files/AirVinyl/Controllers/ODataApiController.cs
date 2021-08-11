using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System;

namespace AirVinyl.Controllers
{
    [ApiController]
    [Route("odata")]
    public abstract class ODataApiController : ODataController
    {
        public Uri GetRequestUri() =>
            new(HttpContext.Request.GetEncodedUrl());
    }
}
