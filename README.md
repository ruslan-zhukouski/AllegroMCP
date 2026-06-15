# AllegroMCP - Unofficial Allegro MCP Server

![.NET 10](https://img.shields.io/badge/.NET-10-blue) ![License](https://img.shields.io/badge/license-MIT-green)

**AllegroMCP** is a Model Context Protocol (MCP) server for integrating with the Allegro e-commerce platform. It provides a comprehensive set of tools for managing your Allegro shop, including orders, offers, messages, returns, disputes, and more.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
  - [Docker Installation](#docker-installation)
  - [Local Installation](#local-installation)
- [Configuration](#configuration)
- [Usage](#usage)
  - [Authorization](#authorization)
  - [Supported Tools](#supported-tools)
- [Project Structure](#project-structure)
- [Development](#development)
- [Contributing](#contributing)
- [License](#license)

## Overview

AllegroMCP is an MCP server that bridges your applications with the Allegro API, allowing you to programmatically manage your e-commerce operations. The server exposes various tools for handling orders, offers, communications, returns, and shop quality metrics.

Built with .NET 10 and the MCP protocol, it can be deployed as a Docker container or run locally.

## Features

### 🛒 Order Management
- Get new orders

### 📦 Offer Management
- Get offers
- View offer details
- Manage offer pricing and stock
- Control offer publication status (start/stop offers)
- Link offers to products (EAN/GTIN)
- Monitor offer events

### 💬 Messaging
- Get user threads
- View thread details
- List messages within a thread

### 📋 Returns & Disputes
- View returned items (created, in-transit, delivered)
- Track claims status (submitted, accepted, rejected)

### 💳 Payment & Refunds
- Monitor account balance

### ⭐ Shop Quality
- View quality scores and metrics
- Track performance indicators
- Monitor recent score trend

### 🔐 Authorization
- OAuth 2.0 device authorization flow
- Token generation and refresh
- Token storage

## Prerequisites

### For Docker Deployment
- Docker
- Allegro API credentials (Client ID and Client Secret)

### For Local Development
- .NET 10 SDK
- Visual Studio 2026 or another compatible IDE
- Allegro API credentials (Client ID and Client Secret)

## Installation

### Docker Installation

#### 1. Set up Environment Variables

Create a `.env` file or set the following environment variables:

```bash
export ALLEGRO_CLIENT_ID="your_client_id_here"
export ALLEGRO_CLIENT_SECRET="your_client_secret_here"
```

#### 2. Using Docker Run

```bash
docker run -i --rm \
  -e ALLEGRO_CLIENT_ID \
  -e ALLEGRO_CLIENT_SECRET \
  --mount type=volume,src=allegromcp,target=/home/app \
  ruslanzhukouski/allegromcp
```

#### 3. Using Docker Compose

Create a `docker-compose.yml` file:

```yaml
version: '3.8'

services:
  allegromcp:
	image: ruslanzhukouski/allegromcp:latest
	environment:
	  - ALLEGRO_CLIENT_ID=${ALLEGRO_CLIENT_ID}
	  - ALLEGRO_CLIENT_SECRET=${ALLEGRO_CLIENT_SECRET}
	volumes:
	  - allegromcp:/home/app
	stdin_open: true
	tty: true

volumes:
  allegromcp:
```

Then run:

```bash
docker-compose up
```

### Local Installation

#### 1. Clone the Repository

```bash
git clone https://github.com/ruslan-zhukouski/AllegroMCP.git
cd AllegroMCP
```

#### 2. Set Up Environment Variables

```bash
# Windows (PowerShell)
$env:ALLEGRO_CLIENT_ID = "your_client_id"
$env:ALLEGRO_CLIENT_SECRET = "your_client_secret"

# Linux/macOS
export ALLEGRO_CLIENT_ID="your_client_id"
export ALLEGRO_CLIENT_SECRET="your_client_secret"
```

#### 3. Build the Solution

```bash
dotnet build
```

#### 4. Run the Host

```bash
dotnet run --project Host/Host.csproj
```

## Configuration

### Getting Allegro API Credentials

1. Log in to your Allegro seller account
2. Go to [Allegro application management](https://apps.developer.allegro.pl/)
3. Create a new application (without access to the browser) and note:
   - **Client ID**
   - **Client Secret**

### MCP Configuration

The server is configured via `.mcp.json`. For local development, you can modify it to run directly:

```json
{
  "servers": {
	"AllegroMCPServer": {
	  "command": "dotnet",
	  "args": [
		"run",
		"--project",
		"/path/to/Host/Host.csproj"
	  ]
	}
  }
}
```

## Usage

### Authorization

When you first start the server, you'll need to authorize it with your Allegro account:

1. **Get Authorization Code**
   - Call the authorization tool
   - Receive a user code and verification URL

2. **Authorize in Browser**
   - Visit the verification URL provided
   - Enter your user code
   - Grant permissions

3. **Generate Tokens**
   - The server will automatically generate and store your tokens
   - Tokens are saved securely in `/home/app/allegro_tokens.txt` (Docker) or locally

### Supported Tools

#### Authorization Tools
- `get_user_and_device_codes` - Initiate authorization flow
- `generate_tokens` - Generate access and refresh tokens
- `refresh_tokens` - Refresh expired tokens

#### Order Tools
- `get_new_orders` - Fetch new orders that are ready for processing

#### Offer Tools
- `get_offers` - List all your offers
- `get_offer` - Get details of a specific offer
- `get_selected_data_from_offer` - Get filtered offer data
- `get_stock_and_price_from_offer` - Get pricing and stock info
- `update_price` - Change offer price
- `update_stock` - Update available inventory
- `update_underlying_product` - Link offer to product (EAN/GTIN)
- `start_offer` - Publish an offer
- `stop_offer` - End an offer

#### Message Tools
- `get_threads` - List all message threads
- `get_thread` - Get specific thread details
- `list_messages` - List messages in a thread

#### Return Tools
- `get_returns` - List all sent returns that have not yet been processed by seller
- `get_created_returns` - Retrieve newly created returns that have not yet been sent 
- `get_in_transit_returns` - Get in-transit returns
- `get_delivered_returns` - Get delivered returns

#### Dispute & Claims Tools
- `get_issues` - List all disputes and claims
- `get_issue` - Get specific dispute/claim details
- `get_ongoing_disputes` - Get active disputes
- `get_unresolved_disputes` - Get unresolved disputes
- `get_closed_disputes` - Get closed disputes
- `get_submitted_claims` - Get submitted claims
- `get_accepted_claims` - Get accepted claims
- `get_rejected_claims` - Get rejected claims

#### Quality Tools
- `get_quality` - Get shop quality metrics and scores

#### Payment Tools
- `get_balance` - Check your account balance

#### Event Tools
- `get_offer_events` - Monitor offer activity changes

## Project Structure

```
AllegroMCP/
├── Host/                          # Entry point and configuration
│   ├── Host.csproj
│   └── Program.cs                 # MCP server initialization
│
├── Server/                        # Core server implementation
│   ├── Tools/                     # MCP tool implementations
│   │   ├── AuthorizationTools.cs
│   │   ├── OrderTools.cs
│   │   ├── OfferTools.cs
│   │   ├── MessageTools.cs
│   │   ├── IssueTools.cs
│   │   ├── EventTools.cs
│   │   ├── PaymentTools.cs
│   │   ├── QualityTools.cs
│   │   ├── SaleTools.cs
│   │   └── ToolsBase.cs
│   │
│   ├── Services/                  # Business logic and API integration
│   │   ├── ITokenProvider.cs
│   │   └── FileTokenProvider.cs
│   │
│   ├── Models/                    # Data models
│   │   ├── GetCodesResponse.cs
│   │   ├── GetTokensResponse.cs
│   │   └── GetPaymentOperationsResponse.cs
│   │
│   ├── Helpers/                   # Extension methods and utilities
│   │   └── IServiceCollectionExtensions.cs
│   │
│   └── Server.csproj
│
├── Server.Tests/                  # Unit tests
│   └── Server.Tests.csproj
│
├── .mcp.json                      # MCP server configuration
├── AllegroMCP.slnx                # Solution file
└── LICENSE                        # License file
└── README.md                      # This file
```

## Development

### Running Tests

```bash
dotnet test
```

### Building for Release

```bash
dotnet publish -c Release
```

### Code Structure

The project follows these architectural principles:

- **Tools**: MCP protocol tools that expose server capabilities
- **Services**: Abstraction layer for Allegro API integration
- **Models**: Data transfer objects and API responses
- **Helpers**: Extension methods and configuration utilities

### Adding New Tools

1. Create a new tool class in `Server/Tools/` and decorate it with `[McpServerToolType]` attribute
2. Inherit from `ToolsBase`
3. Implement tool methods with `[McpServerTool]` attribute

Example:

```csharp
[McpServerToolType]
public class MyNewTools : ToolsBase
{
	[McpServerTool]
	public MyResponse MyTool(MyRequest request)
	{
		// Implementation
		return new MyResponse();
	}
}
```

## Environment Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `ALLEGRO_CLIENT_ID` | Your Allegro API Client ID | `abc123def456` |
| `ALLEGRO_CLIENT_SECRET` | Your Allegro API Client Secret | `xyz789uvw012` |

## Troubleshooting

### Authorization Issues
- Ensure your Client ID and Client Secret are correct
- Check that your Allegro account has API access enabled
- Verify tokens haven't expired; refresh if necessary

### Connection Issues
- For Docker: Ensure the volume is properly mounted
- Check firewall settings if running on a network
- Verify environment variables are properly set

### Token Storage
- Docker: Tokens are stored in the `allegromcp` volume
- Local: Tokens are stored in the current working directory
- Never commit tokens to version control

## API Reference

For detailed information about the Allegro API, visit:
- [Allegro API Documentation](https://developer.allegro.pl/documentation)
- [Allegro Developer Portal](https://developer.allegro.pl/)

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For issues, questions, or feature requests, please:
- Open an issue on GitHub
- Check existing issues for similar problems
- Provide detailed information about your setup and the problem

## Changelog

### Version 1.0.0
- Initial release
- Partial Allegro API integration
- Docker support
- Comprehensive tool set for shop management

---

**Made with ❤️ for Allegro sellers**
