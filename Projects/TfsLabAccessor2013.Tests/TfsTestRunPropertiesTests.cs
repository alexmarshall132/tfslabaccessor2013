//  -----------------------------------------------------------------------
//  <copyright file="TfsTestRunPropertiesTests.cs" company="West Peaks Consulting.">
//      Copyright (c) West Peaks Consulting. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace TfsLabAccessor2013.Tests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test suite for the <see cref="TfsTestRunProperties"/> class.
    /// </summary>
    [TestClass]
    public class TfsTestRunPropertiesTests
    {
        /// <summary>
        /// Verifies that all properties are correctly parsed
        /// </summary>
        [TestMethod, TestCategory("StandAlone")]
        [Description("Verifies that all properties are correctly parsed")]
        public void CreateCorrectlyParsesValidProperties()
        {
            Dictionary<string, string> testProperties = new Dictionary<string, string>
            {
                { "TestName", "CodedUITestMethod1" },
                { "__Tfs_BuildUri__", "vstfs:///Build/Build/18" },
                { "__Tfs_IsInLabEnvironment__", "True" },
                { "__Tfs_TestRunId__", "22" },
                { "__Tfs_TestCaseId__", "117" },
                { "__Tfs_TeamProject__", "MyTeamProjectName" },
                { "__Tfs_BuildDirectory__", @"\\vsalm\ffdrops\New Build Definition 1\New Build Definition 1_20130222.7" },
                { "__Tfs_LabEnvironment__", @"<?xml version=""1.0"" encoding=""utf-16""?><LabEnvironment Id=""5f37b167-ad24-4f7e-bb1e-2e65a3e71a1f"" Name=""Windows 7 Client"" Uri=""vstfs:///LabManagement/LabEnvironment/2""><LabSystems><LabSystem Name=""TestClient"" ComputerName=""TestClient"" Roles=""Desktop Client""><CustomProperties /></LabSystem></LabSystems></LabEnvironment>" },
                { "__Tfs_TestConfigurationId__", @"2" },
                { "__Tfs_TestPlanId__", @"4" },
                { "__Tfs_TestConfigurationName__", @"Chrome" },
                { "__Tfs_TestPointId__", @"12" },
                { "__Tfs_TfsServerCollectionUrl__", @"http://vsalm:8080/tfs/fabrikamfibercollection" },
                { "__Tfs_BuildPlatform__", @"Any CPU" },
                { "__Tfs_BuildNumber__", @"New Build Definition 1_20130222.7" },
                { "__Tfs_BuildFlavor__", @"Debug" },
                { "__Tfs_BuildConfigurationId__", @"22" }
            };

            TfsTestRunProperties runProperties = TfsTestRunProperties.Create(testProperties);

            Assert.IsNotNull(runProperties, "The run properties returned from Create must never be null");
            Assert.AreEqual("vstfs:///Build/Build/18", runProperties.BuildUri.AbsoluteUri, "The build URIs must match");
            Assert.IsTrue(runProperties.IsInLabEnvironment, "Should have been in a lab environment");
            Assert.AreEqual(22, runProperties.TestRunId, "TestRunId must match");
            Assert.AreEqual(117, runProperties.TestCaseId, "TestCaseId must match");
            Assert.AreEqual("MyTeamProjectName", runProperties.TeamProject, "TeamProject must match");
            Assert.AreEqual(@"\\vsalm\ffdrops\New Build Definition 1\New Build Definition 1_20130222.7", runProperties.BuildDirectory, "BuildDirectory must match");
            Assert.IsNotNull(runProperties.LabEnvironment, "The lab environment must not be null");
            Assert.AreEqual(2, runProperties.TestConfigurationId, "TestConfigurationId must match");
            Assert.AreEqual(4, runProperties.TestPlanId, "TestPlanId must match");
            Assert.AreEqual("Chrome", runProperties.TestConfigurationName, "TestConfigurationName must match");
            Assert.AreEqual(12, runProperties.TestPointId, "TestPointId must match");
            Assert.AreEqual(@"http://vsalm:8080/tfs/fabrikamfibercollection", runProperties.TfsServerCollectionUrl.AbsoluteUri, "TfsServerCollectionUrl must match");
            Assert.AreEqual("Any CPU", runProperties.BuildPlatform, "BuildPlatform must match");
            Assert.AreEqual("New Build Definition 1_20130222.7", runProperties.BuildNumber, "BuildNumber must match");
            Assert.AreEqual("Debug", runProperties.BuildFlavor, "BuildFlavor must match");
            Assert.AreEqual(22, runProperties.BuildConfigurationId, "BuildConfigurationId must match");
        }
    }
}
