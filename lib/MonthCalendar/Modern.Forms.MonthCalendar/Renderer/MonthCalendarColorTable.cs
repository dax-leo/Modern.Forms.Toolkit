namespace Modern.Forms.MonthCalendar.Renderer
{
    using Modern.Forms.MonthCalendar;
    using SkiaSharp;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// Class that hold mainly SKColor information for the <see cref="MonthCalendar"/> renderer.
    /// </summary>    
    public class MonthCalendarColorTable : IXmlSerializable
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendarColorTable"/> class.
        /// </summary>
        public MonthCalendarColorTable()
        {
            SetTheme(Theme.Style);
        }

        #endregion

        #region properties

        #region control properties

        /// <summary>
        /// Gets or sets the start SKColor of the background of the <see cref="MonthCalendar"/> control.
        /// </summary>
        [DefaultValue(typeof(SKColor), "White")]
        [Description("Start SKColor of the month calendar background.")]
        public virtual SKColor BackgroundGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the end SKColor of the background of the <see cref="MonthCalendar"/> control.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("End SKColor of the month calendar background.")]
        public virtual SKColor BackgroundGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the fill mode of the background of the <see cref="MonthCalendar"/> control.
        /// </summary>
        [DefaultValue(null)]
        [Description("Fill mode of the month calendar background.")]
        public LinearGradientMode? BackgroundGradientMode { get; set; }

        /// <summary>
        /// Gets or sets border color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "154, 198, 255")]
        [Description("The SKColor of the border for the month calendar.")]
        public virtual SKColor Border { get; set; }

        /// <summary>
        /// Gets or sets the separator SKColor of the month body and week header and/or day header.
        /// </summary>
        [DefaultValue(typeof(SKColor), "154, 198, 255")]
        [Description("The SKColor of the separator line between month body and/or day header.")]
        public virtual SKColor MonthSeparator { get; set; }

        #endregion

        #region month header properties

        /// <summary>
        /// Gets or sets the start SKColor of the header background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "191, 219, 255")]
        [Description("Start SKColor of the header background.")]
        public virtual SKColor HeaderGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the end SKColor of the header background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("End SKColor of the header background.")]
        public virtual SKColor HeaderGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the fill mode of the header background.
        /// </summary>
        [DefaultValue(null)]
        [Description("Fill mode of the header background.")]
        public LinearGradientMode? HeaderGradientMode { get; set; }

        /// <summary>
        /// Gets or sets the active start SKColor of the header background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "191, 219, 255")]
        [Description("Active start SKColor of the header background.")]
        public virtual SKColor HeaderActiveGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the active end SKColor of the header background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("Active end SKColor of the header background.")]
        public virtual SKColor HeaderActiveGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the active fill mode of the header background.
        /// </summary>
        [DefaultValue(null)]
        [Description("Active fill mode of the header background.")]
        public LinearGradientMode? HeaderActiveGradientMode { get; set; }

        /// <summary>
        /// Gets or sets the SKColor of the header text.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Black")]
        [Description("Header text color.")]
        public virtual SKColor HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the SKColor of the active header text.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Navy")]
        [Description("Active text SKColor of the header.")]
        public virtual SKColor HeaderActiveText { get; set; }

        /// <summary>
        /// Gets or sets the SKColor of the selected header text.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Navy")]
        [Description("Selected text SKColor of the header.")]
        public virtual SKColor HeaderSelectedText { get; set; }

        /// <summary>
        /// Gets or sets the header arrow color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "111, 157, 217")]
        [Description("Color of the header arrows.")]
        public virtual SKColor HeaderArrow { get; set; }

        /// <summary>
        /// Gets or sets the active header arrow color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Navy")]
        [Description("Active SKColor of the header arrows.")]
        public virtual SKColor HeaderActiveArrow { get; set; }

        #endregion

        #region footer properties

        /// <summary>
        /// Gets or sets the start SKColor of the footer background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Transparent")]
        [Description("Start SKColor of the footer background.")]
        public virtual SKColor FooterGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the end SKColor of the footer background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("End SKColor of the footer background.")]
        public virtual SKColor FooterGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the fill mode of the footer background.
        /// </summary>
        [DefaultValue(null)]
        [Description("Fill mode of the footer background.")]
        public LinearGradientMode? FooterGradientMode { get; set; }

        /// <summary>
        /// Gets or sets the active start SKColor of the footer background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("Active start SKColor of the footer background.")]
        public virtual SKColor FooterActiveGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the active end SKColor of the footer background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("Active end SKColor of the footer background.")]
        public virtual SKColor FooterActiveGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the fill mode of the active footer background.
        /// </summary>
        [DefaultValue(null)]
        [Description("Active fill mode of the footer background.")]
        public LinearGradientMode? FooterActiveGradientMode { get; set; }

        /// <summary>
        /// Gets or sets the SKColor of the footer text.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Black")]
        [Description("Footer text color.")]
        public virtual SKColor FooterText { get; set; }

        /// <summary>
        /// Gets or sets the SKColor of the active footer text.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Black")]
        [Description("Active SKColor of the footer text.")]
        public virtual SKColor FooterActiveText { get; set; }

        /// <summary>
        /// Gets or sets the today circle border SKColor of the footer.
        /// </summary>
        [DefaultValue(typeof(SKColor), "187, 85, 3")]
        [Description("Border SKColor of the today circle in the footer.")]
        public virtual SKColor FooterTodayCircleBorder { get; set; }

        #endregion

        #region month body properties

        /// <summary>
        /// Gets or sets the start SKColor of the month body background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Transparent")]
        [Description("Start SKColor of the month body background.")]
        public virtual SKColor MonthBodyGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the end SKColor of the month body background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("End SKColor of the month body background.")]
        public virtual SKColor MonthBodyGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the fill mode of the month body background.
        /// </summary>
        [DefaultValue(null)]
        [Description("Fill mode of the month body background.")]
        public LinearGradientMode? MonthBodyGradientMode { get; set; }

        #endregion

        #region day properties

        /// <summary>
        /// Gets or sets the active start SKColor of the day background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "DarkOrange")]
        [Description("Active start SKColor of the day background.")]
        public virtual SKColor DayActiveGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the active end SKColor of the day background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("Active end SKColor of the day background.")]
        public virtual SKColor DayActiveGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the fill mode of the active day background.
        /// </summary>
        [DefaultValue(null)]
        [Description("Active fill mode of the day background.")]
        public LinearGradientMode? DayActiveGradientMode { get; set; }

        /// <summary>
        /// Gets or sets the selected start SKColor of the day background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "251, 200, 79")]
        [Description("Start SKColor of selected day background.")]
        public virtual SKColor DaySelectedGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the selected end SKColor of the day background.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("End SKColor of selected day background.")]
        public virtual SKColor DaySelectedGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the fill mode of the selected day background.
        /// </summary>
        [DefaultValue(null)]
        [Description("Fill mode of selected day background.")]
        public LinearGradientMode? DaySelectedGradientMode { get; set; }

        /// <summary>
        /// Gets or sets the SKColor of the day text.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Black")]
        [Description("The SKColor of the day text.")]
        public virtual SKColor DayText { get; set; }

        /// <summary>
        /// Gets or sets the text SKColor for bolded dates.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Black")]
        [Description("The text SKColor for bolded dates.")]
        public virtual SKColor DayTextBold { get; set; }

        /// <summary>
        /// Gets or sets the active SKColor of the day text.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Black")]
        [Description("The active SKColor of the day text.")]
        public virtual SKColor DayActiveText { get; set; }

        /// <summary>
        /// Gets or sets the selected SKColor of the day text.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Black")]
        [Description("The selection SKColor of the day text.")]
        public virtual SKColor DaySelectedText { get; set; }

        /// <summary>
        /// Gets or sets the trailing SKColor of the day text.
        /// </summary>
        [DefaultValue(typeof(SKColor), "179, 179, 179")]
        [Description("The trailing SKColor of the day text.")]
        public virtual SKColor DayTrailingText { get; set; }

        /// <summary>
        /// Gets or sets the circle border color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "187, 85, 3")]
        [Description("Color of the border of the today circle.")]
        public virtual SKColor DayTodayCircleBorder { get; set; }

        /// <summary>
        /// Gets or sets the active circle border color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "187, 85, 3")]
        [Description("The SKColor of the border of the active today circle.")]
        public virtual SKColor DayActiveTodayCircleBorder { get; set; }

        /// <summary>
        /// Gets or sets the selected circle border color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "187, 85, 3")]
        [Description("The selection SKColor of the border of the today circle.")]
        public virtual SKColor DaySelectedTodayCircleBorder { get; set; }

        #endregion

        #region day header properties

        /// <summary>
        /// Gets or sets the day header background start color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Transparent")]
        [Description("Start SKColor of the day header background.")]
        public virtual SKColor DayHeaderGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the day header background end color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("End SKColor of the day header background.")]
        public virtual SKColor DayHeaderGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the fill mode of the day header background.
        /// </summary>
        [DefaultValue(null)]
        [Description("Fill mode of the day header background.")]
        public LinearGradientMode? DayHeaderGradientMode { get; set; }

        /// <summary>
        /// Gets or sets the text SKColor of the day header.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Black")]
        [Description("The text SKColor of the day header.")]
        public virtual SKColor DayHeaderText { get; set; }

        #endregion

        #region week header properties

        /// <summary>
        /// Gets or sets the week header background start color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "Transparent")]
        [Description("Start SKColor of the week header background.")]
        public virtual SKColor WeekHeaderGradientBegin { get; set; }

        /// <summary>
        /// Gets or sets the week header background end color.
        /// </summary>
        [DefaultValue(typeof(SKColor), "")]
        [Description("End SKColor of the week header background.")]
        public virtual SKColor WeekHeaderGradientEnd { get; set; }

        /// <summary>
        /// Gets or sets the fill mode of the week header background.
        /// </summary>
        [DefaultValue(null)]
        [Description("Fill mode of the week header background.")]
        public LinearGradientMode? WeekHeaderGradientMode { get; set; }

        /// <summary>
        /// Gets or sets the text SKColor of the week header.
        /// </summary>
        [DefaultValue(typeof(SKColor), "179, 179, 179")]
        [Description("Text SKColor of the week header.")]
        public virtual SKColor WeekHeaderText { get; set; }

        #endregion

        #endregion

        #region methods

        public void SetTheme(ThemeStyle style)
        {
            switch (style)
            {
                case ThemeStyle.Dark: SetDarkTheme(); break;
                case ThemeStyle.Light: SetLightTheme(); break;
            }
        }

        private void SetDarkTheme()
        {
            BackgroundGradientBegin = Theme.FormBackgroundColor;// SKColors.White;
            Border = Theme.DisabledTextColor;//new SKColor(154, 198, 255);
            MonthSeparator = SKColors.DimGray;// new SKColor(154, 198, 255);
            HeaderGradientBegin = new SKColor(68, 68, 68); //Theme.ItemSelectedColor;// new SKColor(191, 219, 255);
            HeaderActiveGradientBegin = new SKColor(68, 68, 68);
            HeaderText = SKColors.LightGray;//Theme.PrimaryTextColor;//SKColors.Black;
            HeaderActiveText = SKColors.Navy;
            HeaderSelectedText = SKColors.Navy;
            HeaderArrow = SKColors.DarkGray;// new SKColor(111, 157, 217);
            HeaderActiveArrow = SKColors.Navy;
            FooterGradientBegin = SKColors.Transparent;
            FooterText = SKColors.DarkGray;// Theme.PrimaryTextColor;//SKColors.Black;
            FooterActiveText = Theme.PrimaryTextColor;//SKColors.Black;
            FooterTodayCircleBorder = new SKColor(187, 85, 3);
            MonthBodyGradientBegin = SKColors.Transparent;
            DayActiveGradientBegin = SKColors.DarkOrange;
            DaySelectedGradientBegin = new SKColor(251, 200, 79);
            DayText = SKColors.DarkGray;// Theme.PrimaryTextColor;// SKColors.Black;
            DayTextBold = Theme.PrimaryTextColor;//SKColors.Black;
            DayActiveText = Theme.PrimaryTextColor;//SKColors.Black;
            DaySelectedText = SKColors.Black;
            DayTrailingText = SKColors.DimGray;// new SKColor(179, 179, 179);
            DayTodayCircleBorder = new SKColor(187, 85, 3);
            DayActiveTodayCircleBorder = new SKColor(187, 85, 3);
            DaySelectedTodayCircleBorder = new SKColor(187, 85, 3);
            DayHeaderGradientBegin = SKColors.Transparent;
            DayHeaderText = SKColors.DarkGray; //Theme.PrimaryTextColor;//SKColors.Black;
            WeekHeaderGradientBegin = SKColors.Transparent;
            WeekHeaderText = SKColors.DimGray; //new SKColor(179, 179, 179);            
        }

        private void SetLightTheme()
        {
            BackgroundGradientBegin = SKColors.WhiteSmoke; //Theme.FormBackgroundColor;// SKColors.White;
            Border = Theme.ItemSelectedColor;//new SKColor(154, 198, 255);
            MonthSeparator = SKColors.LightGray;// new SKColor(154, 198, 255);
            HeaderGradientBegin = SKColors.White;// new SKColor(178, 197, 221);
            HeaderActiveGradientBegin = SKColors.White;//new SKColor(178, 197, 221);
            HeaderText = Theme.PrimaryTextColor;//SKColors.Black;
            HeaderActiveText = SKColors.Navy;
            HeaderSelectedText = SKColors.Navy;
            HeaderArrow = SKColors.Black;//  new SKColor(111, 157, 217);
            HeaderActiveArrow = SKColors.Navy;
            FooterGradientBegin = SKColors.Transparent;
            FooterText = Theme.PrimaryTextColor;//SKColors.Black;
            FooterActiveText = Theme.PrimaryTextColor;//SKColors.Black;
            FooterTodayCircleBorder = new SKColor(187, 85, 3);
            MonthBodyGradientBegin = SKColors.Transparent;
            DayActiveGradientBegin = SKColors.DarkOrange;
            DaySelectedGradientBegin = Theme.PrimaryColor;// new SKColor(251, 200, 79);
            DayText = Theme.PrimaryTextColor;// SKColors.Black;
            DayTextBold = Theme.PrimaryTextColor;//SKColors.Black;
            DayActiveText = Theme.PrimaryTextColor;//SKColors.Black;
            DaySelectedText = SKColors.White;// Theme.PrimaryTextColor;//SKColors.Black;
            DayTrailingText = SKColors.DarkGray; //new SKColor(179, 179, 179);
            DayTodayCircleBorder = Theme.PrimaryColor;//new SKColor(187, 85, 3);
            DayActiveTodayCircleBorder = Theme.PrimaryColor;// new SKColor(187, 85, 3);
            DaySelectedTodayCircleBorder = Theme.PrimaryColor;// new SKColor(187, 85, 3);
            DayHeaderGradientBegin = SKColors.Transparent;
            DayHeaderText = Theme.PrimaryTextColor;//SKColors.Black;
            WeekHeaderGradientBegin = SKColors.Transparent;
            WeekHeaderText = SKColors.DarkGray; //new SKColor(179, 179, 179);
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the <see cref="T:System.Xml.Serialization.IXmlSerializable"/> 
        /// interface, you should return <c>null</c> (<c>Nothing</c> in Visual Basic) from this method, and instead, if specifying 
        /// a custom schema is required, apply the XmlSchemaProviderAttribute to the class.
        /// </summary>
        /// <returns>An <see cref="XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="WriteXml"/> method 
        /// and consumed by the <see cref="ReadXml"/> method.</returns>
        public virtual XmlSchema GetSchema()
        {
            return new XmlSchema();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/>-stream from which the object is deserialized.</param>
        public virtual void ReadXml(XmlReader reader)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            if (doc.GetElementsByTagName("BackgroundGradientBegin").Count > 0)
            {
                BackgroundGradientBegin = GetColorFromString(doc.GetElementsByTagName("BackgroundGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("BackgroundGradientEnd").Count > 0)
            {
                BackgroundGradientEnd = GetColorFromString(doc.GetElementsByTagName("BackgroundGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("BackgroundGradientMode").Count > 0)
            {
                BackgroundGradientMode = GetModeFromString(doc.GetElementsByTagName("BackgroundGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("Border").Count > 0)
            {
                Border = GetColorFromString(doc.GetElementsByTagName("Border")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayActiveGradientBegin").Count > 0)
            {
                DayActiveGradientBegin = GetColorFromString(doc.GetElementsByTagName("DayActiveGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayActiveGradientEnd").Count > 0)
            {
                DayActiveGradientEnd = GetColorFromString(doc.GetElementsByTagName("DayActiveGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayActiveGradientMode").Count > 0)
            {
                DayActiveGradientMode = GetModeFromString(doc.GetElementsByTagName("DayActiveGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayActiveText").Count > 0)
            {
                DayActiveText = GetColorFromString(doc.GetElementsByTagName("DayActiveText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayActiveTodayCircleBorder").Count > 0)
            {
                DayActiveTodayCircleBorder = GetColorFromString(doc.GetElementsByTagName("DayActiveTodayCircleBorder")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayHeaderGradientBegin").Count > 0)
            {
                DayHeaderGradientBegin = GetColorFromString(doc.GetElementsByTagName("DayHeaderGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayHeaderGradientEnd").Count > 0)
            {
                DayHeaderGradientEnd = GetColorFromString(doc.GetElementsByTagName("DayHeaderGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayHeaderGradientMode").Count > 0)
            {
                DayHeaderGradientMode = GetModeFromString(doc.GetElementsByTagName("DayHeaderGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayHeaderText").Count > 0)
            {
                DayHeaderText = GetColorFromString(doc.GetElementsByTagName("DayHeaderText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DaySelectedGradientBegin").Count > 0)
            {
                DaySelectedGradientBegin = GetColorFromString(doc.GetElementsByTagName("DaySelectedGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DaySelectedGradientEnd").Count > 0)
            {
                DaySelectedGradientEnd = GetColorFromString(doc.GetElementsByTagName("DaySelectedGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DaySelectedGradientMode").Count > 0)
            {
                DaySelectedGradientMode = GetModeFromString(doc.GetElementsByTagName("DaySelectedGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DaySelectedText").Count > 0)
            {
                DaySelectedText = GetColorFromString(doc.GetElementsByTagName("DaySelectedText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DaySelectedTodayCircleBorder").Count > 0)
            {
                DaySelectedTodayCircleBorder = GetColorFromString(doc.GetElementsByTagName("DaySelectedTodayCircleBorder")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayText").Count > 0)
            {
                DayText = GetColorFromString(doc.GetElementsByTagName("DayText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayTextBold").Count > 0)
            {
                DayTextBold = GetColorFromString(doc.GetElementsByTagName("DayTextBold")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayTodayCircleBorder").Count > 0)
            {
                DayTodayCircleBorder = GetColorFromString(doc.GetElementsByTagName("DayTodayCircleBorder")[0].InnerText);
            }

            if (doc.GetElementsByTagName("DayTrailingText").Count > 0)
            {
                DayTrailingText = GetColorFromString(doc.GetElementsByTagName("DayTrailingText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("FooterActiveGradientBegin").Count > 0)
            {
                FooterActiveGradientBegin = GetColorFromString(doc.GetElementsByTagName("FooterActiveGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("FooterActiveGradientEnd").Count > 0)
            {
                FooterActiveGradientEnd = GetColorFromString(doc.GetElementsByTagName("FooterActiveGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("FooterActiveGradientMode").Count > 0)
            {
                FooterActiveGradientMode = GetModeFromString(doc.GetElementsByTagName("FooterActiveGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("FooterActiveText").Count > 0)
            {
                FooterActiveText = GetColorFromString(doc.GetElementsByTagName("FooterActiveText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("FooterGradientBegin").Count > 0)
            {
                FooterGradientBegin = GetColorFromString(doc.GetElementsByTagName("FooterGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("FooterGradientEnd").Count > 0)
            {
                FooterGradientEnd = GetColorFromString(doc.GetElementsByTagName("FooterGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("FooterGradientMode").Count > 0)
            {
                FooterGradientMode = GetModeFromString(doc.GetElementsByTagName("FooterGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("FooterText").Count > 0)
            {
                FooterText = GetColorFromString(doc.GetElementsByTagName("FooterText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("FooterTodayCircleBorder").Count > 0)
            {
                FooterTodayCircleBorder = GetColorFromString(doc.GetElementsByTagName("FooterTodayCircleBorder")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderActiveArrow").Count > 0)
            {
                HeaderActiveArrow = GetColorFromString(doc.GetElementsByTagName("HeaderActiveArrow")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderActiveGradientBegin").Count > 0)
            {
                HeaderActiveGradientBegin = GetColorFromString(doc.GetElementsByTagName("HeaderActiveGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderActiveGradientEnd").Count > 0)
            {
                HeaderActiveGradientEnd = GetColorFromString(doc.GetElementsByTagName("HeaderActiveGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderActiveGradientMode").Count > 0)
            {
                HeaderActiveGradientMode = GetModeFromString(doc.GetElementsByTagName("HeaderActiveGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderActiveText").Count > 0)
            {
                HeaderActiveText = GetColorFromString(doc.GetElementsByTagName("HeaderActiveText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderArrow").Count > 0)
            {
                HeaderArrow = GetColorFromString(doc.GetElementsByTagName("HeaderArrow")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderGradientBegin").Count > 0)
            {
                HeaderGradientBegin = GetColorFromString(doc.GetElementsByTagName("HeaderGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderGradientEnd").Count > 0)
            {
                HeaderGradientEnd = GetColorFromString(doc.GetElementsByTagName("HeaderGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderGradientMode").Count > 0)
            {
                HeaderGradientMode = GetModeFromString(doc.GetElementsByTagName("HeaderGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderSelectedText").Count > 0)
            {
                HeaderSelectedText = GetColorFromString(doc.GetElementsByTagName("HeaderSelectedText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("HeaderText").Count > 0)
            {
                HeaderText = GetColorFromString(doc.GetElementsByTagName("HeaderText")[0].InnerText);
            }

            if (doc.GetElementsByTagName("MonthBodyGradientBegin").Count > 0)
            {
                MonthBodyGradientBegin = GetColorFromString(doc.GetElementsByTagName("MonthBodyGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("MonthBodyGradientEnd").Count > 0)
            {
                MonthBodyGradientEnd = GetColorFromString(doc.GetElementsByTagName("MonthBodyGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("MonthBodyGradientMode").Count > 0)
            {
                MonthBodyGradientMode = GetModeFromString(doc.GetElementsByTagName("MonthBodyGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("MonthSeparator").Count > 0)
            {
                MonthSeparator = GetColorFromString(doc.GetElementsByTagName("MonthSeparator")[0].InnerText);
            }

            if (doc.GetElementsByTagName("WeekHeaderGradientBegin").Count > 0)
            {
                WeekHeaderGradientBegin = GetColorFromString(doc.GetElementsByTagName("WeekHeaderGradientBegin")[0].InnerText);
            }

            if (doc.GetElementsByTagName("WeekHeaderGradientEnd").Count > 0)
            {
                WeekHeaderGradientEnd = GetColorFromString(doc.GetElementsByTagName("WeekHeaderGradientEnd")[0].InnerText);
            }

            if (doc.GetElementsByTagName("WeekHeaderGradientMode").Count > 0)
            {
                WeekHeaderGradientMode = GetModeFromString(doc.GetElementsByTagName("WeekHeaderGradientMode")[0].InnerText);
            }

            if (doc.GetElementsByTagName("WeekHeaderText").Count > 0)
            {
                WeekHeaderText = GetColorFromString(doc.GetElementsByTagName("WeekHeaderText")[0].InnerText);
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/>-stream to which the object is serialized.</param>
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("BackgroundGradientBegin", GetColorString(BackgroundGradientBegin));
            writer.WriteElementString("BackgroundGradientEnd", GetColorString(BackgroundGradientEnd));
            writer.WriteElementString("BackgroundGradientMode", GetModeString(BackgroundGradientMode));
            writer.WriteElementString("Border", GetColorString(Border));
            writer.WriteElementString("DayActiveGradientBegin", GetColorString(DayActiveGradientBegin));
            writer.WriteElementString("DayActiveGradientEnd", GetColorString(DayActiveGradientEnd));
            writer.WriteElementString("DayActiveGradientMode", GetModeString(DayActiveGradientMode));
            writer.WriteElementString("DayActiveText", GetColorString(DayActiveText));
            writer.WriteElementString("DayActiveTodayCircleBorder", GetColorString(DayActiveTodayCircleBorder));
            writer.WriteElementString("DayHeaderGradientBegin", GetColorString(DayHeaderGradientBegin));
            writer.WriteElementString("DayHeaderGradientEnd", GetColorString(DayHeaderGradientEnd));
            writer.WriteElementString("DayHeaderGradientMode", GetModeString(DayHeaderGradientMode));
            writer.WriteElementString("DayHeaderText", GetColorString(DayHeaderText));
            writer.WriteElementString("DaySelectedGradientBegin", GetColorString(DaySelectedGradientBegin));
            writer.WriteElementString("DaySelectedGradientEnd", GetColorString(DaySelectedGradientEnd));
            writer.WriteElementString("DaySelectedGradientMode", GetModeString(DaySelectedGradientMode));
            writer.WriteElementString("DaySelectedText", GetColorString(DaySelectedText));
            writer.WriteElementString("DaySelectedTodayCircleBorder", GetColorString(DaySelectedTodayCircleBorder));
            writer.WriteElementString("DayText", GetColorString(DayText));
            writer.WriteElementString("DayTextBold", GetColorString(DayTextBold));
            writer.WriteElementString("DayTodayCircleBorder", GetColorString(DayTodayCircleBorder));
            writer.WriteElementString("DayTrailingText", GetColorString(DayTrailingText));
            writer.WriteElementString("FooterActiveGradientBegin", GetColorString(FooterActiveGradientBegin));
            writer.WriteElementString("FooterActiveGradientEnd", GetColorString(FooterActiveGradientEnd));
            writer.WriteElementString("FooterActiveGradientMode", GetModeString(FooterActiveGradientMode));
            writer.WriteElementString("FooterActiveText", GetColorString(FooterActiveText));
            writer.WriteElementString("FooterGradientBegin", GetColorString(FooterGradientBegin));
            writer.WriteElementString("FooterGradientEnd", GetColorString(FooterGradientEnd));
            writer.WriteElementString("FooterGradientMode", GetModeString(FooterGradientMode));
            writer.WriteElementString("FooterText", GetColorString(FooterText));
            writer.WriteElementString("FooterTodayCircleBorder", GetColorString(FooterTodayCircleBorder));
            writer.WriteElementString("HeaderActiveArrow", GetColorString(HeaderActiveArrow));
            writer.WriteElementString("HeaderActiveGradientBegin", GetColorString(HeaderActiveGradientBegin));
            writer.WriteElementString("HeaderActiveGradientEnd", GetColorString(HeaderActiveGradientEnd));
            writer.WriteElementString("HeaderActiveGradientMode", GetModeString(HeaderActiveGradientMode));
            writer.WriteElementString("HeaderActiveText", GetColorString(HeaderActiveText));
            writer.WriteElementString("HeaderArrow", GetColorString(HeaderArrow));
            writer.WriteElementString("HeaderGradientBegin", GetColorString(HeaderGradientBegin));
            writer.WriteElementString("HeaderGradientEnd", GetColorString(HeaderGradientEnd));
            writer.WriteElementString("HeaderGradientMode", GetModeString(HeaderGradientMode));
            writer.WriteElementString("HeaderSelectedText", GetColorString(HeaderSelectedText));
            writer.WriteElementString("HeaderText", GetColorString(HeaderText));
            writer.WriteElementString("MonthBodyGradientBegin", GetColorString(MonthBodyGradientBegin));
            writer.WriteElementString("MonthBodyGradientEnd", GetColorString(MonthBodyGradientEnd));
            writer.WriteElementString("MonthBodyGradientMode", GetModeString(MonthBodyGradientMode));
            writer.WriteElementString("MonthSeparator", GetColorString(MonthSeparator));
            writer.WriteElementString("WeekHeaderGradientBegin", GetColorString(WeekHeaderGradientBegin));
            writer.WriteElementString("WeekHeaderGradientEnd", GetColorString(WeekHeaderGradientEnd));
            writer.WriteElementString("WeekHeaderGradientMode", GetModeString(WeekHeaderGradientMode));
            writer.WriteElementString("WeekHeaderText", GetColorString(WeekHeaderText));
        }

        /// <summary>
        /// Converts the specified <see cref="Color"/> to an string representation.
        /// </summary>
        /// <param name="c">The <see cref="Color"/> value to convert.</param>
        /// <returns>A <see cref="string"/> representation of the SKColor specified by <paramref name="c"/>.</returns>
        private static string GetColorString(SKColor c)
        {
            //if (c.IsNamedColor || c.IsKnownColor || c.IsSystemColor)
            //{
            //   return c.Name;
            //}

            if (c == SKColor.Empty)
            {
                return string.Empty;
            }

            return c.Alpha + "," + c.Red + "," + c.Green + "," + c.Blue;
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> to a SKColor value.
        /// </summary>
        /// <param name="c">The <see cref="string"/> which holds the SKColor name or the ARGB values.</param>
        /// <returns>A <see cref="Color"/> value.</returns>
        private static SKColor GetColorFromString(string c)
        {
            if (c.IndexOf(',') > 0)
            {
                string[] parts = c.Split(',');

                //return Color.FromArgb(Convert.ToInt32(parts[0].Trim()), Convert.ToInt32(parts[1].Trim()),
                //                      Convert.ToInt32(parts[2].Trim()),
                //                      Convert.ToInt32(parts[3].Trim()));
                return new SKColor((byte)Convert.ToInt32(parts[0].Trim()), (byte)Convert.ToInt32(parts[1].Trim()),
                                  (byte)Convert.ToInt32(parts[2].Trim()),
                                  (byte)Convert.ToInt32(parts[3].Trim()));
            }

            if (string.IsNullOrEmpty(c.Trim()))
            {
                return SKColor.Empty;
            }

            return SKColors.Black;
            //return Color.FromName(c);SKColor.
        }

        /// <summary>
        /// Converts the specified <see cref="Nullable{LinearGradientMode}"/> to a string representation.
        /// </summary>
        /// <param name="mode">The <see cref="Nullable{LinearGradientMode}"/> to convert.</param>
        /// <returns>A <see cref="string"/> representation of the <paramref name="mode"/>.</returns>
        private static string GetModeString(LinearGradientMode? mode)
        {
            return mode.HasValue ? ((int)mode.Value).ToString(CultureInfo.InvariantCulture) : (-1).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> value to an <see cref="Nullable{LinearGradientMode}"/> value.
        /// </summary>
        /// <param name="mode">The <see cref="Nullable{LinearGradientMode}"/> to convert.</param>
        /// <returns>A <see cref="Nullable{LinearGradientMode}"/> value.</returns>
        private static LinearGradientMode? GetModeFromString(string mode)
        {
            if (mode == "-1")
            {
                return null;
            }

            return (LinearGradientMode)Convert.ToInt32(mode);
        }

        #endregion
    }
}