##############################################################
#  RunTests.ps1 - Chay Unit Test ShopFlower
#  Cach dung: powershell -ExecutionPolicy Bypass -File .\RunTests.ps1
#  Vi du:
#    .\RunTests.ps1              -> Chay tat ca 89 tests
#    .\RunTests.ps1 -Nhom bug   -> Chi chay Bug tests (29 tests)
#    .\RunTests.ps1 -Nhom reg   -> Nhom Dang ky
#    .\RunTests.ps1 -Nhom hd    -> Nhom Hoa don
#    .\RunTests.ps1 -ChiTiet    -> Hien thi tung test
##############################################################

param(
    [string]$Nhom = "tat_ca",
    [switch]$ChiTiet
)

# -- Duong dan -------------------------------------------------
$vstest  = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe"
$adapter = ".\packages\MSTest.TestAdapter.3.1.1\build\net462"
$dll     = "ShopFlower.Tests\bin\Debug\ShopFlower.Tests.dll"
$msbuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
$proj    = "ShopFlower.Tests\ShopFlower.Tests.csproj"

# -- Banner ----------------------------------------------------
Write-Host ""
Write-Host "====================================================" -ForegroundColor Cyan
Write-Host "   SHOPFLOWER - UNIT TEST THANH VIEN 2              " -ForegroundColor Cyan
Write-Host "====================================================" -ForegroundColor Cyan
Write-Host ""

# -- Buoc 1: Build ---------------------------------------------
Write-Host "[BUOC 1] Dang build du an..." -ForegroundColor Yellow
$buildOutput = & $msbuild $proj /p:Configuration=Debug /v:minimal /t:Build 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "[FAILED] BUILD THAT BAI!" -ForegroundColor Red
    $buildOutput | Write-Host
    exit 1
}
Write-Host "[OK] Build thanh cong!" -ForegroundColor Green
Write-Host ""

# -- Mapping nhom test -----------------------------------------
$filterMap = @{
    "tat_ca" = ""
    "reg"    = "TestCategory=BugRevealing_REG"
    "log"    = "TestCategory=BugRevealing_LOG"
    "pwd"    = "TestCategory=BugRevealing_PWD"
    "hd"     = "TestCategory=BugRevealing_HD"
    "biz"    = "TestCategory=BugRevealing_BIZ"
    "sp"     = "TestCategory=BugRevealing_SP"
    "lh"     = "TestCategory=BugRevealing_LH"
    "calc"   = "TestCategory=BugRevealing_CALC"
    "bug"    = "TestCategory~BugRevealing"
}

$labelMap = @{
    "tat_ca" = "TAT CA 89 TESTS"
    "reg"    = "NHOM REG - Dang ky (5 tests)"
    "log"    = "NHOM LOG - Dang nhap & Bao mat (3 tests)"
    "pwd"    = "NHOM PWD - Mat khau (3 tests)"
    "hd"     = "NHOM HD  - Hoa don (6 tests)"
    "biz"    = "NHOM BIZ - Nghiep vu ton kho (4 tests)"
    "sp"     = "NHOM SP  - San pham (3 tests)"
    "lh"     = "NHOM LH  - Lien he (3 tests)"
    "calc"   = "NHOM CALC - Tinh toan (3 tests)"
    "bug"    = "TAT CA BUG TESTS (29 tests)"
}

$nhomKey = $Nhom.ToLower()
if (-not $filterMap.ContainsKey($nhomKey)) {
    Write-Host "Nhom khong hop le! Dung: tat_ca | reg | log | pwd | hd | biz | sp | lh | calc | bug" -ForegroundColor Red
    exit 1
}

$filter = $filterMap[$nhomKey]
$label  = $labelMap[$nhomKey]

Write-Host "[BUOC 2] Dang chay: $label" -ForegroundColor Yellow
Write-Host "----------------------------------------------------" -ForegroundColor DarkGray
Write-Host ""

# -- Buoc 2: Chay test -----------------------------------------
$args_list = @(
    $dll,
    "/TestAdapterPath:$adapter",
    "/Framework:.NETFramework,Version=v4.8"
)
if ($filter -ne "") {
    $args_list += "/TestCaseFilter:$filter"
}

$output = & $vstest @args_list 2>&1

if ($ChiTiet) {
    # Hien thi tung test co mau sac
    $output | ForEach-Object {
        if ($_ -match "^\s+Passed") {
            Write-Host $_ -ForegroundColor Green
        } elseif ($_ -match "^\s+Failed") {
            Write-Host $_ -ForegroundColor Red
        } elseif ($_ -match "Error Message:") {
            Write-Host $_ -ForegroundColor DarkRed
        } elseif ($_ -match "^\s+at ShopFlower") {
            Write-Host $_ -ForegroundColor DarkGray
        } else {
            Write-Host $_
        }
    }
} else {
    # Chi hien thi tests da FAIL
    $failLines = $output | Where-Object { $_ -match "^\s+Failed " }
    if ($failLines.Count -gt 0) {
        Write-Host "=== TESTS DA FAIL ($($failLines.Count)) ===" -ForegroundColor Red
        $failLines | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
        Write-Host ""
    } else {
        Write-Host "=== KHONG CO TEST NAO FAIL ===" -ForegroundColor Green
        Write-Host ""
    }
}

# -- Tong ket --------------------------------------------------
Write-Host "====================================================" -ForegroundColor Cyan
Write-Host "                    TONG KET                       " -ForegroundColor Cyan
Write-Host "====================================================" -ForegroundColor Cyan
$output | Where-Object { $_ -match "(Total tests|Passed:|Failed:|Total time)" } | ForEach-Object {
    if ($_ -match "Failed") { Write-Host "  $_" -ForegroundColor Red }
    else { Write-Host "  $_" -ForegroundColor Green }
}
Write-Host ""

# -- Menu huong dan --------------------------------------------
Write-Host "CACH CHAY THEO NHOM:" -ForegroundColor Yellow
Write-Host "  .\RunTests.ps1              -> Chay tat ca (89 tests)" -ForegroundColor DarkCyan
Write-Host "  .\RunTests.ps1 -Nhom bug   -> Chi bug tests (29 tests)" -ForegroundColor DarkCyan
Write-Host "  .\RunTests.ps1 -Nhom reg   -> Nhom Dang ky" -ForegroundColor DarkCyan
Write-Host "  .\RunTests.ps1 -Nhom log   -> Nhom Dang nhap" -ForegroundColor DarkCyan
Write-Host "  .\RunTests.ps1 -Nhom hd    -> Nhom Hoa don" -ForegroundColor DarkCyan
Write-Host "  .\RunTests.ps1 -Nhom biz   -> Nhom Ton kho" -ForegroundColor DarkCyan
Write-Host "  .\RunTests.ps1 -ChiTiet    -> Hien thi tung test" -ForegroundColor DarkCyan
Write-Host ""
