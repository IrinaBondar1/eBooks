using NivelAccessDate;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eBooks_API.Controllers
{
    public class CartiController : ApiController
    {
        private readonly CarteAccessor carteAccessor = new CarteAccessor();

        // GET: api/Carti
        public IHttpActionResult Get()
        {
            var carti = carteAccessor.GetAll(); 
            return Ok(carti);
        }

        // GET: api/Carti
        public IHttpActionResult Get(int id)
        {
            var carte = carteAccessor.GetById(id);
            if (carte == null) return NotFound();
            return Ok(carte);
        }
    }
}
