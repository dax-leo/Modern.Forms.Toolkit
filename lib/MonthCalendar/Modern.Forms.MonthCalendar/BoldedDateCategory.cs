﻿namespace Modern.Forms.MonthCalendar
{
    using SkiaSharp;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    /// <summary>
    /// Struct to store information about a bold date category.
    /// </summary>
    public struct BoldedDateCategory
    {
        #region fields

        /// <summary>
        /// Stores the name of the category.
        /// </summary>
        private string name;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BoldedDateCategory"/> struct.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        public BoldedDateCategory(string name)
           : this()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("parameter 'name' is invalid");
            }

            this.name = name.Trim();
            GradientMode = LinearGradientMode.Vertical;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets a empty instance of the <see cref="BoldedDateCategory"/> struct.
        /// </summary>
        public static BoldedDateCategory Empty
        {
            get
            {
                return new BoldedDateCategory
                {
                    BackColorStart = SKColor.Empty,
                    BackColorEnd = SKColor.Empty,
                    GradientMode = LinearGradientMode.Vertical,
                    ForeColor = SKColor.Empty,
                    name = string.Empty
                };
            }
        }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    name = value.Trim();
                }
            }
        }

        /// <summary>
        /// Gets or sets the forecolor of the category (the text color).
        /// </summary>
        public SKColor ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the start backcolor of the category.
        /// </summary>
        public SKColor BackColorStart { get; set; }

        /// <summary>
        /// Gets or sets the end backcolor of the category.
        /// If set to <see cref="Color.Empty"/> or <see cref="Color.Transparent"/> no gradient background is painted.
        /// </summary>
        public SKColor BackColorEnd { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LinearGradientMode"/> to use if <see cref="BackColorStart"/> and <see cref="BackColorEnd"/> are valid.
        /// </summary>
        public LinearGradientMode GradientMode { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is empty/invalid.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(name) || ForeColor == SKColor.Empty && BackColorStart == SKColor.Empty;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Gets a <see cref="string"/> instance representing the current instance of the <see cref="BoldedDateCategory"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> instance representing the current instance of the <see cref="BoldedDateCategory"/>.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}