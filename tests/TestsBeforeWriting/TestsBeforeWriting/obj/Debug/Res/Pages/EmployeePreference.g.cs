﻿#pragma checksum "..\..\..\..\Res\Pages\EmployeePreference.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1AF45B9878CA07000BB003C19E6B81B4ED4F05ADF24B5DB3C6E45E5E1B2CB686"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TestsBeforeWriting.Res.Pages;


namespace TestsBeforeWriting.Res.Pages {
    
    
    /// <summary>
    /// EmployeePreference
    /// </summary>
    public partial class EmployeePreference : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\..\Res\Pages\EmployeePreference.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox employeesList;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Res\Pages\EmployeePreference.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button showClick;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Res\Pages\EmployeePreference.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label show;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TestsBeforeWriting;component/res/pages/employeepreference.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Res\Pages\EmployeePreference.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\..\Res\Pages\EmployeePreference.xaml"
            ((TestsBeforeWriting.Res.Pages.EmployeePreference)(target)).Loaded += new System.Windows.RoutedEventHandler(this.onLoadPreferencePage);
            
            #line default
            #line hidden
            return;
            case 2:
            this.employeesList = ((System.Windows.Controls.ComboBox)(target));
            
            #line 17 "..\..\..\..\Res\Pages\EmployeePreference.xaml"
            this.employeesList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.showClick = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\..\Res\Pages\EmployeePreference.xaml"
            this.showClick.Click += new System.Windows.RoutedEventHandler(this.showClick_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.show = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

