using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semanix.Common.CustomException
{
    public class RequestHeader
    {
        private readonly HttpRequest _httpRequest;
        private readonly HttpContext _httpContext;
        public RequestHeader(HttpContext context)
        {
            _httpRequest = context.Request;
            _httpContext = context;
        }
        public string? IpAddress
        {
            get
            {
                if (_httpRequest.HttpContext.Connection.RemoteIpAddress != null)
                {
                    return _httpRequest.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                else { return string.Empty; }

            }
        }

        public string? ClientId
        {
            get
            {
                var value = _httpContext.Request.Headers[ClientIdKey];
                return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
            }
        }
        public string ClientIdKey
        {
            get
            {
                var key = _httpRequest.Headers.Keys.FirstOrDefault(n => n.ToLower().Equals("client_id"));
                return string.IsNullOrWhiteSpace(key) ? string.Empty : key;
            }
        }
        public string? CorrelationId
        {
            get
            {
                var value = _httpContext.Request.Headers[CorrelationIdKey];
                return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
            }
        }
        public string CorrelationIdKey
        {
            get
            {
                var key = _httpRequest.Headers.Keys.FirstOrDefault(n => n.ToLower().Equals("x-correlation-id"));
                return string.IsNullOrWhiteSpace(key) ? string.Empty : key;
            }
        }
    }
}
