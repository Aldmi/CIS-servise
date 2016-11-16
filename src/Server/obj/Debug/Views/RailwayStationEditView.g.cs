﻿#pragma checksum "..\..\..\Views\RailwayStationEditView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "066761A81D18A61442D56198F82E7FE4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Server.Utils;
using Server.Views;
using Server.WPFExtensions;
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
using System.Windows.Interactivity;
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
using Xceed.Wpf.Toolkit;


namespace Server.Views {
    
    
    /// <summary>
    /// RailwayStationEditView
    /// </summary>
    public partial class RailwayStationEditView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Server.Views.RailwayStationEditView EditView;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton OperativeSchedule;
        
        #line default
        #line hidden
        
        
        #line 156 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton RegulatorySchedule;
        
        #line default
        #line hidden
        
        
        #line 165 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton Info;
        
        #line default
        #line hidden
        
        
        #line 174 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton Diagnostic;
        
        #line default
        #line hidden
        
        
        #line 183 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton Station;
        
        #line default
        #line hidden
        
        
        #line 197 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid StationsTable;
        
        #line default
        #line hidden
        
        
        #line 273 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid OperativeScheduleTable;
        
        #line default
        #line hidden
        
        
        #line 491 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid RegulatorySchedulesTable;
        
        #line default
        #line hidden
        
        
        #line 711 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Save;
        
        #line default
        #line hidden
        
        
        #line 717 "..\..\..\Views\RailwayStationEditView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Clouse;
        
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
            System.Uri resourceLocater = new System.Uri("/Server;component/views/railwaystationeditview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\RailwayStationEditView.xaml"
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
            this.EditView = ((Server.Views.RailwayStationEditView)(target));
            return;
            case 2:
            this.OperativeSchedule = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 3:
            this.RegulatorySchedule = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.Info = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.Diagnostic = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 6:
            this.Station = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 7:
            this.StationsTable = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.OperativeScheduleTable = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 9:
            this.RegulatorySchedulesTable = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 10:
            this.Save = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.Clouse = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

