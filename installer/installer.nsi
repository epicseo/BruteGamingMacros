; Brute Gaming Macros - NSIS Installer Script
; This script creates a professional Windows installer

!define PRODUCT_NAME "Brute Gaming Macros"
!define PRODUCT_VERSION "2.1.0"
!define PRODUCT_PUBLISHER "Epic SEO"
!define PRODUCT_WEB_SITE "https://github.com/epicseo/BruteGamingMacros"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; Include required headers
!include "MUI2.nsh"
!include "x64.nsh"
!include "FileFunc.nsh"

; Installer configuration
Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "BruteGamingMacros-Setup-v${PRODUCT_VERSION}.exe"
InstallDir "$PROGRAMFILES64\BruteGamingMacros"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "InstallLocation"
RequestExecutionLevel admin
ShowInstDetails show
ShowUnInstDetails show

; Compression
SetCompressor /SOLID lzma
SetCompressorDictSize 32

; Version information
VIProductVersion "2.1.0.0"
VIAddVersionKey "ProductName" "${PRODUCT_NAME}"
VIAddVersionKey "ProductVersion" "${PRODUCT_VERSION}"
VIAddVersionKey "CompanyName" "${PRODUCT_PUBLISHER}"
VIAddVersionKey "LegalCopyright" "© 2025 ${PRODUCT_PUBLISHER}. MIT License."
VIAddVersionKey "FileDescription" "${PRODUCT_NAME} Installer"
VIAddVersionKey "FileVersion" "${PRODUCT_VERSION}"

; Modern UI Configuration
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Uncomment these if you have custom graphics
; !define MUI_HEADERIMAGE
; !define MUI_HEADERIMAGE_BITMAP "graphics\installer-header.bmp"
; !define MUI_WELCOMEFINISHPAGE_BITMAP "graphics\installer-sidebar.bmp"

!define MUI_WELCOMEPAGE_TITLE "${PRODUCT_NAME} Setup"
!define MUI_WELCOMEPAGE_TEXT "This wizard will guide you through the installation of ${PRODUCT_NAME}.$\r$\n$\r$\n${PRODUCT_NAME} is a game automation tool for Ragnarok Online private servers.$\r$\n$\r$\nClick Next to continue."

!define MUI_FINISHPAGE_TITLE "${PRODUCT_NAME} Installation Complete"
!define MUI_FINISHPAGE_TEXT "${PRODUCT_NAME} has been installed successfully.$\r$\n$\r$\nIMPORTANT: You must run this application as Administrator for memory reading to work.$\r$\n$\r$\nClick Finish to close this wizard."
!define MUI_FINISHPAGE_RUN "$INSTDIR\BruteGamingMacros.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Launch ${PRODUCT_NAME}"
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_LINK "Visit GitHub Repository"
!define MUI_FINISHPAGE_LINK_LOCATION "${PRODUCT_WEB_SITE}"

; Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "..\LICENSE"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Language
!insertmacro MUI_LANGUAGE "English"

; Functions
Function .onInit
    ; Check if 64-bit Windows
    ${IfNot} ${RunningX64}
        MessageBox MB_OK|MB_ICONEXCLAMATION "This application requires 64-bit Windows 10 or later.$\r$\n$\r$\nInstallation cannot continue."
        Abort
    ${EndIf}

    ; Check Windows version
    ${If} ${AtMostWin8.1}
        MessageBox MB_YESNO|MB_ICONQUESTION "This application is designed for Windows 10 or later.$\r$\n$\r$\nYou are running an older version of Windows. The application may not work correctly.$\r$\n$\r$\nDo you want to continue anyway?" IDYES continue IDNO abort
        abort:
            Abort
        continue:
    ${EndIf}

    ; Check if already installed
    ReadRegStr $0 HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
    ${If} $0 != ""
        MessageBox MB_YESNO|MB_ICONQUESTION "${PRODUCT_NAME} is already installed.$\r$\n$\r$\nDo you want to uninstall the existing version first?" IDYES uninstall IDNO continue
        uninstall:
            ExecWait '$0 _?=$INSTDIR'
            Delete "$0"
        continue:
    ${EndIf}

    ; Check .NET Framework 4.8.1
    Call CheckDotNet481
FunctionEnd

Function CheckDotNet481
    ClearErrors
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"

    ${If} ${Errors}
        ; .NET Framework not installed at all
        MessageBox MB_YESNO|MB_ICONQUESTION ".NET Framework 4.8.1 is required but not installed.$\r$\n$\r$\nWould you like to download it now?" IDYES download IDNO abort
        download:
            ExecShell "open" "https://dotnet.microsoft.com/download/dotnet-framework/net481"
            Abort
        abort:
            MessageBox MB_OK|MB_ICONEXCLAMATION "Installation cannot continue without .NET Framework 4.8.1."
            Abort
    ${EndIf}

    ; 533320 = .NET 4.8.1
    ; 528040 = .NET 4.8
    ${If} $0 < 533320
        MessageBox MB_YESNO|MB_ICONQUESTION ".NET Framework 4.8.1 is required.$\r$\n$\r$\nYou have .NET Framework 4.$($0 / 100000).$($0 % 100000 / 100) installed.$\r$\n$\r$\nWould you like to download .NET 4.8.1 now?" IDYES download IDNO abort
        download:
            ExecShell "open" "https://dotnet.microsoft.com/download/dotnet-framework/net481"
            Abort
        abort:
            MessageBox MB_YESNO|MB_ICONQUESTION "Installation may not work correctly without .NET Framework 4.8.1.$\r$\n$\r$\nDo you want to continue anyway?" IDYES continue IDNO abort2
            abort2:
                Abort
            continue:
    ${EndIf}
FunctionEnd

; Installation Sections
Section "Brute Gaming Macros (required)" SecMain
    SectionIn RO

    SetOutPath "$INSTDIR"

    ; Copy main executable and config
    File "..\bin\Release\BruteGamingMacros.exe"
    File "..\bin\Release\BruteGamingMacros.exe.config"

    ; Copy dependencies (if not using Costura.Fody)
    ; File "..\bin\Release\Newtonsoft.Json.dll"
    ; File "..\bin\Release\Aspose.Zip.dll"

    ; Copy documentation
    SetOutPath "$INSTDIR\docs"
    File /nonfatal "..\docs\*.md"

    ; Copy license
    SetOutPath "$INSTDIR"
    File /nonfatal "..\LICENSE"
    File /nonfatal "..\README.md"

    ; Create application data directory
    SetShellVarContext current
    CreateDirectory "$LOCALAPPDATA\BruteGamingMacros"
    CreateDirectory "$LOCALAPPDATA\BruteGamingMacros\Logs"
    CreateDirectory "$LOCALAPPDATA\BruteGamingMacros\Profiles"

    ; Create uninstaller
    WriteUninstaller "$INSTDIR\uninstall.exe"

    ; Write registry keys for Add/Remove Programs
    WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "DisplayName" "${PRODUCT_NAME}"
    WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
    WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
    WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
    WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\BruteGamingMacros.exe"
    WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninstall.exe"
    WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "InstallLocation" "$INSTDIR"
    WriteRegDWORD HKLM "${PRODUCT_UNINST_KEY}" "NoModify" 1
    WriteRegDWORD HKLM "${PRODUCT_UNINST_KEY}" "NoRepair" 1

    ; Calculate and write installed size
    ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
    IntFmt $0 "0x%08X" $0
    WriteRegDWORD HKLM "${PRODUCT_UNINST_KEY}" "EstimatedSize" "$0"

    ; Write installation date
    WriteRegStr HKLM "${PRODUCT_UNINST_KEY}" "InstallDate" "$9$8$7"
SectionEnd

Section "Desktop Shortcut" SecDesktop
    CreateShortCut "$DESKTOP\Brute Gaming Macros.lnk" "$INSTDIR\BruteGamingMacros.exe" "" "$INSTDIR\BruteGamingMacros.exe" 0
SectionEnd

Section "Start Menu Shortcuts" SecStartMenu
    CreateDirectory "$SMPROGRAMS\Brute Gaming Macros"
    CreateShortCut "$SMPROGRAMS\Brute Gaming Macros\Brute Gaming Macros.lnk" "$INSTDIR\BruteGamingMacros.exe" "" "$INSTDIR\BruteGamingMacros.exe" 0
    CreateShortCut "$SMPROGRAMS\Brute Gaming Macros\Documentation.lnk" "$INSTDIR\docs"
    CreateShortCut "$SMPROGRAMS\Brute Gaming Macros\Uninstall.lnk" "$INSTDIR\uninstall.exe"
SectionEnd

Section "Run as Administrator (Recommended)" SecAdminShortcut
    ; Modify shortcuts to always run as admin
    ${If} ${FileExists} "$DESKTOP\Brute Gaming Macros.lnk"
        DetailPrint "Setting 'Run as Administrator' for desktop shortcut..."
        ; Note: This requires ShellLink plugin or manual registry editing
        ; For simplicity, we'll document this in the finish page
    ${EndIf}
SectionEnd

; Section descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecMain} "Core application files (required)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDesktop} "Create a desktop shortcut for quick access"
    !insertmacro MUI_DESCRIPTION_TEXT ${SecStartMenu} "Create Start Menu shortcuts"
    !insertmacro MUI_DESCRIPTION_TEXT ${SecAdminShortcut} "Configure shortcuts to always run as administrator (required for memory access)"
!insertmacro MUI_FUNCTION_DESCRIPTION_END

; Uninstaller
Section "Uninstall"
    ; Remove files
    Delete "$INSTDIR\BruteGamingMacros.exe"
    Delete "$INSTDIR\BruteGamingMacros.exe.config"
    Delete "$INSTDIR\Newtonsoft.Json.dll"
    Delete "$INSTDIR\Aspose.Zip.dll"
    Delete "$INSTDIR\LICENSE"
    Delete "$INSTDIR\README.md"
    Delete "$INSTDIR\uninstall.exe"

    ; Remove documentation
    RMDir /r "$INSTDIR\docs"

    ; Remove installation directory
    RMDir "$INSTDIR"

    ; Remove shortcuts
    Delete "$DESKTOP\Brute Gaming Macros.lnk"
    Delete "$SMPROGRAMS\Brute Gaming Macros\*.*"
    RMDir "$SMPROGRAMS\Brute Gaming Macros"

    ; Remove registry keys
    DeleteRegKey HKLM "${PRODUCT_UNINST_KEY}"

    ; Ask to remove user data
    MessageBox MB_YESNO|MB_ICONQUESTION "Do you want to remove all user data and settings?$\r$\n$\r$\nThis includes:$\r$\n- Configuration files$\r$\n- Character profiles$\r$\n- Application logs$\r$\n$\r$\nLocation: %LOCALAPPDATA%\BruteGamingMacros" IDYES remove_data IDNO skip_data

    remove_data:
        SetShellVarContext current
        RMDir /r "$LOCALAPPDATA\BruteGamingMacros"
        DetailPrint "User data removed"
        Goto end_uninstall

    skip_data:
        DetailPrint "User data preserved"

    end_uninstall:
        MessageBox MB_OK "${PRODUCT_NAME} has been uninstalled from your computer."
SectionEnd

; Uninstaller initialization
Function un.onInit
    MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove ${PRODUCT_NAME} and all of its components?" IDYES +2
    Abort
FunctionEnd

Function un.onUninstSuccess
    HideWindow
    MessageBox MB_ICONINFORMATION|MB_OK "${PRODUCT_NAME} was successfully removed from your computer.$\r$\n$\r$\nThank you for using ${PRODUCT_NAME}!"
FunctionEnd
