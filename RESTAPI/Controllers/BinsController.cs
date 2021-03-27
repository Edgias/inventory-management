using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.ApplicationCore.Exceptions;
using Edgias.Inventory.Management.ApplicationCore.Interfaces;
using Edgias.Inventory.Management.ApplicationCore.Specifications;
using Edgias.Inventory.Management.RESTAPI.Interfaces;
using Edgias.Inventory.Management.RESTAPI.Models.Requests;
using Edgias.Inventory.Management.RESTAPI.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edgias.Inventory.Management.RESTAPI.Controllers
{

    [ApiController]
    [Route("v1.0/bins")]
    public class BinsController : ControllerBase
    {
        private readonly IAppLogger<BinsController> _logger;
        private readonly IAsyncRepository<Bin> _repository;
        private readonly IMapper<Bin, BinRequest, BinResponse> _mapper;

        public BinsController(IAppLogger<BinsController> logger,
            IAsyncRepository<Bin> repository,
            IMapper<Bin, BinRequest, BinResponse> mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IReadOnlyList<Bin> bins = await _repository.GetAllAsync();

            if (bins.Any())
            {
                return Ok(bins.Select(p => _mapper.Map(p)));
            }

            return NoContent();
        }

        [HttpGet("{skip}/{take}/{searchQuery?}")]
        public async Task<IActionResult> Get(int skip, int take, string searchQuery = null)
        {
            IReadOnlyList<Bin> bins = await _repository.GetAsync(new BinSpecification(skip, take, searchQuery));

            if (bins.Any())
            {
                ApiResponse<BinResponse> response = new()
                {
                    Data = bins.Select(p => _mapper.Map(p)),
                    Total = await _repository.CountAsync(new BinSpecification(searchQuery))
                };

                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Bin bin = await _repository.GetByIdAsync(id);

            if (bin == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(bin));
        }

        [HttpPost]
        public async Task<IActionResult> Post(BinRequest request)
        {
            try
            {
                Bin bin = _mapper.Map(request);

                bin = await _repository.AddAsync(bin);

                return CreatedAtAction(nameof(GetById), new { id = bin.Id }, _mapper.Map(bin));
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, request);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, BinRequest request)
        {
            try
            {
                Bin bin = await _repository.GetByIdAsync(id);

                if (bin == null)
                {
                    return NotFound();
                }

                _mapper.Map(bin, request);

                await _repository.UpdateAsync(bin);

                return Ok();
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, request);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            try
            {
                Bin bin = await _repository.GetByIdAsync(id);

                if (bin == null)
                {
                    return NotFound();
                }

                bin.ChangeStatus();

                await _repository.UpdateAsync(bin);

                return Ok();
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, id);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                Bin bin = await _repository.GetByIdAsync(id);

                if (bin == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(bin);

                return Ok(id);
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, id);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
    }
}
