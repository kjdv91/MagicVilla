using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
        public VillaController(ILogger<VillaController> logger)
        {
            _logger = logger; //servicio

            
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult< IEnumerable<VillageDto>> GetVillas()
        {
            _logger.LogInformation("Obteniendo Villas");  //servicio
            return Ok(VillageStore.villaList);  //retorna un status code

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
            var villa = VillageStore.villaList.FirstOrDefault(y => y.Id == id);
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
            if(VillageStore.villaList.FirstOrDefault(x=>x.Name.ToLower() == villaDto.Name.ToLower()) != null){
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
            villaDto.Id = VillageStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillageStore.villaList.Add(villaDto);
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
            var villa = VillageStore.villaList.FirstOrDefault(z => z.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            VillageStore.villaList.Remove(villa);
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
            var villa = VillageStore.villaList.FirstOrDefault(x => x.Id == id);
            villa.Name = villageDto.Name;
            villa.SqauerMeter = villageDto.SqauerMeter;
            villa.Capacity = villageDto.Capacity;

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
            var villa = VillageStore.villaList.FirstOrDefault(x => x.Id == id);
            patchDto.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return NoContent();


        }
    }
}
