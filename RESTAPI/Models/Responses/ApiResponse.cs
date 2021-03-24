using System.Collections.Generic;

namespace Edgias.Inventory.Management.RESTAPI.Models.Responses
{
    public class ApiResponse<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }

        public ApiResponse()
        {
            Data = new List<T>();
        }
    }
}
