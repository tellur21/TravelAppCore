using Application.DTOs;
using System.Text.Json;

namespace TravelAppUI.Services
{
    public class TravelPackageService : ITravelPackageService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TravelPackageService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public TravelPackageService(HttpClient httpClient, ILogger<TravelPackageService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<PagedResult<TravelPackageDto>> GetTravelPackagesAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/v1/travelpackages?page={page}&pageSize={pageSize}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<PagedResult<TravelPackageDto>>>(content, _jsonOptions);
                    
                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return apiResponse.Data;
                    }
                }
                
                _logger.LogWarning("Failed to get travel packages. Status: {StatusCode}", response.StatusCode);
                return new PagedResult<TravelPackageDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting travel packages");
                return new PagedResult<TravelPackageDto>();
            }
        }

        public async Task<TravelPackageDto?> GetTravelPackageByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/v1/travelpackages/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<TravelPackageDto>>(content, _jsonOptions);
                    
                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return apiResponse.Data;
                    }
                }
                
                _logger.LogWarning("Failed to get travel package {Id}. Status: {StatusCode}", id, response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting travel package {Id}", id);
                return null;
            }
        }
    }
}

