using Modern.Forms.MonthCalendar.EventClasses;
using Modern.Forms.MonthCalendar.Helper;
using Modern.Forms.MonthCalendar.Interfaces;
using Modern.Forms.MonthCalendar.Renderer;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace Modern.Forms.MonthCalendar
{
    /// <summary>
    /// A highly customizable and culture dependent month calendar control.
    /// </summary>
    public class MonthCalendar : Control
    {
        #region fields

        /// <summary>
        /// Redraw const.
        /// </summary>
        private const int SETREDRAW = 11;

        private double _scaling = 0;

        /// <summary>
        /// The list containing the bolded dates and their Category.
        /// </summary>
        private readonly BoldedDatesCollection boldDatesCollection;

        /// <summary>
        /// The list containing the bolded date types.
        /// </summary>
        private readonly BoldedDateCategoryCollection boldDateCategoryCollection;

        /// <summary>
        /// Use right to left layout.
        /// </summary>
        private bool rightToLeftLayout;

        /// <summary>
        /// Shows the footer.
        /// </summary>
        private bool showFooter;

        /// <summary>
        /// Selection started state.
        /// </summary>
        private bool selectionStarted;

        /// <summary>
        /// Indicates that an menu is currently displayed.
        /// </summary>
        private bool showingMenu;

        /// <summary>
        /// Indicates whether to show the week header.
        /// </summary>
        private bool showWeekHeader;

        /// <summary>
        /// Indicates whether the control is calculating it's sizes.
        /// </summary>
        private bool inUpdate;

        /// <summary>
        /// Indicates to use the shortest day names in the day header.
        /// </summary>        
        private bool useShortestDayNames;

        /// <summary>
        /// Indicates whether to use the native digits in <see cref="NumberFormatInfo.NativeDigits"/>
        /// specified by <see cref="Culture"/>s <see cref="CultureInfo.NumberFormat"/>
        /// for day number display.
        /// </summary>
        private bool useNativeDigits;

        /// <summary>
        /// The height of the header.
        /// </summary>
        private int headerHeight;

        /// <summary>
        /// The width of a day.
        /// </summary>
        private int dayWidth;

        /// <summary>
        /// The height of a day.
        /// </summary>
        private int dayHeight;

        /// <summary>
        /// The height of the footer.
        /// </summary>
        private int footerHeight;

        /// <summary>
        /// The width of the week header.
        /// </summary>
        private int weekNumberWidth;

        /// <summary>
        /// The height of the day name header.
        /// </summary>
        private int dayNameHeight;

        /// <summary>
        /// The width of a single month element.
        /// </summary>
        private int monthWidth;

        /// <summary>
        /// The height of a single month element.
        /// </summary>
        private int monthHeight;

        /// <summary>
        /// The scroll change if clicked on an arrow.
        /// </summary>
        private int scrollChange;

        /// <summary>
        /// The max. selection count of days.
        /// </summary>
        private int maxSelectionCount;

        /// <summary>
        /// The renderer.
        /// </summary>
        private MonthCalendarRenderer renderer;

        /// <summary>
        /// The font for the header.
        /// </summary>
        private SKFont headerFont;

        /// <summary>
        /// The font for the footer.
        /// </summary>
        private SKFont footerFont;

        /// <summary>
        /// The font for the day names.
        /// </summary>
        private SKFont dayHeaderFont;

        /// <summary>
        /// The calendar dimensions.
        /// </summary>
        private Size calendarDimensions;

        /// <summary>
        /// The mouse location.
        /// </summary>
        private Point mouseLocation;

        /// <summary>
        /// The start date.
        /// </summary>
        private DateTime viewStart;

        /// <summary>
        /// The real start date.
        /// </summary>
        private DateTime realStart;

        /// <summary>
        /// The last visible date.
        /// </summary>
        private DateTime lastVisibleDate;

        /// <summary>
        /// The clicked year in a month header.
        /// </summary>
        private DateTime yearSelected;

        /// <summary>
        /// The clicked month name in a month header.
        /// </summary>
        private DateTime monthSelected;

        /// <summary>
        /// The current selection type.
        /// </summary>
        private MonthCalendarHitType currentHitType;

        /// <summary>
        /// Dates which are bolded.
        /// </summary>
        private List<DateTime> boldedDates;

        /// <summary>
        /// The footer rectangle.
        /// </summary>
        private Rectangle footerRect;

        /// <summary>
        /// The rectangle for the left arrow.
        /// </summary>
        private Rectangle leftArrowRect;

        /// <summary>
        /// The rectangle for the right arrow.
        /// </summary>
        private Rectangle rightArrowRect;

        /// <summary>
        /// The current bounds of the hit test result item.
        /// </summary>
        private Rectangle currentMoveBounds;

        /// <summary>
        /// The text align for the days.
        /// </summary>
        private Forms.ContentAlignment dayTextAlign;

        /// <summary>
        /// The selection start date.
        /// </summary>
        private DateTime selectionStart;

        /// <summary>
        /// The selection end date.
        /// </summary>
        private DateTime selectionEnd;

        /// <summary>
        /// The minimum date of the control.
        /// </summary>
        private DateTime minDate;

        /// <summary>
        /// The maximum date of the control.
        /// </summary>
        private DateTime maxDate;

        /// <summary>
        /// The months displayed.
        /// </summary>        
        private MonthCalendarMonth[] months;

        /// <summary>
        /// The status information for mouse moving.
        /// </summary>
        private MonthCalendarMouseMoveFlags mouseMoveFlags;

        /// <summary>
        /// The selection start range if in week selection mode.
        /// </summary>
        private SelectionRange selectionStartRange;

        /// <summary>
        /// The selection range for backup purposes.
        /// </summary>
        private SelectionRange backupRange;

        /// <summary>
        /// The selection ranges.
        /// </summary>
        private List<SelectionRange> selectionRanges;

        /// <summary>
        /// The day selection mode.
        /// </summary>
        private MonthCalendarSelectionMode daySelectionMode;

        /// <summary>
        /// The non work days.
        /// </summary>
        private CalendarDayOfWeek nonWorkDays;

        /// <summary>
        /// The culture used by the control.
        /// </summary>
        private CultureInfo culture;

        /// <summary>
        /// The calendar used for displaying dates, months and everywhere else.
        /// </summary>
        private Calendar cultureCalendar;

        /// <summary>
        /// Interface field for handling month and day names.
        /// </summary>
        private ICustomFormatProvider formatProvider;

        /// <summary>
        /// The era date ranges.
        /// </summary>
        private MonthCalendarEraRange[] eraRanges;

        #region menu fields

        /// <summary>
        /// Context menu.
        /// </summary>
        private ContextMenu monthMenu;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiJan;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiFeb;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiMar;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiApr;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiMay;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiJun;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiJul;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiAug;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiSep;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiOct;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiNov;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiDez;

        /// <summary>
        /// Context menu strip.
        /// </summary>
        private MenuItem yearMenu;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiYear1;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiYear2;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiYear3;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiYear4;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiYear5;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiYear6;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiYear7;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiYear8;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiYear9;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiA1;

        /// <summary>
        /// Menu item.
        /// </summary>
        private MenuItem tsmiA2;

        /// <summary>
        /// Container for components.
        /// </summary>
        private IContainer components;

        private bool extendSelection;

        #endregion

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendar"/> class.
        /// </summary>
        public MonthCalendar()
        {            
            // initialize menus
            InitializeComponent();

            ShowHover = false;
            extendSelection = false;
            showFooter = true;
            showWeekHeader = true;
            mouseLocation = Point.Empty;
            yearSelected = DateTime.MinValue;
            monthSelected = DateTime.MinValue;
            selectionStart = DateTime.Today;
            selectionEnd = DateTime.Today;
            currentHitType = MonthCalendarHitType.None;
            boldedDates = new List<DateTime>();
            boldDatesCollection = new BoldedDatesCollection();
            boldDateCategoryCollection = new BoldedDateCategoryCollection(this);
            currentMoveBounds = Rectangle.Empty;
            mouseMoveFlags = new MonthCalendarMouseMoveFlags();
            selectionRanges = new List<SelectionRange>();
            daySelectionMode = MonthCalendarSelectionMode.Manual;
            nonWorkDays = CalendarDayOfWeek.Saturday | CalendarDayOfWeek.Sunday;
            culture = CultureInfo.CurrentUICulture;
            cultureCalendar = CultureInfo.CurrentUICulture.DateTimeFormat.Calendar;
            eraRanges = GetEraRanges(cultureCalendar);
            minDate = cultureCalendar.MinSupportedDateTime.Date < new DateTime(1900, 1, 1) ? new DateTime(1900, 1, 1) : cultureCalendar.MinSupportedDateTime.Date;
            maxDate = cultureCalendar.MaxSupportedDateTime.Date > new DateTime(9998, 12, 31) ? new DateTime(9998, 12, 31) : cultureCalendar.MaxSupportedDateTime.Date;
            formatProvider = new MonthCalendarFormatProvider(culture, null, culture.TextInfo.IsRightToLeft) { MonthCalendar = this };
            renderer = new MonthCalendarRenderer(this);
            calendarDimensions = new Size(1, 1);
            headerFont = new SKFont(SKTypeface.Default, 9);// new Font("Segoe UI", 9f, FontStyle.Regular);
            footerFont = new SKFont(SKTypeface.Default, 9);//new Font("Arial", 9f, FontStyle.Bold);
            dayHeaderFont = new SKFont(SKTypeface.Default, 9);//new Font("Segoe UI", 8f, FontStyle.Regular);
            dayTextAlign = Forms.ContentAlignment.MiddleCenter;
            
            // update year menu
            UpdateYearMenu(DateTime.Today.Year);
            
            // set start date
            SetStartDate(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));

            CalculateSize(true);
        }

        protected override void OnThemeChanged(EventArgs e)
        {
            base.OnThemeChanged(e);
            renderer.ColorTable.SetTheme(Theme.Style);
        }

        #endregion

        #region events

        /// <summary>
        /// Occurs when the user selects a date or a date range.
        /// </summary>
        [Category("Action")]
        [Description("Is raised, when the user selected a date or date range.")]
        public event EventHandler<DateRangeEventArgs> DateSelected;

        /// <summary>
        /// Occurs when the user changed the month or year.
        /// </summary>
        [Category("Action")]
        [Description("Is raised, when the user changed the month or year.")]
        public event EventHandler<DateRangeEventArgs> DateChanged;

        /// <summary>
        /// Is Raised when the mouse is over an date.
        /// </summary>
        [Category("Action")]
        [Description("Is raised when the mouse is over an date.")]
        public event EventHandler<ActiveDateChangedEventArgs> ActiveDateChanged;

        /// <summary>
        /// Is raised when the selection extension ended.
        /// </summary>
        [Category("Action")]
        [Description("Is raised when the selection extension ended.")]
        public event EventHandler SelectionExtendEnd;

        /// <summary>
        /// Is raised when a date was clicked.
        /// </summary>
        [Category("Action")]
        [Description("Is raised when a date in selection mode 'Day' was clicked.")]
        public event EventHandler<DateEventArgs> DateClicked;

        /// <summary>
        /// Is raises when a date was selected.
        /// </summary>
        internal event EventHandler<DateEventArgs> InternalDateSelected;

        #endregion

        #region properties

        #region public properties

        /// <summary>
        /// Gets or sets the start month and year.
        /// </summary>
        [Category("Appearance")]
        [Description("Sets the first displayed month and year.")]
        public DateTime ViewStart
        {
            get
            {
                return viewStart;
            }

            set
            {
                if (value == viewStart
                   || value < minDate || value > maxDate)
                {
                    return;
                }

                SetStartDate(value);

                UpdateMonths();

                Invalidate(); // refresh
            }
        }

        /// <summary>
        /// Gets the last in-month date of the last displayed month.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime ViewEnd
        {
            get
            {
                MonthCalendarDate dt = new MonthCalendarDate(cultureCalendar, viewStart)
                   .GetEndDateOfWeek(formatProvider).FirstOfMonth.AddMonths(months != null ? months.Length - 1 : 1).FirstOfMonth;

                int daysInMonth = dt.DaysInMonth;

                dt = dt.AddDays(daysInMonth - 1);

                return dt.Date;
            }
        }

        /// <summary>
        /// Gets the real start date of the month calendar.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime RealStartDate
        {
            get { return realStart; }
        }

        /// <summary>
        /// Gets or sets the lower limit of the visible month and year.
        /// </summary>
        [Category("Behavior")]
        [Description("The viewable minimum month and year.")]
        public DateTime MinDate
        {
            get
            {
                return minDate;
            }

            set
            {
                if (value < CultureCalendar.MinSupportedDateTime || value > CultureCalendar.MaxSupportedDateTime
                   || value >= maxDate)
                {
                    return;
                }

                value = GetMinDate(value);

                minDate = value.Date;

                UpdateMonths();

                int dim1 = Math.Max(1, calendarDimensions.Width * calendarDimensions.Height);
                int dim2 = months != null ? months.Length : 1;

                if (dim1 != dim2)
                {
                    SetStartDate(new MonthCalendarDate(CultureCalendar, viewStart).AddMonths(dim2 - dim1).Date);
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the upper limit of the visible month and year.
        /// </summary>
        [Category("Behavior")]
        [Description("The viewable maximum month and year.")]
        public DateTime MaxDate
        {
            get
            {
                return maxDate;
            }

            set
            {
                if (value < CultureCalendar.MinSupportedDateTime || value > CultureCalendar.MaxSupportedDateTime
                   || value <= minDate)
                {
                    return;
                }

                value = GetMaxDate(value);

                maxDate = value.Date;

                UpdateMonths();

                int dim1 = Math.Max(1, calendarDimensions.Width * calendarDimensions.Height);
                int dim2 = months != null ? months.Length : 1;

                if (dim1 != dim2)
                {
                    SetStartDate(new MonthCalendarDate(CultureCalendar, viewStart).AddMonths(dim2 - dim1).Date);
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the calendar dimensions.
        /// </summary>
        [Category("Appearance")]
        [Description("The number of rows and columns of months in the calendar.")]
        [DefaultValue(typeof(Size), "1,1")]
        public Size CalendarDimensions
        {
            get
            {
                return calendarDimensions;
            }

            set
            {
                if (value == calendarDimensions || value.IsEmpty)
                {
                    return;
                }

                // get number of months in a row
                value.Width = Math.Max(1, Math.Min(value.Width, 7));

                // get number of months in a column
                value.Height = Math.Max(1, Math.Min(value.Height, 7));

                // set new dimension
                calendarDimensions = value;

                // update width and height of control
                inUpdate = true;

                Width = value.Width * monthWidth;
                Height = value.Height * monthHeight + (showFooter ? footerHeight : 0);

                inUpdate = false;

                // adjust scroll change
                scrollChange = Math.Max(0, Math.Min(scrollChange, calendarDimensions.Width * calendarDimensions.Height));

                // calculate sizes
                CalculateSize(false);
            }
        }

        /// <summary>
        /// Gets or sets the header font.
        /// </summary>
        [Category("Appearance")]
        [Description("The font for the header.")]
        public SKFont HeaderFont
        {
            get
            {
                return headerFont;
            }

            set
            {
                if (value == headerFont || value == null)
                {
                    return;
                }

                BeginUpdate();

                if (headerFont != null)
                {
                    headerFont.Dispose();
                }

                headerFont = value;

                CalculateSize(false);

                EndUpdate();
            }
        }

        /// <summary>
        /// Gets or sets the footer font.
        /// </summary>
        [Category("Appearance")]
        [Description("The font for the footer.")]
        public SKFont FooterFont
        {
            get
            {
                return footerFont;
            }

            set
            {
                if (value == footerFont || value == null)
                {
                    return;
                }

                //this.BeginUpdate();

                if (footerFont != null)
                {
                    footerFont.Dispose();
                }

                footerFont = value;

                CalculateSize(false);

                //this.EndUpdate();
            }
        }

        /// <summary>
        /// Gets or sets the font for the day header.
        /// </summary>
        [Category("Appearance")]
        [Description("The font for the day header.")]
        public SKFont DayHeaderFont
        {
            get
            {
                return dayHeaderFont;
            }

            set
            {
                if (value == dayHeaderFont || value == null)
                {
                    return;
                }

                BeginUpdate();

                if (dayHeaderFont != null)
                {
                    dayHeaderFont.Dispose();
                }

                dayHeaderFont = value;

                CalculateSize(false);

                EndUpdate();
            }
        }

        /// <summary>
        /// Gets or sets the font used for the week header and days.
        /// </summary>
        public SKFont Font
        {
            get
            {
                return dayHeaderFont;
            }

            set
            {                
                dayHeaderFont = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        /// <summary>
        /// Gets or sets the text alignment for the days.
        /// </summary>       
        public Forms.ContentAlignment DayTextAlignment
        {
            get
            {
                return dayTextAlign;
            }

            set
            {
                if (value == dayTextAlign)
                {
                    return;
                }

                dayTextAlign = value;

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use a right to left layout.
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Indicates whether to use the RTL layout.")]
        public bool RightToLeftLayout
        {
            get
            {
                return rightToLeftLayout;
            }

            set
            {
                if (value == rightToLeftLayout)
                {
                    return;
                }

                rightToLeftLayout = value;

                //this.formatProvider.IsRTLLanguage = this.UseRTL;

                Size calDim = calendarDimensions;

                UpdateMonths();

                CalendarDimensions = calDim;

                Invalidate();//refresh
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the footer.
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        [Description("Indicates whether to show the footer.")]
        public bool ShowFooter
        {
            get
            {
                return showFooter;
            }

            set
            {
                if (value == showFooter)
                {
                    return;
                }

                showFooter = value;

                Height += value ? footerHeight : -footerHeight;

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the week header.
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        [Description("Indicates whether to show the week header.")]
        public bool ShowWeekHeader
        {
            get
            {
                return showWeekHeader;
            }

            set
            {
                if (showWeekHeader == value)
                {
                    return;
                }

                showWeekHeader = value;

                CalculateSize(false);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the shortest or the abbreviated day names in the day header.
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Indicates whether to use the shortest or the abbreviated day names in the day header.")]
        public bool UseShortestDayNames
        {
            get
            {
                return useShortestDayNames;
            }

            set
            {
                useShortestDayNames = value;

                CalculateSize(false);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the native digits in <see cref="NumberFormatInfo.NativeDigits"/>
        /// specified by <see cref="Culture"/>s <see cref="CultureInfo.NumberFormat"/>
        /// for number display.
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Indicates whether to use the native digits as specified by the current Culture property.")]
        public bool UseNativeDigits
        {
            get { return useNativeDigits; }

            set
            {
                if (value == useNativeDigits)
                {
                    return;
                }

                useNativeDigits = value;

                Invalidate();//refresh 
            }
        }

        /// <summary>
        /// Gets or sets the list for bolded dates.
        /// </summary>
        [Description("The bolded dates in the month calendar.")]
        public List<DateTime> BoldedDates
        {
            get
            {
                return boldedDates;
            }

            set
            {
                boldedDates = value ?? new List<DateTime>();
            }
        }

        /// <summary>
        /// Gets the bolded dates.
        /// </summary>
        [Description("The bolded dates in the calendar.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public BoldedDatesCollection BoldedDatesCollection
        {
            get
            {
                return boldDatesCollection;
            }
        }

        /// <summary>
        /// Gets a collection holding the defined categories of bold dates.
        /// </summary>
        [Description("The bold date categories in the calendar.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public BoldedDateCategoryCollection BoldedDateCategoryCollection
        {
            get
            {
                return boldDateCategoryCollection;
            }
        }

        /// <summary>
        /// Gets or sets the selection start date.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectionStart
        {
            get
            {
                return selectionStart;
            }

            set
            {
                value = value.Date;

                // valid value ?
                if (value < CultureCalendar.MinSupportedDateTime || value > CultureCalendar.MaxSupportedDateTime)
                {
                    return;
                }

                if (value < minDate)
                {
                    value = minDate;
                }
                else if (value > maxDate)
                {
                    value = maxDate;
                }

                switch (daySelectionMode)
                {
                    case MonthCalendarSelectionMode.Day:
                        {
                            selectionStart = value;
                            selectionEnd = value;

                            break;
                        }

                    case MonthCalendarSelectionMode.WorkWeek:
                    case MonthCalendarSelectionMode.FullWeek:
                        {
                            MonthCalendarDate dt = new MonthCalendarDate(CultureCalendar, value).GetFirstDayInWeek(formatProvider);
                            selectionStart = dt.Date;
                            selectionEnd = dt.GetEndDateOfWeek(formatProvider).Date;

                            break;
                        }

                    case MonthCalendarSelectionMode.Month:
                        {
                            MonthCalendarDate dt = new MonthCalendarDate(CultureCalendar, value).FirstOfMonth;
                            selectionStart = dt.Date;
                            selectionEnd = dt.AddMonths(1).AddDays(-1).Date;

                            break;
                        }

                    case MonthCalendarSelectionMode.Manual:
                        {
                            value = GetSelectionDate(selectionEnd, value);

                            if (value == DateTime.MinValue)
                            {
                                selectionEnd = value;
                                selectionStart = value;
                            }
                            else
                            {
                                selectionStart = value;

                                if (selectionEnd == DateTime.MinValue)
                                {
                                    selectionEnd = value;
                                }
                            }

                            break;
                        }
                }

                Invalidate();//refresh
            }
        }

        /// <summary>
        /// Gets or sets the selection end date.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectionEnd
        {
            get
            {
                return selectionEnd;
            }

            set
            {
                value = value.Date;

                if (value < CultureCalendar.MinSupportedDateTime || value > CultureCalendar.MaxSupportedDateTime
                   || daySelectionMode != MonthCalendarSelectionMode.Manual)
                {
                    return;
                }

                if (value < minDate)
                {
                    value = minDate;
                }
                else if (value > maxDate)
                {
                    value = maxDate;
                }

                value = GetSelectionDate(selectionStart, value);

                if (value == DateTime.MinValue || selectionStart == DateTime.MinValue)
                {
                    selectionStart = value;
                    selectionEnd = value;

                    Invalidate();//refresh

                    return;
                }

                selectionEnd = value;

                Invalidate();//refresh
            }
        }

        /// <summary>
        /// Gets or sets the selection range of the selected dates.
        /// </summary>
        [Category("Behavior")]
        [Description("The selection range.")]
        public SelectionRange SelectionRange
        {
            get
            {
                return new SelectionRange(selectionStart, selectionEnd);
            }

            set
            {
                if (value == null)
                {
                    ResetSelectionRange();

                    return;
                }

                switch (daySelectionMode)
                {
                    case MonthCalendarSelectionMode.Day:
                    case MonthCalendarSelectionMode.WorkWeek:
                    case MonthCalendarSelectionMode.FullWeek:
                    case MonthCalendarSelectionMode.Month:
                        {
                            SelectionStart = selectionStart == value.Start ?
                               value.End : value.Start;

                            break;
                        }

                    case MonthCalendarSelectionMode.Manual:
                        {
                            selectionStart = value.Start;
                            SelectionEnd = value.End;

                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Gets the selection ranges.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<SelectionRange> SelectionRanges
        {
            get
            {
                return selectionRanges;
            }
        }

        /// <summary>
        /// Gets or sets the scroll change if clicked on an arrow button.
        /// </summary>
        [DefaultValue(0)]
        [Category("Behavior")]
        [Description("The number of months the calendar is going for- or backwards if clicked on an arrow."
         + "A value of 0 indicates the last visible month is the first month (forwards) and vice versa.")]
        public int ScrollChange
        {
            get
            {
                return scrollChange;
            }

            set
            {
                if (value == scrollChange)
                {
                    return;
                }

                scrollChange = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum selectable days.
        /// </summary>
        [DefaultValue(0)]
        [Category("Behavior")]
        [Description("The maximum selectable days. A value of 0 means no limit.")]
        public int MaxSelectionCount
        {
            get
            {
                return maxSelectionCount;
            }

            set
            {
                if (value == maxSelectionCount)
                {
                    return;
                }

                maxSelectionCount = Math.Max(0, value);
            }
        }

        /// <summary>
        /// Gets or sets the day selection mode.
        /// </summary>
        [DefaultValue(MonthCalendarSelectionMode.Manual)]
        [Category("Behavior")]
        [Description("Sets the day selection mode.")]
        public MonthCalendarSelectionMode SelectionMode
        {
            get
            {
                return daySelectionMode;
            }

            set
            {
                if (value == daySelectionMode || !Enum.IsDefined(typeof(MonthCalendarSelectionMode), value))
                {
                    return;
                }

                daySelectionMode = value;

                SetSelectionRange(value);

                Invalidate();//refresh
            }
        }

        /// <summary>
        /// Gets or sets the non working days.
        /// </summary>     
        public CalendarDayOfWeek NonWorkDays
        {
            get
            {
                return nonWorkDays;
            }

            set
            {
                if (value == nonWorkDays)
                {
                    return;
                }

                nonWorkDays = value;

                if (daySelectionMode == MonthCalendarSelectionMode.WorkWeek)
                {
                    Invalidate();//refresh
                }
            }
        }

        /// <summary>
        /// Gets or sets the used renderer.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MonthCalendarRenderer Renderer
        {
            get
            {
                return renderer;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                renderer = value;

                Invalidate();//refresh
            }
        }

        /// <summary>
        /// Gets or sets the culture used by the <see cref="MonthCalendar"/>.
        /// </summary>
        [Category("Behavior")]
        [Description("The culture used by the MonthCalendar.")]
        public CultureInfo Culture
        {
            get
            {
                return culture;
            }

            set
            {
                if (value == null || value.IsNeutralCulture)
                {
                    return;
                }

                culture = value;
                formatProvider.DateTimeFormat = value.DateTimeFormat;
                CultureCalendar = null;

                //if (DateMethods.IsRTLCulture(value))
                //{
                //    //this.RightToLeft = RightToLeft.Yes;
                //    this.RightToLeftLayout = true;
                //}
                //else
                //{
                //    this.RightToLeftLayout = false;
                //    //this.RightToLeft = RightToLeft.Inherit;
                //}

                //this.formatProvider.IsRTLLanguage = this.UseRTL;
            }
        }

        /// <summary>
        /// Gets or sets the used calendar.
        /// </summary>      
        public Calendar CultureCalendar
        {
            get
            {
                return cultureCalendar;
            }

            set
            {
                if (value == null)
                {
                    value = culture.Calendar;
                }

                cultureCalendar = value;
                formatProvider.Calendar = value;

                if (value.GetType() == typeof(PersianCalendar) && !value.IsReadOnly)
                {
                    value.TwoDigitYearMax = 1410;
                }

                foreach (Calendar c in culture.OptionalCalendars)
                {
                    if (value.GetType() == c.GetType())
                    {
                        if (value.GetType() == typeof(GregorianCalendar))
                        {
                            GregorianCalendar g1 = (GregorianCalendar)value;
                            GregorianCalendar g2 = (GregorianCalendar)c;

                            if (g1.CalendarType != g2.CalendarType)
                            {
                                continue;
                            }
                        }

                        culture.DateTimeFormat.Calendar = c;
                        formatProvider.DateTimeFormat = culture.DateTimeFormat;
                        cultureCalendar = c;

                        value = c;

                        break;
                    }
                }

                eraRanges = GetEraRanges(value);

                ReAssignSelectionMode();

                minDate = GetMinDate(value.MinSupportedDateTime.Date);
                maxDate = GetMaxDate(value.MaxSupportedDateTime.Date);

                SetStartDate(DateTime.Today);

                CalculateSize(false);
            }
        }

        /// <summary>
        /// Gets or sets the used color table.
        /// </summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("The used colors for the month calendar.")]
        public MonthCalendarColorTable ColorTable
        {
            get
            {
                return renderer.ColorTable;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                renderer.ColorTable = value;
            }
        }

        /// <summary>
        /// Gets or sets the interface for day name handling.
        /// </summary>       
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Behavior")]
        [Description("Culture dependent settings for month/day names and date formatting.")]
        public ICustomFormatProvider FormatProvider
        {
            get { return formatProvider; }
            set { formatProvider = value ?? new MonthCalendarFormatProvider(culture, null, culture.TextInfo.IsRightToLeft); }
        }

        #endregion

        #region internal properties

        /// <summary>
        /// Gets the size of a single <see cref="MonthCalendarMonth"/>.
        /// </summary>
        internal Size MonthSize
        {
            get { return new Size(monthWidth, monthHeight); }
        }

        /// <summary>
        /// Gets the size of a single day.
        /// </summary>
        internal Size DaySize
        {
            get { return new Size(dayWidth, dayHeight); }
        }

        /// <summary>
        /// Gets the footer size.
        /// </summary>
        internal Size FooterSize
        {
            get { return new Size(Width, footerHeight); }
        }

        /// <summary>
        /// Gets the header size.
        /// </summary>
        internal Size HeaderSize
        {
            get { return new Size(monthWidth, headerHeight); }
        }

        /// <summary>
        /// Gets the size of the day names.
        /// </summary>
        internal Size DayNamesSize
        {
            get { return new Size(dayWidth * 7, dayNameHeight); }
        }

        /// <summary>
        /// Gets the size of the week numbers.
        /// </summary>
        internal Size WeekNumberSize
        {
            get { return new Size(weekNumberWidth, dayHeight * 7); }
        }

        /// <summary>
        /// Gets the date for the current day the mouse is over.
        /// </summary>
        internal DateTime MouseOverDay
        {
            get { return mouseMoveFlags.Day; }
        }

        /// <summary>
        /// Gets a value indicating whether the control is using the RTL layout.
        /// </summary>
        //internal bool UseRTL
        //{
        //    get { return this.RightToLeft == RightToLeft.Yes && this.rightToLeftLayout; }
        //}

        /// <summary>
        /// Gets the current left button state.
        /// </summary>
        //internal ButtonState LeftButtonState
        //{
        //    get
        //    {
        //        return this.mouseMoveFlags.LeftArrow ? ButtonState.Pushed : ButtonState.Normal;
        //    }
        //}

        /// <summary>
        /// Gets the current right button state.
        /// </summary>
        //internal ButtonState RightButtonState
        //{
        //    get
        //    {
        //        return this.mouseMoveFlags.RightArrow ? ButtonState.Pushed : ButtonState.Normal;
        //    }
        //}

        /// <summary>
        /// Gets the current hit type result.
        /// </summary>
        internal MonthCalendarHitType CurrentHitType
        {
            get { return currentHitType; }
        }

        /// <summary>
        /// Gets the month menu.
        /// </summary>
        //internal ContextMenu MonthMenu
        //{
        //    get { return this.monthMenu; }
        //}

        /// <summary>
        /// Gets the year menu.
        /// </summary>
        //internal ContextMenu YearMenu
        //{
        //    get { return this.yearMenu; }
        //}

        /// <summary>
        /// Gets the era date ranges for the current calendar.
        /// </summary>
        internal MonthCalendarEraRange[] EraRanges
        {
            get { return eraRanges; }
        }

        public bool ShowHover { get; private set; }

        #endregion

        #endregion

        #region methods

        #region public methods

        /// <summary>
        /// Prevents a drawing of the control until <see cref="EndUpdate"/> is called.
        /// </summary>
        public void BeginUpdate()
        {
            //SendMessage(this.Handle, SETREDRAW, false, 0);
        }

        /// <summary>
        /// Ends the updating process and the control can draw itself again.
        /// </summary>
        public void EndUpdate()
        {            
            Invalidate();
        }

        /// <summary>
        /// Performs a hit test with the specified <paramref name="location"/> as mouse location.
        /// </summary>
        /// <param name="location">The mouse location.</param>
        /// <returns>A <see cref="MonthCalendarHitTest"/> object.</returns>
        public MonthCalendarHitTest HitTest(Point location)
        {
            if (!ClientRectangle.Contains(location))
            {
                return MonthCalendarHitTest.Empty;
            }

            if (rightArrowRect.Contains(location))
            {
                return new MonthCalendarHitTest(GetNewScrollDate(false), MonthCalendarHitType.Arrow, rightArrowRect);
            }

            if (leftArrowRect.Contains(location))
            {
                return new MonthCalendarHitTest(GetNewScrollDate(true), MonthCalendarHitType.Arrow, leftArrowRect);
            }

            if (showFooter && footerRect.Contains(location))
            {
                return new MonthCalendarHitTest(DateTime.Today, MonthCalendarHitType.Footer, footerRect);
            }

            foreach (MonthCalendarMonth month in months)
            {
                MonthCalendarHitTest hit = month.HitTest(location);

                if (!hit.IsEmpty)
                {
                    return hit;
                }
            }

            return MonthCalendarHitTest.Empty;
        }

        #endregion

        #region internal methods

        /// <summary>
        /// Gets all bolded dates.
        /// </summary>
        /// <returns>A generic List of type <see cref="DateTime"/> with the bolded dates.</returns>
        internal List<DateTime> GetBoldedDates()
        {
            List<DateTime> dates = new List<DateTime>();

            // remove all duplicate dates
            boldedDates.ForEach(date =>
            {
                if (!dates.Contains(date))
                {
                    dates.Add(date);
                }
            });

            return dates;
        }

        /// <summary>
        /// Determines if the <paramref name="date"/> is selected.
        /// </summary>
        /// <param name="date">The date to determine if checked.</param>
        /// <returns>true if <paramref name="date"/> is selected; false otherwise.</returns>
        internal bool IsSelected(DateTime date)
        {
            // get if date is in first selection range
            bool selected = SelectionRange.Contains(date);

            // get if date is in all other selection ranges (only in WorkWeek day selection mode)
            selectionRanges.ForEach(range =>
            {
                selected |= range.Contains(date);
            });

            // if in WorkWeek day selection mode a date is only selected if a work day
            if (daySelectionMode == MonthCalendarSelectionMode.WorkWeek)
            {
                // get all work days
                List<DayOfWeek> workDays = DateMethods.GetWorkDays(nonWorkDays);

                // return if date is selected
                return workDays.Contains(date.DayOfWeek) && selected;
            }

            // return if date is selected
            return selected;
        }

        /// <summary>
        /// Scrolls to the selection start.
        /// </summary>
        internal void EnsureSeletedDateIsVisible()
        {
            if (RealStartDate > selectionStart || selectionStart > lastVisibleDate)
            {
                SetStartDate(new DateTime(selectionStart.Year, selectionStart.Month, 1));

                UpdateMonths();
            }
        }

        /// <summary>
        /// Sets the bounds of the left arrow.
        /// </summary>
        /// <param name="rect">The bounds of the left arrow.</param>
        internal void SetLeftArrowRect(Rectangle rect)
        {            
            leftArrowRect = rect;            
        }

        /// <summary>
        /// Sets the bounds of the right arrow.
        /// </summary>
        /// <param name="rect">The bounds of the right arrow.</param>
        internal void SetRightArrowRect(Rectangle rect)
        {
            // if in RTL mode
            //if (this.UseRTL)
            //{
            //    // the right arrow bounds are the left ones
            //    this.leftArrowRect = rect;
            //}
            //else
            //{
            rightArrowRect = rect;
            //}
        }

        #endregion

        #region protected methods


        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="MonthCalendar"/> control and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (headerFont != null)
                {
                    headerFont.Dispose();
                }

                if (footerFont != null)
                {
                    footerFont.Dispose();
                }

                if (dayHeaderFont != null)
                {
                    dayHeaderFont.Dispose();
                }

                if (monthMenu != null)
                {
                    monthMenu.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        ///// <summary>
        ///// Processes a dialog key.
        ///// </summary>
        ///// <param name="keyData">One of the <see cref="Keys"/> value that represents the key to process.</param>
        ///// <returns>true if the key was processed by the control; otherwise, false.</returns>
        //protected override bool ProcessDialogKey(Keys keyData)
        //{
        //    if (this.daySelectionMode != MonthCalendarSelectionMode.Day)
        //    {
        //        return base.ProcessDialogKey(keyData);
        //    }

        //    MonthCalendarDate dt = new MonthCalendarDate(this.cultureCalendar, this.selectionStart);
        //    bool retValue = false;

        //    if (keyData == Keys.Left)
        //    {
        //        this.selectionStart = dt.AddDays(-1).Date;

        //        retValue = true;
        //    }
        //    else if (keyData == Keys.Right)
        //    {
        //        this.selectionStart = dt.AddDays(1).Date;

        //        retValue = true;
        //    }
        //    else if (keyData == Keys.Up)
        //    {
        //        this.selectionStart = dt.AddDays(-7).Date;

        //        retValue = true;
        //    }
        //    else if (keyData == Keys.Down)
        //    {
        //        this.selectionStart = dt.AddDays(7).Date;

        //        retValue = true;
        //    }

        //    if (retValue)
        //    {
        //        if (this.selectionStart < this.minDate)
        //        {
        //            this.selectionStart = this.minDate;
        //        }
        //        else if (this.selectionStart > this.maxDate)
        //        {
        //            this.selectionStart = this.maxDate;
        //        }

        //        this.SetSelectionRange(this.daySelectionMode);

        //        this.EnsureSeletedDateIsVisible();

        //        this.RaiseInternalDateSelected();

        //        this.Invalidate();

        //        return true;
        //    }

        //    return base.ProcessDialogKey(keyData);
        //}

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                extendSelection = true;
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                extendSelection = false;

                RaiseSelectExtendEnd();
            }

            base.OnKeyUp(e);
        }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        /// <param name="x">The new <see cref="Windows.Forms.Control.Left"/> property value of the control.</param>
        /// <param name="y">The new <see cref="Windows.Forms.Control.Top"/> property value of the control.</param>
        /// <param name="width">The new <see cref="Windows.Forms.Control.Width"/> property value of the control.</param>
        /// <param name="height">The new <see cref="Windows.Forms.Control.Height"/> property value of the control.</param>
        /// <param name="specified">A bitwise combination of the <see cref="Windows.Forms.BoundsSpecified"/> values.</param>
        protected override void SetBoundsCore(
           int x,
           int y,
           int width,
           int height,
           BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);

            if (Created || DesignMode)
            {
                CalculateSize(true);
            }
        }

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_scaling != Scaling)
            {
                CalculateSize(false);
                _scaling = Scaling;
            }

            // return if in update mode of if nothing to draw
            if (calendarDimensions.IsEmpty || inUpdate)
            {
                return;
            }

            renderer.DrawControlBackground(e.Canvas);

            // loop through all months to draw
            foreach (MonthCalendarMonth month in months)
            {
                // if month is null or not in the clip rectangle - continue
                if (month == null)//|| !e.Canvas.LocalClipBounds.IntersectsWith(month.Bounds))
                {
                    continue;
                }

                MonthCalendarHeaderState state = GetMonthHeaderState(month.Date);

                // draw the title background
                renderer.DrawTitleBackground(e.Canvas, month, state);

                // draw the background for the month body
                renderer.DrawMonthBodyBackground(e.Canvas, month);

                // draw the background for the day header
                renderer.DrawDayHeaderBackground(e.Canvas, month);

                // draw the month header
                renderer.DrawMonthHeader(e.Canvas, month, state, Scaling);

                // draw the day names header
                renderer.DrawDayHeader(e.Canvas, month.DayNamesBounds);

                // show week header ?
                if (showWeekHeader)
                {
                    // draw the background of the week header
                    renderer.DrawWeekHeaderBackground(e.Canvas, month);

                    // loop through all week header elements
                    foreach (MonthCalendarWeek week in month.Weeks)
                    {
                        // if week not visible continue
                        if (!week.Visible)
                        {
                            continue;
                        }

                        // draw week header element
                        renderer.DrawWeekHeaderItem(e.Canvas, week);
                    }
                }

                // loop through all days in current month
                foreach (MonthCalendarDay day in month.Days)
                {
                    // if day is not visible continue
                    if (!day.Visible)
                    {
                        continue;
                    }

                    // draw the day
                    renderer.DrawDay(e.Canvas, day);
                }

                // draw week header separator line
                renderer.DrawWeekHeaderSeparator(e.Canvas, month.WeekBounds);
            }

            // if footer is shown
            if (showFooter)
            {
                // draw the background of the footer
                renderer.DrawFooterBackground(e.Canvas, footerRect, mouseMoveFlags.Footer);

                // draw the footer
                renderer.DrawFooter(e.Canvas, footerRect, mouseMoveFlags.Footer);
            }

            // draw the border
            //Rectangle r = this.ClientRectangle;
            //r.Width--;
            //r.Height--;

            Rectangle rb = new Rectangle(0, 0, Bounds.Width, Bounds.Height);
            rb.Width--;
            rb.Height--;
            e.Canvas.DrawRectangle(rb, renderer.ColorTable.Border);
        }

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            //this.Focus();

            Capture = true;

            // reset the selection range where selection started
            selectionStartRange = null;

            if (e.Button == MouseButtons.Left)
            {
                // perform hit test
                MonthCalendarHitTest hit = HitTest(e.Location);

                // set current bounds
                currentMoveBounds = hit.Bounds;

                // set current hit type
                currentHitType = hit.Type;

                switch (hit.Type)
                {
                    case MonthCalendarHitType.Day:
                        {
                            // save old selection range
                            SelectionRange oldRange = SelectionRange;

                            if (!extendSelection || daySelectionMode != MonthCalendarSelectionMode.Manual)
                            {
                                // clear all selection ranges
                                selectionRanges.Clear();
                            }

                            switch (daySelectionMode)
                            {
                                case MonthCalendarSelectionMode.Day:
                                    {
                                        OnDateClicked(new DateEventArgs(hit.Date));

                                        // only single days are selectable
                                        if (selectionStart != hit.Date)
                                        {
                                            SelectionStart = hit.Date;

                                            RaiseDateSelected();
                                        }

                                        break;
                                    }

                                case MonthCalendarSelectionMode.WorkWeek:
                                    {
                                        // only single work week is selectable
                                        // get first day of week
                                        DateTime firstDay = new MonthCalendarDate(CultureCalendar, hit.Date).GetFirstDayInWeek(formatProvider).Date;

                                        // get work days
                                        List<DayOfWeek> workDays = DateMethods.GetWorkDays(nonWorkDays);

                                        // reset selection start and end
                                        selectionEnd = DateTime.MinValue;
                                        selectionStart = DateTime.MinValue;

                                        // current range
                                        SelectionRange currentRange = null;

                                        // build selection ranges for work days
                                        for (int i = 0; i < 7; i++)
                                        {
                                            DateTime toAdd = firstDay.AddDays(i);

                                            if (workDays.Contains(toAdd.DayOfWeek))
                                            {
                                                if (currentRange == null)
                                                {
                                                    currentRange = new SelectionRange(DateTime.MinValue, DateTime.MinValue);
                                                }

                                                if (currentRange.Start == DateTime.MinValue)
                                                {
                                                    currentRange.Start = toAdd;
                                                }
                                                else
                                                {
                                                    currentRange.End = toAdd;
                                                }
                                            }
                                            else if (currentRange != null)
                                            {
                                                selectionRanges.Add(currentRange);

                                                currentRange = null;
                                            }
                                        }

                                        if (selectionRanges.Count >= 1)
                                        {
                                            // set first selection range
                                            SelectionRange = selectionRanges[0];
                                            selectionRanges.RemoveAt(0);

                                            // if selection range changed, raise event
                                            if (SelectionRange != oldRange)
                                            {
                                                RaiseDateSelected();
                                            }
                                        }
                                        else
                                        {
                                            Invalidate();//refresh
                                        }

                                        break;
                                    }

                                case MonthCalendarSelectionMode.FullWeek:
                                    {
                                        // only a full week is selectable
                                        // get selection start and end
                                        MonthCalendarDate dt =
                                           new MonthCalendarDate(CultureCalendar, hit.Date).GetFirstDayInWeek(
                                              formatProvider);

                                        selectionStart = dt.Date;
                                        selectionEnd = dt.GetEndDateOfWeek(formatProvider).Date;

                                        // if range changed, raise event
                                        if (SelectionRange != oldRange)
                                        {
                                            RaiseDateSelected();

                                            Invalidate();//refresh
                                        }

                                        break;
                                    }

                                case MonthCalendarSelectionMode.Month:
                                    {
                                        // only a full month is selectable
                                        MonthCalendarDate dt = new MonthCalendarDate(CultureCalendar, hit.Date).FirstOfMonth;

                                        // get selection start and end
                                        selectionStart = dt.Date;
                                        selectionEnd = dt.AddMonths(1).AddDays(-1).Date;

                                        // if range changed, raise event
                                        if (SelectionRange != oldRange)
                                        {
                                            RaiseDateSelected();

                                            Invalidate();//refresh
                                        }

                                        break;
                                    }

                                case MonthCalendarSelectionMode.Manual:
                                    {
                                        if (extendSelection)
                                        {
                                            var range = selectionRanges.Find(r => hit.Date >= r.Start && hit.Date <= r.End);

                                            if (range != null)
                                            {
                                                selectionRanges.Remove(range);
                                            }
                                        }

                                        // manual mode - selection ends when user is releasing the left mouse button
                                        selectionStarted = true;
                                        backupRange = SelectionRange;
                                        selectionEnd = DateTime.MinValue;
                                        SelectionStart = hit.Date;

                                        break;
                                    }
                            }

                            break;
                        }

                    case MonthCalendarHitType.Week:
                        {
                            selectionRanges.Clear();

                            if (maxSelectionCount > 6 || maxSelectionCount == 0)
                            {
                                backupRange = SelectionRange;
                                selectionStarted = true;
                                selectionEnd = new MonthCalendarDate(CultureCalendar, hit.Date).GetEndDateOfWeek(formatProvider).Date;
                                SelectionStart = hit.Date;

                                selectionStartRange = SelectionRange;
                            }

                            break;
                        }

                    case MonthCalendarHitType.MonthName:
                        {
                            monthSelected = hit.Date;
                            mouseMoveFlags.HeaderDate = hit.Date;

                            Invalidate(hit.InvalidateBounds);
                            //this.Update();

                            monthMenu.Tag = hit.Date;
                            UpdateMonthMenu(CultureCalendar.GetYear(hit.Date));

                            showingMenu = true;

                            // show month menu
                            monthMenu.Show(this, new Point(hit.Bounds.Right, e.Location.Y));

                            break;
                        }

                    case MonthCalendarHitType.MonthYear:
                        {
                            yearSelected = hit.Date;
                            mouseMoveFlags.HeaderDate = hit.Date;

                            Invalidate(hit.InvalidateBounds);
                            //this.Update();

                            UpdateYearMenu(CultureCalendar.GetYear(hit.Date));

                            //this.yearMenu.Tag = hit.Date;

                            showingMenu = true;

                            // show year menu
                            //this.yearMenu.Show(this, new Point(hit.Bounds.Right, e.Location.Y));

                            break;
                        }

                    case MonthCalendarHitType.Arrow:
                        {
                            // an arrow was pressed
                            // set new start date
                            if (SetStartDate(hit.Date))
                            {
                                // update months
                                UpdateMonths();

                                // raise event
                                RaiseDateChanged();

                                mouseMoveFlags.HeaderDate = leftArrowRect.Contains(e.Location) ?
                                   months[0].Date : months[calendarDimensions.Width - 1].Date;

                                Invalidate();//refresh
                            }

                            break;
                        }

                    case MonthCalendarHitType.Footer:
                        {
                            // footer was pressed
                            selectionRanges.Clear();

                            bool raiseDateChanged = false;

                            SelectionRange range = SelectionRange;

                            // determine if date changed event has to be raised
                            if (DateTime.Today < months[0].FirstVisibleDate || DateTime.Today > lastVisibleDate)
                            {
                                // set new start date
                                if (SetStartDate(DateTime.Today))
                                {
                                    // update months
                                    UpdateMonths();

                                    raiseDateChanged = true;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            // set new selection start and end values
                            selectionStart = DateTime.Today;
                            selectionEnd = DateTime.Today;

                            SetSelectionRange(daySelectionMode);

                            OnDateClicked(new DateEventArgs(DateTime.Today));

                            // raise events if necessary
                            if (range != SelectionRange)
                            {
                                RaiseDateSelected();
                            }

                            if (raiseDateChanged)
                            {
                                RaiseDateChanged();
                            }

                            Invalidate();//refresh

                            break;
                        }

                    case MonthCalendarHitType.Header:
                        {
                            // header was pressed
                            Invalidate(hit.Bounds);
                            //this.Update();

                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.MouseMove"/> event.
        /// </summary>
        /// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Location == mouseLocation)
            {
                return;
            }

            mouseLocation = e.Location;

            // backup and reset mouse move flags
            mouseMoveFlags.BackupAndReset();

            // perform hit test
            MonthCalendarHitTest hit = HitTest(e.Location);

            if (e.Button == MouseButtons.Left)
            {
                // if selection started - only in manual selection mode
                if (selectionStarted)
                {
                    // if selection started with hit type Day and mouse is over new date
                    if (hit.Type == MonthCalendarHitType.Day
                       && currentHitType == MonthCalendarHitType.Day
                       && currentMoveBounds != hit.Bounds)
                    {
                        currentMoveBounds = hit.Bounds;

                        // set new selection end
                        SelectionEnd = hit.Date;
                    }
                    else if (hit.Type == MonthCalendarHitType.Week
                       && currentHitType == MonthCalendarHitType.Week)
                    {
                        // set indicator that a week header element is selected
                        mouseMoveFlags.WeekHeader = true;

                        // get new end date
                        DateTime endDate = new MonthCalendarDate(CultureCalendar, hit.Date).AddDays(6).Date;

                        // if new week header element
                        if (currentMoveBounds != hit.Bounds)
                        {
                            currentMoveBounds = hit.Bounds;

                            // check if selection is switched
                            if (selectionStart == selectionStartRange.End)
                            {
                                // are we after the original end date?
                                if (endDate > selectionStart)
                                {
                                    // set original start date
                                    selectionStart = selectionStartRange.Start;

                                    // set new end date
                                    SelectionEnd = endDate;
                                }
                                else
                                {
                                    // going backwards - set new "end" date - it's now the start date
                                    SelectionEnd = hit.Date;
                                }
                            }
                            else
                            {
                                // we are after the start date
                                if (endDate > selectionStart)
                                {
                                    // set end date
                                    SelectionEnd = endDate;
                                }
                                else
                                {
                                    // switch start and end
                                    selectionStart = selectionStartRange.End;
                                    SelectionEnd = hit.Date;
                                }
                            }
                        }
                    }
                }
                else
                {
                    switch (hit.Type)
                    {
                        case MonthCalendarHitType.MonthName:
                            {
                                mouseMoveFlags.MonthName = hit.Date;
                                mouseMoveFlags.HeaderDate = hit.Date;

                                Invalidate(hit.InvalidateBounds);

                                break;
                            }

                        case MonthCalendarHitType.MonthYear:
                            {
                                mouseMoveFlags.Year = hit.Date;
                                mouseMoveFlags.HeaderDate = hit.Date;

                                Invalidate(hit.InvalidateBounds);

                                break;
                            }

                        case MonthCalendarHitType.Header:
                            {
                                mouseMoveFlags.HeaderDate = hit.Date;
                                Invalidate(hit.InvalidateBounds);

                                break;
                            }

                        case MonthCalendarHitType.Arrow:
                            {
                                bool useRTL = false;// this.UseRTL;

                                if (leftArrowRect.Contains(e.Location))
                                {
                                    mouseMoveFlags.LeftArrow = !useRTL;
                                    mouseMoveFlags.RightArrow = useRTL;

                                    mouseMoveFlags.HeaderDate = months[0].Date;
                                }
                                else
                                {
                                    mouseMoveFlags.LeftArrow = useRTL;
                                    mouseMoveFlags.RightArrow = !useRTL;

                                    mouseMoveFlags.HeaderDate = months[calendarDimensions.Width - 1].Date;
                                }

                                Invalidate(hit.InvalidateBounds);

                                break;
                            }

                        case MonthCalendarHitType.Footer:
                            {
                                mouseMoveFlags.Footer = true;

                                Invalidate(hit.InvalidateBounds);

                                break;
                            }

                        default:
                            {
                                Invalidate();

                                break;
                            }
                    }
                }
            }
            else if (e.Button == MouseButtons.None)
            {
                if (!ShowHover) return; // skip hover rendering
                // no mouse button is pressed
                // set flags and invalidate corresponding region
                switch (hit.Type)
                {
                    case MonthCalendarHitType.Day:
                        {
                            mouseMoveFlags.Day = hit.Date;

                            var bold = GetBoldedDates().Contains(hit.Date) || boldDatesCollection.Exists(d => d.Value.Date == hit.Date.Date);

                            OnActiveDateChanged(new ActiveDateChangedEventArgs(hit.Date, bold));

                            InvalidateMonth(hit.Date, true);

                            break;
                        }

                    case MonthCalendarHitType.Week:
                        {
                            mouseMoveFlags.WeekHeader = true;

                            break;
                        }

                    case MonthCalendarHitType.MonthName:
                        {
                            mouseMoveFlags.MonthName = hit.Date;
                            mouseMoveFlags.HeaderDate = hit.Date;

                            break;
                        }

                    case MonthCalendarHitType.MonthYear:
                        {
                            mouseMoveFlags.Year = hit.Date;
                            mouseMoveFlags.HeaderDate = hit.Date;

                            break;
                        }

                    case MonthCalendarHitType.Header:
                        {
                            mouseMoveFlags.HeaderDate = hit.Date;

                            break;
                        }

                    case MonthCalendarHitType.Arrow:
                        {
                            bool useRTL = false;

                            if (leftArrowRect.Contains(e.Location))
                            {
                                mouseMoveFlags.LeftArrow = !useRTL;
                                mouseMoveFlags.RightArrow = useRTL;

                                mouseMoveFlags.HeaderDate = months[0].Date;
                            }
                            else if (rightArrowRect.Contains(e.Location))
                            {
                                mouseMoveFlags.LeftArrow = useRTL;
                                mouseMoveFlags.RightArrow = !useRTL;

                                mouseMoveFlags.HeaderDate = months[calendarDimensions.Width - 1].Date;
                            }

                            break;
                        }

                    case MonthCalendarHitType.Footer:
                        {
                            mouseMoveFlags.Footer = true;

                            break;
                        }
                }

                // if left arrow state changed
                if (mouseMoveFlags.LeftArrowChanged)
                {
                    Invalidate(leftArrowRect);

                    //this.Update();
                }

                // if right arrow state changed
                if (mouseMoveFlags.RightArrowChanged)
                {
                    Invalidate(rightArrowRect);

                    //this.Update();
                }

                // if header state changed
                if (mouseMoveFlags.HeaderDateChanged)
                {
                    Invalidate();
                }
                else if (mouseMoveFlags.MonthNameChanged || mouseMoveFlags.YearChanged)
                {
                    // if state of month name or year in header changed
                    SelectionRange range1 = new SelectionRange(mouseMoveFlags.MonthName, mouseMoveFlags.Backup.MonthName);

                    SelectionRange range2 = new SelectionRange(mouseMoveFlags.Year, mouseMoveFlags.Backup.Year);

                    Invalidate(months[GetIndex(range1.End)].TitleBounds);

                    if (range1.End != range2.End)
                    {
                        Invalidate(months[GetIndex(range2.End)].TitleBounds);
                    }
                }

                // if day state changed
                if (mouseMoveFlags.DayChanged)
                {
                    // invalidate current day
                    InvalidateMonth(mouseMoveFlags.Day, false);

                    // invalidate last day
                    InvalidateMonth(mouseMoveFlags.Backup.Day, false);
                }

                // if footer state changed
                if (mouseMoveFlags.FooterChanged)
                {
                    Invalidate(footerRect);
                }
            }

            // if mouse is over a week header, change cursor
            //if (this.mouseMoveFlags.WeekHeaderChanged)
            //{
            //    this.Cursor = this.mouseMoveFlags.WeekHeader ? Cursors.UpArrow : Cursors.Arrow;
            //}
        }

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            // if left mouse button is pressed and selection process was started
            if (e.Button == MouseButtons.Left && selectionStarted)
            {
                selectionRanges.Add(new SelectionRange(SelectionRange.Start, SelectionRange.End));

                // reset selection process
                selectionStarted = false;

                Invalidate();//refresh

                // raise selected event if necessary
                if (backupRange.Start != SelectionRange.Start
                   || backupRange.End != SelectionRange.End)
                {
                    // raise date 
                    RaiseDateSelected();
                }
            }

            // reset current hit type
            currentHitType = MonthCalendarHitType.None;

            Capture = false;
        }

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.MouseLeave"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            // reset some of the mouse move flags
            mouseMoveFlags.LeftArrow = false;
            mouseMoveFlags.RightArrow = false;
            mouseMoveFlags.MonthName = DateTime.MinValue;
            mouseMoveFlags.Year = DateTime.MinValue;
            mouseMoveFlags.Footer = false;
            mouseMoveFlags.Day = DateTime.MinValue;

            if (!showingMenu)
            {
                mouseMoveFlags.HeaderDate = DateTime.MinValue;
            }

            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.MouseWheel"/> event.
        /// </summary>
        /// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!showingMenu)
            {
                Scroll(e.Delta.Y > 0); //?
            }
        }

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.ParentChanged"/> event.
        /// </summary>
        /// <param name="e">A <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent != null && Created)
            {
                UpdateMonths();

                Invalidate();
            }
        }

        /// <summary>
        /// Raises the <see cref="Windows.Forms.Control.EnabledChanged"/> event.
        /// </summary>
        /// <param name="e">A <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            Invalidate();//refresh
        }

        /// <summary>
        /// Raises the <see cref="DateSelected"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DateRangeEventArgs"/> object that contains the event data.</param>
        protected virtual void OnDateSelected(DateRangeEventArgs e)
        {
            if (DateSelected != null)
            {
                DateSelected(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DateClicked"/> event.
        /// </summary>
        /// <param name="e">A <see cref="DateEventArgs"/> that contains the event data.</param>
        protected virtual void OnDateClicked(DateEventArgs e)
        {
            if (DateClicked != null)
            {
                DateClicked(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DateChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DateRangeEventArgs"/> object that contains the event data.</param>
        protected virtual void OnDateChanged(DateRangeEventArgs e)
        {
            if (DateChanged != null)
            {
                DateChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ActiveDateChanged"/> event.
        /// </summary>
        /// <param name="e">A <see cref="ActiveDateChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnActiveDateChanged(ActiveDateChangedEventArgs e)
        {
            if (ActiveDateChanged != null)
            {
                ActiveDateChanged(this, e);
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Builds an array of <see cref="MonthCalendarEraRange"/> to store the min and max date of the eras of the specified <see cref="Calendar"/>.
        /// </summary>
        /// <param name="cal">The <see cref="Calendar"/> to retrieve the era ranges for.</param>
        /// <returns>An array of type <see cref="MonthCalendarEraRange"/>.</returns>
        private static MonthCalendarEraRange[] GetEraRanges(Calendar cal)
        {
            if (cal.Eras.Length == 1)
            {
                return new[] { new MonthCalendarEraRange(cal.Eras[0], cal.MinSupportedDateTime.Date, cal.MaxSupportedDateTime.Date) };
            }

            List<MonthCalendarEraRange> ranges = new List<MonthCalendarEraRange>();

            DateTime date = cal.MinSupportedDateTime.Date;

            int currentEra = -1;

            while (date < cal.MaxSupportedDateTime.Date)
            {
                int era = cal.GetEra(date);

                if (era != currentEra)
                {
                    ranges.Add(new MonthCalendarEraRange(era, date));

                    if (currentEra != -1)
                    {
                        ranges[ranges.Count - 2].MaxDate = cal.AddDays(date, -1);
                    }

                    currentEra = era;
                }

                date = cal.AddDays(date, 1);
            }

            ranges[ranges.Count - 1].MaxDate = date;

            return ranges.ToArray();
        }

        /// <summary>
        /// Gets the era range for the specified era.
        /// </summary>
        /// <param name="era">The era to get the date range for.</param>
        /// <returns>A <see cref="MonthCalendarEraRange"/> object.</returns>
        private MonthCalendarEraRange GetEraRange(int era)
        {
            foreach (MonthCalendarEraRange e in eraRanges)
            {
                if (e.Era == era)
                {
                    return e;
                }
            }

            return new MonthCalendarEraRange(
               CultureCalendar.GetEra(DateTime.Today),
               CultureCalendar.MinSupportedDateTime.Date,
               CultureCalendar.MaxSupportedDateTime.Date);
        }

        /// <summary>
        /// Gets the era range for the era the current date is in.
        /// </summary>
        /// <returns>A <see cref="MonthCalendarEraRange"/>.</returns>
        private MonthCalendarEraRange GetEraRange()
        {
            return GetEraRange(CultureCalendar.GetEra(DateTime.Today));
        }

        /// <summary>
        /// Calculates the various sizes of a single month view and the global size of the control.
        /// </summary>
        private void CalculateSize(bool changeDimension)
        {
            // if already calculating - return
            if (inUpdate)
            {
                return;
            }

            headerFont = new SKFont(SKTypeface.Default, (int)(9 * Scaling));
            footerFont = new SKFont(SKTypeface.Default, (int)(9 * Scaling));
            dayHeaderFont = new SKFont(SKTypeface.Default, (int)(8 * Scaling));

            inUpdate = true;

            // get sizes for different elements of the calendar
            SKSize daySize = TextMeasurer.MeasureText("30", SKTypeface.Default, (int)(Font.Size * Scaling));
            SKSize weekNumSize = TextMeasurer.MeasureText("59", SKTypeface.Default, (int)(Font.Size * Scaling));

            MonthCalendarDate date = new MonthCalendarDate(CultureCalendar, viewStart);

            SKSize monthNameSize = TextMeasurer.MeasureText(
                formatProvider.GetMonthName(date.Year, date.Month), SKTypeface.Default, (int)(headerFont.Size));

            SKSize yearStringSize = TextMeasurer.MeasureText(viewStart.ToString("yyyy"), SKTypeface.Default, (int)(headerFont.Size));

            SKSize footerStringSize = TextMeasurer.MeasureText(viewStart.ToShortDateString(), SKTypeface.Default, (int)(headerFont.Size));

            // calculate the header height
            headerHeight = Math.Max(
                   (int)Math.Max(monthNameSize.Height + 3, yearStringSize.Height) + 1, (int)(15 * Scaling));
            
            // calculate the width of a single day
            dayWidth = (int)(14 * Scaling) + 5;// Math.Max((int)(14 * Scaling), (int)daySize.Width + 1) + 5;

            // calculate the height of a single day
            dayHeight = (int)(14 * Scaling) + 2;// Math.Max(Math.Max((int)(14 * Scaling), (int)weekNumSize.Height + 1), (int)daySize.Height + 1) + 2;
            
            // calculate the height of the footer
            footerHeight = Math.Max((int)(14 * Scaling), (int)footerStringSize.Height + 1);

            // calculate the width of the week number header
            weekNumberWidth = showWeekHeader ? Math.Max((int)(14 * Scaling), (int)weekNumSize.Width + 1) + 1 : 0;

            // set minimal height of the day name header
            dayNameHeight = (int)(14 * Scaling);

            // loop through all day names
            foreach (string str in DateMethods.GetDayNames(formatProvider, useShortestDayNames ? 2 : 1))
            {
                // get the size of the name                    
                SKSize dayNameSize = TextMeasurer.MeasureText(str, SKTypeface.Default, (int)(dayHeaderFont.Size * 1));

                // adjust the width of the day and the day name header height
                dayWidth = Math.Max(dayWidth, (int)dayNameSize.Width + 1);
                dayNameHeight = Math.Max(
                   dayNameHeight,
                   (int)dayNameSize.Height + 1);
            }

            // calculate the width and height of a MonthCalendarMonth element
            monthWidth = weekNumberWidth + dayWidth * 7 * 1 + 1 * 1;
            monthHeight = headerHeight
               + dayNameHeight + dayHeight * 6 * 1 + 1 * 1;

            if (changeDimension)
            {
                // calculate the dimension of the control
                int calWidthDim = Math.Max((int)(1 * Scaling), Width / monthWidth);
                int calHeightDim = Math.Max((int)(1 * Scaling), Height / monthHeight);

                // set the dimensions
                CalendarDimensions = new Size(calWidthDim, calHeightDim);
                //Debug.WriteLine(CalendarDimensions.ToString());
            }

            // set the width and height of the control
            Height = monthHeight * calendarDimensions.Height + (showFooter ? footerHeight : 0);
            Width = monthWidth * calendarDimensions.Width;

            // calculate the footer bounds
            footerRect = new Rectangle(
               1,
               Height - footerHeight - 1,
               Width - 2,
               footerHeight);

            // update the months
            UpdateMonths();

            inUpdate = false;

            Invalidate();//refresh
        }

        /// <summary>
        /// Calculates the various sizes of a single month view and the global size of the control.
        /// </summary>
        //private void CalculateSize_(bool changeDimension)
        //{
        //    // if already calculating - return
        //    if (this.inUpdate)
        //    {
        //        return;
        //    }

        //    this.inUpdate = true;

        //    // get sizes for different elements of the calendar
        //    SKSize daySize = TextMeasurer.MeasureText("30", SKTypeface.Default, (int)this.Font.Size);
        //    SKSize weekNumSize = TextMeasurer.MeasureText("59", SKTypeface.Default, (int)this.Font.Size);

        //    MonthCalendarDate date = new MonthCalendarDate(this.CultureCalendar, this.viewStart);

        //    SKSize monthNameSize = TextMeasurer.MeasureText(
        //        this.formatProvider.GetMonthName(date.Year, date.Month), SKTypeface.Default, (int)this.headerFont.Size);

        //    SKSize yearStringSize = TextMeasurer.MeasureText(this.viewStart.ToString("yyyy"), SKTypeface.Default, (int)this.headerFont.Size);

        //    SKSize footerStringSize = TextMeasurer.MeasureText(this.viewStart.ToShortDateString(), SKTypeface.Default, (int)this.footerFont.Size);

        //    // calculate the header height
        //    this.headerHeight = Math.Max(
        //           (int)Math.Max(monthNameSize.Height + 3, yearStringSize.Height) + 1, 15);

        //    // calculate the width of a single day
        //    this.dayWidth = Math.Max(12, (int)daySize.Width + 1) + 5;

        //    // calculate the height of a single day
        //    this.dayHeight = Math.Max(Math.Max(12, (int)weekNumSize.Height + 1), (int)daySize.Height + 1) + 2;

        //    // calculate the height of the footer
        //    this.footerHeight = Math.Max(12, (int)footerStringSize.Height + 1);

        //    // calculate the width of the week number header
        //    this.weekNumberWidth = this.showWeekHeader ? Math.Max(12, (int)weekNumSize.Width + 1) + 2 : 0;

        //    // set minimal height of the day name header
        //    this.dayNameHeight = 14;

        //    // loop through all day names
        //    foreach (string str in DateMethods.GetDayNames(this.formatProvider, this.useShortestDayNames ? 2 : 1))
        //    {
        //        // get the size of the name                    
        //        SKSize dayNameSize = TextMeasurer.MeasureText(str, SKTypeface.Default, (int)this.dayHeaderFont.Size);

        //        // adjust the width of the day and the day name header height
        //        this.dayWidth = Math.Max(this.dayWidth, (int)dayNameSize.Width + 1);
        //        this.dayNameHeight = Math.Max(
        //           this.dayNameHeight,
        //           (int)dayNameSize.Height + 1);
        //    }

        //    // calculate the width and height of a MonthCalendarMonth element
        //    this.monthWidth = this.weekNumberWidth + (this.dayWidth * 7) + 1;
        //    this.monthHeight = this.headerHeight
        //       + this.dayNameHeight + (this.dayHeight * 6) + 1;

        //    if (changeDimension)
        //    {
        //        // calculate the dimension of the control
        //        int calWidthDim = Math.Max(1, this.Width / this.monthWidth);
        //        int calHeightDim = Math.Max(1, this.Height / this.monthHeight);

        //        // set the dimensions
        //        this.CalendarDimensions = new Size(calWidthDim, calHeightDim);
        //    }

        //    // set the width and height of the control
        //    this.Height = (this.monthHeight * this.calendarDimensions.Height) + (this.showFooter ? this.footerHeight : 0);
        //    this.Width = this.monthWidth * this.calendarDimensions.Width;

        //    // calculate the footer bounds
        //    this.footerRect = new Rectangle(
        //       1,
        //       this.Height - this.footerHeight - 1,
        //       this.Width - 2,
        //       this.footerHeight);

        //    // update the months
        //    this.UpdateMonths();

        //    this.inUpdate = false;

        //    this.Invalidate();//refresh
        //}

        /// <summary>
        /// Sets the start date.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <returns>true if <paramref name="start"/> is valid; false otherwise.</returns>
        private bool SetStartDate(DateTime start)
        {
            if (start < DateTime.MinValue.Date || start > DateTime.MaxValue.Date)
            {
                return false;
            }

            DayOfWeek firstDayOfWeek = formatProvider.FirstDayOfWeek;

            MonthCalendarDate dt = new MonthCalendarDate(CultureCalendar, maxDate);

            if (start > maxDate)
            {
                start = dt.AddMonths(1 - months.Length).FirstOfMonth.Date;
            }

            if (start < minDate)
            {
                start = minDate;
            }

            dt = new MonthCalendarDate(CultureCalendar, start);
            int length = months != null ? months.Length - 1 : 0;

            while (dt.Date > minDate && dt.Day != 1)
            {
                dt = dt.AddDays(-1);
            }

            MonthCalendarDate endDate = dt.AddMonths(length);
            MonthCalendarDate endDateDay = endDate.AddDays(endDate.DaysInMonth - 1 - (endDate.Day - 1));

            if (endDate.Date >= maxDate || endDateDay.Date >= maxDate)
            {
                dt = new MonthCalendarDate(CultureCalendar, maxDate).AddMonths(-length).FirstOfMonth;
            }

            viewStart = dt.Date;

            while (dt.Date > CultureCalendar.MinSupportedDateTime.Date && dt.DayOfWeek != firstDayOfWeek)
            {
                dt = dt.AddDays(-1);
            }

            realStart = dt.Date;

            return true;
        }

        /// <summary>
        /// Gets the index of the <see cref="MonthCalendarMonth"/> in the array of the specified monthYear datetime.
        /// </summary>
        /// <param name="monthYear">The date to search for.</param>
        /// <returns>The index in the array.</returns>
        private int GetIndex(DateTime monthYear)
        {
            return (from month in months where month != null where month.Date == monthYear select month.Index)
               .FirstOrDefault();
        }

        /// <summary>
        /// Gets the <see cref="MonthCalendarMonth"/> which contains the specified date.
        /// </summary>
        /// <param name="day">The day to search for.</param>
        /// <returns>An <see cref="MonthCalendarMonth"/> if day is valid; otherwise null.</returns>
        private MonthCalendarMonth GetMonth(DateTime day)
        {
            if (day == DateTime.MinValue)
            {
                return null;
            }

            return months.Where(month => month != null)
               .FirstOrDefault(month => month.ContainsDate(day));
        }

        /// <summary>
        /// Updates the shown months.
        /// </summary>
        public void UpdateMonths()
        {
            int x = 0, y = 0, index = 0;
            int calWidthDim = calendarDimensions.Width;
            int calHeightDim = calendarDimensions.Height;

            List<MonthCalendarMonth> monthList = new List<MonthCalendarMonth>();

            MonthCalendarDate dt = new MonthCalendarDate(CultureCalendar, viewStart);

            if (dt.GetEndDateOfWeek(formatProvider).Month != dt.Month)
            {
                dt = dt.GetEndDateOfWeek(formatProvider).FirstOfMonth;
            }

            //if (this.UseRTL)
            //{
            //    x = this.monthWidth * (calWidthDim - 1);

            //    for (int i = 0; i < calHeightDim; i++)
            //    {
            //        for (int j = calWidthDim - 1; j >= 0; j--)
            //        {
            //            if (dt.Date >= this.maxDate)
            //            {
            //                break;
            //            }

            //            monthList.Add(new MonthCalendarMonth(this, dt.Date)
            //            {
            //                Location = new Point(x, y),
            //                Index = index++
            //            });

            //            x -= this.monthWidth;
            //            dt = dt.AddMonths(1);
            //        }

            //        x = this.monthWidth * (calWidthDim - 1);
            //        y += this.monthHeight;
            //    }
            //}
            //else
            {
                for (int i = 0; i < calHeightDim; i++)
                {
                    for (int j = 0; j < calWidthDim; j++)
                    {
                        if (dt.Date >= maxDate)
                        {
                            break;
                        }

                        monthList.Add(new MonthCalendarMonth(this, dt.Date)
                        {
                            Location = new Point(x, y),
                            Index = index++
                        });

                        x += monthWidth;
                        dt = dt.AddMonths(1);
                    }

                    x = 0;
                    y += monthHeight;
                }
            }

            if (monthList.Count > 0)
            {
                lastVisibleDate = monthList[monthList.Count - 1].LastVisibleDate;

                months = monthList.ToArray();
            }
        }

        /// <summary>
        /// Updates the month menu.
        /// </summary>
        /// <param name="year">The year to calculate the months for.</param>
        private void UpdateMonthMenu(int year)
        {
            int i = 1;

            int monthsInYear = CultureCalendar.GetMonthsInYear(year);

            // set month names in menu
            foreach (MenuItem item in monthMenu.Items)
            {
                if (i <= monthsInYear)
                {
                    //item.Tag = i;
                    item.Text = formatProvider.GetMonthName(year, i++);
                    item.Enabled = true;
                }
                else
                {
                    //item.Tag = null;
                    item.Text = string.Empty;
                    item.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Updates the year menu.
        /// </summary>
        /// <param name="year">The year in the middle to display.</param>
        private void UpdateYearMenu(int year)
        {
            //year -= 4;

            //int maxYear = this.CultureCalendar.GetYear(this.maxDate);
            //int minYear = this.CultureCalendar.GetYear(this.minDate);

            //if (year + 8 > maxYear)
            //{
            //    year = maxYear - 8;
            //}
            //else if (year < minYear)
            //{
            //    year = minYear;
            //}

            //year = Math.Max(1, year);

            //foreach (MenuItem item in this.yearMenu.Items)
            //{
            //    item.Text = DateMethods.GetNumberString(year, this.UseNativeDigits ? this.Culture.NumberFormat.NativeDigits : null, false);
            //    //item.Tag = year;

            //    //item.Font = year == this.CultureCalendar.GetYear(DateTime.Today) ?
            //    //   new Font("Tahoma", 8.25F, FontStyle.Bold)
            //    //   : new Font("Tahoma", 8.25F, FontStyle.Regular);
            //    //item.ForeColor = year == this.CultureCalendar.GetYear(DateTime.Today) ?
            //    //   Color.FromArgb(251, 200, 79)
            //    //   : Color.Black;

            //    year++;
            //}
        }

        /// <summary>
        /// Handles clicks in the month menu.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event data.</param>
        private void MonthClick(object sender, EventArgs e)
        {
            //MonthCalendarDate currentMonthYear = new MonthCalendarDate(this.CultureCalendar, (DateTime)this.monthMenu.Tag);

            //int monthClicked = (int)((MenuItem)sender).Tag;

            //if (currentMonthYear.Month != monthClicked)
            //{
            //    MonthCalendarDate dt = new MonthCalendarDate(this.CultureCalendar, new DateTime(currentMonthYear.Year, monthClicked, 1, this.CultureCalendar));
            //    DateTime newStartDate = dt.AddMonths(-this.GetIndex(currentMonthYear.Date)).Date;

            //    if (this.SetStartDate(newStartDate))
            //    {
            //        this.UpdateMonths();

            //        this.RaiseDateChanged();

            //        //this.Focus();

            //        this.Invalidate();// this.Refresh();
            //    }
            //}
        }

        /// <summary>
        /// Handles clicks in the year menu.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event data.</param>
        private void YearClick(object sender, EventArgs e)
        {
            //DateTime currentMonthYear = (DateTime)this.yearMenu.Tag;

            //int yearClicked = (int)((MenuItem)sender).Tag;

            //MonthCalendarDate dt = new MonthCalendarDate(this.CultureCalendar, currentMonthYear);

            //if (dt.Year != yearClicked)
            //{
            //    MonthCalendarDate newStartDate = new MonthCalendarDate(this.CultureCalendar, new DateTime(yearClicked, dt.Month, 1, this.CultureCalendar))
            //       .AddMonths(-this.GetIndex(currentMonthYear));

            //    if (this.SetStartDate(newStartDate.Date))
            //    {
            //        this.UpdateMonths();

            //        this.RaiseDateChanged();

            //        //this.Focus();

            //        this.Invalidate();//refresh
            //    }
            //}
        }

        ///// <summary>
        ///// Is called when the month menu was closed.
        ///// </summary>
        ///// <param name="sender">The sender.</param>
        ///// <param name="e">A <see cref="ToolStripDropDownClosedEventArgs"/> that contains the event data.</param>
        //private void MonthMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        //{
        //    this.monthSelected = DateTime.MinValue;
        //    this.showingMenu = false;

        //    this.Invalidate(this.months[this.GetIndex((DateTime)this.monthMenu.Tag)].TitleBounds);
        //}

        ///// <summary>
        ///// Is called when the year menu was closed.
        ///// </summary>
        ///// <param name="sender">The sender.</param>
        ///// <param name="e">A <see cref="ToolStripDropDownClosedEventArgs"/> that contains the event data.</param>
        //private void YearMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        //{
        //    this.yearSelected = DateTime.MinValue;
        //    this.showingMenu = false;

        //    this.Invalidate(this.months[this.GetIndex((DateTime)this.yearMenu.Tag)].TitleBounds);
        //}

        /// <summary>
        /// Calls <see cref="OnDateSelected"/>.
        /// </summary>
        private void RaiseDateSelected()
        {
            SelectionRange range = SelectionRange;

            DateTime selStart = range.Start;
            DateTime selEnd = range.End;

            if (selStart == DateTime.MinValue)
            {
                selStart = selEnd;
            }

            OnDateSelected(new DateRangeEventArgs(selStart, selEnd));
        }

        /// <summary>
        /// Calls <see cref="OnDateChanged"/>.
        /// </summary>
        private void RaiseDateChanged()
        {
            OnDateChanged(
               new DateRangeEventArgs(realStart, lastVisibleDate));
        }

        /// <summary>
        /// Raises the <see cref="SelectionExtendEnd"/> event.
        /// </summary>
        private void RaiseSelectExtendEnd()
        {
            var handler = SelectionExtendEnd;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="InternalDateSelected"/> event.
        /// </summary>
        private void RaiseInternalDateSelected()
        {
            if (InternalDateSelected != null)
            {
                InternalDateSelected(this, new DateEventArgs(selectionStart));
            }
        }

        /// <summary>
        /// Adjusts the currently displayed month by setting a new start date.
        /// </summary>
        /// <param name="up">true for scrolling upwards, false otherwise.</param>
        private void Scroll(bool up)
        {
            if (SetStartDate(GetNewScrollDate(up)))
            {
                UpdateMonths();

                RaiseDateChanged();

                Invalidate();//refresh
            }
        }

        /// <summary>
        /// Gets the new date for the specified scroll direction.
        /// </summary>
        /// <param name="up">true for scrolling upwards, false otherwise.</param>
        /// <returns>The new start date.</returns>
        private DateTime GetNewScrollDate(bool up)
        {
            if (lastVisibleDate == maxDate && !up || months[0].FirstVisibleDate == minDate && up)
            {
                return viewStart;
            }

            MonthCalendarDate dt = new MonthCalendarDate(CultureCalendar, viewStart);

            int monthsToAdd = (scrollChange == 0 ?
               Math.Max(calendarDimensions.Width * calendarDimensions.Height - 1, 1)
               : scrollChange) * (up ? -1 : 1);

            int length = months == null ? Math.Max(1, calendarDimensions.Width * calendarDimensions.Height) : months.Length;

            MonthCalendarDate newStartMonthDate = dt.AddMonths(monthsToAdd);

            MonthCalendarDate lastMonthDate = newStartMonthDate.AddMonths(length - 1);

            MonthCalendarDate lastMonthEndDate = lastMonthDate.AddDays(lastMonthDate.DaysInMonth - 1 - lastMonthDate.Day);

            if (newStartMonthDate.Date < minDate)
            {
                newStartMonthDate = new MonthCalendarDate(CultureCalendar, minDate);
            }
            else if (lastMonthEndDate.Date >= maxDate || lastMonthDate.Date >= maxDate)
            {
                MonthCalendarDate maxdt = new MonthCalendarDate(CultureCalendar, maxDate).FirstOfMonth;
                newStartMonthDate = maxdt.AddMonths(1 - length);
            }

            return newStartMonthDate.Date;
        }

        /// <summary>
        /// Gets the current month header state.
        /// </summary>
        /// <param name="monthDate">The month date.</param>
        /// <returns>A <see cref="MonthCalendarHeaderState"/> value.</returns>
        private MonthCalendarHeaderState GetMonthHeaderState(DateTime monthDate)
        {
            MonthCalendarHeaderState state = MonthCalendarHeaderState.Default;

            if (monthSelected == monthDate)
            {
                state = MonthCalendarHeaderState.MonthNameSelected;
            }
            else if (yearSelected == monthDate)
            {
                state = MonthCalendarHeaderState.YearSelected;
            }
            else if (mouseMoveFlags.MonthName == monthDate)
            {
                state = MonthCalendarHeaderState.MonthNameActive;
            }
            else if (mouseMoveFlags.Year == monthDate)
            {
                state = MonthCalendarHeaderState.YearActive;
            }
            else if (mouseMoveFlags.HeaderDate == monthDate)
            {
                state = MonthCalendarHeaderState.Active;
            }

            return state;
        }

        /// <summary>
        /// Invalidates the region taken up by the month specified by the <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date specifying the <see cref="MonthCalendarMonth"/> to invalidate.</param>
        /// <param name="refreshInvalid">true for refreshing the whole control if invalid <paramref name="date"/> passed; otherwise false.</param>
        private void InvalidateMonth(DateTime date, bool refreshInvalid)
        {
            if (date == DateTime.MinValue)
            {
                if (refreshInvalid)
                {
                    Invalidate();//refresh
                }

                return;
            }

            MonthCalendarMonth month = GetMonth(date);

            if (month != null)
            {
                Invalidate(month.MonthBounds);
                //this.Update();
            }
            else if (date > lastVisibleDate)
            {
                Invalidate(months[months.Length - 1].Bounds);
                //this.Update();
            }
            else if (refreshInvalid)
            {
                //this.Refresh();
                Invalidate();
            }
        }

        /// <summary>
        /// Checks if the <paramref name="newSelectionDate"/> is within bounds of the <paramref name="baseDate"/>
        /// and the <see cref="MaxSelectionCount"/>.
        /// </summary>
        /// <param name="baseDate">The base date from where to check.</param>
        /// <param name="newSelectionDate">The new selection date.</param>
        /// <returns>A valid new selection date if valid parameters, otherwise <c>DateTime.MinValue</c>.</returns>
        private DateTime GetSelectionDate(DateTime baseDate, DateTime newSelectionDate)
        {
            if (maxSelectionCount == 0 || baseDate == DateTime.MinValue)
            {
                return newSelectionDate;
            }

            if (baseDate >= CultureCalendar.MinSupportedDateTime && newSelectionDate >= CultureCalendar.MinSupportedDateTime
               && baseDate <= CultureCalendar.MaxSupportedDateTime && newSelectionDate <= CultureCalendar.MaxSupportedDateTime)
            {
                int days = (baseDate - newSelectionDate).Days;

                if (Math.Abs(days) >= maxSelectionCount)
                {

                    newSelectionDate =
                       new MonthCalendarDate(CultureCalendar, baseDate).AddDays(days < 0
                                                                                     ? maxSelectionCount - 1
                                                                                     : 1 - maxSelectionCount).Date;
                }

                return newSelectionDate;
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// Returns the minimum date for the control.
        /// </summary>
        /// <param name="date">The date to set as min date.</param>
        /// <returns>The min date.</returns>
        private DateTime GetMinDate(DateTime date)
        {
            DateTime dt = new DateTime(1900, 1, 1);
            DateTime minEra = GetEraRange().MinDate;

            // bug in JapaneseLunisolarCalendar - JapaneseLunisolarCalendar.GetYear() with date parameter
            // between 1989/1/8 and 1989/2/6 returns 0 therefore make sure, the calendar
            // can display date range if ViewStart set to min date
            if (cultureCalendar.GetType() == typeof(JapaneseLunisolarCalendar))
            {
                minEra = new DateTime(1989, 4, 1);
            }

            DateTime mindate = minEra < dt ? dt : minEra;

            return date < mindate ? mindate : date;
        }

        /// <summary>
        /// Returns the maximum date for the control.
        /// </summary>
        /// <param name="date">The date to set as max date.</param>
        /// <returns>The max date.</returns>
        private DateTime GetMaxDate(DateTime date)
        {
            DateTime dt = new DateTime(9998, 12, 31);
            DateTime maxEra = GetEraRange().MaxDate;

            DateTime maxdate = maxEra > dt ? dt : maxEra;

            return date > maxdate ? maxdate : date;
        }

        /// <summary>
        /// If changing the used calendar, then this method reassigns the selection mode to set the correct selection range.
        /// </summary>
        private void ReAssignSelectionMode()
        {
            SelectionRange = null;

            MonthCalendarSelectionMode selMode = daySelectionMode;

            daySelectionMode = MonthCalendarSelectionMode.Manual;

            SelectionMode = selMode;
        }

        /// <summary>
        /// Sets the selection range for the specified <see cref="MonthCalendarSelectionMode"/>.
        /// </summary>
        /// <param name="selMode">The <see cref="MonthCalendarSelectionMode"/> value to set the selection range for.</param>
        private void SetSelectionRange(MonthCalendarSelectionMode selMode)
        {
            switch (selMode)
            {
                case MonthCalendarSelectionMode.Day:
                    {
                        selectionEnd = selectionStart;

                        break;
                    }

                case MonthCalendarSelectionMode.WorkWeek:
                case MonthCalendarSelectionMode.FullWeek:
                    {
                        MonthCalendarDate dt =
                           new MonthCalendarDate(CultureCalendar, selectionStart).GetFirstDayInWeek(
                              formatProvider);
                        selectionStart = dt.Date;
                        selectionEnd = dt.AddDays(6).Date;

                        break;
                    }

                case MonthCalendarSelectionMode.Month:
                    {
                        MonthCalendarDate dt = new MonthCalendarDate(CultureCalendar, selectionStart).FirstOfMonth;
                        selectionStart = dt.Date;
                        selectionEnd = dt.AddMonths(1).AddDays(-1).Date;

                        break;
                    }
            }
        }

        #region private design time methods

        /// <summary>
        /// Resets the property <see cref="SelectionRange"/>.
        /// </summary>
        /// <remarks>Only used by the designer.</remarks>
        private void ResetSelectionRange()
        {
            selectionEnd = DateTime.Today;
            selectionStart = DateTime.Today;

            Invalidate();//refresh
        }

        /// <summary>
        /// Initializes the context menus.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            //this.monthMenu = new ContextMenu();
            //this.tsmiJan = new MenuItem();
            //this.tsmiFeb = new MenuItem();
            //this.tsmiMar = new MenuItem();
            //this.tsmiApr = new MenuItem();
            //this.tsmiMay = new MenuItem();
            //this.tsmiJun = new MenuItem();
            //this.tsmiJul = new MenuItem();
            //this.tsmiAug = new MenuItem();
            //this.tsmiSep = new MenuItem();
            //this.tsmiOct = new MenuItem();
            //this.tsmiNov = new MenuItem();
            //this.tsmiDez = new MenuItem();
            //this.tsmiA1 = new MenuItem();
            //this.tsmiA2 = new MenuItem();
            //this.yearMenu = new ContextMenu();
            //this.tsmiYear1 = new MenuItem();
            //this.tsmiYear2 = new MenuItem();
            //this.tsmiYear3 = new MenuItem();
            //this.tsmiYear4 = new MenuItem();
            //this.tsmiYear5 = new MenuItem();
            //this.tsmiYear6 = new MenuItem();
            //this.tsmiYear7 = new MenuItem();
            //this.tsmiYear8 = new MenuItem();
            //this.tsmiYear9 = new MenuItem();
            //this.monthMenu.SuspendLayout();
            //this.yearMenu.SuspendLayout();
            //this.SuspendLayout();
            // 
            // monthMenu
            // 
            //this.monthMenu.Items.AddRange(new ToolStripItem[] {
            //this.tsmiJan,
            //this.tsmiFeb,
            //this.tsmiMar,
            //this.tsmiApr,
            //this.tsmiMay,
            //this.tsmiJun,
            //this.tsmiJul,
            //this.tsmiAug,
            //this.tsmiSep,
            //this.tsmiOct,
            //this.tsmiNov,
            //this.tsmiDez,
            //this.tsmiA1,
            //this.tsmiA2});
            //this.monthMenu.Name = "monthMenu";
            //this.monthMenu.ShowImageMargin = false;
            //this.monthMenu.Size = new Size(54, 312);
            //this.monthMenu.Closed += this.MonthMenuClosed;
            //// 
            //// tsmiJan
            //// 
            //this.tsmiJan.Size = new Size(78, 22);
            //this.tsmiJan.Tag = 1;
            //this.tsmiJan.Click += this.MonthClick;
            //// 
            //// tsmiFeb
            //// 
            //this.tsmiFeb.Size = new Size(78, 22);
            //this.tsmiFeb.Tag = 2;
            //this.tsmiFeb.Click += this.MonthClick;
            //// 
            //// tsmiMar
            //// 
            //this.tsmiMar.Size = new Size(78, 22);
            //this.tsmiMar.Tag = 3;
            //this.tsmiMar.Click += this.MonthClick;
            //// 
            //// tsmiApr
            //// 
            //this.tsmiApr.Size = new Size(78, 22);
            //this.tsmiApr.Tag = 4;
            //this.tsmiApr.Click += this.MonthClick;
            //// 
            //// tsmiMay
            //// 
            //this.tsmiMay.Size = new Size(78, 22);
            //this.tsmiMay.Tag = 5;
            //this.tsmiMay.Click += this.MonthClick;
            //// 
            //// tsmiJun
            //// 
            //this.tsmiJun.Size = new Size(78, 22);
            //this.tsmiJun.Tag = 6;
            //this.tsmiJun.Click += this.MonthClick;
            //// 
            //// tsmiJul
            //// 
            //this.tsmiJul.Size = new Size(78, 22);
            //this.tsmiJul.Tag = 7;
            //this.tsmiJul.Click += this.MonthClick;
            //// 
            //// tsmiAug
            //// 
            //this.tsmiAug.Size = new Size(78, 22);
            //this.tsmiAug.Tag = 8;
            //this.tsmiAug.Click += this.MonthClick;
            //// 
            //// tsmiSep
            //// 
            //this.tsmiSep.Size = new Size(78, 22);
            //this.tsmiSep.Tag = 9;
            //this.tsmiSep.Click += this.MonthClick;
            //// 
            //// tsmiOct
            //// 
            //this.tsmiOct.Size = new Size(78, 22);
            //this.tsmiOct.Tag = 10;
            //this.tsmiOct.Click += this.MonthClick;
            //// 
            //// tsmiNov
            //// 
            //this.tsmiNov.Size = new Size(78, 22);
            //this.tsmiNov.Tag = 11;
            //this.tsmiNov.Click += this.MonthClick;
            //// 
            //// tsmiDez
            //// 
            //this.tsmiDez.Size = new Size(78, 22);
            //this.tsmiDez.Tag = 12;
            //this.tsmiDez.Click += this.MonthClick;
            //// 
            //// tsmiA1
            //// 
            //this.tsmiA1.Size = new Size(78, 22);
            //this.tsmiA1.Click += this.MonthClick;
            //// 
            //// tsmiA2
            //// 
            //this.tsmiA2.Size = new Size(78, 22);
            //this.tsmiA2.Click += this.MonthClick;

            //// 
            //// yearMenu
            //// 
            //this.yearMenu.Items.AddRange(new ToolStripItem[] {
            //this.tsmiYear1,
            //this.tsmiYear2,
            //this.tsmiYear3,
            //this.tsmiYear4,
            //this.tsmiYear5,
            //this.tsmiYear6,
            //this.tsmiYear7,
            //this.tsmiYear8,
            //this.tsmiYear9});
            //this.yearMenu.Name = "yearMenu";
            //this.yearMenu.ShowImageMargin = false;
            //this.yearMenu.ShowItemToolTips = false;
            //this.yearMenu.Size = new Size(54, 202);
            //this.yearMenu.Closed += this.YearMenuClosed;

            //// 
            //// tsmiYear1
            //// 
            //this.tsmiYear1.Size = new Size(53, 22);
            //this.tsmiYear1.Click += this.YearClick;
            //// 
            //// tsmiYear2
            //// 
            //this.tsmiYear2.Size = new Size(53, 22);
            //this.tsmiYear2.Click += this.YearClick;
            //// 
            //// tsmiYear3
            //// 
            //this.tsmiYear3.Size = new Size(53, 22);
            //this.tsmiYear3.Click += this.YearClick;
            //// 
            //// tsmiYear4
            //// 
            //this.tsmiYear4.Size = new Size(53, 22);
            //this.tsmiYear4.Click += this.YearClick;
            //// 
            //// tsmiYear5
            //// 
            //this.tsmiYear5.Size = new Size(53, 22);
            //this.tsmiYear5.Click += this.YearClick;
            //// 
            //// tsmiYear6
            //// 
            //this.tsmiYear6.Size = new Size(53, 22);
            //this.tsmiYear6.Click += this.YearClick;
            //// 
            //// tsmiYear7
            //// 
            //this.tsmiYear7.Size = new Size(53, 22);
            //this.tsmiYear7.Click += this.YearClick;
            //// 
            //// tsmiYear8
            //// 
            //this.tsmiYear8.Size = new Size(53, 22);
            //this.tsmiYear8.Click += this.YearClick;
            //// 
            //// tsmiYear9
            //// 
            //this.tsmiYear9.Size = new Size(53, 22);
            //this.tsmiYear9.Click += this.YearClick;

            //this.monthMenu.ResumeLayout(false);
            //this.yearMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        #endregion

        #endregion
    }
}