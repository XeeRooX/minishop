FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["minishop/minishop.csproj", "minishop/"]
RUN dotnet restore "minishop/minishop.csproj"
COPY . .
WORKDIR "/src/minishop"
RUN dotnet build "minishop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "minishop.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir -p /app/Data

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "minishop.dll"]
