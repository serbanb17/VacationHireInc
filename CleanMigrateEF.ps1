dotnet tool install --global dotnet-ef --version 5.0.10
Remove-Item -LiteralPath "VacationHireInc.DataLayer\Migrations" -Force -Recurse
dotnet ef migrations add Initial  --project VacationHireInc.DataLayer -- "Server=localhost,44001;Database=VacationHireInc;User Id=sa;Password=sqlDb123!;" "qYdx6HDQM7c4d4OxZHLytaiPvZ7sEsXy"
dotnet ef database update --project VacationHireInc.DataLayer -- "Server=localhost,44001;Database=VacationHireInc;User Id=sa;Password=sqlDb123!;" "qYdx6HDQM7c4d4OxZHLytaiPvZ7sEsXy"