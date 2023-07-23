namespace Modern.Forms.MonthCalendar
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Class for storing hit test data.
    /// </summary>
    public class MonthCalendarHitTest
    {
        #region fields

        /// <summary>
        /// The empty instance.
        /// </summary>
        public static readonly MonthCalendarHitTest Empty = new MonthCalendarHitTest();

        /// <summary>
        /// The invalidate bounds of the element.
        /// </summary>
        private Rectangle invalidateBounds = Rectangle.Empty;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendarHitTest"/> class.
        /// </summary>
        public MonthCalendarHitTest()
           : this(DateTime.MinValue, MonthCalendarHitType.None, Rectangle.Empty, Rectangle.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendarHitTest"/> class.
        /// </summary>
        /// <param name="date">The date of the hit test.</param>
        /// <param name="type">The result type of the hit test.</param>
        /// <param name="bounds">The bounds of the resulting element.</param>
        public MonthCalendarHitTest(
           DateTime date,
           MonthCalendarHitType type,
           Rectangle bounds)
           : this(date, type, bounds, Rectangle.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendarHitTest"/> class.
        /// </summary>
        /// <param name="date">The date of the hit test.</param>
        /// <param name="type">The result type of the hit test.</param>
        /// <param name="bounds">The bounds of the resulting element.</param>
        /// <param name="invalidateBounds">The bounds to invalidate.</param>
        public MonthCalendarHitTest(
           DateTime date,
           MonthCalendarHitType type,
           Rectangle bounds,
           Rectangle invalidateBounds)
        {
            Date = date;
            Type = type;
            Bounds = bounds;
            this.invalidateBounds = invalidateBounds;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets the date if appropiate.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the hit test result type.
        /// </summary>
        public MonthCalendarHitType Type { get; private set; }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Gets the invalidate bounds of the element.
        /// </summary>
        public Rectangle InvalidateBounds
        {
            get
            {
                if (invalidateBounds.IsEmpty)
                {
                    return Bounds;
                }

                return invalidateBounds;
            }

            internal set
            {
                invalidateBounds = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance of the <see cref="MonthCalendarHitTest"/> is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return Type == MonthCalendarHitType.None || Date == DateTime.MinValue || Bounds.IsEmpty; }
        }

        #endregion
    }
}