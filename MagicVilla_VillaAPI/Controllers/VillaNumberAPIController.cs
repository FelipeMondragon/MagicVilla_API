using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTOs;
using MagicVilla_VillaAPI.IRepository;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILogging _logger;
        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _repository;
        private readonly IVillaRepository _villaRepository;

        public VillaNumberAPIController(ILogging logger, IVillaNumberRepository repository, IMapper mapper, IVillaRepository villaRepository)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _response = new();
            _villaRepository = villaRepository;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                _logger.Log("Geting all Villa Numbers", "");
                IEnumerable<VillaNumber> villaNumberList = await _repository.GetAllAsync(includeProperties: "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("Get Villa Number error with Id: " + id, "error");
                    return BadRequest();
                }
                var villaNumber = await _repository.GetAsync(v => v.VillaNo == id, includeProperties: "Villa");
                if (villaNumber == null) return NotFound();

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> createVillaNumber([FromBody] VillaNumberCreateDTO villaNumberDTO)
        {
            try
            {
                if (await _repository.GetAsync(v => v.VillaNo == villaNumberDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa already exists");
                    return BadRequest(ModelState);
                }

                if (await _villaRepository.GetAsync(v => v.Id == villaNumberDTO.VillaId) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa Id isn't valid");
                    return BadRequest(ModelState);
                }

                if (villaNumberDTO == null) return BadRequest(villaNumberDTO);
                var villaNumber = _mapper.Map<VillaNumber>(villaNumberDTO);
                await _repository.CreateAsync(villaNumber);

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.statusCode = HttpStatusCode.OK;

                return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0) return BadRequest();
                var villaNumber = await _repository.GetAsync(v => v.VillaNo == id);
                if (villaNumber == null) return NotFound();
                await _repository.DeleteAsync(villaNumber);

                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSucces = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VillaNo) return BadRequest();

                if (await _villaRepository.GetAsync(v => v.Id == updateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa Id isn't valid");
                    return BadRequest(ModelState);
                }

                var model = _mapper.Map<VillaNumber>(updateDTO);
                await _repository.UpdateAsync(model);

                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSucces = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0) return BadRequest();
            var villaNumber = await _repository.GetAsync(v => v.VillaNo == id, false);
            if(villaNumber == null) return NotFound();

            var villaNumberDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);

            patchDTO.ApplyTo(villaNumberDTO, ModelState);

            var model = _mapper.Map<VillaNumber>(villaNumberDTO);

            await _repository.UpdateAsync(model);
            if(!ModelState.IsValid) return BadRequest();

            return NoContent();

        }

    }

}
