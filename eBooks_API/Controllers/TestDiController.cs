using NivelServicii;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace eBooks_API.Controllers
{
    public class TestDiController : ApiController
    {
        private readonly ITestService _service1;
        private readonly ITestService _service2;

        public TestDiController(ITestService service1, ITestService service2)
        {
            _service1 = service1;
            _service2 = service2;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/testdi")]
        public IHttpActionResult Get()
        {
            return Ok(new
            {
                First = _service1.Id,
                Second = _service2.Id,
                AreEqual = _service1.Id == _service2.Id
            });
        }
    }
}