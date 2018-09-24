REM cd C:\MyFiles\_CodeProjectMicroServices\SalesOrderManagement
REM cd CodeProject.SalesOrderManagement.WebApi\bin\debug\netcoreapp2.1
REM setx ASPNETCORE_ENVIRONMENT "Development"
REM dotnet codeproject.SalesOrdermanagement.webapi.dll --verbosity detailed --launch-profile "CodeProject.SalesOrderManagement.WebApi"



cd C:\MyFiles\_CodeProjectMicroServices\SalesOrderManagement
cd CodeProject.SalesOrderManagement.WebApi
dotnet run --verbosity m --launch-profile CodeProject.SalesOrderManagement.WebApi --no-build

