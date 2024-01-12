using System.Text;
using System.Text.Json;

namespace Dashboard.Blazor.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<T>> GetAllAsync<T>(string endPoint) where T : class
    {
        var responseMessage = await _httpClient.GetAsync(endPoint);

        if (responseMessage.IsSuccessStatusCode)
        {
            var responseObjects = await responseMessage.Content.ReadFromJsonAsync<List<T>>();

            return new ApiResponse<T>
            {
                IsSuccess = true,
                ObjectsList = responseObjects
            };
        }

        return await GetErrorAsync<T>(responseMessage);
    }

    public async Task<ApiResponse<T>> GetByIdAsync<T>(string endPoint) where T : class
    {
        HttpResponseMessage responseMessage = new();

        try
        {
            responseMessage = await _httpClient.GetAsync(endPoint);
        }
        catch
        {
            return await GetErrorAsync<T>(responseMessage);
        }

        if (responseMessage.IsSuccessStatusCode)
            return await GetResponseMessage<T>(responseMessage);

        return await GetErrorAsync<T>(responseMessage);
    }

    public async Task<ApiResponse<T>> AddAsync<T>(string endPoint, T model) where T : class
    {

        HttpResponseMessage responseMessage = new();

        try
        {
            responseMessage = await _httpClient.PostAsJsonAsync(endPoint, model);
        }
        catch
        {
            return await GetErrorAsync<T>(responseMessage);
        }

        if (responseMessage.IsSuccessStatusCode)
            return await GetResponseMessage<T>(responseMessage);

        return await GetErrorAsync<T>(responseMessage);
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string endPoint) where T : class
    {
        HttpResponseMessage responseMessage = new();

        try
        {
            responseMessage = await _httpClient.DeleteAsync(endPoint);
        }
        catch
        {
            return await GetErrorAsync<T>(responseMessage);
        }

        if (responseMessage.IsSuccessStatusCode)
            return await GetResponseMessage<T>(responseMessage);

        return await GetErrorAsync<T>(responseMessage);
    }

    public async Task<ApiResponse<T>> DeleteAllAsync<T>(string endPoint, List<int> deletedIds) where T : class
    {
        HttpResponseMessage responseMessage = new();

        try
        {
            // Serialize the list of deletedIds to JSON
            string jsonBody = JsonSerializer.Serialize(deletedIds);

            // Create the HTTP request content
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Create the DELETE request with the content
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"https://api.pestsweepy.com/{endPoint}"),
                Content = content
            };

            // Send the DELETE request
            responseMessage = await _httpClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            return await GetErrorAsync<T>(responseMessage, ex);
        }

        if (responseMessage.IsSuccessStatusCode)
            return await GetResponseMessage<T>(responseMessage);

        return await GetErrorAsync<T>(responseMessage);
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string endPoint, HttpContent? model = null) where T : class
    {
        HttpResponseMessage responseMessage = new();

        try
        {
            responseMessage = await _httpClient.PostAsync(endPoint, model);
        }
        catch
        {
            return await GetErrorAsync<T>(responseMessage);
        }

        if (responseMessage.IsSuccessStatusCode)
            return await GetResponseMessage<T>(responseMessage);

        return await GetErrorAsync<T>(responseMessage);
    }

    public async Task<ApiResponse<T>> UpdateAsync<T>(string endPoint, T model) where T : class
    {
        HttpResponseMessage responseMessage = new();

        try
        {
            responseMessage = await _httpClient.PutAsJsonAsync(endPoint, model);
        }
        catch
        {
            return await GetErrorAsync<T>(responseMessage);
        }

        if (responseMessage.IsSuccessStatusCode)
            return await GetResponseMessage<T>(responseMessage);

        return await GetErrorAsync<T>(responseMessage);
    }

    private static async Task<ApiResponse<T>> GetResponseMessage<T>(HttpResponseMessage responseMessage) where T : class
    {
        var responseObject = await responseMessage.Content.ReadFromJsonAsync<T>();

        return new ApiResponse<T>
        {
            IsSuccess = true,
            Object = responseObject
        };
    }

    private static async Task<ApiResponse<T>> GetErrorAsync<T>(HttpResponseMessage responseMessage, Exception? exception = null) where T : class
    {
        string error = await responseMessage.Content.ReadAsStringAsync() ?? string.Empty;

        if (exception is not null)
        {
            error = $"{error}, {exception.Message}";
        }

        return new ApiResponse<T>
        {
            IsSuccess = false,
            StatusCode = responseMessage.StatusCode.ToString(),
            Error = error
        };
    }
}
