namespace TfsLabAccessor2013
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.TestManagement.Client;

    /// <summary>
    /// Default implementation of <see cref="ITfsLabAccessor2013"/> that uses the TFS
    /// SDK to access TFS.
    /// </summary>
    public class Tfs2013LabAccessor : ITfsLabAccessor2013
    {
        /// <summary>
        /// The credentials used to access the TFS server
        /// </summary>
        private readonly NetworkCredential credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tfs2013LabAccessor"/> class. Use this
        /// constructor by default, as this should provide the correct access on a Test Agent
        /// by the agent principal executing the automated tests.
        /// </summary>
        public Tfs2013LabAccessor()
            : this(CredentialCache.DefaultNetworkCredentials)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tfs2013LabAccessor"/> class.
        /// </summary>
        /// <param name="credentials">
        /// The credentials used to access the TFS server. Must not be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="credentials"/> is null.
        /// </exception>
        public Tfs2013LabAccessor(NetworkCredential credentials)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException("credentials");
            }

            this.credentials = credentials;
        }

        /// <inheritdoc />
        public IDictionary<string, string> GetConfigurationVariables(TfsTestRunProperties runProperties)
        {
            if (runProperties == null)
            {
                throw new ArgumentNullException("runProperties");
            }

            TfsTeamProjectCollection teamProjectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(runProperties.TfsServerCollectionUrl));

            teamProjectCollection.Credentials = this.credentials;

            TestManagementService testService = teamProjectCollection.GetService<TestManagementService>();

            ITestManagementTeamProject teamProject = testService.GetTeamProject(runProperties.TeamProject);

            if (teamProject == null)
            {
                throw new InvalidOperationException(String.Format("Failed to find team project named '{0}'", runProperties.TeamProject));
            }

            ITestConfiguration testConfiguration = teamProject.TestConfigurations.Find(runProperties.TestConfigurationId);

            if (testConfiguration == null)
            {
                throw new InvalidOperationException(String.Format("Failed to find test configuration with ID '{0}'", runProperties.TestConfigurationId));
            }

            return testConfiguration.Values;
        }
    }
}