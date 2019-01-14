FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["InvoiceBuilder.App/InvoiceBuilder.App.csproj", "InvoiceBuilder.App/"]
COPY ["InvoiceBuilder/InvoiceBuilder.csproj", "InvoiceBuilder/"]
COPY ["InvoiceBuilder.UseCase.Impl/InvoiceBuilder.UseCase.Impl.csproj", "InvoiceBuilder.UseCase.Impl/"]
COPY ["InvoiceBuilder.Report/InvoiceBuilder.Report.csproj", "InvoiceBuilder.Report/"]
COPY ["InvoiceBuilder.UseCase/InvoiceBuilder.UseCase.csproj", "InvoiceBuilder.UseCase/"]
COPY ["InvoiceBuilder.Repository/InvoiceBuilder.Repository.csproj", "InvoiceBuilder.Repository/"]
COPY ["InvoiceBuilder.Transaction/InvoiceBuilder.Transaction.csproj", "InvoiceBuilder.Transaction/"]
COPY ["InvoiceBuilder.Report.Impl/InvoiceBuilder.Report.Impl.csproj", "InvoiceBuilder.Report.Impl/"]
COPY ["InvoiceBuilder.Repository.Impl/InvoiceBuilder.Repository.Impl.csproj", "InvoiceBuilder.Repository.Impl/"]
RUN dotnet restore "InvoiceBuilder.App/InvoiceBuilder.App.csproj"
COPY . .
WORKDIR "/src/InvoiceBuilder.App"
RUN dotnet build "InvoiceBuilder.App.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "InvoiceBuilder.App.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "InvoiceBuilder.App.dll"]