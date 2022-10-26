param(
    [Parameter(Mandatory=$true)][string]$testName,
    [Parameter(Mandatory=$true)][string]$testId,
    [Parameter(Mandatory=$true)][string]$apiKey,
    [Parameter()][int]$pollingSeconds = 60,
    [Parameter()][int]$maxPollingSeconds = 300,
    [Parameter()][string]$scriptPath = '',
    # use scale factor to scale the number of test cases, to minimise the number of entries and avoid generating large test results files;
    # default is 1  
    [Parameter()][int]$scaleFactor = 1,
    [ValidateSet('JUnit', 'VSTest')]
    [Parameter()][string]$testResultsFormat = 'JUnit'
)

if($scriptPath -ne '') {
  . $scriptPath/Get-JUnit.ps1
  . $scriptPath/Get-VSTest.ps1
}
else {
  . ./Get-JUnit.ps1
  . ./Get-VSTest.ps1
}

$url = "https://api.loader.io/v2/tests/$testId/results"
$d = Get-Date -Format "yyyyMMdd_HHmmss"

# the following GET endpoint can be used to trigger a test, however it is recommended to use he trun webhook
$resultJson = Invoke-WebRequest -Headers @{'loaderio-auth' = $apiKey} -Uri "$url" -UseBasicParsing
$results = ConvertFrom-Json $resultJson.Content
$filteredResults = @($results | Where-Object { $_.status -eq "not_ready" })
$sd = Get-Date
# poll until all results are ready or $maxPollingSeconds timeout exceeded - decide what to do
while ($filteredResults.Count -gt 0) {
    Start-Sleep -s $pollingSeconds
    $resultJson = Invoke-WebRequest -Headers @{'loaderio-auth' = $apiKey} -Uri "$url" -UseBasicParsing
    $results = ConvertFrom-Json $resultJson.Content
    $filteredResults = $results | Where-Object { $_.status -eq "not_ready" }
    $ed = Get-Date
    if(($ed - $sd).Seconds -ge $maxPollingSeconds) {
        Write-Output "Test timed out"
        break
    }
}

# extract last result and save it as JSON file
$testResult = ($results | Sort-Object -Property "started_at" -Descending)[0]
Write-Output $testResult
New-Item -Path tmp -Type Directory -Force
$jsonFilePath = "./tmp/$testName-$d.json"
Set-Content -Path $jsonFilePath  -Value ($testResult | ConvertTo-Json)

# # default Xml conversion - not suitable for Test results
# $xml = $testResult | ConvertTo-Xml
# $xml.OuterXml

$sOutput = ''
$extension = 'txt'
switch ($testResultsFormat) {
    'JUnit' {
      $sOutput = Get-JUnit -testName $testName -testResult $testResult
      $extension = 'xml'
    }
    'VSTest' {
      $sOutput = Get-VSTest -testName $testName -testResult $testResult
      $extension = 'trx'
    }
}

$xmlFilePath = "./tmp/$testName-$d.$extension"
Set-Content -Path $xmlFilePath  -Value $sOutput

