﻿#pragma checksum "..\..\..\frm_EntryForm.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "634909B908300D3FC7FE22D83AE24E8B43D10DF3"
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
using System.Windows.Controls.Ribbon;
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


namespace Winform_ThoiTrang {
    
    
    /// <summary>
    /// frm_EntryForm
    /// </summary>
    public partial class frm_EntryForm : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 40 "..\..\..\frm_EntryForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox NhaCungCapComboBox;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\frm_EntryForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SanPhamComboBox;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\frm_EntryForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox KichThuocComboBox;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\frm_EntryForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox GiaNhapTextBox;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\frm_EntryForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SoLuongTextBox;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\..\frm_EntryForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox GhiChuTextBox;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\..\frm_EntryForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NhapHangButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.7.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Winform_ThoiTrang;component/frm_entryform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\frm_EntryForm.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.7.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.NhaCungCapComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.SanPhamComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 67 "..\..\..\frm_EntryForm.xaml"
            this.SanPhamComboBox.DropDownOpened += new System.EventHandler(this.SanPhamComboBox_DropDownOpened);
            
            #line default
            #line hidden
            return;
            case 3:
            this.KichThuocComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.GiaNhapTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.SoLuongTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.GhiChuTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.NhapHangButton = ((System.Windows.Controls.Button)(target));
            
            #line 163 "..\..\..\frm_EntryForm.xaml"
            this.NhapHangButton.Click += new System.Windows.RoutedEventHandler(this.NhapHangButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

