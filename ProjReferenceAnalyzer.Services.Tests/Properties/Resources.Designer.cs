﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjReferenceAnalyzer.Services.Tests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ProjReferenceAnalyzer.Services.Tests.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;packages&gt;
        ///  This represents NuGet references in an old style csproj.
        /// </summary>
        internal static string packages {
            get {
                return ResourceManager.GetString("packages", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] ProjReferenceAnalyzer {
            get {
                object obj = ResourceManager.GetObject("ProjReferenceAnalyzer", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Project Sdk=&quot;Microsoft.NET.Sdk&quot;&gt;
        ///
        ///  &lt;PropertyGroup&gt;
        ///    &lt;TargetFramework&gt;netcoreapp2.1&lt;/TargetFramework&gt;
        ///
        ///    &lt;IsPackable&gt;false&lt;/IsPackable&gt;
        ///  &lt;/PropertyGroup&gt;
        ///
        ///  &lt;ItemGroup&gt;
        ///    &lt;PackageReference Include=&quot;Microsoft.NET.Test.Sdk&quot; Version=&quot;15.7.0&quot; /&gt;
        ///    &lt;PackageReference Include=&quot;Shouldly&quot; Version=&quot;3.0.0&quot; /&gt;
        ///    &lt;PackageReference Include=&quot;xunit&quot; Version=&quot;2.3.1&quot; /&gt;
        ///    &lt;PackageReference Include=&quot;xunit.runner.visualstudio&quot; Version=&quot;2.3.1&quot; /&gt;
        ///    &lt;DotNetCliToolReference Include=&quot;dotnet-xunit&quot; Ver [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ProjReferenceAnalyzer_Services_Tests {
            get {
                return ResourceManager.GetString("ProjReferenceAnalyzer_Services_Tests", resourceCulture);
            }
        }
    }
}
