﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wordinator.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Sami Viitanen")]
        public string TekijanNimi {
            get {
                return ((string)(this["TekijanNimi"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Wordinator")]
        public string OhjelmanNimi {
            get {
                return ((string)(this["OhjelmanNimi"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1.0")]
        public string Versio {
            get {
                return ((string)(this["Versio"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("28/12/2012")]
        public string VersioPvm {
            get {
                return ((string)(this["VersioPvm"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Vaikeustaso {
            get {
                return ((int)(this["Vaikeustaso"]));
            }
            set {
                this["Vaikeustaso"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("#FFADD8E6")]
        public global::System.Windows.Media.SolidColorBrush ActiveKirjainlaatanTaustaVari {
            get {
                return ((global::System.Windows.Media.SolidColorBrush)(this["ActiveKirjainlaatanTaustaVari"]));
            }
            set {
                this["ActiveKirjainlaatanTaustaVari"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("#FF4682B4")]
        public global::System.Windows.Media.SolidColorBrush ActiveKirjainlaatanReunusvari {
            get {
                return ((global::System.Windows.Media.SolidColorBrush)(this["ActiveKirjainlaatanReunusvari"]));
            }
            set {
                this["ActiveKirjainlaatanReunusvari"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("#FF4682B4")]
        public global::System.Windows.Media.SolidColorBrush InactiveKirjainlaatanTaustavari {
            get {
                return ((global::System.Windows.Media.SolidColorBrush)(this["InactiveKirjainlaatanTaustavari"]));
            }
            set {
                this["InactiveKirjainlaatanTaustavari"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("#FFDAA520")]
        public global::System.Windows.Media.SolidColorBrush InactiveKirjainlaatanReunusvari {
            get {
                return ((global::System.Windows.Media.SolidColorBrush)(this["InactiveKirjainlaatanReunusvari"]));
            }
            set {
                this["InactiveKirjainlaatanReunusvari"] = value;
            }
        }
    }
}
