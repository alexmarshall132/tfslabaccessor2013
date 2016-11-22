
namespace TfsLabAccessor2013
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Represents the properties for a Test Run injected into tests during a TFS Lab Managed
    /// test run execution.
    /// </summary>
    public class TfsTestRunProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TfsTestRunProperties"/>
        /// </summary>
        private TfsTestRunProperties()
        {
        }

        /// <summary>
        /// Gets the <see cref="Uri"/> of the build in TFS.
        /// </summary>
        public Uri BuildUri { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the build is currently running in a Lab managed environment.
        /// </summary>
        public bool IsInLabEnvironment { get; private set; }

        /// <summary>
        /// Gets the ID of the current test run within TFS.
        /// </summary>
        public int TestRunId { get; private set; }

        /// <summary>
        /// Gets the ID of the Test Case associated with the currently executing test method.
        /// </summary>
        public int TestCaseId { get; private set; }

        /// <summary>
        /// Gets the name of the Team Project in which the currently executing Test Plan is defined.
        /// </summary>
        public string TeamProject { get; private set; }

        /// <summary>
        /// Gets the current build directory.
        /// </summary>
        public string BuildDirectory { get; private set; }

        /// <summary>
        /// Gets the XML representation of the lab environment executing the Test Plan.
        /// </summary>
        public XDocument LabEnvironment { get; private set; }

        /// <summary>
        /// Gets the ID of the configuration used with the Test Plan.
        /// </summary>
        public int TestConfigurationId { get; private set; }

        /// <summary>
        /// Gets the ID of the currently executing Test Plan.
        /// </summary>
        public int TestPlanId { get; private set; }

        /// <summary>
        /// Gets the name of the test configuration used with the currently executing Test Plan.
        /// </summary>
        public string TestConfigurationName { get; private set; }

        /// <summary>
        /// Gets the ID of the Test Point associated with the currently executing Test Method.
        /// </summary>
        public int TestPointId { get; private set; }

        /// <summary>
        /// Gets the URL of the TFS Team Project Collection that holds the Team Project that owns
        /// the currently executing Test Method.
        /// </summary>
        public string TfsServerCollectionUrl { get; private set; }

        /// <summary>
        /// Gets the build platform of the current test run.
        /// </summary>
        public string BuildPlatform { get; private set; }

        /// <summary>
        /// Gets the build configuration (aka build flavor) of the test build that's currently executing
        /// </summary>
        public string BuildFlavor { get; private set; }

        /// <summary>
        /// Gets the build number that's currently executing.
        /// </summary>
        public string BuildNumber { get; private set; }

        /// <summary>
        /// Gets the ID of the build configuration.
        /// </summary>
        public int BuildConfigurationId { get; private set; }

        /// <summary>
        /// Gets a boolean value indicating whether or not this instance is valid, i.e. properties
        /// were found to populate it.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Creates a new <see cref="TfsTestRunProperties"/> instance from the given <paramref name="testProperties"/>
        /// </summary>
        /// <param name="testProperties">
        /// The test properties from which we want to extract the TFS Test Run settings. Must not be null.
        /// This should typically be <see cref="TestContext.Properties"/>
        /// </param>
        /// <returns>
        /// A new <see cref="TfsTestRunProperties"/> instance generated from the given <paramref name="testProperties"/>.
        /// Guaranteed not to be null.
        /// </returns>
        public static TfsTestRunProperties Create(IDictionary testProperties)
        {
            if (testProperties == null)
            {
                throw new ArgumentNullException("testProperties");
            }

            Dictionary<string, object> properties = testProperties.Keys.OfType<string>()
                .Where(k => k.StartsWith("__Tfs_") && k.EndsWith("__"))
                .ToDictionary(x => x.TrimStart('_').TrimEnd('_').Substring("Tfs_".Length), k => testProperties[k]);

            TfsTestRunProperties runProperties = new TfsTestRunProperties();

            if (properties.Count == 0)
            {
                return runProperties;
            }

            foreach (var kvp in properties)
            {
                PropertyInfo property = runProperties.GetType().GetProperty(kvp.Key);

                if (property == null)
                {
                    continue;
                }

                if (kvp.Value == null || (String.Empty.Equals(kvp.Value) && property.PropertyType != typeof(string)))
                {
                    continue;
                }

                // Special case because there's no default converter for XDocument.
                if (property.Name == "LabEnvironment")
                {
                    runProperties.LabEnvironment = XDocument.Parse(Convert.ToString(kvp.Value));

                    continue;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);

                if (converter == null)
                {
                    Trace.TraceError("Failed to find converter for type '{0}'", property.PropertyType.FullName);

                    continue;
                }

                property.GetSetMethod(true).Invoke(runProperties, new[] { kvp.Value.GetType() == property.PropertyType ? kvp.Value : converter.ConvertFrom(kvp.Value) });
            }

            runProperties.IsValid = true;

            return runProperties;
        }
    }
}
