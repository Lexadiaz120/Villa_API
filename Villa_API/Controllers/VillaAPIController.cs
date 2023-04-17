using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Villa_API.Data;
using Villa_API.Logging;
using Villa_API.Models;
using Villa_API.Models.Dto;

namespace Villa_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper; 

        public VillaAPIController(ApplicationDBContext db, IMapper mapper)
        {
            _db = db; 
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult <IEnumerable<VillaCreateDTO>>> GetVillas()
        {  
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync(); // store in a variable 
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async  Task<ActionResult<VillaDTO>> GetVilla(int id)
        {

            if(id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefaultAsync(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDTO>(villa));
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaCreateDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {             
            
            if( await _db.Villas.FirstOrDefaultAsync (u => u.Name.ToLower() == createDTO.Name.ToLower()) != null )
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
            } 

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(createDTO == null)
            {
                return BadRequest(createDTO);
            }
            //if(villaDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //} 

            Villa model = _mapper.Map<Villa>(createDTO);

            //Villa model = new Villa()
            //{
            //    Amenity = createDTO.Amenity,
            //    Details = createDTO.Details,
            //    ImageUrl = createDTO.imageUrl,
            //    Name = createDTO.Name,
            //    Occupancy = createDTO.Occupancy,
            //    Rate = createDTO.Rate,
            //    Sqft = createDTO.Sqft
            //};
           await  _db.Villas.AddAsync(model); 
           await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new {id = model.Id}, model);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]    
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0 )
            {
                return BadRequest();
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id); 
            if(villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
           await _db.SaveChangesAsync(); 
            return NoContent();
        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if(updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            //villa.Name = villaDto.Name;
            //villa.Sqft = villaDto.Sqft;
            //villa.Occupancy = villaDto.Occupancy;  

            Villa model = _mapper.Map<Villa>(updateDTO);

            //Villa model = new()
            //{
            //    Amenity = updateDTO.Amenity,
            //    Details = updateDTO.Details, 
            //    Id = updateDTO.Id, 
            //    ImageUrl = updateDTO.imageUrl, 
            //    Name = updateDTO.Name, 
            //    Occupancy = updateDTO.Occupancy, 
            //    Rate = updateDTO.Rate, 
            //    Sqft = updateDTO.Sqft
            //};

            _db.Villas.Update(model);
          await  _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] 

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO )
        {
            if(patchDTO == null || id == 0 )
            {
                return BadRequest();
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);
            //VillaUpdateDTO villaDTO = new()
            //{
            //    Amenity = villa.Amenity,
            //    Details = villa.Details, 
            //    Id = villa.Id, 
            //    imageUrl = villa.ImageUrl, 
            //    Name = villa.Name, 
            //    Occupancy = villa.Occupancy, 
            //    Rate = villa.Rate, 
            //    Sqft = villa.Sqft
            //};
          
            if(villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);

            //Villa model = new Villa()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.Id,
            //    ImageUrl = villaDTO.imageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft
            //};

            _db.Villas.Update(model); 
          await   _db.SaveChangesAsync();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }

    }
}
