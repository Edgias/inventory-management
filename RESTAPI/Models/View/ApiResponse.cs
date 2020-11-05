using System.Collections.Generic;

namespace RESTAPI.Models.View
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
