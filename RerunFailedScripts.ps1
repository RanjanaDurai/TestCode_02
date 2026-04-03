# =================================================================
# RerunFailedScripts.ps1
#
# Run this AFTER your manual dotnet test run completes.
# It reads the TRX results file, finds every failed test,
# and reruns only those tests.
#
# USAGE:
#   .\RerunFailedScripts.ps1
#   .\RerunFailedScripts.ps1 -DryRun
# =================================================================

param(
    [string]$TrxPath = "TestResults\results.trx",
    [string]$ProjectPath = ".",
    [string]$ResultsDirectory = "TestResults",
    [switch]$DryRun
)

Write-Host "=====================================================================" -ForegroundColor Cyan
Write-Host "                  Rerun Failed Tests Script" -ForegroundColor Cyan
Write-Host "=====================================================================" -ForegroundColor Cyan

# -----------------------------------------------------------------
# Ensure ResultsDirectory exists
# -----------------------------------------------------------------
if (-not (Test-Path $ResultsDirectory)) {
    New-Item -Path $ResultsDirectory -ItemType Directory -Force | Out-Null
    Write-Host "  Created results directory: $ResultsDirectory" -ForegroundColor Gray
}

# -----------------------------------------------------------------
# Smart TRX path handling - fallback to latest .trx if default not found
# -----------------------------------------------------------------
if (-not (Test-Path $TrxPath)) {
    $latestTrx = Get-ChildItem -Path $ResultsDirectory -Filter "*.trx" -ErrorAction SilentlyContinue |
                 Sort-Object LastWriteTime -Descending | Select-Object -First 1

    if ($latestTrx) {
        $TrxPath = $latestTrx.FullName
    }
}

Write-Host "  TRX File Being Used -> " -NoNewline -ForegroundColor Cyan
Write-Host $TrxPath -ForegroundColor White

# -----------------------------------------------------------------
# GUARD 1 - TRX file must exist
# -----------------------------------------------------------------
if (-not (Test-Path $TrxPath)) {
    Write-Host ""
    Write-Host "  ERROR: No TRX file found at: $TrxPath" -ForegroundColor Red
    Write-Host ""
    Write-Host "  Make sure your full test run includes the TRX logger:" -ForegroundColor Yellow
    Write-Host "    dotnet test --logger `"trx;LogFileName=results.trx`" --results-directory $ResultsDirectory" -ForegroundColor White
    Write-Host ""
    exit 1
}

# -----------------------------------------------------------------
# STEP 1 - Read the TRX file as XML
# -----------------------------------------------------------------
Write-Host ""
Write-Host "  Reading results from: $TrxPath" -ForegroundColor Cyan

[xml]$trx = Get-Content -Path $TrxPath -Encoding UTF8 -Raw

# -----------------------------------------------------------------
# STEP 2 - Extract failed nodes
# -----------------------------------------------------------------
$ns = New-Object System.Xml.XmlNamespaceManager($trx.NameTable)
$ns.AddNamespace("t", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")

$failedNodes  = $trx.SelectNodes("//t:UnitTestResult[@outcome='Failed']",  $ns)
$passedNodes  = $trx.SelectNodes("//t:UnitTestResult[@outcome='Passed']",  $ns)
$skippedNodes = $trx.SelectNodes("//t:UnitTestResult[@outcome='NotExecuted']", $ns)

$failedCount  = $failedNodes.Count
$passedCount  = $passedNodes.Count
$skippedCount = $skippedNodes.Count
$totalCount   = $failedCount + $passedCount + $skippedCount

Write-Host "  Found $failedCount failed test(s) in TRX file" -ForegroundColor Red

# -----------------------------------------------------------------
# GUARD 2 - Nothing failed, nothing to rerun
# -----------------------------------------------------------------
if ($failedCount -eq 0) {
    Write-Host "  All tests passed. Nothing to rerun." -ForegroundColor Green
    Write-Host ""
    exit 0
}

# -----------------------------------------------------------------
# STEP 3 - Collect the full names of failed tests
# -----------------------------------------------------------------
$failedNames = @()
foreach ($node in $failedNodes) {
    $fullName = $node.GetAttribute("testName")
    if ($fullName) {
        $failedNames += $fullName
    }
}

# -----------------------------------------------------------------
# STEP 4 - Print the failed test names
# -----------------------------------------------------------------
Write-Host "  Failed tests:" -ForegroundColor Red
Write-Host ""

foreach ($name in $failedNames) {
    $shortName = ($name -split '\.')[-1]
    Write-Host "    FAIL  $shortName" -ForegroundColor Red
    Write-Host "          $name"      -ForegroundColor DarkGray
    Write-Host ""
}

# -----------------------------------------------------------------
# STEP 5 - Write failed_tests.txt
# -----------------------------------------------------------------
$outputFile = "failed_tests.txt"
Set-Content -Path $outputFile -Value $failedNames -Encoding UTF8

Write-Host "  Failed test names written to: $outputFile" -ForegroundColor Yellow
Write-Host ""

# -----------------------------------------------------------------
# STEP 6 - Build the --filter expression
# -----------------------------------------------------------------
$filterParts      = $failedNames | ForEach-Object { "FullyQualifiedName=$_" }
$filterExpression = $filterParts -join "|"

$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"

$rerunCommand = "dotnet test `"$ProjectPath`" " +
                "--no-build " +
                "--filter `"$filterExpression`" " +
                "--logger `"trx;LogFileName=FailedRerun_$timestamp.trx`" " +
                "--results-directory `"$ResultsDirectory`""

# -----------------------------------------------------------------
# STEP 7 - DryRun mode
# -----------------------------------------------------------------
if ($DryRun) {
    Write-Host "  -- Dry run mode -----------------------------------------" -ForegroundColor Cyan
    Write-Host "  Command that WOULD run:" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  $rerunCommand" -ForegroundColor White
    Write-Host ""
    Write-Host "  Remove -DryRun to execute it." -ForegroundColor Yellow
    Write-Host ""
    exit 0
}

# -----------------------------------------------------------------
# STEP 8 - Run the rerun command
# -----------------------------------------------------------------
Write-Host "  -- Rerunning $failedCount failed test(s) --------------------" -ForegroundColor Cyan
Write-Host ""

Invoke-Expression $rerunCommand

$rerunExitCode = $LASTEXITCODE

# -----------------------------------------------------------------
# STEP 9 - Report the rerun outcome
# -----------------------------------------------------------------
Write-Host ""

if ($rerunExitCode -eq 0) {
    Write-Host "  All previously failed tests now pass." -ForegroundColor Green
}
else {
    Write-Host "  Some tests are still failing." -ForegroundColor Red
    Write-Host "  Run .\RerunFailedScripts.ps1 again to retry." -ForegroundColor Yellow
}

Write-Host ""

exit $rerunExitCode