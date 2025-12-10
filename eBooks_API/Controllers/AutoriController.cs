using AutoMapper;

using eBooks_API.Models;

using LibrarieModele;

using NivelAccessDate;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;




namespace eBooks_API.Controllers
{
    public class AutoriController : ApiController
    {
        private readonly AutorAccessor autorAccessor = new AutorAccessor();
        private readonly IMapper _mapper;

        public AutoriController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<eBooks_API.Mapping.AutoMapperProfile>();
            });
            _mapper = config.CreateMapper();
        }

        // GET /api/autori
        [HttpGet]
        public IEnumerable<AutorDto> Get()
        {
            var autori = autorAccessor.GetAll();
            return _mapper.Map<List<AutorDto>>(autori);
        }

        // GET /api/autori/{id}
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var autor = autorAccessor.GetById(id);
            if (autor == null) return NotFound();
            return Ok(_mapper.Map<AutorDto>(autor));
        }

        // POST /api/autori
        [HttpPost]
        public IHttpActionResult Post([FromBody] AutorCreateDto dto)
        {
            if (dto == null) return BadRequest("Body is required.");
            
            if (string.IsNullOrWhiteSpace(dto.nume_autor))
                ModelState.AddModelError("nume_autor", "Numele autorului este obligatoriu.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exista = autorAccessor.GetAll()
                .Any(a => a.nume_autor.Trim().Equals(dto.nume_autor.Trim(), StringComparison.OrdinalIgnoreCase));
            if (exista)
            {
                ModelState.AddModelError("nume_autor", "Exista deja un autor cu acest nume.");
                return BadRequest(ModelState);
            }

            var autorEnt = _mapper.Map<Autor>(dto);
            autorAccessor.Add(autorEnt);
            var outputDto = _mapper.Map<AutorDto>(autorEnt);
            var location = new Uri(Request.RequestUri + "/" + autorEnt.id_autor);

            return Created(location, outputDto); 
        }

        // PUT /api/autori/{id}
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody] AutorUpdateDto dto)
        {
            if (dto == null) return BadRequest("Body is required.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = autorAccessor.GetById(id);
            if (existing == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.nume_autor))
            {
                var existaAltAutor = autorAccessor.GetAll()
                    .Any(a => a.id_autor != id &&
                              a.nume_autor.Trim().Equals(dto.nume_autor.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existaAltAutor)
                {
                    ModelState.AddModelError("nume_autor", "Alt autor are deja acest nume.");
                    return BadRequest(ModelState);
                }
            }
            _mapper.Map(dto, existing);
            autorAccessor.Update(existing);

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}
