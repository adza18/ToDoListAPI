using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.DTOs.Response;

namespace ToDoList.Infrastructure.Middleware
{
    public class AuthorizationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationExceptionMiddleware> _logger;
        private readonly IConfiguration configuration;


        public AuthorizationExceptionMiddleware(RequestDelegate requestDelegate, ILogger<AuthorizationExceptionMiddleware> logger, IConfiguration configuration)
        {
            _next = requestDelegate;
            _logger = logger;
            this.configuration = configuration;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {

                    throw new Exception("Token empty");
                }

                if (token != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ClockSkew = TimeSpan.Zero,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:JWTSecretKey"]!))
                   
                };


                    var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                }

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
            int StatusCode = (int)HttpStatusCode.Unauthorized;
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
