param(
    [Parameter(Mandatory=$true)][string]$testName,
    [Parameter(Mandatory=$true)][string]$testId,
    [Parameter(Mandatory=$true)][string]$testToken,
    [Parameter(Mandatory=$true)][string]$apiKey,
    [Parameter()][switch]$checkTestResults,
    [Parameter()][int]$pollingSeconds = 60,
    [Parameter()][int]$maxPollingSeconds = 300,
    [Parameter()][string]$scriptPath = '',
    # use scale factor to scale the number of test cases, to minimise the number of entries and avoid generating large test results files;
    # default is 1  
    [Parameter()][int]$scaleFactor = 1,
    [ValidateSet('JUnit', 'VSTest')]
    [Parameter()][string]$testResultsFormat = 'JUnit'
)

$url = "https://api.loader.io/v2/tests/$testId"

# the following GET endpoint can be used to trigger a test, however it is recommended to use run webhook
# Invoke-WebRequest -Headers @{'loaderio-auth' = $apiKey} -Uri "$url" -UseBasicParsing

# run web hook: use POST to trigger the test
$url = "https://api.loader.io/v2/tests/$testId/token/$testToken/run"
$result = Invoke-WebRequest -Headers @{'loaderio-auth' = $apiKey} -Uri "$url" -Method POST -UseBasicParsing

Write-Output $result

$test = $result.Content | ConvertFrom-Json
if($checkTestResults -and $test.message -eq 'success') {
    Start-Sleep -Seconds $pollingSeconds # wait for test to start
    if($scriptPath -ne '') {
        $path = "./$scriptPath/check-test-results.ps1"
        & $path -testName "$testName" -testId $testId -apiKey $apiKey -pollingSeconds $pollingSeconds -maxPollingSeconds $maxPollingSeconds `
            -scriptPath $scriptPath -scaleFactor $scaleFactor -testResultsFormat $testResultsFormat
    } else {
        & ./check-test-results.ps1 -testName "$testName" -testId $testId -apiKey $apiKey -pollingSeconds $pollingSeconds -maxPollingSeconds $maxPollingSeconds `
            -scaleFactor $scaleFactor -testResultsFormat $testResultsFormat
    }
}
