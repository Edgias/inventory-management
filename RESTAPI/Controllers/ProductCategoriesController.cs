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
    [Route("v1.0/product-categories")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IAppLogger<ProductCategoriesController> _logger;
        private readonly IAsyncRepository<ProductCategory> _repository;
        private readonly IMapper<ProductCategory, ProductCategoryRequest, ProductCategoryResponse> _mapper;

        public ProductCategoriesController(IAppLogger<ProductCategoriesController> logger,
            IAsyncRepository<ProductCategory> repository,
            IMapper<ProductCategory, ProductCategoryRequest, ProductCategoryResponse> mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IReadOnlyList<ProductCategory> productCategories = await _repository.GetAllAsync();

            if (productCategories.Any())
            {
                return Ok(productCategories.Select(p => _mapper.Map(p)));
            }

            return NoContent();
        }

        
        [HttpGet("{skip}/{take}/{searchQuery?}")]
        public async Task<IActionResult> Get(int skip, int take, string searchQuery = null)
        {
            IReadOnlyList<ProductCategory> productCategories = await _repository.GetAsync(new ProductCategorySpecification(skip, take, searchQuery));

            if (productCategories.Any())
            {
                ApiResponse<ProductCategoryResponse> response = new()
                {
                    Data = productCategories.Select(p => _mapper.Map(p)),
                    Total = await _repository.CountAsync(new ProductCategorySpecification(searchQuery))
                };

                return Ok(response);
            }

            return NoContent();
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            ProductCategory productCategory = await _repository.GetByIdAsync(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(productCategory));
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductCategoryRequest request)
        {
            try
            {
                ProductCategory productCategory = _mapper.Map(request);

                productCategory = await _repository.AddAsync(productCategory);

                return CreatedAtAction(nameof(GetById), new { id = productCategory.Id }, _mapper.Map(productCategory));
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, request);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ProductCategoryRequest request)
        {
            try
            {
                ProductCategory productCategory = await _repository.GetByIdAsync(id);

                if (productCategory == null)
                {
                    return NotFound();
                }

                _mapper.Map(productCategory, request);

                await _repository.UpdateAsync(productCategory);

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
                ProductCategory productCategory = await _repository.GetByIdAsync(id);

                if (productCategory == null)
                {
                    return NotFound();
                }

                productCategory.ChangeStatus();

                await _repository.UpdateAsync(productCategory);

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
                ProductCategory productCategory = await _repository.GetByIdAsync(id);

                if (productCategory == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(productCategory);

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
