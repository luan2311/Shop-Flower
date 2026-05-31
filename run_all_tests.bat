@echo off
:: =====================================================================
:: SCRIPT CHẠY TOÀN BỘ SUITE KIỂM THỬ TỰ ĐỘNG - SHOPFLOWER
:: =====================================================================
chcp 65001 > nul
setlocal enabledelayedexpansion

echo =====================================================================
echo    🚀 KHỞI ĐỘNG HỆ THỐNG KIỂM THỬ LIÊN HOÀN (C# + SOAPUI + PLAYWRIGHT)
echo =====================================================================
echo.

:: 1. Chạy C# Unit & Integration Tests
echo [+] BƯỚC 1: Chạy C# Unit Tests (MSTest & Hashing Logic)
echo ---------------------------------------------------------------------
call dotnet test "%~dp0ShopFlower.Tests\ShopFlower.Tests.csproj" --no-build
set "CS_RESULT=%ERRORLEVEL%"
if %CS_RESULT% equ 0 (
    echo [🟢 OK] C# Unit Tests hoàn thành xuất sắc!
) else (
    echo [🔴 LỖI] Phát hiện bài kiểm thử C# bị lỗi!
)
echo.

:: 2. Chạy SoapUI REST API Tests
echo [+] BƯỚC 2: Chạy SoapUI REST API TestSuite
echo ---------------------------------------------------------------------
if exist "%~dp0run_soapui_tests.bat" (
    call "%~dp0run_soapui_tests.bat"
    set "SOAP_RESULT=%ERRORLEVEL%"
) else (
    echo [⚠ CẢNH BÁO] Không tìm thấy file run_soapui_tests.bat!
    set "SOAP_RESULT=99"
)
echo.

:: 3. Chạy Playwright E2E UI Tests
echo [+] BƯỚC 3: Chạy Playwright E2E Cross-Browser UI Tests
echo ---------------------------------------------------------------------
set "UI_DIR=%~dp0ShopFlower.Tests.UI"
if not exist "%UI_DIR%" (
    echo [🔴 LỖI] Thư mục %UI_DIR% không tồn tại!
    set "PLAY_RESULT=99"
    goto :SUMMARY
)

cd /d "%UI_DIR%"
if not exist "node_modules" (
    echo [i] Đang phát hiện thiếu thư viện node_modules. Tiến hành cài đặt tự động...
    call npm install
)

echo [i] Bắt đầu chạy Playwright E2E Tests (Headless)...
call npx playwright test
set "PLAY_RESULT=%ERRORLEVEL%"
if %PLAY_RESULT% equ 0 (
    echo [🟢 OK] Playwright UI Tests hoàn thành xuất sắc!
) else (
    echo [🔴 LỖI] Phát hiện lỗi kiểm thử giao diện Playwright!
)
echo.

:SUMMARY
echo =====================================================================
echo    📊 TỔNG HỢP KẾT QUẢ KIỂM THỬ (TEST EXECUTION SUMMARY)
echo =====================================================================
if %CS_RESULT% equ 0 (
    echo   [🟢] 1. C# Unit Tests        : PASSED
) else (
    echo   [🔴] 1. C# Unit Tests        : FAILED (Code: %CS_RESULT%)
)

if %SOAP_RESULT% equ 0 (
    echo   [🟢] 2. SoapUI REST APIs     : PASSED
) else (
    echo   [🔴] 2. SoapUI REST APIs     : FAILED/SKIPPED (Code: %SOAP_RESULT%)
)

if %PLAY_RESULT% equ 0 (
    echo   [🟢] 3. Playwright UI Tests  : PASSED
) else (
    echo   [🔴] 3. Playwright UI Tests  : FAILED (Code: %PLAY_RESULT%)
)
echo =====================================================================
echo.
pause
exit /b 0
