using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.ApplicationCore.Exceptions;
using Edgias.Inventory.Management.ApplicationCore.Interfaces;
using Edgias.Inventory.Management.ApplicationCore.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAPI.Interfaces;
using RESTAPI.Models.Form;
using RESTAPI.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAPI.Controllers
{
    [ApiController]
    [Route("v1.0/product-categories")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IAppLogger<ProductCategoriesController> _logger;
        private readonly IAsyncRepository<ProductCategory> _repository;
        private readonly IMapper<ProductCategory, ProductCategoryFormApiModel, ProductCategoryApiModel> _mapper;

        public ProductCategoriesController(IAppLogger<ProductCategoriesController> logger,
            IAsyncRepository<ProductCategory> repository,
            IMapper<ProductCategory, ProductCategoryFormApiModel, ProductCategoryApiModel> mapper)
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
                ApiResponse<ProductCategoryApiModel> response = new ApiResponse<ProductCategoryApiModel>
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
        public async Task<IActionResult> Post(ProductCategoryFormApiModel apiModel)
        {
            try
            {
                ProductCategory productCategory = _mapper.Map(apiModel);

                productCategory = await _repository.AddAsync(productCategory);

                return CreatedAtAction(nameof(GetById), new { id = productCategory.Id }, _mapper.Map(productCategory));
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, apiModel);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ProductCategoryFormApiModel apiModel)
        {
            try
            {
                ProductCategory productCategory = await _repository.GetByIdAsync(id);

                if (productCategory == null)
                {
                    return NotFound();
                }

                _mapper.Map(productCategory, apiModel);

                await _repository.UpdateAsync(productCategory);

                return Ok();
            }

            catch (DataStoreException e)
            {
                _logger.LogError(e.Message, e, apiModel);
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
