# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: auth.spec.js >> Phân hệ Đăng Ký & Đăng Nhập (Authentication Test Suite) >> REG_03: Chặn đăng ký với Email sai định dạng
- Location: tests\auth.spec.js:52:3

# Error details

```
Test timeout of 30000ms exceeded.
```

```
Error: browserContext.close: Test timeout of 30000ms exceeded.
Browser logs:

<launching> C:\Users\LUAN\AppData\Local\ms-playwright\firefox-1522\firefox\firefox.exe -no-remote -headless -profile C:\Users\LUAN\AppData\Local\Temp\playwright_firefoxdev_profile-8XH47H -juggler-pipe -silent
<launched> pid=17864
[pid=17864][err] *** You are running in headless mode.
[pid=17864][err] JavaScript warning: resource://services-settings/Utils.sys.mjs, line 119: unreachable code after return statement
[pid=17864][out] 
[pid=17864][out] Juggler listening to the pipe
[pid=17864][out] Crash Annotation GraphicsCriticalError: |[0][GFX1-]: RenderCompositorSWGL failed mapping default framebuffer, no dt (t=1.54752) [GFX1-]: RenderCompositorSWGL failed mapping default framebuffer, no dt
[pid=17864][err] JavaScript error: chrome://juggler/content/Helper.js, line 82: NS_ERROR_FAILURE: Component returned failure code: 0x80004005 (NS_ERROR_FAILURE) [nsIWebProgress.removeProgressListener]
[pid=17864][out] console.warn: services.settings: #fetchAttachment: Forcing fallbackToDump to false due to Utils.LOAD_DUMPS being false
[pid=17864][out] console.error: (new NotFoundError("Could not find fa0fc42c-d91d-fca7-34eb-806ff46062dc in cache or dump", "resource://services-settings/Attachments.sys.mjs", 48))
[pid=17864][out] console.warn: "Unable to find the attachment for" "fa0fc42c-d91d-fca7-34eb-806ff46062dc"
[pid=17864][out] console.error: "Error fetching remote settings base url from CDN. Falling back to https://firefox-settings-attachments.cdn.mozilla.net/" (new SyntaxError("XMLHttpRequest.open: '/' is not a valid URL.", (void 0), 126))
[pid=17864][out] console.error: services.settings: 
[pid=17864][out]   Message: EmptyDatabaseError: "main/nimbus-desktop-experiments" has not been synced yet
[pid=17864][out]   Stack:
[pid=17864][out]     EmptyDatabaseError@resource://services-settings/Database.sys.mjs:19:5
[pid=17864][out] list@resource://services-settings/Database.sys.mjs:96:13
[pid=17864][out] 
[pid=17864][out] console.error: [Exception... "Component returned failure code: 0x80070057 (NS_ERROR_ILLEGAL_VALUE) [nsIWinTaskbar.getTaskbarProgress]"  nsresult: "0x80070057 (NS_ERROR_ILLEGAL_VALUE)"  location: "JS frame :: moz-src:///browser/components/downloads/DownloadsTaskbar.sys.mjs :: #windowsAttachIndicator :: line 183"  data: no]
[pid=17864][err] JavaScript warning: resource://gre/modules/UpdateService.sys.mjs, line 4031: unreachable code after return statement
[pid=17864] <gracefully close start>
```

# Page snapshot

```yaml
- generic [active] [ref=e1]:
  - generic [ref=e2]:
    - heading "Server Error in '/' Application." [level=1] [ref=e3]:
      - text: Server Error in '/' Application.
      - separator [ref=e4]
    - heading "Parser Error" [level=2] [ref=e5]
  - generic [ref=e6]:
    - text: "Description: An error occurred during the parsing of a resource required to service this request. Please review the following specific parse error details and modify your source file appropriately."
    - text: "Parser Error Message: Encountered end tag \"div\" with no matching start tag. Are your start/end tags properly balanced?"
    - text: "Source Error:"
    - table [ref=e7]:
      - rowgroup [ref=e8]:
        - 'row "Line 57: </div> Line 58: </div> Line 59: </div> Line 60: } Line 61: </div>" [ref=e9]':
          - 'cell "Line 57: </div> Line 58: </div> Line 59: </div> Line 60: } Line 61: </div>" [ref=e10]':
            - code [ref=e11]:
              - generic [ref=e12]: "Line 57: </div> Line 58: </div> Line 59: </div> Line 60: } Line 61: </div>"
    - text: "Source File: /Views/Account/Dang_ky.cshtml Line: 59"
    - separator [ref=e13]
    - text: "Version Information: Microsoft .NET Framework Version:4.0.30319; ASP.NET Version:4.8.9319.0"
```