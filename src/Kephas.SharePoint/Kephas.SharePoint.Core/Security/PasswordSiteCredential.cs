// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordSiteCredential.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the password site credential class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security
{
    using Kephas.Diagnostics.Contracts;

    /// <summary>
    /// A password site credential.
    /// </summary>
    public class PasswordSiteCredential : ISiteCredential
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets or sets the encrypted password.
        /// </summary>
        /// <value>
        /// The encrypted password.
        /// </value>
        public string UserEncryptedPassword { get; set; }

        /// <summary>
        /// Parses the string and loads the values from the result.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        public void Load(string value)
        {
            Requires.NotNullOrEmpty(value, nameof(value));

            var idx = value.IndexOf(',');
            if (idx < 0)
            {
                this.UserName = value;
            }
            else
            {
                this.UserName = value.Substring(0, idx);
                this.UserPassword = value.Substring(idx + 1);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"{base.ToString()}: {this.UserName}";
        }
    }
}
