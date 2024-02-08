using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Entity.DTOs.Response;
using ToDoList.Entity.Enums;

namespace ToDoList.Application.Helper
{
    public static class MapResponse
    {
        public static Dictionary<ServiceStatusEnum, HttpStatusCode> ResponseDictionary = new Dictionary<ServiceStatusEnum, HttpStatusCode>()
        {
            { ServiceStatusEnum.Success, HttpStatusCode.OK },
            { ServiceStatusEnum.NotFound, HttpStatusCode.NotFound },
            { ServiceStatusEnum.PermissionDenied, HttpStatusCode.Unauthorized },
            { ServiceStatusEnum.Unauthorized, HttpStatusCode.Unauthorized },
            { ServiceStatusEnum.Error, HttpStatusCode.InternalServerError },
            { ServiceStatusEnum.Unknown, HttpStatusCode.NotImplemented }
        };

        public static ApiResponseDTO<T> MapToApiResponse<T> (ServiceResponseDTO<T> res)
        {
            var result = new ApiResponseDTO<T>()
            {
                IsSuccess = res.IsSuccess,
                Data = res.Data,
                StatusCode = ResponseDictionary[res.Status]
            };

            if (res.ErrorMessage != null && !res.IsSuccess)
            {
                result.AddMessage(MessageTypeEnum.Error, res.ErrorMessage);
            }
            return result;
        }


        public static IActionResult ActionResponse<T>(ApiResponseDTO<T> data)
        {
            if (data.StatusCode == HttpStatusCode.OK) return new OkObjectResult(data);
            if (data.StatusCode == HttpStatusCode.BadRequest) return new BadRequestObjectResult(data);
            if (data.StatusCode == HttpStatusCode.NotFound) return new NotFoundObjectResult(data);
            if (data.StatusCode == HttpStatusCode.InternalServerError) return new BadRequestObjectResult(data);
            if (data.StatusCode == HttpStatusCode.Unauthorized) return new UnauthorizedObjectResult(data);
            return new BadRequestObjectResult(data);
        }


    }
}
