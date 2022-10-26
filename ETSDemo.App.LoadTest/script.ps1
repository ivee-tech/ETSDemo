
# https://api.loader.io/v2/tests/007d20122e421d755a8c747bcfb9a67f/token/1c47da317db90741c3fc7a7fc91300fc/run
$testName = 'Expression - API'
$testId = '007d20122e421d755a8c747bcfb9a67f'
$testToken = '1c47da317db90741c3fc7a7fc91300fc'
$apiKey = '***'

$scriptPath = 'ETSDemo.App.LoadTest'
$path = './ETSDemo.App.LoadTest/trigger-test.ps1'
& $path -testName $testName -testId $testId -testToken $testToken -apiKey $apiKey -checkTestResults -scriptPath $scriptPath

& ./ETSDemo.App.LoadTest/check-test-results.ps1 -testName $testName -testId $testId -apiKey $apiKey

$url = "https://api.loader.io/v2/tests/007d20122e421d755a8c747bcfb9a67f/results"
$resultJson = Invoke-WebRequest -Headers @{'loaderio-auth' = $apiKey} -Uri "$url" -UseBasicParsing
$results = ConvertFrom-Json $resultJson.Content
$results | ConvertTo-Csv



