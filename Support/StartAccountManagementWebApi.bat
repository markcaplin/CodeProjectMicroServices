cd C:\MyFiles\_CodeProjectMicroServices\AccountManagement
cd CodeProject.AccountManagement.WebApi
REM cd CodeProject.AccountManagement.WebApi\bin\debug\netcoreapp2.1
REM setx ASPNETCORE_ENVIRONMENT "Development"
dir
REM dotnet codeproject.accountmanagement.webapi.dll --verbosity detailed --launch-profile "CodeProject.AccountManagement.WebApi"
dotnet run --verbosity m --launch-profile CodeProject.AccountManagement.WebApi
pause

