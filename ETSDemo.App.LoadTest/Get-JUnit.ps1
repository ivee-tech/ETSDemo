Function Get-JUnit {
    param(
        [Parameter(Mandatory=$true)][string]$testName,
        [Parameter(Mandatory=$true)][PSCustomObject]$testResult
    )

# generate test results in JUnit format
$total = $testResult.success + $testResult.error
$timeInSeconds = $testResult.avg_response_time / 1000 # no test execution duration? 

$rtotal = [math]::Ceiling($total / $scaleFactor)
$rsuccess = [math]::Ceiling($testResult.success / $scaleFactor)
$rerror = [math]::Ceiling($testResult.error / $scaleFactor)
$testCasesSuccess = ""
if ($testResult.success -gt 0) {
  1..$rsuccess | ForEach-Object { $testCasesSuccess += @"
    <testcase classname="$($testName)-succ-$($_)" name="$($testName)-succ-$($_)" time="0">
    </testcase>
"@ 
  }
}
$testCasesError = ""
if ($rerror -gt 0) {
  1..$testResult.error | ForEach-Object { $testCasesError += @"
  <testcase classname="$($testName)-err-$($_)" name="$($testName)-err-$($_)" time="0">
  <failure>$($testName)-err-$($_)</failure>
  </testcase>
"@ 
  }
}
$sXml = @"
<?xml version="1.0" encoding="UTF-8"?>
<testsuites name="$($testName)" tests="$($rtotal)" failures="$($rerror)" time="$($timeInSeconds)" success="$($testResult.success)" error="$($testResult.error)" scaleFactor="$($scaleFactor)">
  <testsuite name="$($testName)" failures="$($rerror)" timestamp="$($testResult.started_at)" time="$($timeInSeconds)" tests="$($rtotal)">
  $($testCasesSuccess)
  $($testCasesError)
</testsuite>
</testsuites>
"@

return $sXml
}
