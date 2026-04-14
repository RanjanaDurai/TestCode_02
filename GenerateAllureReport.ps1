# GenerateAllureReport.ps1
# Automatically generates Allure report + zips it with timestamp

param(
    [string]$ResultsPath = "bin\Debug\net8.0\allure-results",
    [string]$ReportPath  = "allure-report",
    [string]$ZipPath     = "Reports\allure-report\allure-report-$(Get-Date -Format 'yyyyMMdd-HHmmss').zip"
)

Write-Host "Starting Allure report generation..." -ForegroundColor Cyan

# Step 1: Preserve history from previous report (for trends & flaky detection)
if (Test-Path "$ReportPath\history") {
    Write-Host "Found previous history - copying it..." -ForegroundColor Green
    
    if (-not (Test-Path "$ResultsPath\history")) {
        New-Item -Path "$ResultsPath\history" -ItemType Directory -Force | Out-Null
    }
    
    Copy-Item -Path "$ReportPath\history\*" -Destination "$ResultsPath\history" -Recurse -Force
    Write-Host "History copied successfully" -ForegroundColor Green
} else {
    Write-Host "No previous history found (normal on first run)" -ForegroundColor Yellow
}

# Step 2: Remove old report folder (clean start)
if (Test-Path $ReportPath) {
    Write-Host "Removing old report folder..." -ForegroundColor DarkYellow
    Remove-Item $ReportPath -Recurse -Force
}

# Step 3: Generate fresh Allure report
Write-Host "Generating Allure report..." -ForegroundColor Cyan
allure generate "$ResultsPath/" --clean --output $ReportPath

# Step 4: Zip the report folder into project root
Write-Host "Creating ZIP archive..." -ForegroundColor Cyan

# Remove old zip if same name exists (rare because of timestamp)
if (Test-Path $ZipPath) { Remove-Item $ZipPath -Force }

# Compress the report folder
Compress-Archive -Path "$ReportPath\*" -DestinationPath $ZipPath -Force

Write-Host "ZIP created successfully: $ZipPath" -ForegroundColor Green
Write-Host "Size: $((Get-Item $ZipPath).Length / 1MB) MB" -ForegroundColor Green

# Step 5: Open the report in browser
Write-Host "Opening report..." -ForegroundColor Green
allure open $ReportPath

Write-Host "All done!" -ForegroundColor Magenta
Write-Host "→ Report folder: $ReportPath" -ForegroundColor White
Write-Host "→ Zipped archive (shareable): $ZipPath" -ForegroundColor White

GenerateAllureReport.ps1
Automatically generates Allure report + zips it with timestamp

GenerateAllureReport.ps1 - FIXED VERSION
Clean → Run Tests → Generate Report (with history preserved)

