@echo off
:: =====================================================================
:: SCRIPT CHẠY KIỂM THỬ API TỰ ĐỘNG BẰNG SOAPUI - SHOPFLOWER
:: =====================================================================
chcp 65001 > nul
setlocal enabledelayedexpansion

echo =====================================================================
echo    🚀 KHỞI ĐỘNG HỆ THỐNG KIỂM THỬ API SOAPUI - BLOSSOM CORNER
echo =====================================================================
echo.

:: 1. Cấu hình các đường dẫn mặc định của SoapUI trên Windows
set "SOAPUI_PATHS[0]=C:\Program Files\SmartBear\SoapUI-5.7.0\bin\testrunner.bat"
set "SOAPUI_PATHS[1]=C:\Program Files\SmartBear\SoapUI-5.7.1\bin\testrunner.bat"
set "SOAPUI_PATHS[2]=C:\Program Files\SmartBear\SoapUI-5.7.2\bin\testrunner.bat"
set "SOAPUI_PATHS[3]=C:\Program Files (x86)\SmartBear\SoapUI-5.7.0\bin\testrunner.bat"
set "SOAPUI_PATH="

:: 2. Tự động quét tìm file testrunner.bat
for /L %%i in (0,1,3) do (
    set "current_path=!SOAPUI_PATHS[%%i]!"
    if exist "!current_path!" (
        set "SOAPUI_PATH=!current_path!"
        goto :FOUND
    )
)

:FOUND
if "%SOAPUI_PATH%"=="" (
    echo [⚠ CẢNH BÁO] Không tìm thấy file testrunner.bat của SoapUI tại các thư mục mặc định.
    echo Vui lòng nhập đường dẫn thủ công đến thư mục bin của SoapUI.
    set /p "SOAPUI_DIR=Đường dẫn (ví dụ: C:\Program Files\SmartBear\SoapUI-5.7.0\bin): "
    set "SOAPUI_PATH=!SOAPUI_DIR!\testrunner.bat"
)

if not exist "%SOAPUI_PATH%" (
    echo [🔴 LỖI] File '%SOAPUI_PATH%' không tồn tại!
    echo Không thể khởi chạy kiểm thử SoapUI. Hãy cài đặt SoapUI hoặc kiểm tra lại đường dẫn.
    pause
    exit /b 1
)

:: 3. Thiết lập tệp tin Project và tham số
set "PROJECT_FILE=%~dp0ShopFlower_SoapUI_Project.xml"
set "REPORT_DIR=%~dp0SoapUI_Reports"

if not exist "%PROJECT_FILE%" (
    echo [⚠ LỖI] Không tìm thấy tệp cấu hình %PROJECT_FILE%.
    echo Vui lòng đảm bảo tệp ShopFlower_SoapUI_Project.xml nằm cùng thư mục với script này.
    pause
    exit /b 1
)

echo [+] Tìm thấy SoapUI tại: "%SOAPUI_PATH%"
echo [+] Tệp cấu hình Test:  "%PROJECT_FILE%"
echo [+] Thư mục xuất báo cáo: "%REPORT_DIR%"
echo.
echo ---------------------------------------------------------------------
echo    Đang bắt đầu chạy TestSuite ... Vui lòng không tắt cửa sổ này.
echo ---------------------------------------------------------------------
echo.

:: Tạo thư mục báo cáo nếu chưa có
if not exist "%REPORT_DIR%" mkdir "%REPORT_DIR%"

:: 4. Chạy kiểm thử tự động với các tham số:
:: -r: Xuất báo cáo tóm tắt
:: -a: Xuất báo cáo cho tất cả kết quả test
:: -f: Thư mục lưu báo cáo kết quả
:: -j: Tạo báo cáo định dạng JUnit XML (phục vụ CI/CD)
call "%SOAPUI_PATH%" -r -a -f"%REPORT_DIR%" -j "%PROJECT_FILE%"

set "EXIT_CODE=%ERRORLEVEL%"
echo.
echo ---------------------------------------------------------------------
if %EXIT_CODE% equ 0 (
    echo [🟢 THÀNH CÔNG] Tất cả các ca kiểm thử API đều VƯỢT QUA (Passed)!
) else (
    echo [🔴 THẤT BẠI] Phát hiện ca kiểm thử API bị LỖI (Failed) với mã lỗi: %EXIT_CODE%
)
echo Báo cáo chi tiết đã được xuất ra thư mục: "%REPORT_DIR%"
echo ---------------------------------------------------------------------
echo.
pause
exit /b %EXIT_CODE%
