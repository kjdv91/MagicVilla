using AutoMapper;
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
        private readonly IMapper _mapper;
        public VillaController(ILogger<VillaController> logger,ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger; //servicio
            _db = db;
            _mapper = mapper;

            
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillageDto>>> GetVillas()
        {
            _logger.LogInformation("Obteniendo Villas");  //servicio
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<VillageDto>>(villaList));  //retorna un status code

        }
        [HttpGet("id:int", Name ="GetVilla")]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public async Task<ActionResult< VillageDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al consultar villa con el id", id);
                return BadRequest();  //status code no existe id

            }
            var  villa = await _db.Villas.FirstOrDefaultAsync(y => y.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(villa);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult<VillageDto>> CreateVilla([FromBody] VillageCreateDto createDto )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validacion personalizada nombre repetido
            if( await _db.Villas.FirstOrDefaultAsync(x=>x.Name.ToLower() == createDto.Name.ToLower()) != null){
                ModelState.AddModelError("ExitsName", "La villa con este nombre ya existe");
                return BadRequest(ModelState);

            }
            if(createDto == null)
            {
                return BadRequest(createDto);
            }
            Villa model = _mapper.Map<Villa>(createDto);
         
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new { Id = model.Id }, model);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        //IactionResult porque no necesitamos el modelo y siempre 
        //se retorna un notContent
        public async Task <IActionResult> DeleteVilla(int id)
        {
            if (!ModelState.IsValid) {
            }
            if (id ==0)
            {
                return BadRequest();
            }
            var villa = await  _db.Villas.FirstOrDefaultAsync(z => z.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> UpdateVilla(int id, [FromBody] VillageUpdateDto updateDto)
        {
            if(updateDto ==null || id!= updateDto.Id)
            {
                return BadRequest();
            }
            //var villa = VillageStore.villaList.FirstOrDefault(x => x.Id == id);
            //villa.Name = villageDto.Name;
            //villa.SquareMetters = villageDto.SquareMetters;
            //villa.Capacity = villageDto.Capacity;
            Villa model = _mapper.Map<Villa>(updateDto);
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();


        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillageUpdateDto> patchDto)
        {
            if (patchDto == null || id ==0)
            {
                return BadRequest();
            }
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            VillageUpdateDto villaDto = _mapper.Map<VillageUpdateDto>(villa); 
            
            
            if (villa == null)return BadRequest();

            
            patchDto.ApplyTo(villaDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            Villa model = _mapper.Map<Villa>(villaDto);
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();


        }
    }
}
