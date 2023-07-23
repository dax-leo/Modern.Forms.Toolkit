namespace Modern.Forms.MonthCalendar
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using Modern.Forms.MonthCalendar.Interfaces;

    /// <summary>
    /// Class for handling day and month names.
    /// </summary>
    public class MonthCalendarFormatProvider : ICustomFormatProvider
    {
        #region fields

        /// <summary>
        /// Holds the date time format info.
        /// </summary>
        private DateTimeFormatInfo dtfi;

        /// <summary>
        /// Holds the number format info.
        /// </summary>
        private NumberFormatInfo nfi;

        /// <summary>
        /// The calendar to use.
        /// </summary>
        private Calendar calendar;

        /// <summary>
        /// Stores the month names.
        /// </summary>
        private string[] monthNames;

        /// <summary>
        /// Stores the abbreviated month names.
        /// </summary>
        private string[] abbrMonthNames;

        /// <summary>
        /// Stores the day names.
        /// </summary>
        private string[] dayNames;

        /// <summary>
        /// Stores the abbreviated day names.
        /// </summary>
        private string[] abbrDayNames;

        /// <summary>
        /// Stores the shortest day names.
        /// </summary>
        private string[] shortestDayNames;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendarFormatProvider"/> class.
        /// </summary>
        /// <param name="ci">The <see cref="CultureInfo"/> object to use.</param>
        /// <param name="cal">The calendar to use. If <c>null</c>, the calendar of the <paramref name="ci"/> is used.</param>
        /// <param name="rtlCulture">true if culture is right to left; otherwise false.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="ci"/> is <c>null</c>.</exception>
        public MonthCalendarFormatProvider(CultureInfo ci, Calendar cal, bool rtlCulture)
        {
            if (ci == null)
            {
                throw new ArgumentNullException("ci", "parameter 'ci' cannot be null.");
            }

            DateTimeFormat = ci.DateTimeFormat;
            nfi = ci.NumberFormat;
            Calendar = cal;
            IsRTLLanguage = rtlCulture;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the date time format info object.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTimeFormatInfo DateTimeFormat
        {
            get
            {
                return dtfi;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                dtfi = value;
                calendar = value.Calendar;
                dayNames = dtfi.DayNames;
                abbrDayNames = dtfi.AbbreviatedDayNames;
                shortestDayNames = dtfi.ShortestDayNames;
                FirstDayOfWeek = dtfi.FirstDayOfWeek;
                monthNames = dtfi.MonthNames;
                abbrMonthNames = dtfi.AbbreviatedMonthNames;
                DateSeparator = dtfi.DateSeparator;
                ShortDatePattern = dtfi.ShortDatePattern;
                LongDatePattern = dtfi.LongDatePattern;
                MonthDayPattern = dtfi.MonthDayPattern;
                YearMonthPattern = dtfi.YearMonthPattern;
            }
        }

        /// <summary>
        /// Gets or sets the used calendar.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Calendar Calendar
        {
            get
            {
                return calendar;
            }

            set
            {
                calendar = value ?? dtfi.Calendar;
            }
        }

        /// <summary>
        /// Gets or sets the month names.
        /// </summary>
        [Description("The full month names.")]
        public virtual string[] MonthNames
        {
            get
            {
                return monthNames;
            }

            set
            {
                if (value == null || value.Length < 12)
                {
                    return;
                }

                if (value.Where((t, i) => string.IsNullOrEmpty(t) && i != 12).Any())
                {
                    return;
                }

                monthNames = value;
            }
        }

        /// <summary>
        /// Gets or sets the abbreviated month names.
        /// </summary>
        [Description("The abbreviated month names.")]
        public virtual string[] AbbreviatedMonthNames
        {
            get
            {
                return abbrMonthNames;
            }

            set
            {
                if (value == null || value.Length < 12)
                {
                    return;
                }

                if (value.Where((t, i) => string.IsNullOrEmpty(t) && i != 12).Any())
                {
                    return;
                }

                abbrMonthNames = value;
            }
        }

        /// <summary>
        /// Gets or sets the string that separates the day components, that is the year, month and day.
        /// </summary>
        [Description("The date separator.")]
        public virtual string DateSeparator { get; set; }

        /// <summary>
        /// Gets or sets the short date pattern.
        /// </summary>
        [Description("The short date pattern.")]
        public virtual string ShortDatePattern { get; set; }

        /// <summary>
        /// Gets or sets the long date pattern.
        /// </summary>
        [Description("The long date pattern.")]
        public virtual string LongDatePattern { get; set; }

        /// <summary>
        /// Gets or sets the month day pattern.
        /// </summary>
        [Description("The month day pattern.")]
        public virtual string MonthDayPattern { get; set; }

        /// <summary>
        /// Gets or sets the year month pattern.
        /// </summary>
        [Description("The year month pattern.")]
        public virtual string YearMonthPattern { get; set; }

        /// <summary>
        /// Gets or sets an array of type <see cref="string"/> containing the day names.
        /// </summary>
        [Description("The full day names.")]
        public virtual string[] DayNames
        {
            get
            {
                return dayNames;
            }

            set
            {
                if (value == null || value.Length != 7)
                {
                    return;
                }

                if (value.Any(string.IsNullOrEmpty))
                {
                    return;
                }

                dayNames = value;
            }
        }

        /// <summary>
        /// Gets or sets an array of type <see cref="string"/> containing the abbreviated day names.
        /// </summary>
        [Description("The abbreviated day names.")]
        public virtual string[] AbbreviatedDayNames
        {
            get
            {
                return abbrDayNames;
            }

            set
            {
                if (value == null || value.Length != 7)
                {
                    return;
                }

                if (value.Any(string.IsNullOrEmpty))
                {
                    return;
                }

                abbrDayNames = value;
            }
        }

        /// <summary>
        /// Gets or sets an array of type <see cref="string"/> containing the shortest day names.
        /// </summary>
        [Description("The shortest day names.")]
        public virtual string[] ShortestDayNames
        {
            get
            {
                return shortestDayNames;
            }

            set
            {
                if (value == null || value.Length != 7)
                {
                    return;
                }

                if (value.Any(string.IsNullOrEmpty))
                {
                    return;
                }

                shortestDayNames = value;
            }
        }

        /// <summary>
        /// Gets or sets the first day of the week.
        /// </summary>
        [Description("The first day of the week.")]
        public virtual DayOfWeek FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the provider belongs to an RTL language.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsRTLLanguage { get; set; }

        /// <summary>
        /// Gets or sets the month calendar control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MonthCalendar MonthCalendar { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Gets the month name as string for the specified month.
        /// </summary>
        /// <param name="year">The year for which to retrieve the month name.</param>
        /// <param name="month">The month to get the name for.</param>
        /// <returns>The string value of the month.</returns>
        public virtual string GetMonthName(int year, int month)
        {
            if (!CheckMonthAndYear(year, month))
            {
                return string.Empty;
            }

            if (calendar.GetType() == typeof(HebrewCalendar))
            {
                return calendar.GetMonthsInYear(year) == 13 ?
                   monthNames[month - 1]
                   : monthNames[month == 12 ? 11 : month - 1];
            }

            return monthNames[month - 1];
        }

        /// <summary>
        /// Gets the abbreviated month name as string for the specified month.
        /// </summary>
        /// <param name="year">The year for which to retrieve the abbreviated month name.</param>
        /// <param name="month">The month to get the name for.</param>
        /// <returns>A string representing the abbreviated month name.</returns>
        public virtual string GetAbbreviatedMonthName(int year, int month)
        {
            if (!CheckMonthAndYear(year, month))
            {
                return string.Empty;
            }

            if (calendar.GetType() == typeof(HebrewCalendar))
            {
                return calendar.GetMonthsInYear(year) == 13 ?
                   abbrMonthNames[month - 1]
                   : abbrMonthNames[month == 12 ? 11 : month - 1];
            }

            return abbrMonthNames[month - 1];
        }

        /// <summary>
        /// Gets the number of months for the specified year.
        /// </summary>
        /// <param name="year">The year to get the number of months for.</param>
        /// <returns>The number of months is the year.</returns>
        public virtual int GetMonthsInYear(int year)
        {
            return calendar.GetMonthsInYear(year);
        }

        /// <summary>
        /// Returns the string representation of the specified era.
        /// </summary>
        /// <param name="era">The era as <see cref="int"/> value to get the name for.</param>
        /// <returns>The era name.</returns>
        public virtual string GetEraName(int era)
        {
            return dtfi.GetEraName(era);
        }

        /// <summary>
        /// Returns the day name of the specified <see cref="DayOfWeek"/> value.
        /// </summary>
        /// <param name="dayofweek">The <see cref="DayOfWeek"/> value to get the day name for.</param>
        /// <returns>A <see cref="string"/> value representing the day name specified by <paramref name="dayofweek"/>.</returns>
        public virtual string GetDayName(DayOfWeek dayofweek)
        {
            return dayNames[(int)dayofweek];
        }

        /// <summary>
        /// Returns the abbreviated day name of the specified <see cref="DayOfWeek"/> value.
        /// </summary>
        /// <param name="dayofweek">The <see cref="DayOfWeek"/> value to get the abbreviated day name for.</param>
        /// <returns>A <see cref="string"/> value representing the abbreviated day name specified by <paramref name="dayofweek"/>.</returns>
        public virtual string GetAbbreviatedDayName(DayOfWeek dayofweek)
        {
            return abbrDayNames[(int)dayofweek];
        }

        /// <summary>
        /// Returns the shortest day name of the specified <see cref="DayOfWeek"/> value.
        /// </summary>
        /// <param name="dayofweek">The <see cref="DayOfWeek"/> value to get the shortest day name for.</param>
        /// <returns>A <see cref="string"/> value representing the shortest day name specified by <paramref name="dayofweek"/>.</returns>
        public virtual string GetShortestDayName(DayOfWeek dayofweek)
        {
            return shortestDayNames[(int)dayofweek];
        }

        /// <summary>
        /// Checks if the specified year and month are valid.
        /// </summary>
        /// <param name="year">The year value.</param>
        /// <param name="month">The month value.</param>
        protected virtual bool CheckMonthAndYear(int year, int month)
        {
            MonthCalendarEraRange[] ranges = MonthCalendar.EraRanges;

            bool validYear = false;

            foreach (var range in ranges)
            {
                int minYear = calendar.GetYear(range.MinDate);
                int maxYear = calendar.GetYear(range.MaxDate);

                if (year >= minYear && year <= maxYear)
                {
                    validYear = true;
                }
            }

            return validYear && month > 0 && month < 14;
        }

        #region design time methods

        private bool ShouldSerializeMonthNames()
        {
            return ShouldSerialize(dtfi.MonthNames, monthNames);
        }

        private void ResetMonthNames()
        {
            monthNames = dtfi.MonthNames;
        }

        private bool ShouldSerializeAbbreviatedMonthNames()
        {
            return ShouldSerialize(dtfi.AbbreviatedMonthNames, abbrMonthNames);
        }

        private void ResetAbbreviatedMonthNames()
        {
            abbrMonthNames = dtfi.AbbreviatedMonthNames;
        }

        private bool ShouldSerializeDateSeparator()
        {
            return DateSeparator != dtfi.DateSeparator;
        }

        private void ResetDateSeparator()
        {
            DateSeparator = dtfi.DateSeparator;
        }

        private bool ShouldSerializeShortDatePattern()
        {
            return ShortDatePattern != dtfi.ShortDatePattern;
        }

        private void ResetShortDatePattern()
        {
            ShortDatePattern = dtfi.ShortDatePattern;
        }

        private bool ShouldSerializeLongDatePattern()
        {
            return LongDatePattern != dtfi.LongDatePattern;
        }

        private void ResetLongDatePattern()
        {
            LongDatePattern = dtfi.LongDatePattern;
        }

        private bool ShouldSerializeMonthDayPattern()
        {
            return MonthDayPattern != dtfi.MonthDayPattern;
        }

        private void ResetMonthDayPattern()
        {
            MonthDayPattern = dtfi.MonthDayPattern;
        }

        private bool ShouldSerializeYearMonthPattern()
        {
            return YearMonthPattern != dtfi.YearMonthPattern;
        }

        private void ResetYearMonthPattern()
        {
            YearMonthPattern = dtfi.YearMonthPattern;
        }

        private bool ShouldSerializeDayNames()
        {
            return ShouldSerialize(dtfi.DayNames, dayNames);
        }

        private void ResetDayNames()
        {
            dayNames = dtfi.DayNames;
        }

        private bool ShouldSerializeAbbreviatedDayNames()
        {
            return ShouldSerialize(dtfi.AbbreviatedDayNames, abbrDayNames);
        }

        private void ResetAbbreviatedDayNames()
        {
            abbrDayNames = dtfi.AbbreviatedDayNames;
        }

        private bool ShouldSerializeShortestDayNames()
        {
            return ShouldSerialize(dtfi.ShortestDayNames, shortestDayNames);
        }

        private void ResetShortestDayNames()
        {
            shortestDayNames = dtfi.ShortestDayNames;
        }

        private bool ShouldSerializeFirstDayOfWeek()
        {
            return dtfi.FirstDayOfWeek != FirstDayOfWeek;
        }

        private void ResetFirstDayOfWeek()
        {
            FirstDayOfWeek = dtfi.FirstDayOfWeek;
        }

        private static bool ShouldSerialize(string[] source, string[] custom)
        {
            return source == null || custom == null
               || source.Length != custom.Length
               || source.Where((t, i) => t != custom[i]).Any();
        }

        #endregion

        #endregion
    }
}