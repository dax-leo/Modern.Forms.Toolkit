namespace Modern.Forms.MonthCalendar
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Represents a day in the <see cref="MonthCalendar"/>.
    /// </summary>
    public class MonthCalendarDay
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendarDay"/> class.
        /// </summary>
        /// <param name="month">The <see cref="MonthCalendarMonth"/> in which the day is in.</param>
        /// <param name="date">The <see cref="DateTime"/> the <see cref="MonthCalendarDay"/> represents.</param>
        public MonthCalendarDay(MonthCalendarMonth month, DateTime date)
        {
            Month = month;
            Date = date;
            MonthCalendar = month.MonthCalendar;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the bounds of the day.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets the date the <see cref="MonthCalendarDay"/> represents.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the <see cref="MonthCalendarMonth"/> the day is in.
        /// </summary>
        public MonthCalendarMonth Month { get; private set; }

        /// <summary>
        /// Gets the <see cref="MonthCalendar"/> the <see cref="MonthCalendarMonth"/> is in.
        /// </summary>
        public MonthCalendar MonthCalendar { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the represented date is selected.
        /// </summary>
        public bool Selected
        {
            get { return MonthCalendar.IsSelected(Date); }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is over the represented date.
        /// </summary>
        public bool MouseOver
        {
            get
            {
                return Date == MonthCalendar.MouseOverDay;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the represented date is a trailing one.
        /// </summary>
        public bool TrailingDate
        {
            get
            {
                return MonthCalendar.CultureCalendar.GetMonth(Date) != MonthCalendar.CultureCalendar.GetMonth(Month.Date);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the represented date is visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                if (Date == MonthCalendar.ViewStart && MonthCalendar.ViewStart == MonthCalendar.MinDate)
                {
                    return true;
                }

                return Date >= MonthCalendar.MinDate && Date <= MonthCalendar.MaxDate && !(TrailingDate
                   && Date >= MonthCalendar.ViewStart
                   && Date <= MonthCalendar.ViewEnd);
            }
        }

        #endregion
    }
}