if [! -d "bin"] (
    dotnet build 
)
dotnet run --no-build