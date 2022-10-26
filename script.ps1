$url = 'https://localhost:44341/api/Calculator/build' # wi'
$url = 'https://ets-demo-api-dev.azurewebsites.net/api/calculator/build'
$url = 'https://awe-apim.azure-api.net/ets-demo-api/api/calculator/build'
$body = @{
    id = '2';
    name = "PBI 002";
    t = @{ p1 = 0; p2 = "XYZ"; p3 = $true; p4 = Get-Date }
} | ConvertTo-Json -Depth 10
$method = 'POST'
$contentType = 'application/json'
$body = Get-Content -Path "C:\Data\DAWR\build.json"
$headers = @{
    "Ocp-Apim-Subscription-Key" = "daeb596051764692aeb93711f8653a02";
    "Ocp-Apim-Trace" = $true
}
Invoke-RestMethod -Uri $url -Headers $headers -Method $method -Body $body -ContentType $contentType


$url = 'https://awe-apim.azure-api.net/ets-demo-api/api/calculator/wi'

$url = 'https://awe-apim.azure-api.net/ets-demo-api/api/calculator/result?expression=1%2B2'
Invoke-RestMethod -Uri $url -Headers $headers
$url = 'https://ets-demo-api-dev.azurewebsites.net/api/calculator/result?expression=1%2B2'
Invoke-RestMethod -Uri $url

$certName = 'MININT-4A72GTP'
$tenant = 'MININT-4A72GTP'
$cert = New-SelfSignedCertificate -DnsName $tenant -CertStoreLocation "Cert:\CurrentUser\My" -FriendlyName $certName
$thumbprint = $cert.Thumbprint
Get-ChildItem Cert:\CurrentUser\my\$thumbprint | Export-Certificate -FilePath C:\temp\$certName.cer

New-SelfSignedCertificate -DnsName $tenant -CertStoreLocation cert:\LocalMachine\My -FriendlyName $certName

$name = 'MININT-4A72GTP'
New-SelfSignedCertificate -NotBefore (Get-Date) -NotAfter (Get-Date).AddYears(1) -Subject $name -KeyAlgorithm "RSA" `
    -KeyLength 2048 -HashAlgorithm "SHA256" -CertStoreLocation "Cert:\CurrentUser\My" -KeyUsage KeyEncipherment `
    -FriendlyName "HTTPS development certificate" -TextExtension @("2.5.29.19={critical}{text}","2.5.29.37={critical}{text}1.3.6.1.5.5.7.3.1","2.5.29.17={critical}{text}DNS=$name")

[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}



