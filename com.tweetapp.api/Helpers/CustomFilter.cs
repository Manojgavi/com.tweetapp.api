﻿using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
namespace com.tweetapp.api.Helpers
{
    public class CustomFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context != null)
            {
                HttpStatusCode status = HttpStatusCode.InternalServerError;
                string message = string.Empty;

                var exceptionType = context.Exception.GetType();
                if (exceptionType == typeof(UnauthorizedAccessException))
                {
                    message = "Unauthorized Access";
                    status = HttpStatusCode.Unauthorized;
                }
                else if (exceptionType == typeof(NotImplementedException))
                {
                    message = "A server error occurred.";
                    status = HttpStatusCode.NotImplemented;
                }
                else if (exceptionType == typeof(Exception))
                {
                    message = context.Exception.ToString();
                    status = HttpStatusCode.InternalServerError;
                }
                else
                {
                    message = context.Exception.Message;
                    status = HttpStatusCode.NotFound;
                }

                context.ExceptionHandled = true;

                HttpResponse response = context.HttpContext.Response;
                response.StatusCode = (int)status;
                response.ContentType = "application/json";
                var err = message + " " + context.Exception.StackTrace;
                response.WriteAsync(err);
            }
        }
    }
}