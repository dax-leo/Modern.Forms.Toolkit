namespace Modern.Forms.MonthCalendar.Renderer
{
    using Modern.Forms.MonthCalendar;
    using SkiaSharp;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    /// <summary>
    /// The base renderer class for the <see cref="MonthCalendar"/><c>.</c><see cref="MonthCalendar.Renderer"/>.
    /// </summary>
    public abstract class MonthCalendarAbstractRenderer
    {
        #region fields

        /// <summary>
        /// The corresponding <see cref="MonthCalendar"/>.
        /// </summary>
        private readonly MonthCalendar calendar;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendarAbstractRenderer"/> class.
        /// </summary>
        /// <param name="calendar">The corresponding <see cref="MonthCalendar"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="calendar"/> is null.</exception>
        protected MonthCalendarAbstractRenderer(MonthCalendar calendar)
        {
            if (calendar == null)
            {
                throw new ArgumentNullException("calendar", "Parameter 'calendar' cannot be null.");
            }

            this.calendar = calendar;

            ColorTable = new MonthCalendarColorTable();
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets the associated <see cref="MonthCalendar"/> control.
        /// </summary>
        public MonthCalendar Calendar
        {
            get { return calendar; }
        }

        /// <summary>
        /// Gets or sets the used color table.
        /// </summary>
        public MonthCalendarColorTable ColorTable { get; set; }

        #endregion

        #region methods

        #region static methods

        /// <summary>
        /// Fills the specified <paramref name="path"/> either with a gradient background or a solid one.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="path">The <see cref="GraphicsPath"/> to fill.</param>
        /// <param name="colorStart">The start color.</param>
        /// <param name="colorEnd">The end color if drawing a gradient background.</param>
        /// <param name="mode">The <see cref="LinearGradientMode"/> to use.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is null.</exception>
        public static void FillBackground(
           SKCanvas g,
           Rectangle path,
           SKColor colorStart,
           SKColor colorEnd,
           LinearGradientMode? mode)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "parameter 'path' cannot be null.");
            }

            if (colorEnd == SKColor.Empty)
            {
                g.DrawRect(path.X, path.Y, path.Width, path.Height, new SKPaint() { Color = colorStart });
            }
            else
            {
                //rect.Height += 2;
                //rect.Y--;

                path.Inflate(0, path.Height + 2);
                path.Offset(0, path.Height - 1);

                g.DrawRect(path.X, path.Y, path.Width, path.Height, new SKPaint() { Color = colorStart });
            }
        }

        /// <summary>
        /// Checks the parameters of methods.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="rect">The <see cref="Rectangle"/> to draw in.</param>
        /// <returns>true, if all is ok, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="g"/> is null.</exception>
        protected static bool CheckParams(SKCanvas g, SKRect rect)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }

            if (rect.IsEmpty || !g.LocalClipBounds.IntersectsWith(rect))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Fills the specified <paramref name="rect"/> either with a gradient background or a solid one.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="rect">The <see cref="Rectangle"/> to fill.</param>
        /// <param name="colorStart">The start color.</param>
        /// <param name="colorEnd">The end color if drawing a gradient background.</param>
        /// <param name="mode">The <see cref="LinearGradientMode"/> to use.</param>
        protected static void FillBackground(
           SKCanvas g,
           SKRect rect,
           SKColor colorStart,
           SKColor colorEnd,
           LinearGradientMode? mode)
        {
            //using (SKPath path = new SKPath())
            //{
            //    path.AddRect(rect);

            FillBackground(g, rect, colorStart, colorEnd, mode);
            //}
        }

        #endregion

        #region virtual methods

        /// <summary>
        /// Draws the background of the <see cref="MonthCalendar"/>.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="g"/> is null.</exception>
        public virtual void DrawControlBackground(SKCanvas g)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }

            Rectangle r = new Rectangle(0, 0, calendar.Bounds.Width, calendar.Bounds.Height);

            FillBackground(
               g,
               r,//this.calendar.ClientRectangle, 
               ColorTable.BackgroundGradientBegin,
               ColorTable.BackgroundGradientEnd,
               ColorTable.BackgroundGradientMode);
        }

        /// <summary>
        /// Draws the title background for the specified <paramref name="month"/>.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="month">The <see cref="MonthCalendarMonth"/> to draw the title for.</param>
        /// <param name="state">The <see cref="MonthCalendarHeaderState"/> of the title.</param>
        public virtual void DrawTitleBackground(SKCanvas g, MonthCalendarMonth month, MonthCalendarHeaderState state)
        {
            //if (!CheckParams(g, month.TitleBounds))
            //{
            //   return;
            //}

            SKColor backStart, backEnd;
            LinearGradientMode? mode;

            if (state == MonthCalendarHeaderState.Default)
            {
                backStart = ColorTable.HeaderGradientBegin;
                backEnd = ColorTable.HeaderGradientEnd;
                mode = ColorTable.HeaderGradientMode;
            }
            else
            {
                backStart = ColorTable.HeaderActiveGradientBegin;
                backEnd = ColorTable.HeaderActiveGradientEnd;
                mode = ColorTable.HeaderActiveGradientMode;
            }

            FillBackgroundInternal(g, month.TitleBounds, backStart, backEnd, mode);
        }

        /// <summary>
        /// Draws the background of the month body.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> used to draw.</param>
        /// <param name="month">The <see cref="MonthCalendarMonth"/> to draw the body background for.</param>
        public virtual void DrawMonthBodyBackground(SKCanvas g, MonthCalendarMonth month)
        {
            //if (!CheckParams(g, month.MonthBounds))
            //{
            //   return;
            //}

            FillBackground(
               g,
               month.MonthBounds,
               ColorTable.MonthBodyGradientBegin,
               ColorTable.MonthBodyGradientEnd,
               ColorTable.MonthBodyGradientMode);
        }

        /// <summary>
        /// Draws the background of the day header.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> used to draw.</param>
        /// <param name="month">The <see cref="MonthCalendarMonth"/> to draw the day header background for.</param>
        public virtual void DrawDayHeaderBackground(SKCanvas g, MonthCalendarMonth month)
        {
            //if (!CheckParams(g, month.DayNamesBounds))
            //{
            //   return;
            //}

            FillBackground(
               g,
               month.DayNamesBounds,
               ColorTable.DayHeaderGradientBegin,
               ColorTable.DayHeaderGradientEnd,
               ColorTable.DayHeaderGradientMode);
        }

        /// <summary>
        /// Draws the background of the week header.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="month">The <see cref="MonthCalendarMonth"/> to draw the week header for.</param>
        public virtual void DrawWeekHeaderBackground(SKCanvas g, MonthCalendarMonth month)
        {
            //if (!CheckParams(g, month.WeekBounds))
            //{
            //   return;
            //}

            FillBackground(
               g,
               month.WeekBounds,
               ColorTable.WeekHeaderGradientBegin,
               ColorTable.WeekHeaderGradientEnd,
               ColorTable.WeekHeaderGradientMode);
        }

        /// <summary>
        /// Draws the background of the footer.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="footerBounds">The bounds of the footer.</param>
        /// <param name="active">true if the footer is in mouse over state, otherwise false.</param>
        public virtual void DrawFooterBackground(SKCanvas g, Rectangle footerBounds, bool active)
        {
            //if (!CheckParams(g, footerBounds))
            //{
            //   return;
            //}

            MonthCalendarColorTable colors = ColorTable;

            if (active)
            {
                FillBackground(
                   g,
                   footerBounds,
                   colors.FooterActiveGradientBegin,
                   colors.FooterActiveGradientEnd,
                   colors.FooterActiveGradientMode);
            }
            else
            {
                FillBackground(
                   g,
                   footerBounds,
                   colors.FooterGradientBegin,
                   colors.FooterGradientEnd,
                   colors.FooterGradientMode);
            }
        }

        #endregion

        #region abstract methods

        /// <summary>
        /// Draws the header of a <see cref="MonthCalendarMonth"/>.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="calMonth">The <see cref="MonthCalendarMonth"/> to draw the header for.</param>
        /// <param name="state">The <see cref="MonthCalendarHeaderState"/>.</param>
        public abstract void DrawMonthHeader(
           SKCanvas g,
           MonthCalendarMonth calMonth,
           MonthCalendarHeaderState state, double scaling);

        /// <summary>
        /// Draws a day in the month body of the calendar control.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="day">The <see cref="MonthCalendarDay"/> to draw.</param>
        public abstract void DrawDay(SKCanvas g, MonthCalendarDay day);

        /// <summary>
        /// Draws the day names.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="rect">The <see cref="Rectangle"/> to draw in.</param>
        public abstract void DrawDayHeader(SKCanvas g, Rectangle rect);

        /// <summary>
        /// Draws the footer.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="rect">The <see cref="Rectangle"/> to draw in.</param>
        /// <param name="active">true if the footer is in mouse over state; otherwise false.</param>
        public abstract void DrawFooter(SKCanvas g, Rectangle rect, bool active);

        /// <summary>
        /// Draws a single week header element.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="week">The <see cref="MonthCalendarWeek"/> to draw.</param>
        public abstract void DrawWeekHeaderItem(SKCanvas g, MonthCalendarWeek week);

        /// <summary>
        /// Draws the separator line between week header and month body.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> used to draw.</param>
        /// <param name="rect">The bounds of the week header.</param>
        public abstract void DrawWeekHeaderSeparator(SKCanvas g, Rectangle rect);

        #endregion

        #region internal methods

        /// <summary>
        /// Gets the gray scaled color of the specified <paramref name="baseColor"/>.
        /// </summary>
        /// <param name="enabled">true if the control is enabled; false otherwise.</param>
        /// <param name="baseColor">The base color.</param>
        /// <returns>The gray scaled color.</returns>
        internal static SKColor GetGrayColor(bool enabled, SKColor baseColor)
        {
            if (baseColor == SKColor.Empty || enabled)
            {
                return baseColor;
            }

            float lumi = .3F * baseColor.Red + .59F * baseColor.Green + .11F * baseColor.Blue;

            return new SKColor((byte)lumi, (byte)lumi, (byte)lumi);
        }

        /// <summary>
        /// Fills the specified <paramref name="rect"/> with the given colors.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="rect">The rectangle to fill.</param>
        /// <param name="start">The start color.</param>
        /// <param name="end">The end color.</param>
        /// <param name="mode">The fill mode.</param>
        internal void FillBackgroundInternal(
           SKCanvas g,
           Rectangle rect,
           SKColor start,
           SKColor end,
           LinearGradientMode? mode)
        {
            if (!Calendar.Enabled)
            {
                float lumiStart = .3F * start.Red + .59F * start.Green + .11F * start.Blue;
                float lumiEnd = .3F * end.Red + .59F * end.Green + .11F * end.Blue;

                if (start != SKColor.Empty)
                {
                    start = new SKColor((byte)lumiStart, (byte)lumiStart, (byte)lumiStart);
                }

                if (end != SKColor.Empty)
                {
                    end = new SKColor((byte)lumiEnd, (byte)lumiEnd, (byte)lumiEnd);
                }
            }

            FillBackground(g, rect, start, end, mode);
        }

        #endregion

        #endregion
    }
}