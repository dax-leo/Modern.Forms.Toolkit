using Modern.Forms.MonthCalendar.Helper;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection.Emit;
using Topten.RichTextKit;

namespace Modern.Forms.MonthCalendar.Renderer
{
    /// <summary>
    /// The MonthCalendar control renderer.
    /// </summary>
    public class MonthCalendarRenderer : MonthCalendarAbstractRenderer
    {
        #region fields

        /// <summary>
        /// The month calendar control.
        /// </summary>
        private readonly MonthCalendar monthCal;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendarRenderer"/> class.
        /// </summary>
        /// <param name="calendar">The corresponding <see cref="MonthCalendar"/>.</param>
        public MonthCalendarRenderer(MonthCalendar calendar)
           : base(calendar)
        {
            monthCal = calendar;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets the corresponding <see cref="MonthCalendar"/>.
        /// </summary>
        public MonthCalendar MonthCalendar
        {
            get { return monthCal; }
        }

        #endregion

        #region methods

        /// <summary>
        /// Draws the header of a <see cref="MonthCalendarMonth"/>.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="calMonth">The <see cref="MonthCalendarMonth"/> to draw the header for.</param>
        /// <param name="state">The <see cref="MonthCalendarHeaderState"/>.</param>
        public override void DrawMonthHeader(
           SKCanvas g,
           MonthCalendarMonth calMonth,
           MonthCalendarHeaderState state, 
           double scaling)
        {
            //if (calMonth == null || !CheckParams(g, calMonth.TitleBounds))
            //{
            //    return;
            //}

            // get title bounds
            Rectangle rect = calMonth.TitleBounds;

            MonthCalendarDate date = new MonthCalendarDate(monthCal.CultureCalendar, calMonth.Date);
            MonthCalendarDate firstVisible = new MonthCalendarDate(monthCal.CultureCalendar, calMonth.FirstVisibleDate);

            string month;
            int year;

            // gets the month name for the month the MonthCalendarMonth represents and the year string
            if (firstVisible.Era != date.Era)
            {
                month = monthCal.FormatProvider.GetMonthName(firstVisible.Year, firstVisible.Month);
                year = firstVisible.Year;
            }
            else
            {
                month = monthCal.FormatProvider.GetMonthName(date.Year, date.Month);
                year = date.Year;
            }

            string yearString = monthCal.UseNativeDigits
               ? DateMethods.GetNativeNumberString(year, monthCal.Culture.NumberFormat.NativeDigits, false)
               : year.ToString(CultureInfo.CurrentUICulture);

            // get used font
            SKFont headerFont = monthCal.HeaderFont;

            // create bold font
            SKFont boldFont = new SKFont();// (headerFont.FontFamily, headerFont.SizeInPoints, FontStyle.Bold);
            boldFont.Typeface = SKTypeface.Default;
            boldFont.Size = headerFont.Size;

            // measure sizes
            SKSize monthSize = TextMeasurer.MeasureText(month, SKTypeface.Default, (int)boldFont.Size);// g.MeasureString(month, boldFont);

            SKSize yearSize = TextMeasurer.MeasureText(yearString, SKTypeface.Default, (int)boldFont.Size);

            float maxHeight = Math.Max(monthSize.Height, yearSize.Height);

            // calculates the width and the starting position of the arrows
            int width = (int)monthSize.Width + (int)yearSize.Width + 7;
            int arrowLeftX = rect.X + 6;
            int arrowRightX = rect.Right - 6;
            int arrowY = rect.Y + rect.Height / 2 - 4;

            int x = Math.Max(0, rect.X + rect.Width / 2 + 1 - width / 2);
            int y = Math.Max(
               0,
               rect.Y + rect.Height / 2 + 1 - ((int)maxHeight + 1) / 2);

            // set the title month name bounds
            calMonth.TitleMonthBounds = new Rectangle(
               x,
               y,
               (int)monthSize.Width + 1,
               (int)maxHeight + 1);

            // set the title year bounds
            calMonth.TitleYearBounds = new Rectangle(
               x + calMonth.TitleMonthBounds.Width + 7,
               y,
               (int)yearSize.Width + 1,
               (int)maxHeight + 1);

            // generate points for the left and right arrow
            SKPoint[] arrowLeft = new[]
            {
            new SKPoint(arrowLeftX-1, arrowY + 4),
            new SKPoint(arrowLeftX + 4, arrowY),
            new SKPoint(arrowLeftX + 4, arrowY + 8),
            new SKPoint(arrowLeftX-1, arrowY + 4)
         };

            SKPoint[] arrowRight = new[]
            {
            new SKPoint(arrowRightX, arrowY + 4),
            new SKPoint(arrowRightX - 4, arrowY),
            new SKPoint(arrowRightX - 4, arrowY + 8),
            new SKPoint(arrowRightX, arrowY + 4)
         };

            // get color table
            MonthCalendarColorTable colorTable = ColorTable;

            // get brushes for normal, mouse over and selected state

            // get title month name and year bounds
            Rectangle monthRect = calMonth.TitleMonthBounds;
            Rectangle yearRect = calMonth.TitleYearBounds;

            // set used fonts
            SKFont monthFont = headerFont;
            SKFont yearFont = headerFont;

            // set used brushes
            //SolidBrush monthBrush = brush, yearBrush = brush;

            // adjust brush and font if year selected
            //if (state == MonthCalendarHeaderState.YearSelected)
            //{
            //    yearBrush = brushSelected;
            //    yearFont = boldFont;
            //    yearRect.Width += 4;
            //}
            //else if (state == MonthCalendarHeaderState.YearActive)
            //{
            //    // adjust brush if mouse over year
            //    yearBrush = brushOver;
            //}

            //// adjust brush and font if month name is selected
            //if (state == MonthCalendarHeaderState.MonthNameSelected)
            //{
            //    monthBrush = brushSelected;
            //    monthFont = boldFont;
            //    monthRect.Width += 4;
            //}
            //else if (state == MonthCalendarHeaderState.MonthNameActive)
            //{
            //    // adjust brush if mouse over month name
            //    monthBrush = brushOver;
            //}

            // draws the month name and year string                
            g.DrawText(month, monthFont.Typeface,
                                (int)(monthFont.Size),
                                monthRect,
                                colorTable.HeaderText,
                                Forms.ContentAlignment.MiddleCenter);

            g.DrawText(yearString, yearFont.Typeface,
                                (int)yearFont.Size,
                                yearRect,
                                colorTable.HeaderText,
                                Forms.ContentAlignment.MiddleCenter);

            boldFont.Dispose();

            // if left arrow has to be drawn
            if (calMonth.DrawLeftButton)
            {
                // get arrow color                
                SKColor arrowColor = GetGrayColor(monthCal.Enabled, colorTable.HeaderArrow);// : colorTable.HeaderActiveArrow;

                // set left arrow rect
                monthCal.SetLeftArrowRect(new Rectangle(rect.X, rect.Y, (int)(15 * scaling), (int)(rect.Height * scaling)));

                // draw left arrow
                using (SKPath path = new SKPath())
                {
                    path.AddPoly(arrowLeft);
                    g.DrawPath(path, new SKPaint() { Color = arrowColor });
                }
            }

            // if right arrow has to be drawn
            if (calMonth.DrawRightButton)
            {
                // get arrow color                
                SKColor arrowColor = GetGrayColor(monthCal.Enabled, colorTable.HeaderArrow); //: colorTable.HeaderActiveArrow;

                // set right arrow rect
                monthCal.SetRightArrowRect(new Rectangle(rect.Right - 15, rect.Y, 15, rect.Height));

                // draw arrow
                using (SKPath path = new SKPath())
                {
                    path.AddPoly(arrowRight);

                    //using (SolidBrush brush = new SolidBrush(arrowColor))
                    //{
                    //    g.FillPath(brush, path);
                    //}

                    //using (Pen p = new Pen(arrowColor))                    
                    g.DrawPath(path, new SKPaint() { Color = arrowColor });                    
                }
            }

            // draw seperator
            //g.DrawLine(rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1,
            //    new SKPaint() { Color = GetGrayColor(monthCal.Enabled, ColorTable.MonthSeparator) });
        }

        /// <summary>
        /// Draws a day in the month body of the calendar control.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="day">The <see cref="MonthCalendarDay"/> to draw.</param>
        public override void DrawDay(SKCanvas g, MonthCalendarDay day)
        {
            // get color table
            MonthCalendarColorTable colors = ColorTable;

            // get the bounds of the day
            Rectangle rect = day.Bounds;

            var boldDate = monthCal.BoldedDatesCollection.Find(d => d.Value.Date == day.Date.Date);

            // if day is selected or in mouse over state
            if (day.MouseOver)
            {
                FillBackground(
                   g,
                   rect,
                   colors.DayActiveGradientBegin,
                   colors.DayActiveGradientEnd,
                   colors.DayActiveGradientMode);
            }
            else if (day.Selected)
            {
                FillBackgroundInternal(
                   g,
                   rect,
                   colors.DaySelectedGradientBegin,
                   colors.DaySelectedGradientEnd,
                   colors.DaySelectedGradientMode);
            }
            else if (!boldDate.IsEmpty && boldDate.Category.BackColorStart != SKColors.Empty && boldDate.Category.BackColorStart != SKColors.Transparent)
            {
                FillBackground(
                   g,
                   rect,
                   boldDate.Category.BackColorStart,
                   SKColor.Empty,
                   //boldDate.Category.BackColorEnd = SKColor.Empty || boldDate.Category.BackColorEnd == SKColors.Transparent ? boldDate.Category.BackColorStart : boldDate.Category.BackColorEnd,
                   boldDate.Category.GradientMode);
            }

            // get bolded dates
            List<DateTime> boldedDates = monthCal.GetBoldedDates();

            bool bold = boldedDates.Contains(day.Date) || !boldDate.IsEmpty;

            // draw the day            

            SKColor textColor = bold ? boldDate.IsEmpty || boldDate.Category.ForeColor == SKColor.Empty || boldDate.Category.ForeColor == SKColors.Transparent ? colors.DayTextBold : boldDate.Category.ForeColor
               : day.Selected ? colors.DaySelectedText
               : day.MouseOver ? colors.DayActiveText
               : day.TrailingDate ? colors.DayTrailingText
               : colors.DayText;

            // adjust width
            Rectangle textRect = day.Bounds;
            textRect.Width -= 2;

            // determine if to use bold font
            bool useBoldFont = day.Date == DateTime.Today || bold;

            var calDate = new MonthCalendarDate(monthCal.CultureCalendar, day.Date);

            string dayString = monthCal.UseNativeDigits
                                  ? DateMethods.GetNativeNumberString(calDate.Day, monthCal.Culture.NumberFormat.NativeDigits, false)
                                  : calDate.Day.ToString(monthCal.Culture);
            
            if (monthCal.Enabled)
            {                
                // if today, text is bolded
                if (day.Date == DateTime.Today.Date)
                {                    
                    g.DrawText(dayString, Theme.UIFontBold,
                     (int)(monthCal.Font.Size * (monthCal.Scaling >= 1.75 ? 1.0 : 1.15)),
                     textRect,
                     textColor,
                     Forms.ContentAlignment.MiddleCenter);
                }
                else
                {
                    g.DrawText(dayString, Theme.UIFont,
                     (int)(monthCal.Font.Size * (monthCal.Scaling >= 1.75 ? 1.0 : 1.15)),
                     textRect,
                     textColor,
                     Forms.ContentAlignment.MiddleCenter);
                }
            }
            else
            {
                //ControlPaint.DrawStringDisabled(
                //   g,
                //   dayString,
                //   (useBoldFont ? font : this.monthCal.Font),
                //   Color.Transparent,
                //   textRect,
                //   format);
            }

            // if today, draw border
            //if (day.Date == DateTime.Today)
            //{
            //    //rect.Height -= 2;
            //    //rect.Width -= 1;
            //    //SKColor borderColor = day.Selected ? colors.DaySelectedTodayCircleBorder
            //    //   : day.MouseOver ? colors.DayActiveTodayCircleBorder : colors.DayTodayCircleBorder;

            //    //g.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, borderColor);
            //    //g.DrawCircle(rect.X+1 + rect.Width / 2, rect.Y+2 + rect.Height / 2, rect.Width / 2-5, borderColor);
            //}
        }

        /// <summary>
        /// Draws the day header.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="rect">The <see cref="Rectangle"/> to draw in.</param>
        public override void DrawDayHeader(SKCanvas g, Rectangle rect)
        {
            // get day width
            int dayWidth = monthCal.DaySize.Width;

            // get abbreviated day names
            List<string> names = new List<string>(DateMethods.GetDayNames(monthCal.FormatProvider, monthCal.UseShortestDayNames ? 2 : 1));

            // get bounds for a single element
            Rectangle dayRect = rect;
            dayRect.Width = dayWidth;

            names.ForEach(day =>
               {
                   if (monthCal.Enabled)
                   {
                       g.DrawText(day, monthCal.DayHeaderFont.Typeface,
                           (int)monthCal.DayHeaderFont.Size,
                           dayRect, ColorTable.DayHeaderText, Forms.ContentAlignment.MiddleCenter);
                   }
                   else
                   {
                       //  ControlPaint.DrawStringDisabled(
                       //g,
                       //day,
                       //this.monthCal.DayHeaderFont,
                       //Color.Transparent,
                       //dayRect,
                       //format);
                   }

                   dayRect.X += dayWidth;
               });

            // draw separator line            
            g.DrawLine(rect.X, rect.Bottom - 1, rect.Right - 1, rect.Bottom - 1,
                new SKPaint() { Color = GetGrayColor(monthCal.Enabled, ColorTable.MonthSeparator) });
        }

        /// <summary>
        /// Draws the footer.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="rect">The <see cref="Rectangle"/> to draw in.</param>
        /// <param name="active">true if the footer is in mouse over state; otherwise false.</param>
        public override void DrawFooter(SKCanvas g, Rectangle rect, bool active)
        {
            string dateString = new MonthCalendarDate(monthCal.CultureCalendar, DateTime.Today).ToString(
               null,
               null,
               monthCal.FormatProvider,
               monthCal.UseNativeDigits ? monthCal.Culture.NumberFormat.NativeDigits : null);

            // get date size
            SKSize dateSize = TextMeasurer.MeasureText(dateString, SKTypeface.Default, (int)monthCal.FooterFont.Size);// g.MeasureString(dateString, this.monthCal.FooterFont);

            // get today rectangle and adjust position
            Rectangle todayRect = rect;
            todayRect.X += 2;

            // adjust bounds of today rectangle
            todayRect.Y = rect.Y + rect.Height / 2 - 5;
            todayRect.Height = 11;
            todayRect.Width = 18;

            // draw the today rectangle            
            g.DrawRectangle(todayRect, ColorTable.FooterTodayCircleBorder);

            // get top position to draw the text at
            int y = rect.Y + rect.Height / 2 - (int)dateSize.Height / 2;

            Rectangle dateRect;

            {
                // get date bounds
                dateRect = new Rectangle(
                   todayRect.Right + 2,
                   y,
                   rect.Width - todayRect.Width,
                   (int)dateSize.Height + 1);
            }

            g.DrawText(dateString, monthCal.FooterFont.Typeface,
                                   (int)monthCal.FooterFont.Size,
                                   dateRect,
                                   active ? ColorTable.FooterActiveText : GetGrayColor(monthCal.Enabled, ColorTable.FooterText),
                                   Forms.ContentAlignment.MiddleLeft);
        }

        /// <summary>
        /// Draws a single week header element.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used to draw.</param>
        /// <param name="week">The <see cref="MonthCalendarWeek"/> to draw.</param>
        public override void DrawWeekHeaderItem(SKCanvas g, MonthCalendarWeek week)
        {
            var weekString = monthCal.UseNativeDigits
               ? DateMethods.GetNativeNumberString(week.WeekNumber, monthCal.Culture.NumberFormat.NativeDigits, false)
               : week.WeekNumber.ToString(CultureInfo.CurrentUICulture);

            // draw week header element            
            //    // set alignment

            // draw string            
            if (monthCal.Enabled)
            {
                g.DrawText(weekString, monthCal.Font.Typeface,
                                  (int)monthCal.Font.Size,
                                  week.Bounds,
                                  ColorTable.WeekHeaderText,
                                  Forms.ContentAlignment.MiddleCenter);
            }
            else
            {
                //ControlPaint.DrawStringDisabled(
                //   g,
                //   weekString,
                //   this.monthCal.Font,
                //   Color.Transparent,
                //   week.Bounds,
                //   format);
            }
        }

        /// <summary>
        /// Draws the separator line between week header and month body.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> used to draw.</param>
        /// <param name="rect">The bounds of the week header.</param>
        public override void DrawWeekHeaderSeparator(SKCanvas g, Rectangle rect)
        {
            // draw separator line            
            g.DrawLine(rect.Right - 1, rect.Y - 1, rect.Right - 1, rect.Bottom - 1,
                new SKPaint() { Color = GetGrayColor(monthCal.Enabled, ColorTable.MonthSeparator) });
        }

        #endregion
    }
}