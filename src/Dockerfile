FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR .
COPY ["Ildar.Wallet.Bot/Ildar.Wallet.Bot.csproj", "Ildar.Wallet.Bot/"]
COPY ["Ildar.Wallet.Bot.AppServices/Ildar.Wallet.Bot.AppServices.csproj", "Ildar.Wallet.Bot.AppServices/"]
COPY ["Ildar.Wallet.Bot.Common/Ildar.Wallet.Bot.Common.csproj", "Ildar.Wallet.Bot.Common/"]
COPY ["Ildar.Wallet.Bot.Contracts/Ildar.Wallet.Bot.Contracts.csproj", "Ildar.Wallet.Bot.Contracts/"]
COPY ["Ildar.Wallet.Bot.DataAccess/Ildar.Wallet.Bot.DataAccess.csproj", "Ildar.Wallet.Bot.DataAccess/"]
COPY ["Ildar.Wallet.Bot.Domain/Ildar.Wallet.Bot.Domain.csproj", "Ildar.Wallet.Bot.Domain/"]
RUN dotnet restore "Ildar.Wallet.Bot/Ildar.Wallet.Bot.csproj"
COPY . .
WORKDIR "/Ildar.Wallet.Bot"
RUN dotnet build "Ildar.Wallet.Bot.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Ildar.Wallet.Bot.csproj" -c Release -o /app

FROM runtime AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ildar.Wallet.Bot.dll"]