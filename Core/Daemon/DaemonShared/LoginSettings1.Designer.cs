﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DaemonShared {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
    public sealed partial class LoginSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static LoginSettings defaultInstance = ((LoginSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new LoginSettings())));
        
        public static LoginSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Debug {
            get {
                return ((bool)(this["Debug"]));
            }
            set {
                this["Debug"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("VO0e+84BW4wqVYsuUpGeWw==")]
        public string Password {
            get {
                return ((string)(this["Password"]));
            }
            set {
                this["Password"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime LastCommunication {
            get {
                return ((global::System.DateTime)(this["LastCommunication"]));
            }
            set {
                this["LastCommunication"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50a7cd9f-d5f9-4c40-8e0f-bfcbb21a5f0e")]
        public global::System.Guid Uuid {
            get {
                return ((global::System.Guid)(this["Uuid"]));
            }
            set {
                this["Uuid"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:3393")]
        public string Server {
            get {
                return ((string)(this["Server"]));
            }
            set {
                this["Server"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public int SessionLengthMinutes {
            get {
                return ((int)(this["SessionLengthMinutes"]));
            }
            set {
                this["SessionLengthMinutes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int SessionLengthPaddingMinutes {
            get {
                return ((int)(this["SessionLengthPaddingMinutes"]));
            }
            set {
                this["SessionLengthPaddingMinutes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool TimerDebugOnly {
            get {
                return ((bool)(this["TimerDebugOnly"]));
            }
            set {
                this["TimerDebugOnly"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("tRnhF0IfmkDrIZU6dbCusQ==;1")]
        public string PreSharedKeyWithIdSemiColSep {
            get {
                return ((string)(this["PreSharedKeyWithIdSemiColSep"]));
            }
            set {
                this["PreSharedKeyWithIdSemiColSep"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.1.0")]
        public string Version {
            get {
                return ((string)(this["Version"]));
            }
            set {
                this["Version"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public int LoginFailureWaitPeriodMs {
            get {
                return ((int)(this["LoginFailureWaitPeriodMs"]));
            }
            set {
                this["LoginFailureWaitPeriodMs"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("900000")]
        public int TaskRefreshPeriodMs {
            get {
                return ((int)(this["TaskRefreshPeriodMs"]));
            }
            set {
                this["TaskRefreshPeriodMs"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int LoggingLevel {
            get {
                return ((int)(this["LoggingLevel"]));
            }
            set {
                this["LoggingLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("00000000-0000-0000-0000-000000000000")]
        public global::System.Guid SessionUuid {
            get {
                return ((global::System.Guid)(this["SessionUuid"]));
            }
            set {
                this["SessionUuid"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public int LoginMaxRetryCount {
            get {
                return ((int)(this["LoginMaxRetryCount"]));
            }
            set {
                this["LoginMaxRetryCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://localhost:44318")]
        public string SSLServer {
            get {
                return ((string)(this["SSLServer"]));
            }
            set {
                this["SSLServer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SSLUse {
            get {
                return ((bool)(this["SSLUse"]));
            }
            set {
                this["SSLUse"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SSLAllowSelfSigned {
            get {
                return ((bool)(this["SSLAllowSelfSigned"]));
            }
            set {
                this["SSLAllowSelfSigned"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection LocalErrors {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["LocalErrors"]));
            }
            set {
                this["LocalErrors"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6")]
        public int StoreLocalErrorsLevel {
            get {
                return ((int)(this["StoreLocalErrorsLevel"]));
            }
            set {
                this["StoreLocalErrorsLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PipeOK {
            get {
                return ((bool)(this["PipeOK"]));
            }
            set {
                this["PipeOK"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("300000")]
        public int PipeConnectTimeoutMs {
            get {
                return ((int)(this["PipeConnectTimeoutMs"]));
            }
            set {
                this["PipeConnectTimeoutMs"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SSLHttpFallback {
            get {
                return ((bool)(this["SSLHttpFallback"]));
            }
            set {
                this["SSLHttpFallback"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string RSAPrivate {
            get {
                return ((string)(this["RSAPrivate"]));
            }
            set {
                this["RSAPrivate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool FirstSetup {
            get {
                return ((bool)(this["FirstSetup"]));
            }
            set {
                this["FirstSetup"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SkipTasks {
            get {
                return ((bool)(this["SkipTasks"]));
            }
            set {
                this["SkipTasks"] = value;
            }
        }
    }
}
