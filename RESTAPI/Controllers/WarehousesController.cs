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
    [Route("v1.0/warehouses")]
    public class WarehousesController : ControllerBase
    {
        private readonly IAppLogger<WarehousesController> _logger;
        private readonly IAsyncRepository<Warehouse> _repository;
        private readonly IMapper<Warehouse, WarehouseRequest, WarehouseResponse> _mapper;

        public WarehousesController(IAppLogger<WarehousesController> logger,
            IAsyncRepository<Warehouse> repository,
            IMapper<Warehouse, WarehouseRequest, WarehouseResponse> mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IReadOnlyList<Warehouse> warehouses = await _repository.GetAllAsync();

            if (warehouses.Any())
            {
                return Ok(warehouses.Select(p => _mapper.Map(p)));
            }

            return NoContent();
        }

        [HttpGet("{skip}/{take}/{searchQuery?}")]
        public async Task<IActionResult> Get(int skip, int take, string searchQuery = null)
        {
            IReadOnlyList<Warehouse> warehouses = await _repository.GetAsync(new WarehouseSpecification(skip, take, searchQuery));

            if (warehouses.Any())
            {
                ApiResponse<WarehouseResponse> response = new()
                {
                    Data = warehouses.Select(p => _mapper.Map(p)),
                    Total = await _repository.CountAsync(new WarehouseSpecification(searchQuery))
                };

                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Warehouse warehouse = await _repository.GetByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(warehouse));
        }

        [HttpPost]
        public async Task<IActionResult> Post(WarehouseRequest request)
        {
            try
            {
                Warehouse warehouse = _mapper.Map(request);

                warehouse = await _repository.AddAsync(warehouse);

                return CreatedAtAction(nameof(GetById), new { id = warehouse.Id }, _mapper.Map(warehouse));
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, request);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, WarehouseRequest request)
        {
            try
            {
                Warehouse warehouse = await _repository.GetByIdAsync(id);

                if (warehouse == null)
                {
                    return NotFound();
                }

                _mapper.Map(warehouse, request);

                await _repository.UpdateAsync(warehouse);

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
                Warehouse warehouse = await _repository.GetByIdAsync(id);

                if (warehouse == null)
                {
                    return NotFound();
                }

                warehouse.ChangeStatus();

                await _repository.UpdateAsync(warehouse);

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
                Warehouse warehouse = await _repository.GetByIdAsync(id);

                if (warehouse == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(warehouse);

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
