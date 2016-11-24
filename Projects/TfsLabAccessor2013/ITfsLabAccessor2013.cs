//  -----------------------------------------------------------------------
//  <copyright file="ITfsLabAccessor2013.cs" company="West Peaks Consulting.">
//      Copyright (c) West Peaks Consulting. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace TfsLabAccessor2013
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for objects that provide access to TFS lab in TFS 2013 (or versions
    /// of TFS that support that API level)
    /// </summary>
    /// <remarks>
    /// This interface exists so that callers of this library can mock out dependencies.
    /// </remarks>
    public interface ITfsLabAccessor2013
    {
        /// <summary>
        /// Gets the Configuration properties for the Configuration of the current Test Run
        /// </summary>
        /// <param name="runProperties">
        /// The test run properties that provide information about how to access the configuration.
        /// Must not be null.
        /// </param>
        /// <returns>
        /// An <see cref="IDictionary{TKey,TValue}"/> of the properties resolved for the 
        /// configuration of the current test run. Guaranteed not to be null, may be empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="runProperties"/> is null.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if there are any problems retrieving the configuration variables.
        /// </exception>
        IDictionary<string, string> GetConfigurationVariables(TfsTestRunProperties runProperties);
    }
}