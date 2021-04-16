using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    //front end de yapılan her işlem burada try catch bloklarına girecek.
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;
        private IEnumerable<ValidationFailure> errors;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            //ben sana bir tane json yolladım
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //statü kodu verilecek


            //front end de gelen doğrulamaya göre aşağıdaki if döngüsüne göre hata mesajı yollanacak.
            IEnumerable<ValidationFailure> errors;
            string message = "Internal Server Error";
            if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
                errors = ((ValidationException)e).Errors;
                httpContext.Response.StatusCode = 400;

                return httpContext.Response.WriteAsync(new ValidationErrorDetails
                {
                    StatusCode = 400,
                    Message = message,
                    Errors = errors
                }.ToString());
            }

            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
            //olurda sistem hata verirse bunu döndür diyoruz kullandığımız program arayüzünde.
        }
    }
}
