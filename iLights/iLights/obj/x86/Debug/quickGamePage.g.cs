﻿#pragma checksum "C:\Users\t-idgold\Desktop\iLights\iLights\quickGamePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9FDDE3898F89E58D47D34224AA72DC18"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iLights
{
    partial class quickGamePage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.registerButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 16 "..\..\..\quickGamePage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.registerButton).Click += this.sendStart;
                    #line default
                }
                break;
            case 2:
                {
                    this.quickGame = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 22 "..\..\..\quickGamePage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.quickGame).Click += this.sendOver2;
                    #line default
                }
                break;
            case 3:
                {
                    this.testTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    this.lblTime = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.Go_Back = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 31 "..\..\..\quickGamePage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Go_Back).Click += this.onGoBack;
                    #line default
                }
                break;
            case 6:
                {
                    this.time_left = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
