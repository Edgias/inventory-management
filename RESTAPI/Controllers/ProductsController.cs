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
    [Route("v1.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IAppLogger<ProductsController> _logger;
        private readonly IAsyncRepository<Product> _repository;
        private readonly IMapper<Product, ProductRequest, ProductResponse> _mapper;

        public ProductsController(IAppLogger<ProductsController> logger,
            IAsyncRepository<Product> repository,
            IMapper<Product, ProductRequest, ProductResponse> mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("products")]
        public async Task<IActionResult> Get()
        {
            IReadOnlyList<Product> products = await _repository.GetAllAsync();

            if(products.Any())
            {
                return Ok(products.Select(p => _mapper.Map(p)));
            }

            return NoContent();
        }

        [HttpGet("product-categories/{productCategoryId}/products")]
        public async Task<IActionResult> Get(Guid productCategoryId)
        {
            IReadOnlyList<Product> products = await _repository.GetAsync(new ProductSpecification(productCategoryId));

            if (products.Any())
            {
                return Ok(products.Select(p => _mapper.Map(p)));
            }

            return NoContent();
        }

        [HttpGet("products/{skip}/{take}/{searchQuery?}")]
        public async Task<IActionResult> Get(int skip, int take, string searchQuery = null)
        {
            IReadOnlyList<Product> products = await _repository.GetAsync(new ProductSpecification(skip, take, searchQuery));

            if (products.Any())
            {
                ApiResponse<ProductResponse> response = new()
                {
                    Data = products.Select(p => _mapper.Map(p)),
                    Total = await _repository.CountAsync(new ProductSpecification(searchQuery))
                };

                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet("product-categories/{productCategoryId}/products/{skip}/{take}/{searchQuery?}")]
        public async Task<IActionResult> Get(Guid productCategoryId, int skip, int take, string searchQuery = null)
        {
            IReadOnlyList<Product> products = await _repository.GetAsync(new ProductSpecification(productCategoryId, skip, take, searchQuery));

            if (products.Any())
            {
                ApiResponse<ProductResponse> response = new ApiResponse<ProductResponse>
                {
                    Data = products.Select(p => _mapper.Map(p)),
                    Total = await _repository.CountAsync(new ProductSpecification(productCategoryId, searchQuery))
                };

                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Product product = await _repository.GetByIdAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(product));
        }

        [HttpPost("products")]
        public async Task<IActionResult> Post(ProductRequest request)
        {
            try
            {
                Product product = _mapper.Map(request);

                product = await _repository.AddAsync(product);

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, _mapper.Map(product));
            }

            catch(DataStoreException e)
            {
                _logger.LogError(e.Message, e, request);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> Put(Guid id, ProductRequest request)
        {
            try
            {
                Product product = await _repository.GetByIdAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                _mapper.Map(product, request);

                await _repository.UpdateAsync(product);

                return Ok();
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, request);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpPatch("products/{id}")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            try
            {
                Product product = await _repository.GetByIdAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                product.ChangeStatus();

                await _repository.UpdateAsync(product);

                return Ok();
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, id);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                Product product = await _repository.GetByIdAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(product);

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
