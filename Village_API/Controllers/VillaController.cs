using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.CompilerServices;
using Village_API.Datos;
using Village_API.Models;
using Village_API.Models.Dto;
using Village_API.Repository.IRepository;

namespace Village_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        //private readonly ApplicationDbContext _db;
        private readonly IVillaRepository _villaRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public VillaController(ILogger<VillaController> logger,/*ApplicationDbContext db*/ IVillaRepository villarepo, IMapper mapper)
        {
            _logger = logger; //servicio
            //_db = db;
            _villaRepo = villarepo;

            _mapper = mapper;
            _response = new();


            
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obteniendo Villas");  //servicio
                                                              //IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();  //dbContext
                IEnumerable<Villa> villaList = await _villaRepo.GetAlls();
                _response.Result = _mapper.Map<IEnumerable<VillageDto>>(villaList);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);  //retorna un status code

            }
            catch (Exception ex)
            {
                _response.isValid = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
           

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
            //var  villa = await _db.Villas.FirstOrDefaultAsync(y => y.Id == id);  //dbContext
            var villa = await _villaRepo.Get(x => x.Id == id);
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
            /*if( await _db.Villas.FirstOrDefaultAsync(x=>x.Name.ToLower() == createDto.Name.ToLower()) != null)*/
            if(await _villaRepo.Get(x => x.Name.ToLower() == createDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("ExitsName", "La villa con este nombre ya existe");
                return BadRequest(ModelState);

            }
            if(createDto == null)
            {
                return BadRequest(createDto);
            }
            Villa model = _mapper.Map<Villa>(createDto);
            model.EmitionCreated = DateTime.Now;
            model.UpdateDate = DateTime.Now;

            //await _db.Villas.AddAsync(model);
            //await _db.SaveChangesAsync();
            
            await _villaRepo.Create(model);

           
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
            
            if (id ==0)
            {
                return BadRequest();
            }
            //var villa = await  _db.Villas.FirstOrDefaultAsync(z => z.Id == id);
            //var villa = await _.Villas.FirstOrDefaultAsync(z => z.Id == id);
            var villa = await _villaRepo.Get(v=>v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            //_db.Villas.Remove(villa); context
            //await _villaRepo.SaveChangesAsync();  //context
            await _villaRepo.Remove(villa);

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

            //_db.Villas.Update(model);  //context
            //await _db.SaveChangesAsync();

            await _villaRepo.Update(model);

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
            //var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); context
            var villa = await _villaRepo.Get(x => x.Id == id, tracked: false);

            VillageUpdateDto villaDto = _mapper.Map<VillageUpdateDto>(villa); 
            
            
            if (villa == null)return BadRequest();

            
            patchDto.ApplyTo(villaDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            Villa model = _mapper.Map<Villa>(villaDto);
            //_db.Villas.Update(model);
            //await _db.SaveChangesAsync();  //contexto
            await _villaRepo.Update(model);
            return NoContent();


        }
    }
}
