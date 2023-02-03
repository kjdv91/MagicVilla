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
    public class VillaNumberController : ControllerBase
    {
        private readonly ILogger<VillaNumberController> _logger;
        //private readonly ApplicationDbContext _db;
        private readonly IVillaRepository _villaRepo;
        private readonly IVillageNumberRepository _numberRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public VillaNumberController(ILogger<VillaNumberController> logger,/*ApplicationDbContext db*/ 
            IVillaRepository villarepo, IMapper mapper, IVillageNumberRepository numberRepo)
        {
            _logger = logger; //servicio
            //_db = db;
            _villaRepo = villarepo;
            _numberRepo = numberRepo;
            _mapper = mapper;
            _response = new();


            
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetNumberVillas()
        {
            try
            {
                _logger.LogInformation("Obteniendo Villas");  //servicio
                                                              //IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();  //dbContext
                IEnumerable<VillageNumber> villageNumberList = await _numberRepo.GetAlls();
                _response.Result = _mapper.Map<IEnumerable<VillageNumberDto>>(villageNumberList);
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
        [HttpGet("id:int", Name ="GetNumberVilla")]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public async Task<ActionResult< VillageNumberDto>> GetNumberVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al consultar el numero de villa con el id", id);
                return BadRequest();  //status code no existe id

            }
            //var  villa = await _db.Villas.FirstOrDefaultAsync(y => y.Id == id);  //dbContext
            var numberVillage = await _numberRepo.Get(x => x.VillageNro == id);
            if(numberVillage == null)
            {
                return NotFound();
            }
            return Ok(numberVillage);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult<VillageDto>> CreateNumberVilla([FromBody] VillageNumberCreateDto createDto )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validacion personalizada nombre repetido
            /*if( await _db.Villas.FirstOrDefaultAsync(x=>x.Name.ToLower() == createDto.Name.ToLower()) != null)*/
            if(await _numberRepo.Get(x => x.VillageNro == createDto.VillageNro) != null)
            {
                ModelState.AddModelError("ExitsName", "El numero de villa ya existe");
                return BadRequest(ModelState);

            }
            if (await _villaRepo.Get(v => v.Id == createDto.VillaId) == null)
            {

                ModelState.AddModelError("ExitsName", "El Id de la villa no existe");
                return BadRequest(ModelState);

            }
            if(createDto == null)
            {
                return BadRequest(createDto);
            }
            VillageNumber model = _mapper.Map<VillageNumber>(createDto);
            model.CreatedDate = DateTime.Now;
            model.UpdateDate = DateTime.Now;

            //await _db.Villas.AddAsync(model);
            //await _db.SaveChangesAsync();
            
            await _numberRepo.Create(model);

           
            return CreatedAtRoute("GetNumberVilla", new { Id = model.VillaId }, model);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        //IactionResult porque no necesitamos el modelo y siempre 
        //se retorna un notContent
        public async Task <IActionResult> DeleteNumberVilla(int id)
        {
            
            if (id ==0)
            {
                return BadRequest();
            }
            //var villa = await  _db.Villas.FirstOrDefaultAsync(z => z.Id == id);
            //var villa = await _.Villas.FirstOrDefaultAsync(z => z.Id == id);
            var numberVillage = await _numberRepo.Get(v=>v.VillageNro == id);
            if (numberVillage == null)
            {
                return NotFound();
            }

            //_db.Villas.Remove(villa); context
            //await _villaRepo.SaveChangesAsync();  //context
            await _numberRepo.Remove(numberVillage);

            return NoContent();
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> UpdateNumberVilla(int id, [FromBody] VillageNumberUpdateDto updateDto)
        {
            if(updateDto ==null || id!= updateDto.VillageNro)
            {
                return BadRequest();
            }

            if(await _villaRepo.Get(x=> x.Id == updateDto.VillaId ) == null)
            {
                ModelState.AddModelError("ForeingKey", " el id de la villa no existe");
                return BadRequest(ModelState);
            }
            //var villa = VillageStore.villaList.FirstOrDefault(x => x.Id == id);
            //villa.Name = villageDto.Name;
            //villa.SquareMetters = villageDto.SquareMetters;
            //villa.Capacity = villageDto.Capacity;
            VillageNumber model = _mapper.Map<VillageNumber>(updateDto);

            //_db.Villas.Update(model);  //context
            //await _db.SaveChangesAsync();

            await _numberRepo.Update(model);

            return NoContent();


        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> UpdatePartialNumberVilla(int id, JsonPatchDocument<VillageNumberUpdateDto> patchDto)
        {
            if (patchDto == null || id ==0)
            {
                return BadRequest();
            }
            //var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); context
            var numberVillage = await _numberRepo.Get(x => x.VillageNro == id, tracked: false);

            VillageNumberUpdateDto villaNumberDto = _mapper.Map<VillageNumberUpdateDto>(numberVillage); 
            
            
            if (numberVillage == null)return BadRequest();

            
            patchDto.ApplyTo(villaNumberDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            VillageNumber model = _mapper.Map<VillageNumber>(villaNumberDto);
            //_db.Villas.Update(model);
            //await _db.SaveChangesAsync();  //contexto
            await _numberRepo.Update(model);
            return NoContent();


        }
    }
}
