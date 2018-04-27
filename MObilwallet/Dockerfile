FROM microsoft/aspnetcore-build:2.0
COPY out /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet","MobilWallet.dll"]