# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration) by command bellow:

# docker build -f "E:\DotNetTemplatesApi\WebControllerApi\Dockerfile" -t webcontrollerapi:dev --label "com.microsoft.created-by=visual-studio" --label "com.microsoft.visual-studio.project-name=WebControllerApi" --target base  --build-arg "BUILD_CONFIGURATION=Debug" --build-arg "LAUNCHING_FROM_VS=true" "E:\DotNetTemplatesApi"

# docker run -dt -v "C:\Users\Admin\vsdbg\vs2017u5:/remote_debugger:rw" -v "C:\Users\Admin\AppData\Roaming\Microsoft\UserSecrets:/root/.microsoft/usersecrets:ro" -v "C:\Users\Admin\AppData\Roaming\Microsoft\UserSecrets:/home/app/.microsoft/usersecrets:ro" -v "C:\Users\Admin\AppData\Roaming\ASP.NET\Https:/root/.aspnet/https:ro" -v "C:\Users\Admin\AppData\Roaming\ASP.NET\Https:/home/app/.aspnet/https:ro" -v "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Sdks\Microsoft.Docker.Sdk\tools\linux-x64\net6.0:/VSTools:ro" -v "C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\CommonExtensions\Microsoft\HotReload:/HotReloadAgent:ro" -v "C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\extensions\Microsoft\managedprojectsystem\HotReload:/HotReloadAgent2:ro" -v "E:\DotNetTemplatesApi\WebControllerApi:/app:rw" -v "E:\DotNetTemplatesApi:/src/:rw" -v "C:\Users\Admin\.nuget\packages:/.nuget/fallbackpackages2:rw" -v "C:\Program Files (x86)\Microsoft Visual Studio\Shared\NuGetPackages:/.nuget/fallbackpackages:rw" -e "ASPNETCORE_LOGGING__CONSOLE__DISABLECOLORS=True" -e "ASPNETCORE_ENVIRONMENT=Development" -e "DOTNET_USE_POLLING_FILE_WATCHER=1" -e "NUGET_PACKAGES=/.nuget/fallbackpackages2" -e "NUGET_FALLBACK_PACKAGES=/.nuget/fallbackpackages;/.nuget/fallbackpackages2" -e "ASPNETCORE_HTTPS_PORTS=8081" -e "ASPNETCORE_HTTP_PORTS=8080" -P -l "com.microsoft.visualstudio.launch-url.path-query=/swagger" --name WebControllerApi --entrypoint "dotnet" webcontrollerapi:dev --roll-forward Major /VSTools/DistrolessHelper/DistrolessHelper.dll --wait 

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#/src/WebControllerApi/WebControllerApi.csproj
# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/WebControllerApi/WebControllerApi.csproj", "WebControllerApi/"]
COPY ["/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire.ServiceDefaults/"]
# COPY CPM
COPY ["Directory.Packages.props", "Directory.Packages.props"]
RUN dotnet restore "./Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj"
RUN dotnet restore "./WebControllerApi/WebControllerApi.csproj"
# COPY Source code
COPY ["/Aspire.ServiceDefaults/", "Aspire.ServiceDefaults/"]
COPY ["/WebControllerApi/", "WebControllerApi/"]
WORKDIR "/src/WebControllerApi"
RUN dotnet build "./WebControllerApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebControllerApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebControllerApi.dll"]