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

    public async Task<ApiResponse<T>> ChangeStatusAsync<T>(string endPoint) where T : class
    {
        var responseMessage = await _httpClient.PutAsync(endPoint, null);

        if (responseMessage.IsSuccessStatusCode)
        {
            var message = await responseMessage.Content.ReadAsStringAsync();

            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message
            };
        }

        return await GetErrorAsync<T>(responseMessage);
    }

    public async Task<ApiResponse<T>> GetByIdAsync<T>(string endPoint) where T : class
    {
        var responseMessage = await _httpClient.GetAsync(endPoint);

        if (responseMessage.IsSuccessStatusCode)
            return await GetResponseMessage<T>(responseMessage);

        return await GetErrorAsync<T>(responseMessage);
    }

    public async Task<ApiResponse<T>> AddAsync<T>(string endPoint, T model) where T : class
    {
        var responseMessage = await _httpClient.PostAsJsonAsync(endPoint, model);

        if (responseMessage.IsSuccessStatusCode)
            return await GetResponseMessage<T>(responseMessage);

        return await GetErrorAsync<T>(responseMessage);
    }

    public async Task<ApiResponse<T>> UpdateAsync<T>(string endPoint, T model) where T : class
    {
        var responseMessage = await _httpClient.PutAsJsonAsync(endPoint, model);

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

    private static async Task<ApiResponse<T>> GetErrorAsync<T>(HttpResponseMessage responseMessage) where T : class
    {
        var error = await responseMessage.Content.ReadAsStringAsync();

        return new ApiResponse<T>
        {
            IsSuccess = false,
            StatusCode = responseMessage.StatusCode.ToString(),
            Error = error
        };
    }
}
