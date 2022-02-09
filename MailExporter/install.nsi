; �ýű�ʹ�� HM VNISEdit �ű��༭���򵼲���

; ��װ�����ʼ���峣��
!define PRODUCT_NAME "ʱ����Ȯ����������"
!define PRODUCT_VERSION "1.0.0.0"
!define PRODUCT_PUBLISHER "�����������缼�����޹�˾"
!define PRODUCT_WEB_SITE "https://timehound.vip"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\MailExporter.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI �ִ����涨�� (1.67 �汾���ϼ���) ------
!include "MUI.nsh"
!include "LogicLib.nsh"
!include "WinVer.nsh"
!include "WordFunc.nsh"
!include "DotNetChecker.nsh"

; MUI Ԥ���峣��
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; ����ѡ�񴰿ڳ�������
!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"

; ��ӭҳ��
!insertmacro MUI_PAGE_WELCOME
; ���Э��ҳ��
!insertmacro MUI_PAGE_LICENSE "Licence.txt"
; ��װĿ¼ѡ��ҳ��
!insertmacro MUI_PAGE_DIRECTORY
; ��װ����ҳ��
!insertmacro MUI_PAGE_INSTFILES
; ��װ���ҳ��
!define MUI_FINISHPAGE_RUN "$INSTDIR\MailExporter.exe"
!insertmacro MUI_PAGE_FINISH

; ��װж�ع���ҳ��
!insertmacro MUI_UNPAGE_INSTFILES

; ��װ�����������������
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "SimpChinese"
!insertmacro MUI_LANGUAGE "TradChinese"

; ��װԤ�ͷ��ļ�
!insertmacro MUI_RESERVEFILE_LANGDLL
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI �ִ����涨����� ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup.exe"
InstallDir "$PROGRAMFILES\ʱ����Ȯ����������"
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
  CreateDirectory "$SMPROGRAMS\ʱ����Ȯ����������"
  CreateDirectory "$SMPROGRAMS\ʱ����Ȯ����������\Logs"
  CreateShortCut "$SMPROGRAMS\ʱ����Ȯ����������\ʱ����Ȯ����������.lnk" "$INSTDIR\MailExporter.exe"
  CreateShortCut "$DESKTOP\ʱ����Ȯ����������.lnk" "$INSTDIR\MailExporter.exe"
  
  !insertmacro CheckNetFramework 472
  !insertmacro CheckNetFrameworkDelayRestart 472 $0 ; Returns if an install was performed
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\ʱ����Ȯ����������\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\ʱ����Ȯ����������\Uninstall.lnk" "$INSTDIR\uninst.exe"
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

#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd

/******************************
 *  �����ǰ�װ�����ж�ز���  *
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
  Delete "$SMPROGRAMS\ʱ����Ȯ����������\Uninstall.lnk"
  Delete "$SMPROGRAMS\ʱ����Ȯ����������\Website.lnk"
  Delete "$DESKTOP\ʱ����Ȯ����������.lnk"
  Delete "$SMPROGRAMS\ʱ����Ȯ����������\ʱ����Ȯ����������.lnk"

  RMDir "$SMPROGRAMS\ʱ����Ȯ����������"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#

Function un.onInit
!insertmacro MUI_UNGETLANGUAGE
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "��ȷʵҪ��ȫ�Ƴ� $(^Name) ���������е������" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) �ѳɹ��ش����ļ�����Ƴ���"
FunctionEnd
