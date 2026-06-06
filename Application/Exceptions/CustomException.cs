using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace Application.Exceptions
{
    public class CustomException : Exception
    {
        /*  Ok => 200 (get or update data successfully)
            Created => 201 (create data successfully)
            NoContent => 204 (no data to return in body)
            BadRequest => 400 (invalid request, missing required fields, or validation errors)
            Unauthorized => 401 (authentication required or failed)
            Forbidden => 403 (authenticated but not authorized to access the resource)
            NotFound => 404 (resource not found like get user of id doesn't exsist)
            Conflict => 409 (conflicting request, such as duplicate data like register with an exsisting email already)
            Problem => 500 (server error, unexpected condition that prevented the server from fulfilling the request like null reference exception)

        */

        public HttpStatusCode HttpStatusCode { get; set; }
        public CustomException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
