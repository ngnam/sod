﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using StoreOrder.WebApplication.Data.Wrappers;
using StoreOrder.WebApplication.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Middlewares
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiResponseOptions _options = new ApiResponseOptions
        {
            ApiVersion = "1.0.0.0"
        };
        private readonly ILogger<ApiResponseMiddleware> _logger;
        private readonly string _safelist;
        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger, string safelist)
        {
            _next = next;
            _logger = logger;
            _safelist = safelist;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // checkIP
            //var remoteIp = context.Connection.RemoteIpAddress;
            //var ipAddr = context.Request.Headers["x-forwarded-for"];

            //_logger.Log(LogLevel.Information, $"{DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture)}: Request from Remote IP address: {remoteIp}---fwd: {ipAddr}");

            //string[] ip = _safelist.Split(';');

            //if (ip.Contains(ipAddr.ToString()))
            //{
            //    _logger.Log(LogLevel.Warning,
            //        $"{DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture)} --- Forbidden Request from Remote IP address: {remoteIp} -- fwd: {ipAddr}");
            //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //    return;
            //}

            if (IsSwagger(context))
                await this._next(context);
            else
            {
                var stopWatch = Stopwatch.StartNew();

                var request = await FormatRequest(context.Request);

                var originalBodyStream = context.Response.Body;

                using (var bodyStream = new MemoryStream())
                {
                    try
                    {
                        context.Response.Body = bodyStream;

                        await _next.Invoke(context);

                        context.Response.Body = originalBodyStream;
                        if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            var bodyAsText = await FormatResponse(bodyStream);
                            await HandleSuccessRequestAsync(context, bodyAsText, context.Response.StatusCode);
                        }
                        else
                        {
                            await HandleNotSuccessRequestAsync(context, context.Response.StatusCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        await HandleExceptionAsync(context, ex);
                        bodyStream.Seek(0, SeekOrigin.Begin);
                        await bodyStream.CopyToAsync(originalBodyStream);
                    }
                    finally
                    {
                        stopWatch.Stop();
                        _logger.Log(LogLevel.Information,
                                    $@"Request: {request} Responded with [{context.Response.StatusCode}] in {stopWatch.ElapsedMilliseconds}ms");
                    }
                }

            }

        }

        private async Task<string> FormatRequest(HttpRequest request)
        {

            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return $"{request.Method} {request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(Stream bodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(bodyStream).ReadToEndAsync();
            bodyStream.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }

        private Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            ApiError apiError = null;
            int code = 0;

            if (exception is ApiException)
            {
                var ex = exception as ApiException;
                if (ex.IsModelValidatonError)
                {
                    apiError = new ApiError(ResponseMessageEnum.ValidationError.GetDescription(), ex.Errors)
                    {
                        ReferenceErrorCode = ex.ReferenceErrorCode,
                        ReferenceDocumentLink = ex.ReferenceDocumentLink,
                    };

                    _logger.Log(LogLevel.Warning, exception, $"[{ex.StatusCode}]: {ResponseMessageEnum.ValidationError.GetDescription()}");
                }
                else
                {
                    apiError = new ApiError(ex.Message)
                    {
                        ReferenceErrorCode = ex.ReferenceErrorCode,
                        ReferenceDocumentLink = ex.ReferenceDocumentLink,
                    };

                    _logger.Log(LogLevel.Warning, exception, $"[{ex.StatusCode}]: {ResponseMessageEnum.Exception.GetDescription()}");
                }

                code = ex.StatusCode;
                context.Response.StatusCode = code;

            }
            else if (exception is UnauthorizedAccessException)
            {
                apiError = new ApiError(ResponseMessageEnum.UnAuthorized.GetDescription());
                code = (int)HttpStatusCode.Unauthorized;
                context.Response.StatusCode = code;

                _logger.Log(LogLevel.Warning, exception, $"[{code}]: {ResponseMessageEnum.UnAuthorized.GetDescription()}");
            }
            else
            {

                var exceptionMessage = ResponseMessageEnum.Unhandled.GetDescription();
#if !DEBUG
                var message = exceptionMessage;
                string stackTrace = null;
#else
                var message = $"{ exceptionMessage } { exception.GetBaseException().Message }";
                string stackTrace = exception.StackTrace;
#endif

                apiError = new ApiError(message) { Details = stackTrace };
                code = (int)HttpStatusCode.InternalServerError;
                context.Response.StatusCode = code;

                _logger.Log(LogLevel.Error, exception, $"[{code}]: {exceptionMessage}");
            }

            var jsonString = ConvertToJSONString(GetErrorResponse(code, apiError));

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonString);
        }

        private Task HandleNotSuccessRequestAsync(HttpContext context, int code)
        {
            ApiError apiError = null;

            if (code == (int)HttpStatusCode.NotFound)
                apiError = new ApiError(ResponseMessageEnum.NotFound.GetDescription());
            else if (code == (int)HttpStatusCode.NoContent)
                apiError = new ApiError(ResponseMessageEnum.NotContent.GetDescription());
            else if (code == (int)HttpStatusCode.MethodNotAllowed)
                apiError = new ApiError(ResponseMessageEnum.MethodNotAllowed.GetDescription());
            else if (code == (int)HttpStatusCode.Unauthorized)
                apiError = new ApiError(ResponseMessageEnum.UnAuthorized.GetDescription());
            else
                apiError = new ApiError(ResponseMessageEnum.Unknown.GetDescription());

            context.Response.StatusCode = code;

            var jsonString = ConvertToJSONString(GetErrorResponse(code, apiError));

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonString);
        }

        private Task HandleSuccessRequestAsync(HttpContext context, object body, int code)
        {
            string jsonString = string.Empty;

            var bodyText = !body.ToString().IsValidJson() ? ConvertToJSONString(body) : body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            Type type = bodyContent?.GetType();

            if (type.Equals(typeof(Newtonsoft.Json.Linq.JObject)))
            {
                jsonString = ConvertToJSONString(code, bodyContent);
            }
            else
            {
                jsonString = ConvertToJSONString(code, bodyContent);
            }
            context.Response.StatusCode = code;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonString);
        }

        private string ConvertToJSONString(int code, object content)
        {
            return JsonConvert.SerializeObject(new ApiResponse(ResponseMessageEnum.Success.GetDescription(), content, code, GetApiVersion()), JSONSettings());
        }
        private string ConvertToJSONString(ApiResponse apiResponse)
        {
            return JsonConvert.SerializeObject(apiResponse, JSONSettings());
        }
        private string ConvertToJSONString(object rawJSON)
        {
            return JsonConvert.SerializeObject(rawJSON, JSONSettings());
        }
        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger") 
                || context.Request.Path.StartsWithSegments("/docs") 
                || context.Request.Path.Value.Contains("/CaptCha")
                //|| context.Request.Path.StartsWithSegments("/hubs/order")
                || context.Request.Path.Value.Contains("swagger.json");

        }
        private JsonSerializerSettings JSONSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
        }

        private ApiResponse GetErrorResponse(int code, ApiError apiError)
        {
            return new ApiResponse(code, apiError) { Version = GetApiVersion() };
        }

        private ApiResponse GetSucessResponse(ApiResponse apiResponse)
        {
            if (apiResponse.Version.Equals("1.0.0.0"))
                apiResponse.Version = GetApiVersion();

            return apiResponse;
        }

        private string GetApiVersion()
        {
            return string.IsNullOrEmpty(_options.ApiVersion) ? "1.0.0.0" : _options.ApiVersion;
        }
    }

}
