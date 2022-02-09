; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "时间猎犬—邮箱助手"
!define PRODUCT_VERSION "1.0.0.0"
!define PRODUCT_PUBLISHER "杭州引力网络技术有限公司"
!define PRODUCT_WEB_SITE "https://timehound.vip"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\MailExporter.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"
!include "LogicLib.nsh"
!include "WinVer.nsh"
!include "WordFunc.nsh"
!include "DotNetChecker.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; 语言选择窗口常量设置
!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 许可协议页面
!insertmacro MUI_PAGE_LICENSE "Licence.txt"
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!define MUI_FINISHPAGE_RUN "$INSTDIR\MailExporter.exe"
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "SimpChinese"
!insertmacro MUI_LANGUAGE "TradChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_LANGDLL
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup.exe"
InstallDir "$PROGRAMFILES\时间猎犬—邮箱助手"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show


Function KillExist
        StrCpy $1 "TimeHound.exe"

        nsProcess::_FindProcess "$1"
        Pop $R0
        ${If} $R0 = 0
        nsProcess::_KillProcess "$1"
        Pop $R0

        Sleep 500
        ${EndIf}

FunctionEnd

Section "MainSection" SEC01
  CALL KillExist

  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File "bin\Release\MailExporter.exe"
  File "bin\Release\BouncyCastle.Crypto.dll"
  File "bin\Release\ICSharpCode.SharpZipLib.dll"
  File "bin\Release\MailKit.dll"
  File "bin\Release\MimeKit.dll"
  File "bin\Release\Newtonsoft.Json.dll"
  File "bin\Release\NPOI.dll"
  File "bin\Release\NPOI.OOXML.dll"
  File "bin\Release\NPOI.OpenXml4Net.dll"
  File "bin\Release\NPOI.OpenXmlFormats.dll"
  File "bin\Release\Serilog.dll"
  File "bin\Release\Serilog.Sinks.File.dll"
  File "bin\Release\System.Buffers.dll"
  File "bin\Release\Telerik.WinControls.dll"
  File "bin\Release\Telerik.WinControls.GridView.dll"
  File "bin\Release\Telerik.WinControls.UI.dll"
  File "bin\Release\TelerikCommon.dll"
  CreateDirectory "$SMPROGRAMS\时间猎犬—邮箱助手"
  CreateDirectory "$SMPROGRAMS\时间猎犬—邮箱助手\Logs"
  CreateShortCut "$SMPROGRAMS\时间猎犬—邮箱助手\时间猎犬—邮箱助手.lnk" "$INSTDIR\MailExporter.exe"
  CreateShortCut "$DESKTOP\时间猎犬—邮箱助手.lnk" "$INSTDIR\MailExporter.exe"
  
  !insertmacro CheckNetFramework 472
  !insertmacro CheckNetFrameworkDelayRestart 472 $0 ; Returns if an install was performed
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\时间猎犬—邮箱助手\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\时间猎犬—邮箱助手\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\MailExporter.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\MailExporter.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/

Section Uninstall
  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\MailExporter.exe"
  Delete "bin\Release\BouncyCastle.Crypto.dll"
  Delete "bin\Release\ICSharpCode.SharpZipLib.dll"
  Delete "bin\Release\MailKit.dll"
  Delete "bin\Release\MimeKit.dll"
  Delete "bin\Release\Newtonsoft.Json.dll"
  Delete "bin\Release\NPOI.dll"
  Delete "bin\Release\NPOI.OOXML.dll"
  Delete "bin\Release\NPOI.OpenXml4Net.dll"
  Delete "bin\Release\NPOI.OpenXmlFormats.dll"
  Delete "bin\Release\Serilog.dll"
  Delete "bin\Release\Serilog.Sinks.File.dll"
  Delete "bin\Release\System.Buffers.dll"
  Delete "bin\Release\Telerik.WinControls.dll"
  Delete "bin\Release\Telerik.WinControls.GridView.dll"
  Delete "bin\Release\Telerik.WinControls.UI.dll"
  Delete "bin\Release\TelerikCommon.dll"
  Delete "$SMPROGRAMS\时间猎犬—邮箱助手\Uninstall.lnk"
  Delete "$SMPROGRAMS\时间猎犬—邮箱助手\Website.lnk"
  Delete "$DESKTOP\时间猎犬—邮箱助手.lnk"
  Delete "$SMPROGRAMS\时间猎犬—邮箱助手\时间猎犬—邮箱助手.lnk"

  RMDir "$SMPROGRAMS\时间猎犬—邮箱助手"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
!insertmacro MUI_UNGETLANGUAGE
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd
