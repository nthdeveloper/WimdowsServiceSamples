#define MyAppName "Simple Windows Service"
#define MyAppExeName "SimpleWimdowsService.exe"
#define MyAppVersion GetFileVersion('..\Output\' + MyAppExeName)
#define MyAppPublisher "NthDeveloper"
#define MyAppURL "https://github.com/nthdeveloper"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{66120578-536C-4B5C-B44E-DFBD58C3A5D5}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
AppContact=businessintegration@samsung.se
AppCopyright={#MyAppPublisher}
WizardSmallImageFile=compiler:WizModernSmallImage-IS.bmp
WizardImageBackColor=$272727
WizardImageStretch=false
UsePreviousAppDir=False
DefaultDirName={pf32}\{#MyAppPublisher}\{#MyAppName}\
DirExistsWarning=False
EnableDirDoesntExistWarning=False
AppendDefaultDirName=False
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputBaseFilename={#MyAppName} {#MyAppVersion}
Compression=lzma
SolidCompression=yes
UninstallDisplayName={#MyAppName} {#MyAppVersion}
UninstallDisplayIcon={app}\SetupIcon.ico
;ArchitecturesInstallIn64BitMode=x64
;ArchitecturesAllowed=x64
MinVersion=0,6.1

[Dirs]
Name: "{app}\Logs"

[Files]
Source: "..\Output\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Output\{#MyAppExeName}.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "SetupIcon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Output\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Output\*.pdb"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; IconFilename: "{app}\SetupIcon.ico";

[Run]
Filename: "{app}\{#MyAppExeName}"; Parameters: "--install"; Flags: waituntilterminated runhidden; StatusMsg: "Registering service"

[UninstallRun]
Filename: "{app}\{#MyAppExeName}"; Parameters: "--uninstall"; Flags: waituntilterminated runhidden; StatusMsg: "Unregistering service"

[Code]
////// INSTALLED CONTROL ///////////////////
function GetUninstallString(): String;
var
  sUnInstPath: String;
  sUnInstallString: String;
begin
  sUnInstPath := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppId")}_is1');
  sUnInstallString := '';
  if not RegQueryStringValue(HKLM, sUnInstPath, 'UninstallString', sUnInstallString) then
    RegQueryStringValue(HKCU, sUnInstPath, 'UninstallString', sUnInstallString);
  Result := sUnInstallString;
end;
function IsUpgrade(): Boolean;
begin
  Result := (GetUninstallString() <> '');
end;
function InitializeSetup(): Boolean;
begin
    Result:=true;
    if (IsUpgrade()) then
    begin
      MsgBox('Product is already installed. Please uninstall and try again.', mbInformation, MB_OK);
      Result := false;
    end;
end;
////// INSTALLED CONTROL ///////////////////