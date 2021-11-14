############################################################################################
#      NSIS Installation Script created by NSIS Quick Setup Script Generator v1.09.18
#               Entirely Edited with NullSoft Scriptable Installation System                
#              by Vlasis K. Barkas aka Red Wine red_wine@freemail.gr Sep 2006               
############################################################################################

!define APP_NAME "OSRTT Launcher"
!define COMP_NAME "TechteamGB"
!define WEB_SITE "https://youtube.com/techteamgb"
!define VERSION "01.00.00.00"
!define COPYRIGHT "Andrew McDonald  � 2021"
!define DESCRIPTION "OSRTT Launcher & Analysis Tool"
!define LICENSE_TXT "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\LICENSE.txt"
!define INSTALLER_NAME "Y:\$Current Projects\Response Time Tool\$ RELEASE FILES\OSRTT Installer.exe"
!define MAIN_APP_EXE "OSRTT Launcher.exe"
!define INSTALL_TYPE "SetShellVarContext current"
!define REG_ROOT "HKCU"
!define REG_APP_PATH "Software\Microsoft\Windows\CurrentVersion\App Paths\${MAIN_APP_EXE}"
!define UNINSTALL_PATH "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"

######################################################################

VIProductVersion  "${VERSION}"
VIAddVersionKey "ProductName"  "${APP_NAME}"
VIAddVersionKey "CompanyName"  "${COMP_NAME}"
VIAddVersionKey "LegalCopyright"  "${COPYRIGHT}"
VIAddVersionKey "FileDescription"  "${DESCRIPTION}"
VIAddVersionKey "FileVersion"  "${VERSION}"

######################################################################

SetCompressor ZLIB
Name "${APP_NAME}"
Caption "${APP_NAME}"
OutFile "${INSTALLER_NAME}"
BrandingText "${APP_NAME}"
XPStyle on
InstallDirRegKey "${REG_ROOT}" "${REG_APP_PATH}" ""
InstallDir "C:\OSRTT Launcher"

######################################################################

!include "MUI.nsh"

!define MUI_ABORTWARNING
!define MUI_UNABORTWARNING

!insertmacro MUI_PAGE_WELCOME

!ifdef LICENSE_TXT
!insertmacro MUI_PAGE_LICENSE "${LICENSE_TXT}"
!endif

!ifdef REG_START_MENU
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "OSRTT Launcher"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${REG_ROOT}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${UNINSTALL_PATH}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${REG_START_MENU}"
!insertmacro MUI_PAGE_STARTMENU Application $SM_Folder
!endif

!insertmacro MUI_PAGE_INSTFILES

!define MUI_FINISHPAGE_RUN "$INSTDIR\${MAIN_APP_EXE}"
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM

!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

######################################################################

Section -MainProgram
${INSTALL_TYPE}
SetOverwrite ifnewer
SetOutPath "$INSTDIR"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\AutoUpdater.NET.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\AutoUpdater.NET.xml"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\LICENSE.txt"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\Microsoft.Management.Infrastructure.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\Newtonsoft.Json.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\Newtonsoft.Json.xml"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT Launcher.application"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT Launcher.exe"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT Launcher.exe.config"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT Launcher.exe.manifest"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\Results Template.xlsx"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\WindowsDisplayAPI.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\WindowsDisplayAPI.xml"
SetOutPath "$INSTDIR\OSRTT UE4"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Manifest_NonUFSFiles_Win64.txt"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest.exe"
SetOutPath "$INSTDIR\OSRTT UE4\ResponseTimeTest\Content\Paks"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Content\Paks\ResponseTimeTest-WindowsNoEditor.pak"
SetOutPath "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\steamvr_ue_editor_app.json"
SetOutPath "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\gamepad.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\holographic_controller.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\hpmotioncontroller.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\indexhmd.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\knuckles.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\oculus_touch.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\rift.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\steamvr_manifest.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_controller.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_cosmos_controller.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_pro.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_camera.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_chest.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_handed.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_keyboard.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_left_elbow.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_left_foot.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_left_knee.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_left_shoulder.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_right_elbow.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_right_foot.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_right_knee.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_right_shoulder.json"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_waist.json"
SetOutPath "$INSTDIR\OSRTT UE4\ResponseTimeTest\Binaries\Win64"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Binaries\Win64\ResponseTimeTest-Win64-Shipping.exe"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\ResponseTimeTest\Binaries\Win64\turbojpeg.dll"
SetOutPath "$INSTDIR\OSRTT UE4\Engine\Extras\Redist\en-us"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Extras\Redist\en-us\UE4PrereqSetup_x64.exe"
SetOutPath "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Windows\XAudio2_9\x64"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\Windows\XAudio2_9\x64\xaudio2_9redist.dll"
SetOutPath "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Vorbis\Win64\VS2015"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\Vorbis\Win64\VS2015\libvorbisfile_64.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\Vorbis\Win64\VS2015\libvorbis_64.dll"
SetOutPath "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\ApexFramework_x64.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\APEX_Clothing_x64.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\APEX_Legacy_x64.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\NvCloth_x64.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PhysX3Common_x64.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PhysX3Cooking_x64.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PhysX3_x64.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PxFoundation_x64.dll"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PxPvdSDK_x64.dll"
SetOutPath "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Ogg\Win64\VS2015"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\Ogg\Win64\VS2015\libogg_64.dll"
SetOutPath "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Oculus\OVRPlugin\OVRPlugin\Win64"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\Oculus\OVRPlugin\OVRPlugin\Win64\OVRPlugin.dll"
SetOutPath "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\NVIDIA\NVaftermath\Win64"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\NVIDIA\NVaftermath\Win64\GFSDK_Aftermath_Lib.x64.dll"
SetOutPath "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\DbgHelp"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\OSRTT UE4\Engine\Binaries\ThirdParty\DbgHelp\dbghelp.dll"
SetOutPath "$INSTDIR\arduinoCLI"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\arduinoCLI\arduino-cli.exe"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\arduinoCLI\LICENSE.txt"
SetOutPath "$INSTDIR\arduinoCLI\OSRTT_Full_Code"
File "Y:\$Current Projects\Response Time Tool\OSRTT\OSRTT Launcher\OSRTT Launcher\bin\Release\arduinoCLI\OSRTT_Full_Code\OSRTT_Full_Code.ino"
SectionEnd

######################################################################

Section -Icons_Reg
SetOutPath "$INSTDIR"
WriteUninstaller "$INSTDIR\uninstall.exe"

!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_WRITE_BEGIN Application
CreateDirectory "$SMPROGRAMS\$SM_Folder"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\Uninstall ${APP_NAME}.lnk" "$INSTDIR\uninstall.exe"

!ifdef WEB_SITE
WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
!endif
!insertmacro MUI_STARTMENU_WRITE_END
!endif

!ifndef REG_START_MENU
CreateDirectory "$SMPROGRAMS\OSRTT Launcher"
CreateShortCut "$SMPROGRAMS\OSRTT Launcher\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$SMPROGRAMS\OSRTT Launcher\Uninstall ${APP_NAME}.lnk" "$INSTDIR\uninstall.exe"

!ifdef WEB_SITE
WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
CreateShortCut "$SMPROGRAMS\OSRTT Launcher\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
!endif
!endif

WriteRegStr ${REG_ROOT} "${REG_APP_PATH}" "" "$INSTDIR\${MAIN_APP_EXE}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayName" "${APP_NAME}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "UninstallString" "$INSTDIR\uninstall.exe"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayIcon" "$INSTDIR\${MAIN_APP_EXE}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayVersion" "${VERSION}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "Publisher" "${COMP_NAME}"

!ifdef WEB_SITE
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "URLInfoAbout" "${WEB_SITE}"
!endif
SectionEnd

######################################################################

Section Uninstall
${INSTALL_TYPE}
Delete "$INSTDIR\AutoUpdater.NET.dll"
Delete "$INSTDIR\AutoUpdater.NET.xml"
Delete "$INSTDIR\LICENSE.txt"
Delete "$INSTDIR\Microsoft.Management.Infrastructure.dll"
Delete "$INSTDIR\Newtonsoft.Json.dll"
Delete "$INSTDIR\Newtonsoft.Json.xml"
Delete "$INSTDIR\OSRTT Launcher.application"
Delete "$INSTDIR\OSRTT Launcher.exe"
Delete "$INSTDIR\OSRTT Launcher.exe.config"
Delete "$INSTDIR\OSRTT Launcher.exe.manifest"
Delete "$INSTDIR\Results Template.xlsx"
Delete "$INSTDIR\WindowsDisplayAPI.dll"
Delete "$INSTDIR\WindowsDisplayAPI.xml"
Delete "$INSTDIR\OSRTT UE4\Manifest_NonUFSFiles_Win64.txt"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest.exe"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Content\Paks\ResponseTimeTest-WindowsNoEditor.pak"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\steamvr_ue_editor_app.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\gamepad.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\holographic_controller.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\hpmotioncontroller.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\indexhmd.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\knuckles.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\oculus_touch.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\rift.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\steamvr_manifest.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_controller.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_cosmos_controller.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_pro.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_camera.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_chest.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_handed.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_keyboard.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_left_elbow.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_left_foot.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_left_knee.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_left_shoulder.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_right_elbow.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_right_foot.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_right_knee.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_right_shoulder.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings\vive_tracker_waist.json"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Binaries\Win64\ResponseTimeTest-Win64-Shipping.exe"
Delete "$INSTDIR\OSRTT UE4\ResponseTimeTest\Binaries\Win64\turbojpeg.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Extras\Redist\en-us\UE4PrereqSetup_x64.exe"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Windows\XAudio2_9\x64\xaudio2_9redist.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Vorbis\Win64\VS2015\libvorbisfile_64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Vorbis\Win64\VS2015\libvorbis_64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\ApexFramework_x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\APEX_Clothing_x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\APEX_Legacy_x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\NvCloth_x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PhysX3Common_x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PhysX3Cooking_x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PhysX3_x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PxFoundation_x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015\PxPvdSDK_x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Ogg\Win64\VS2015\libogg_64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Oculus\OVRPlugin\OVRPlugin\Win64\OVRPlugin.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\NVIDIA\NVaftermath\Win64\GFSDK_Aftermath_Lib.x64.dll"
Delete "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\DbgHelp\dbghelp.dll"
Delete "$INSTDIR\arduinoCLI\arduino-cli.exe"
Delete "$INSTDIR\arduinoCLI\LICENSE.txt"
Delete "$INSTDIR\arduinoCLI\OSRTT_Full_Code\OSRTT_Full_Code.ino"
 
RmDir "$INSTDIR\arduinoCLI\OSRTT_Full_Code"
RmDir "$INSTDIR\arduinoCLI"
RmDir "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\DbgHelp"
RmDir "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\NVIDIA\NVaftermath\Win64"
RmDir "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Oculus\OVRPlugin\OVRPlugin\Win64"
RmDir "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Ogg\Win64\VS2015"
RmDir "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\PhysX3\Win64\VS2015"
RmDir "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Vorbis\Win64\VS2015"
RmDir "$INSTDIR\OSRTT UE4\Engine\Binaries\ThirdParty\Windows\XAudio2_9\x64"
RmDir "$INSTDIR\OSRTT UE4\Engine\Extras\Redist\en-us"
RmDir "$INSTDIR\OSRTT UE4\ResponseTimeTest\Binaries\Win64"
RmDir "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config\SteamVRBindings"
RmDir "$INSTDIR\OSRTT UE4\ResponseTimeTest\Config"
RmDir "$INSTDIR\OSRTT UE4\ResponseTimeTest\Content\Paks"
RmDir "$INSTDIR\OSRTT UE4"
 
Delete "$INSTDIR\uninstall.exe"
!ifdef WEB_SITE
Delete "$INSTDIR\${APP_NAME} website.url"
!endif

RmDir "$INSTDIR"

!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_GETFOLDER "Application" $SM_Folder
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk"
Delete "$SMPROGRAMS\$SM_Folder\Uninstall ${APP_NAME}.lnk"
!ifdef WEB_SITE
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk"
!endif
Delete "$DESKTOP\${APP_NAME}.lnk"

RmDir "$SMPROGRAMS\$SM_Folder"
!endif

!ifndef REG_START_MENU
Delete "$SMPROGRAMS\OSRTT Launcher\${APP_NAME}.lnk"
Delete "$SMPROGRAMS\OSRTT Launcher\Uninstall ${APP_NAME}.lnk"
!ifdef WEB_SITE
Delete "$SMPROGRAMS\OSRTT Launcher\${APP_NAME} Website.lnk"
!endif
Delete "$DESKTOP\${APP_NAME}.lnk"

RmDir "$SMPROGRAMS\OSRTT Launcher"
!endif

DeleteRegKey ${REG_ROOT} "${REG_APP_PATH}"
DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}"
SectionEnd

######################################################################

