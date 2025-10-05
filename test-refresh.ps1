param(
  [string]$BaseUrl = 'http://localhost:5039/api/accounts'
)

$ErrorActionPreference = 'Stop'

$ErrorActionPreference = 'Stop'

Write-Host "Testing register -> login -> refresh..."

$base = $BaseUrl
$suffix = Get-Random -Minimum 10000 -Maximum 99999
$username = "mechatest_$suffix"
$email = "mechatest_$suffix@local.test"
$password = 'P@ssw0rd!123'

# Register
$registerBody = @{ 
  Username = $username
  Email = $email
  Password = $password
  Role = 'Customer'
} | ConvertTo-Json

Write-Host "REGISTER $username ..."
$regResponse = Invoke-RestMethod -Method Post -Uri ("$base/register") -ContentType 'application/json' -Body $registerBody -TimeoutSec 30
Write-Host "Registered."

# Login
$loginBody = @{ 
  Username = $username
  Password = $password
} | ConvertTo-Json

Write-Host "LOGIN ..."
$loginResponse = Invoke-RestMethod -Method Post -Uri ("$base/login") -ContentType 'application/json' -Body $loginBody -TimeoutSec 30
Write-Host "Login JSON:"
$loginResponse | ConvertTo-Json -Depth 6
$access = $loginResponse.accessToken
if (-not $access -and $loginResponse.value) { $access = $loginResponse.value.accessToken }
$refresh = $loginResponse.refreshToken
if (-not $refresh -and $loginResponse.value) { $refresh = $loginResponse.value.refreshToken }
if ($null -ne $access) { Write-Host "Logged in. Access token length:" ($access.Length) } else { Write-Host "No access token in login response" -ForegroundColor Yellow }

# Refresh
$refreshBody = @{ 
  AccessToken = $access
  RefreshToken = $refresh
} | ConvertTo-Json

Write-Host "REFRESH ..."
Write-Host "Refresh body JSON:"
$refreshBody | Out-String | Write-Host
$refreshResponse = Invoke-RestMethod -Method Post -Uri ("$base/refresh-token") -ContentType 'application/json' -Body $refreshBody -TimeoutSec 30
Write-Host "Refresh JSON:"
$refreshResponse | ConvertTo-Json -Depth 6
if ($null -ne $refreshResponse -and $null -ne $refreshResponse.accessToken) {
  Write-Host "Refreshed. New access token length:" ($refreshResponse.accessToken.Length)
} else {
  Write-Host "Refreshed. No access token in response." -ForegroundColor Yellow
}

# Output summary
"Username: $username"
"Email: $email"
"Login expiresAt: $($loginResponse.expiresAt)"
"Refresh expiresAt: $($refreshResponse.expiresAt)"

