$connectionString = "Server=msdocs-core-sql-001-server.mysql.database.azure.com;Database=msdocs-core-sql-001-database;Port=3306;User Id=wbhiibduhl;Password=Q02476A4O324141U$;SSL Mode=Required"

Import-Module SqlServer

$query = "SELECT * FROM mytable"

Invoke-SqlCmd -ConnectionString $connectionString -Query $query




<#
$username = "wbhiibduhl"
$password = "Q02476A4O324141U$"

$securePassword = ConvertTo-SecureString $password -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential ($username, $securePassword)

Test-AzMySqlFlexibleServerConnect `
    -Name "msdocs-core-sql-001-server" `
    -ResourceGroupName "myResourceGroup" `
    -AdministratorUserName $username `
    -AdministratorLoginPassword $securePassword `
    -DatabaseName "msdocs-core-sql-001-database"


#>