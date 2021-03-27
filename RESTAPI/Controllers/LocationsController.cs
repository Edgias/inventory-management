﻿using Edgias.Inventory.Management.ApplicationCore.Entities;
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
    [Route("v1.0/locations")]
    public class LocationsController : ControllerBase
    {
        private readonly IAppLogger<LocationsController> _logger;
        private readonly IAsyncRepository<Location> _repository;
        private readonly IMapper<Location, LocationRequest, LocationResponse> _mapper;

        public LocationsController(IAppLogger<LocationsController> logger,
            IAsyncRepository<Location> repository,
            IMapper<Location, LocationRequest, LocationResponse> mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IReadOnlyList<Location> locations = await _repository.GetAllAsync();

            if (locations.Any())
            {
                return Ok(locations.Select(p => _mapper.Map(p)));
            }

            return NoContent();
        }

        [HttpGet("{skip}/{take}/{searchQuery?}")]
        public async Task<IActionResult> Get(int skip, int take, string searchQuery = null)
        {
            IReadOnlyList<Location> locations = await _repository.GetAsync(new LocationSpecification(skip, take, searchQuery));

            if (locations.Any())
            {
                ApiResponse<LocationResponse> response = new()
                {
                    Data = locations.Select(p => _mapper.Map(p)),
                    Total = await _repository.CountAsync(new LocationSpecification(searchQuery))
                };

                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Location location = await _repository.GetByIdAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(location));
        }

        [HttpPost]
        public async Task<IActionResult> Post(LocationRequest request)
        {
            try
            {
                Location location = _mapper.Map(request);

                location = await _repository.AddAsync(location);

                return CreatedAtAction(nameof(GetById), new { id = location.Id }, _mapper.Map(location));
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, request);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, LocationRequest request)
        {
            try
            {
                Location location = await _repository.GetByIdAsync(id);

                if (location == null)
                {
                    return NotFound();
                }

                _mapper.Map(location, request);

                await _repository.UpdateAsync(location);

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
                Location location = await _repository.GetByIdAsync(id);

                if (location == null)
                {
                    return NotFound();
                }

                location.ChangeStatus();

                await _repository.UpdateAsync(location);

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
                Location location = await _repository.GetByIdAsync(id);

                if (location == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(location);

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
