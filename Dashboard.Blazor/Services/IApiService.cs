namespace Dashboard.Blazor.Services;

public interface IApiService
{
    Task<ApiResponse<T>> GetAllAsync<T>(string endPoint) where T : class;
    Task<ApiResponse<T>> GetByIdAsync<T>(string endPoint) where T : class;
    Task<ApiResponse<T>> ChangeStatusAsync<T>(string endPoint) where T : class;
    Task<ApiResponse<T>> AddAsync<T>(string endPoint, T model) where T : class;
    Task<ApiResponse<T>> UpdateAsync<T>(string endPoint, T model) where T : class;
    Task<ApiResponse<T>> PostAsync<T>(string endPoint) where T : class;
}
