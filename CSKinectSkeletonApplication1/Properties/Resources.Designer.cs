﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSKinectSkeletonApplication1.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CSKinectSkeletonApplication1.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;grammar version=&quot;1.0&quot; xml:lang=&quot;en-US&quot; tag-format=&quot;semantics/1.0-literals&quot; xmlns=&quot;http://www.w3.org/2001/06/grammar&quot;&gt;
        ///  &lt;rule id=&quot;welcomeRule&quot; scope=&quot;public&quot;&gt;
        ///    &lt;one-of&gt;
        ///      &lt;item&gt;
        ///        &lt;tag&gt;START&lt;/tag&gt;
        ///        &lt;one-of&gt;
        ///          &lt;item&gt;start&lt;/item&gt;
        ///          &lt;item&gt;start game&lt;/item&gt;
        ///        &lt;/one-of&gt;
        ///      &lt;/item&gt;
        ///    &lt;/one-of&gt;
        ///  &lt;/rule&gt;
        ///  &lt;rule id=&quot;gameRule&quot; scope=&quot;public&quot;&gt;
        ///    &lt;one-of&gt;
        ///      &lt;item&gt;
        ///        &lt;tag&gt;RULESENABLE&lt;/tag&gt;
        ///        &lt;one-of&gt;
        ///          &lt;item&gt;enable rules&lt;/item&gt;
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SpeechGrammar {
            get {
                return ResourceManager.GetString("SpeechGrammar", resourceCulture);
            }
        }
    }
}
