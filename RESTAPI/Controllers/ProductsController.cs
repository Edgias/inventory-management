using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAPI.Interfaces;
using RESTAPI.Models.Form;
using RESTAPI.Models.View;

namespace RESTAPI.Controllers
{
    [ApiController]
    [Route("v1.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IAppLogger<ProductsController> _logger;
        private readonly IAsyncRepository<Product> _repository;
        private readonly IMapper<Product, ProductFormApiModel, ProductApiModel> _mapper;

        public ProductsController(IAppLogger<ProductsController> logger,
            IAsyncRepository<Product> repository,
            IMapper<Product, ProductFormApiModel, ProductApiModel> mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


    }
}
