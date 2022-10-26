Function Get-VSTest {
    param(
        [Parameter(Mandatory=$true)][string]$testName,
        [Parameter(Mandatory=$true)][PSCustomObject]$testResult
    )

# generate test results in VSTest format (TRX)
$total = $testResult.success + $testResult.error
$timeInMilliseconds = $testResult.avg_response_time # no test execution duration? 
$startTime = [DateTime]::Parse($testResult.started_at)
$endTime = [DateTime]::Parse($startTime).AddMilliseconds($timeInMilliseconds)
$st = $startTime.ToString("o")
$et = $endTime.ToString("o")

$runId = [Guid]::NewGuid()
$user = whoami
$computer = $env:COMPUTERNAME
$currentTime = Get-Date -Format 'yyyy-MM-ddTHH:mm:ss'

$rtotal = [math]::Ceiling($total / $scaleFactor)
$rsuccess = [math]::Ceiling($testResult.success / $scaleFactor)
$rerror = [math]::Ceiling($testResult.error / $scaleFactor)
$results = ''
if ($testResult.success -gt 0) {
  1..$rsuccess | ForEach-Object { 
    $testId = $([Guid]::NewGuid())
    $results += @"
    <UnitTestResult executionId="$($runId)" testId="$($testId)" computerName="$($computer)"
      testName="$($testName)-succ-$($_)" testType="13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b" 
      outcome="Passed" testListId="$($runId)">
    </UnitTestResult>
"@ 
    $defs += @"
    <UnitTest name="$($testName)-def-$($_)" storage="" id="$($testId)">
      <Execution id="$($runId)" />
      <TestMethod codeBase="" adapterTypeName="executor://mstestadapter/v2" className="" name="$($testName)-def-$($_)" />
    </UnitTest>
"@
    $entries += @"
    <TestEntry testId="$($testId)" executionId="$($runId)" 
      testListId="$($runId)" />
"@
  }
}

if ($testResult.error -gt 0) {
  1..$rerror | ForEach-Object {
    $testId = $([Guid]::NewGuid())
    $results += @"
    <UnitTestResult executionId="$($runId)" testId="$($testId)" computer="$($computer)" 
      testName="$($testName)-succ-$($_)" testType="13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b" 
      outcome="Failed" testListId="$($runId)">
    </UnitTestResult>
"@ 
    $defs += @"
  <UnitTest name="$($testName)-def-$($_)" storage="" id="$($testId)">
    <Execution id="$($runId)" />
    <TestMethod codeBase="" adapterTypeName="executor://mstestadapter/v2" className="" name="$($testName)-def-$($_)" />
  </UnitTest>
"@
    $entries += @"
  <TestEntry testId="$($testId)" executionId="$($runId)" 
    testListId="$($runId)" />
"@
  }
}

$sXml = @"
<?xml version="1.0" encoding="utf-8"?>
<TestRun id="$($runId)" name="$($testName)" runUser="$($user)" xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
  <Times creation="$($currentTime)" queuing="$($currentTime)" start="$($st)" finish="$($et)" />
  <TestSettings name="default" id="$($runId)">
    <Deployment runDeploymentRoot="deploy-$($testName)" />
  </TestSettings>
  <Results>
  $($results)
  </Results>
  <TestDefinitions>
  $($defs)
  </TestDefinitions>
  <TestEntries>
  $($entries)
  </TestEntries>
  <TestLists>
    <TestList name="All Results" id="$($runId)" />
  </TestLists>
  <ResultSummary outcome="Completed">
    <Counters total="$($rtotal)" executed="$($rtotal)" passed="$($rsuccess)" failed="$($rerror)" error="0" timeout="0" 
      aborted="0" inconclusive="0" passedButRunAborted="0" notRunnable="0" notExecuted="0" disconnected="0" warning="0" 
      completed="0" inProgress="0" pending="0" />
  </ResultSummary>
</TestRun>
"@

return $sXml
}
