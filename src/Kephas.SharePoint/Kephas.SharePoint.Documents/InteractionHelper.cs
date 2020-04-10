// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InteractionHelper.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the interaction helper class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// An interaction helper.
    /// </summary>
    public static class InteractionHelper
    {
        /// <summary>
        /// Name of the source argument.
        /// </summary>
        public const string SourceArgName = "Source";

        /// <summary>
        /// Converts the provided pattern to a regular expression.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>The regular expression.</returns>
        public static Regex ToRegex(this string pattern)
        {
            var redirectRegExPattern = new StringBuilder(pattern)
                .Replace(".", "\\.")
                .Replace("*", ".*")
                .Replace("_", ".")
                .Insert(0, '^')
                .Append('$');

            return new Regex(redirectRegExPattern.ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
        }
    }
}
