using Application.DTOs;
using Application.Features.Bookings.Commands;
using System.Text;
using System.Text.Json;

namespace TravelAppUI.Services
{
    public class BookingService : IBookingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookingService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public BookingService(HttpClient httpClient, ILogger<BookingService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<object>> CreateBookingAsync(CreateBookingCommand createBookingCommand)
        {
            try
            {
                var json = JsonSerializer.Serialize(createBookingCommand, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("api/v1/bookings", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);
                
                if (apiResponse == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Failed to parse response from server"
                    };
                }
                
                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating booking");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your booking request"
                };
            }
        }
    }
}

