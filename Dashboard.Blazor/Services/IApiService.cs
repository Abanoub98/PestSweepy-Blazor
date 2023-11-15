namespace Dashboard.Blazor.Services;

public interface IApiService
{
    Task<ApiResponse<T>> GetAllAsync<T>(string endPoint) where T : class;
    Task<ApiResponse<T>> GetByIdAsync<T>(string endPoint) where T : class;
    Task<ApiResponse<T>> AddAsync<T>(string endPoint, T model) where T : class;
    Task<ApiResponse<T>> UpdateAsync<T>(string endPoint, T model) where T : class;
    Task<ApiResponse<T>> DeleteAsync<T>(string endPoint) where T : class;
    Task<ApiResponse<T>> PostAsync<T>(string endPoint, HttpContent? model = null) where T : class;
}
