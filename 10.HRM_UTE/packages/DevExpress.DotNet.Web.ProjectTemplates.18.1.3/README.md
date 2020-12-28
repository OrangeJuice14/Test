# DevExpress ASP.NET Core project templates

## Getting Started
1. Clone the project. 
2. Open the project's root folder.
3. Open console and type the following commands in it:
   - `nuget.exe pack`
   - `dotnet new -i DevExpress.DotNet.Web.ProjectTemplates.17.2.0.nupkg`
4. Replace in NuGet.config value on `http://nuget-w8/internal/nuget/`
5. In work folder type the following command:
	- `dotnet new dx.bootstrap` or 'dotnet new dx.dashboard'
	- `dotnet restore`
	- `dotnet run`

### Optional parameters: 
	- `-nf|--nuget-feed` private key
	- `-v|--version` devexpress controls version
	- '-ie|--include-example' include example files
	- '-au|--authentication'
		-- none: No authentication
		-- identity: Individual authentication
		-- windows: Windows authentication
	
### Examples: 
- `dotnet new dx.bootstrap -au windows -v 17.2`
- `dotnet new dx.bootstrap -au identity -k 000000-000000-00000`
