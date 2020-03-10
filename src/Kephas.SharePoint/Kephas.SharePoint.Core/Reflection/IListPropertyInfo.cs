// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListPropertyInfo.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IListPropertyInfo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Reflection
{
    using Kephas.Reflection;

    /// <summary>
    /// Values that represent list property kinds.
    /// </summary>
    /// <seealso/>
    public enum ListPropertyKind
    {
        Invalid = 0,
        Integer = 1,
        Text = 2,
        Note = 3,
        DateTime = 4,
        Counter = 5,
        Choice = 6,
        Lookup = 7,
        Boolean = 8,
        Number = 9,
        Currency = 10,
        URL = 11,
        Computed = 12,
        Threading = 13,
        Guid = 14,
        MultiChoice = 15,
        GridChoice = 16,
        Calculated = 17,
        File = 18,
        Attachments = 19,
        User = 20,
        Recurrence = 21,
        CrossProjectLink = 22,
        ModStat = 23,
        Error = 24,
        ContentTypeId = 25,
        PageSeparator = 26,
        ThreadIndex = 27,
        WorkflowStatus = 28,
        AllDayEvent = 29,
        WorkflowEventType = 30,
        Geolocation = 31,
        OutcomeChoice = 32,
        Location = 33,
        Thumbnail = 34,
        MaxItems = 35
    }

    /// <summary>
    /// Interface for property information.
    /// </summary>
    public interface IListPropertyInfo : IPropertyInfo
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; }

        /// <summary>
        /// Gets the property kind.
        /// </summary>
        /// <value>
        /// The property kind.
        /// </value>
        public ListPropertyKind Kind { get; }
    }
}
