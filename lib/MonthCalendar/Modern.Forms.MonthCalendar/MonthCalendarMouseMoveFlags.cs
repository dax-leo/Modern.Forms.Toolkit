namespace Modern.Forms.MonthCalendar
{
    using System;

    /// <summary>
    /// Stores information regarding the element currently under the mouse cursor.
    /// </summary>
    internal class MonthCalendarMouseMoveFlags
    {
        #region fields

        /// <summary>
        /// The backup of the states.
        /// </summary>
        private MonthCalendarMouseMoveFlags backup;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendarMouseMoveFlags"/> class.
        /// </summary>
        public MonthCalendarMouseMoveFlags()
        {
            Reset();
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets a value indicating whether the mouse is over the left arrow.
        /// </summary>
        public bool LeftArrow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse is over the right arrow.
        /// </summary>
        public bool RightArrow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse is over a week header element.
        /// </summary>
        public bool WeekHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse is over the footer.
        /// </summary>
        public bool Footer { get; set; }

        /// <summary>
        /// Gets or sets the date of the month name in a header the mouse is currently over.
        /// </summary>
        public DateTime MonthName { get; set; }

        /// <summary>
        /// Gets or sets the date of the year in a header the mouse is currently over.
        /// </summary>
        public DateTime Year { get; set; }

        /// <summary>
        /// Gets or sets the date of the month if the mouse is over a month header.
        /// </summary>
        public DateTime HeaderDate { get; set; }

        /// <summary>
        /// Gets or sets the date if the mouse is over an <see cref="MonthCalendarDay"/>.
        /// </summary>
        public DateTime Day { get; set; }

        /// <summary>
        /// Gets the backup flags.
        /// </summary>
        public MonthCalendarMouseMoveFlags Backup
        {
            get { return backup ?? (backup = new MonthCalendarMouseMoveFlags()); }
        }

        /// <summary>
        /// Gets a value indicating whether the left arrow state changed.
        /// </summary>
        public bool LeftArrowChanged
        {
            get { return LeftArrow != Backup.LeftArrow; }
        }

        /// <summary>
        /// Gets a value indicating whether the right arrow state changed.
        /// </summary>
        public bool RightArrowChanged
        {
            get { return RightArrow != Backup.RightArrow; }
        }

        /// <summary>
        /// Gets a value indicating whether the week header state changed.
        /// </summary>
        public bool WeekHeaderChanged
        {
            get { return WeekHeader != Backup.WeekHeader; }
        }

        /// <summary>
        /// Gets a value indicating whether the month name state changed.
        /// </summary>
        public bool MonthNameChanged
        {
            get { return MonthName != Backup.MonthName; }
        }

        /// <summary>
        /// Gets a value indicating whether the year state changed.
        /// </summary>
        public bool YearChanged
        {
            get { return Year != Backup.Year; }
        }

        /// <summary>
        /// Gets a value indicating whether the header date state changed.
        /// </summary>
        public bool HeaderDateChanged
        {
            get { return HeaderDate != Backup.HeaderDate; }
        }

        /// <summary>
        /// Gets a value indicating whether the day state changed.
        /// </summary>
        public bool DayChanged
        {
            get { return Day != Backup.Day; }
        }

        /// <summary>
        /// Gets a value indicating whether the footer state changed.
        /// </summary>
        public bool FooterChanged
        {
            get { return Footer != Backup.Footer; }
        }

        #endregion

        #region methods

        /// <summary>
        /// Resets the flags.
        /// </summary>
        public void Reset()
        {
            LeftArrow = RightArrow = WeekHeader = Footer = false;
            MonthName = Year = HeaderDate = Day = DateTime.MinValue;
        }

        /// <summary>
        /// Saves the current values to <see cref="Backup"/> and resets then the current values.
        /// </summary>
        public void BackupAndReset()
        {
            Backup.LeftArrow = LeftArrow;
            Backup.RightArrow = RightArrow;
            Backup.WeekHeader = WeekHeader;
            Backup.MonthName = MonthName;
            Backup.Year = Year;
            Backup.HeaderDate = HeaderDate;
            Backup.Day = Day;
            Backup.Footer = Footer;

            Reset();
        }

        #endregion
    }
}