using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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

        public VillaAPIController(ApplicationDBContext db)
        {
            _db = db; 
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult <IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(_db.Villas.ToList());
        }
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult <VillaDTO> GetVilla(int id)
        {

            if(id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {             
            
            if(_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null )
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
            } 

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            } 
            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa model = new Villa()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.imageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };
            _db.Villas.Add(model); 
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new {id = villaDTO.Id}, villaDTO);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]    
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0 )
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id); 
            if(villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            _db.SaveChanges(); 
            return NoContent();
        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDto)
        {
            if(villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            //villa.Name = villaDto.Name;
            //villa.Sqft = villaDto.Sqft;
            //villa.Occupancy = villaDto.Occupancy; 

            Villa model = new()
            {
                Amenity = villaDto.Amenity,
                Details = villaDto.Details, 
                Id = villaDto.Id, 
                ImageUrl = villaDto.imageUrl, 
                Name = villaDto.Name, 
                Occupancy = villaDto.Occupancy, 
                Rate = villaDto.Rate, 
                Sqft = villaDto.Sqft
            };

            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] 

        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO )
        {
            if(patchDTO == null || id == 0 )
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);


            VillaDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Details = villa.Details, 
                Id = villa.Id, 
                imageUrl = villa.ImageUrl, 
                Name = villa.Name, 
                Occupancy = villa.Occupancy, 
                Rate = villa.Rate, 
                Sqft = villa.Sqft

            };
            
            if(villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = new Villa()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.imageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            _db.Villas.Update(model); 
            _db.SaveChanges();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }

    }
}
