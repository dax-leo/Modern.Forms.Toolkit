﻿namespace Modern.Forms.MonthCalendar.EventClasses
{
    using System;
    using Modern.Forms.MonthCalendar;

    /// <summary>
    /// Provides data for the <see cref="MonthCalendar.DateClicked"/> event.
    /// </summary>
    public class DateEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateEventArgs"/> class.
        /// </summary>
        /// <param name="date">The date value.</param>
        public DateEventArgs(DateTime date)
        {
            Date = date;
        }

        /// <summary>
        /// Gets the date value.
        /// </summary>
        public DateTime Date { get; private set; }
    }
}