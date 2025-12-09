# SilkRoad Travel UI

A modern ASP.NET Core Razor Pages application that consumes the SilkRoad Travel API to provide a beautiful user interface for browsing and booking travel packages.

## Features

- **Home Page**: Welcome page with hero section and feature highlights
- **Travel Packages**: Browse all available travel packages with pagination
- **Package Details**: Detailed view of individual packages with booking functionality
- **Contact Form**: Contact page with form submission
- **Responsive Design**: Built with Bootstrap 5 for mobile-first responsive design
- **Modern UI**: Clean, professional design with Font Awesome icons

## Technology Stack

- **.NET 9.0**: Latest .NET framework
- **ASP.NET Core Razor Pages**: Server-side rendering
- **Bootstrap 5**: CSS framework for responsive design
- **Font Awesome**: Icon library
- **HTTP Client**: For API communication
- **JSON Serialization**: For data handling

## Project Structure

```
TravelAppUI/
├── Models/                 # Data models and DTOs
│   ├── TravelPackage.cs
│   ├── ApiResponse.cs
│   ├── PagedResult.cs
│   └── BookingRequest.cs
├── Services/               # API service classes
│   ├── ITravelPackageService.cs
│   ├── TravelPackageService.cs
│   ├── IBookingService.cs
│   └── BookingService.cs
├── Pages/                  # Razor Pages
│   ├── Index.cshtml       # Home page
│   ├── Packages.cshtml    # Travel packages listing
│   ├── PackageDetails.cshtml # Package details and booking
│   ├── Contact.cshtml     # Contact form
│   └── Shared/
│       └── _Layout.cshtml # Main layout
├── wwwroot/               # Static files
│   ├── css/
│   │   └── site.css      # Custom styles
│   └── lib/              # Client-side libraries
└── Program.cs            # Application configuration
```

## Setup Instructions

### Prerequisites

- .NET 9.0 SDK or later
- Visual Studio 2022 or VS Code
- Access to the SilkRoad Travel API (https://www.SilkRoad.travel)

### Configuration

1. **API Base URL Configuration**
   
   The API base URL is configured in `appsettings.json`:
   ```json
   {
     "ApiSettings": {
       "BaseUrl": "https://www.SilkRoad.travel"
     }
   }
   ```

2. **Development Configuration**
   
   For development, you can override the API URL in `appsettings.Development.json`:
   ```json
   {
     "ApiSettings": {
       "BaseUrl": "https://localhost:7001"
     }
   }
   ```

### Running the Application

1. **Navigate to the project directory**:
   ```bash
   cd src/TravelAppUI
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

4. **Open in browser**:
   Navigate to `https://localhost:5001` or `http://localhost:5000`

### Building for Production

1. **Build the application**:
   ```bash
   dotnet build --configuration Release
   ```

2. **Publish the application**:
   ```bash
   dotnet publish --configuration Release --output ./publish
   ```

## API Integration

The application integrates with the following API endpoints:

### Travel Packages
- `GET /api/v1/travelpackages` - Get paginated list of travel packages
- `GET /api/v1/travelpackages/{id}` - Get specific travel package details

### Bookings
- `POST /api/v1/bookings` - Create a new booking

### Error Handling

The application includes comprehensive error handling:
- API communication errors are logged and handled gracefully
- User-friendly error messages are displayed
- Fallback content is shown when API is unavailable

## Customization

### Styling

Custom styles are defined in `wwwroot/css/site.css`:
- Color scheme: Purple gradient theme
- Card hover effects
- Responsive design adjustments
- Custom button and navigation styling

### Adding New Pages

1. Create a new `.cshtml` file in the `Pages` directory
2. Create the corresponding `.cshtml.cs` code-behind file
3. Add navigation links in `_Layout.cshtml`

### Adding New API Endpoints

1. Create new service interfaces and implementations in the `Services` directory
2. Register the services in `Program.cs`
3. Inject the services into your page models

## Troubleshooting

### Common Issues

1. **API Connection Issues**
   - Verify the API base URL in `appsettings.json`
   - Check if the API is running and accessible
   - Review browser developer tools for network errors

2. **Build Errors**
   - Ensure .NET 9.0 SDK is installed
   - Run `dotnet restore` to restore packages
   - Check for any missing dependencies

3. **Styling Issues**
   - Verify Bootstrap and Font Awesome are loading correctly
   - Check browser console for CSS loading errors
   - Ensure custom CSS is not conflicting with Bootstrap

### Logging

The application uses the built-in .NET logging framework. Logs are written to the console by default. For production, configure logging in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "TravelAppUI": "Debug"
    }
  }
}
```

## Future Enhancements

- User authentication and authorization
- User dashboard for managing bookings
- Payment integration
- Email notifications
- Advanced search and filtering
- Multi-language support
- Admin panel for package management

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is part of the SilkRoad Travel Core solution.

