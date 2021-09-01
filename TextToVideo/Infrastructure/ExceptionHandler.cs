using System;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Models.Models;

namespace Uploader.Infrastructure
{
    public class ExceptionHandler
    {
        private RequestDelegate Next { get; set; }

        public ExceptionHandler(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch(InnerException ex)
            {
                var response = context.Response;
                response.ContentType = MediaTypeNames.Application.Json;
                response.StatusCode = StatusCodes.Status400BadRequest;
                BadRequestModel model = new()
                {
                    ExceptionCode = ex.ExceptionCode,
                    Message = ex.Message,
                    PropertyName = ex.PropertyName,
                };
                await response.WriteAsync(JsonSerializer.Serialize(model));
            }
            catch(Exception ex)
            {
                var response = context.Response;
                response.ContentType = MediaTypeNames.Application.Json;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                BadRequestModel model = new()
                {
                    Message = ex.Message,
                };
                await response.WriteAsync(JsonSerializer.Serialize(model));
            }
        }
    }
}
