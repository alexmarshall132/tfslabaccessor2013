//  -----------------------------------------------------------------------
//  <copyright file="TfsLabAccessor2013Tests.cs" company="West Peaks Consulting.">
//      Copyright (c) West Peaks Consulting. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace TfsLabAccessor2013.Tests
{
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test suite for the <see cref="Tfs2013LabAccessor"/> class.
    /// </summary>
    [TestClass]
    public class Tfs2013LabAccessorTests
    {
        public static string UserName
        {
            get { return @"MYDOMAIN\myuser"; }
        }

        public static string Password
        {
            get { return "mypassword"; }
        }

        public static string TeamProjectCollectionUri
        {
            get { return @"https://myorganization.visualstudio.com"; }
        }

        public static string TeamProjectName
        {
            get { return "MyProject"; }
        }

        /// <summary>
        /// The subject under test
        /// </summary>
        private Tfs2013LabAccessor sut;

        /// <summary>
        /// Initializes the current test
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            this.sut = new Tfs2013LabAccessor(new NetworkCredential(UserName, Password));
        }

        /// <summary>
        /// Cleans up the current test
        /// </summary>
        [TestCleanup]
        public void CleanUpTest()
        {
            this.sut = null;
        }

        /// <summary>
        /// Verifies that the accessor correctly retrieves the properties for a configuration
        /// </summary>
        [TestMethod, TestCategory("DevAcceptance")]
        [Description("Verifies that the accessor correctly retrieves the properties for a configuration")]
        public void VerifyAccessorCorrectlyRetrievesConfigurationProperties()
        {
            Dictionary<string, string> runProperties = new Dictionary<string, string>
            {
                { "TestName", "CodedUITestMethod1" },
                { "__Tfs_BuildUri__", "vstfs:///Build/Build/18" },
                { "__Tfs_IsInLabEnvironment__", "True" },
                { "__Tfs_TestRunId__", "22" },
                { "__Tfs_TestCaseId__", "117" },
                { "__Tfs_TeamProject__", TeamProjectName },
                { "__Tfs_BuildDirectory__", @"\\vsalm\ffdrops\New Build Definition 1\New Build Definition 1_20130222.7" },
                { "__Tfs_LabEnvironment__", @"<?xml version=""1.0"" encoding=""utf-16""?><LabEnvironment Id=""5f37b167-ad24-4f7e-bb1e-2e65a3e71a1f"" Name=""Windows 7 Client"" Uri=""vstfs:///LabManagement/LabEnvironment/2""><LabSystems><LabSystem Name=""TestClient"" ComputerName=""TestClient"" Roles=""Desktop Client""><CustomProperties /></LabSystem></LabSystems></LabEnvironment>" },
                { "__Tfs_TestConfigurationId__", @"85" },
                { "__Tfs_TestPlanId__", @"4" },
                { "__Tfs_TestConfigurationName__", @"Chrome" },
                { "__Tfs_TestPointId__", @"12" },
                { "__Tfs_TfsServerCollectionUrl__", TeamProjectCollectionUri },
                { "__Tfs_BuildPlatform__", @"Any CPU" },
                { "__Tfs_BuildNumber__", @"New Build Definition 1_20130222.7" },
                { "__Tfs_BuildFlavor__", @"Debug" },
                { "__Tfs_BuildConfigurationId__", @"85" }
            };

            TfsTestRunProperties tfsTestRunProperties = TfsTestRunProperties.Create(runProperties);

            IDictionary<string, string> configurationVariables = this.sut.GetConfigurationVariables(tfsTestRunProperties);

            Assert.IsNotNull(configurationVariables, "The configuration variables should not have been null");
        }
    }
}