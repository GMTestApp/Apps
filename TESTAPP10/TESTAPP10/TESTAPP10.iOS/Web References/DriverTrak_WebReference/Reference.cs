﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace TESTAPP10.iOS.DriverTrak_WebReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ValidateLoginSoap", Namespace="http://tempuri.org/")]
    public partial class ValidateLogin : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback NewRegistrationOperationCompleted;
        
        private System.Threading.SendOrPostCallback ForgotPasswordOperationCompleted;
        
        private System.Threading.SendOrPostCallback NewDriverEntryOperationCompleted;
        
        private System.Threading.SendOrPostCallback CodeExistOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ValidateLogin() {
            this.Url = "http://204.93.158.163/CentralPool/ValidateLogin.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event NewRegistrationCompletedEventHandler NewRegistrationCompleted;
        
        /// <remarks/>
        public event ForgotPasswordCompletedEventHandler ForgotPasswordCompleted;
        
        /// <remarks/>
        public event NewDriverEntryCompletedEventHandler NewDriverEntryCompleted;
        
        /// <remarks/>
        public event CodeExistCompletedEventHandler CodeExistCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/NewRegistration", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string NewRegistration(string UserID, string Pass, string InviteCode) {
            object[] results = this.Invoke("NewRegistration", new object[] {
                        UserID,
                        Pass,
                        InviteCode});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void NewRegistrationAsync(string UserID, string Pass, string InviteCode) {
            this.NewRegistrationAsync(UserID, Pass, InviteCode, null);
        }
        
        /// <remarks/>
        public void NewRegistrationAsync(string UserID, string Pass, string InviteCode, object userState) {
            if ((this.NewRegistrationOperationCompleted == null)) {
                this.NewRegistrationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnNewRegistrationOperationCompleted);
            }
            this.InvokeAsync("NewRegistration", new object[] {
                        UserID,
                        Pass,
                        InviteCode}, this.NewRegistrationOperationCompleted, userState);
        }
        
        private void OnNewRegistrationOperationCompleted(object arg) {
            if ((this.NewRegistrationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.NewRegistrationCompleted(this, new NewRegistrationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ForgotPassword", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ForgotPassword(string UserID, string Pass, string InviteCode) {
            object[] results = this.Invoke("ForgotPassword", new object[] {
                        UserID,
                        Pass,
                        InviteCode});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ForgotPasswordAsync(string UserID, string Pass, string InviteCode) {
            this.ForgotPasswordAsync(UserID, Pass, InviteCode, null);
        }
        
        /// <remarks/>
        public void ForgotPasswordAsync(string UserID, string Pass, string InviteCode, object userState) {
            if ((this.ForgotPasswordOperationCompleted == null)) {
                this.ForgotPasswordOperationCompleted = new System.Threading.SendOrPostCallback(this.OnForgotPasswordOperationCompleted);
            }
            this.InvokeAsync("ForgotPassword", new object[] {
                        UserID,
                        Pass,
                        InviteCode}, this.ForgotPasswordOperationCompleted, userState);
        }
        
        private void OnForgotPasswordOperationCompleted(object arg) {
            if ((this.ForgotPasswordCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ForgotPasswordCompleted(this, new ForgotPasswordCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/NewDriverEntry", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool NewDriverEntry(string DriverID, string InviteCode, string CompanyId, string Type, string URL, string BackgroundLocationUpdate) {
            object[] results = this.Invoke("NewDriverEntry", new object[] {
                        DriverID,
                        InviteCode,
                        CompanyId,
                        Type,
                        URL,
                        BackgroundLocationUpdate});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void NewDriverEntryAsync(string DriverID, string InviteCode, string CompanyId, string Type, string URL, string BackgroundLocationUpdate) {
            this.NewDriverEntryAsync(DriverID, InviteCode, CompanyId, Type, URL, BackgroundLocationUpdate, null);
        }
        
        /// <remarks/>
        public void NewDriverEntryAsync(string DriverID, string InviteCode, string CompanyId, string Type, string URL, string BackgroundLocationUpdate, object userState) {
            if ((this.NewDriverEntryOperationCompleted == null)) {
                this.NewDriverEntryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnNewDriverEntryOperationCompleted);
            }
            this.InvokeAsync("NewDriverEntry", new object[] {
                        DriverID,
                        InviteCode,
                        CompanyId,
                        Type,
                        URL,
                        BackgroundLocationUpdate}, this.NewDriverEntryOperationCompleted, userState);
        }
        
        private void OnNewDriverEntryOperationCompleted(object arg) {
            if ((this.NewDriverEntryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.NewDriverEntryCompleted(this, new NewDriverEntryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CodeExist", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool CodeExist(string DriverID, string InviteCode) {
            object[] results = this.Invoke("CodeExist", new object[] {
                        DriverID,
                        InviteCode});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void CodeExistAsync(string DriverID, string InviteCode) {
            this.CodeExistAsync(DriverID, InviteCode, null);
        }
        
        /// <remarks/>
        public void CodeExistAsync(string DriverID, string InviteCode, object userState) {
            if ((this.CodeExistOperationCompleted == null)) {
                this.CodeExistOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCodeExistOperationCompleted);
            }
            this.InvokeAsync("CodeExist", new object[] {
                        DriverID,
                        InviteCode}, this.CodeExistOperationCompleted, userState);
        }
        
        private void OnCodeExistOperationCompleted(object arg) {
            if ((this.CodeExistCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CodeExistCompleted(this, new CodeExistCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void NewRegistrationCompletedEventHandler(object sender, NewRegistrationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class NewRegistrationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal NewRegistrationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void ForgotPasswordCompletedEventHandler(object sender, ForgotPasswordCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ForgotPasswordCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ForgotPasswordCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void NewDriverEntryCompletedEventHandler(object sender, NewDriverEntryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class NewDriverEntryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal NewDriverEntryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void CodeExistCompletedEventHandler(object sender, CodeExistCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CodeExistCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CodeExistCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591