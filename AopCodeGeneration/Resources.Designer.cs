﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AopCodeGeneration {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AopCodeGeneration.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using AopCodeGeneration.Behaviour;
        ///using AopCodeGeneration.Metadata;
        ///
        ///namespace AopCodeGeneration.Samples
        ///{
        ///    public sealed class AppConfigPeriodicRefreshHandlerAopProxy : IAppConfigPeriodicRefreshHandler
        ///    {
        ///        private readonly InterceptionHandler _interceptionHandler;
        ///        private readonly IAppConfigPeriodicRefreshHandler _realInstance;
        ///
        ///        public AppConfigPeriodicRefreshHandlerAopProxy(
        ///            IAppConfigPeriodicRefreshHandler realInstance,
        ///            InterceptionHandler [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AppConfigPeriodicRefreshHandlerAopProxy {
            get {
                return ResourceManager.GetString("AppConfigPeriodicRefreshHandlerAopProxy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using AopCodeGeneration.Behaviour;
        ///using AopCodeGeneration.Metadata;
        ///
        ///namespace AopCodeGeneration.Samples
        ///{
        ///    public sealed class IgnoredInterfaceAopProxy : IIgnoredInterface
        ///    {
        ///        private readonly InterceptionHandler _interceptionHandler;
        ///        private readonly IIgnoredInterface _realInstance;
        ///
        ///        public IgnoredInterfaceAopProxy(IIgnoredInterface realInstance, InterceptionHandler interceptionHandler)
        ///        {
        ///            _realInstance = realInstance;
        ///            _interception [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string IgnoredInterfaceAopProxy {
            get {
                return ResourceManager.GetString("IgnoredInterfaceAopProxy", resourceCulture);
            }
        }
    }
}
