REM cd C:\MyFiles\_CodeProjectMicroServices\InventoryManagement
REM cd CodeProject.InventoryManagement.WebApi\bin\debug\netcoreapp2.1
REM setx ASPNETCORE_ENVIRONMENT "Development"
REM dotnet codeproject.Inventorymanagement.webapi.dll --verbosity detailed --launch-profile "CodeProject.InventoryManagement.WebApi"


cd C:\MyFiles\_CodeProjectMicroServices\InventoryManagement
cd CodeProject.InventoryManagement.WebApi
dotnet run --verbosity m --launch-profile CodeProject.InventoryManagement.WebApi --no-build

