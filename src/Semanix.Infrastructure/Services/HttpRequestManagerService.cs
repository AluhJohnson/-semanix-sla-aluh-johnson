using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Semanix.Infrastructure.Services;

public class HttpRequestManagerService : IHttpRequestManagerService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpRequestManagerService> _logger;
    private static int _defaultTimeOut;
    public HttpRequestManagerService(ILogger<HttpRequestManagerService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _defaultTimeOut = int.Parse(configuration["timeout"] ?? "60000");
        _httpClient = new HttpClient();
    }
    public async Task<T> Get<T>(string requestUri, Dictionary<string, string>? httpHeaders, int timeout = 0) where T : class, new()
    {
        timeout = CheckTimeout(timeout);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        //Set headers
        if (httpHeaders != null && httpHeaders.Any())
        {
            foreach (var header in httpHeaders)
            {
                if (_httpClient.DefaultRequestHeaders.Contains(header.Key))
                    _httpClient.DefaultRequestHeaders.Remove(header.Key);
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        var cancellationToken = new CancellationTokenSource();
        cancellationToken.CancelAfter(timeout);

        try
        {
            var response = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Token);
            var content = await response.Content.ReadAsStringAsync();

            _logger.LogDebug("Response from {RequestUri} was {Content}", requestUri, content);

            if (string.IsNullOrWhiteSpace(content))
                return SetHttpResponseMessageOnResponseObject(default(T), response)!;


            var responseObject = JsonConvert.DeserializeObject<T>(content);
            responseObject = SetHttpResponseMessageOnResponseObject(responseObject, response);
            return responseObject!;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred ---- {Message}", ex.Message);
        }
        return default(T)!;
    }

    private T SetHttpResponseMessageOnResponseObject<T>(T responseObject, HttpResponseMessage responseMessage)
    {
        if (responseObject == null)
        {
            responseObject = Activator.CreateInstance<T>();
        }
        try
        {
            var type = typeof(T);
            if (!type.GetInterfaces().Contains(typeof(IHttpResponseMessage)))
            {
                return responseObject;
            }

            var prop = responseObject!.GetType()
                .GetProperty(nameof(IHttpResponseMessage.ResponseMessage), BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(responseObject, responseMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception occured while trying to set HttpResponseMessage on response object. <br/> ExceptionMessage : {Message}", ex.Message);
        }

        return responseObject;
    }

    public async Task<T> Send<T>(HttpMethod method, string requestUri, object? requestData, Dictionary<string, string>? httpHeaders, int timeout = 0) where T : class, new()
    {
        try
        {
            timeout = CheckTimeout(timeout);

            var httpRequestMessage = new HttpRequestMessage(method, requestUri);
            var contentType = requestUri.Contains("SendMail") ? "multipart/form-data" : "application/json";
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            var serializationSetting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            if (requestData != null)
            {
                var jsonData = JsonConvert.SerializeObject(requestData, serializationSetting);
                httpRequestMessage.Content = new StringContent(jsonData, Encoding.UTF8, contentType);
            }

            //Set headers
            if (httpHeaders != null && httpHeaders.Any())
            {
                foreach (var header in httpHeaders)
                {
                    if (_httpClient.DefaultRequestHeaders.Contains(header.Key))
                        _httpClient.DefaultRequestHeaders.Remove(header.Key);
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            var cancellationToken = new CancellationTokenSource();
            cancellationToken.CancelAfter(timeout);

            _logger.LogInformation("HttpRequestManagerService : SendAsync ==> About to call SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Token)");

            var response = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Token);
            var content = await response.Content.ReadAsStringAsync(cancellationToken.Token);

            _logger.LogDebug("HttpRequestManagerService : SendAsync ==> After call About to call SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Token) to Response from {RequestUri} was {Content}", requestUri, content);

            if (string.IsNullOrWhiteSpace(content))
                return SetHttpResponseMessageOnResponseObject(default(T), response)!;

            var responseObject = JsonConvert.DeserializeObject<T>(content);

            responseObject = SetHttpResponseMessageOnResponseObject(responseObject, response);
            return responseObject!;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred ---- {Message}", ex.Message);
        }
        return default(T)!;
    }

    public async Task<T> SendRequest<T>(HttpMethod method, HttpClientHandler httpClientHandler, string requestUri, object? requestData, Dictionary<string, string>? httpHeaders, int timeout = 0) where T : class, new()
    {
        try
        {
            timeout = CheckTimeout(timeout);

            var httpRequestMessage = new HttpRequestMessage(method, requestUri);
            var contentType = requestUri.Contains("SendMail") ? "multipart/form-data" : "application/json";
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            var serializationSetting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            if (requestData != null)
            {
                var jsonData = JsonConvert.SerializeObject(requestData, serializationSetting);
                httpRequestMessage.Content = new StringContent(jsonData, Encoding.UTF8, contentType);
            }

            //Set headers
            if (httpHeaders != null && httpHeaders.Any())
            {
                foreach (var header in httpHeaders)
                {
                    if (_httpClient.DefaultRequestHeaders.Contains(header.Key))
                        _httpClient.DefaultRequestHeaders.Remove(header.Key);
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            var cancellationToken = new CancellationTokenSource();
            cancellationToken.CancelAfter(timeout);

            _logger.LogInformation("HttpRequestManagerService : SendAsync ==> About to call SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Token)");

            var response = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Token);
            var content = await response.Content.ReadAsStringAsync(cancellationToken.Token);

            _logger.LogDebug("HttpRequestManagerService : SendAsync ==> After call About to call SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Token) to Response from {RequestUri} was {Content}", requestUri, content);

            if (string.IsNullOrWhiteSpace(content))
                return SetHttpResponseMessageOnResponseObject(default(T), response)!;

            var responseObject = JsonConvert.DeserializeObject<T>(content);

            responseObject = SetHttpResponseMessageOnResponseObject(responseObject, response);
            return responseObject!;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred ---- {Message}", ex.Message);
        }
        return default(T)!;
    }

    private static int CheckTimeout(int timeout)
    {
        if (timeout <= 0)
        {
            timeout = _defaultTimeOut; //60000;
        }

        return timeout;
    }

    public string GetXTokenHeader(string clientId, string password)
    {
        var utcdate = DateTime.UtcNow;
        var date = utcdate.ToString("yyyy-MM-ddHHmmss");
        var data = date + clientId + password;
        return Sha512(data);
    }

    public string GetUtcTimestamp()
    {
        var utcdate = DateTime.UtcNow;
        var date = utcdate.ToString("yyyy-MM-ddHHmmss");
        return date;
    }

    private static string Sha512(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        using var hash = System.Security.Cryptography.SHA512.Create();
        var hashedInputBytes = hash.ComputeHash(bytes);

        // Convert to text
        // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte
        var hashedInputStringBuilder = new StringBuilder(128);
        foreach (var b in hashedInputBytes)
            hashedInputStringBuilder.Append(b.ToString("x2"));
        return hashedInputStringBuilder.ToString();
    }
}