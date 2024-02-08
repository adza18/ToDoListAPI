using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.DTOs.Response;

namespace ToDoList.Infrastructure.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IConfiguration configuration;



        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
               

                await _next(context);
            }
      
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong");
                await HandleException(context, ex);
            }
        }

     



        public static Task HandleException(HttpContext context, Exception ex)
        {
            int StatusCode = (int)HttpStatusCode.InternalServerError;
            var errorResponse = new ErrorResponse()
            {
                StatusCode = StatusCode,
                Message = ex.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCode;
            return context.Response.WriteAsync(errorResponse.ToString());


        }

    }
}
