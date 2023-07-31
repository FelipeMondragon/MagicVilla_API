using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTOs;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VillaAPIController(ILogging logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVilla()
        {
            _logger.Log("Geting all Villas", "");
            IEnumerable<Villa> villaList = await _context.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.Log("Get Villa error with Id: " +id, "error");
                return BadRequest();
            }
            var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null) return NotFound();
            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> createVilla([FromBody] VillaCreateDTO villaDTO)
        {
            
            if (await _context.Villas.FirstOrDefaultAsync(v => v.Name == villaDTO.Name) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }
            if (villaDTO == null) return BadRequest(villaDTO);
            var villa = _mapper.Map<Villa>(villaDTO);
            await _context.Villas.AddAsync(villa);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null) return NotFound();
            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id) return BadRequest();
            var model = _mapper.Map<Villa>(updateDTO);
            _context.Villas.Update(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0) return BadRequest();
            var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if(villa == null) return NotFound();

            var villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            patchDTO.ApplyTo(villaDTO, ModelState);

            var model = _mapper.Map<Villa>(villaDTO);

            _context.Villas.Update(model);
            await _context.SaveChangesAsync();
            if(!ModelState.IsValid) return BadRequest();

            return NoContent();

        }

    }

}
