FROM microsoft/dotnet:2.2-sdk AS build

# copy everything and build
COPY . /app
WORKDIR /app
RUN dotnet publish -c Release -r linux-x64 -o /release Web

# build runtime image
FROM microsoft/dotnet:2.2-runtime


COPY --from=build /release /app

WORKDIR /app
ENTRYPOINT ["dotnet", "Web.dll"]