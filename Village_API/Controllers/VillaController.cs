using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Village_API.Datos;
using Village_API.Models;
using Village_API.Models.Dto;

namespace Village_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        public VillaController(ILogger<VillaController> logger,ApplicationDbContext db)
        {
            _logger = logger; //servicio
            _db = db;

            
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult< IEnumerable<VillageDto>> GetVillas()
        {
            _logger.LogInformation("Obteniendo Villas");  //servicio
            return Ok(_db.Villas.ToList());  //retorna un status code

        }
        [HttpGet("id:int", Name ="GetVilla")]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public ActionResult< VillageDto> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al consultar villa con el id", id);
                return BadRequest();  //status code no existe id

            }
            var villa = _db.Villas.FirstOrDefault(y => y.Id == id);
            return Ok(villa);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillageDto> CreateVilla([FromBody] VillageDto villaDto )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validacion personalizada nombre repetido
            if(_db.Villas.FirstOrDefault(x=>x.Name.ToLower() == villaDto.Name.ToLower()) != null){
                ModelState.AddModelError("ExitsName", "La villa con este nombre ya existe");
                return BadRequest(ModelState);

            }
            if(villaDto == null)
            {
                return BadRequest();
            }
            if(villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa modeloDto = new()
            {
                
                Name = villaDto.Name,
                Details = villaDto.Details,
                Capacity = villaDto.Capacity,
                ImageUrl = villaDto.ImageUrl,
                SquareMetters = villaDto.SquareMetters,
                UpdateDate = villaDto.UpdateDate,



            };
            _db.Villas.Add(modeloDto);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new { Id = villaDto.Id });
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        //IactionResult porque no necesitamos el modelo y siempre 
        //se retorna un notContent
        public IActionResult DeleteVilla(int id)
        {
            if (!ModelState.IsValid) {
            }
            if (id ==0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(z => z.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillageDto villageDto)
        {
            if(villageDto ==null || id!= villageDto.Id)
            {
                return BadRequest();
            }
            //var villa = VillageStore.villaList.FirstOrDefault(x => x.Id == id);
            //villa.Name = villageDto.Name;
            //villa.SquareMetters = villageDto.SquareMetters;
            //villa.Capacity = villageDto.Capacity;

            Villa modeloDto = new()
            {
                Id = villageDto.Id,
                Name = villageDto.Name,
                Details = villageDto.Details,
                ImageUrl = villageDto.ImageUrl,
                SquareMetters = villageDto.SquareMetters,
                UpdateDate = villageDto.UpdateDate,
                Capacity = villageDto.Capacity




            };
            _db.Villas.Update(modeloDto);
            _db.SaveChanges();
            return NoContent();


        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillageDto> patchDto)
        {
            if (patchDto == null || id ==0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
            VillageDto villageDto = new()
            {
                Id = villa.Id,
                Capacity = villa.Capacity,
                Details = villa.Details,
                SquareMetters = villa.SquareMetters,
                Name = villa.Name,
                ImageUrl= villa.ImageUrl
            };
            
            if (villa == null)return BadRequest();

            
            patchDto.ApplyTo(villageDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            Villa modelo = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                SquareMetters = villa.SquareMetters,
                UpdateDate = villa.UpdateDate,
                Capacity = villa.Capacity,
                Fee = villa.Fee

            };
            _db.Villas.Update(modelo);
            _db.SaveChanges();
            return NoContent();


        }
    }
}
