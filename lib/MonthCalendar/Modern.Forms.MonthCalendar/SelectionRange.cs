using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Forms.MonthCalendar
{
    public sealed class SelectionRange
    {
        private DateTime start = DateTime.MinValue.Date;

        private DateTime end = DateTime.MaxValue.Date;

        //
        // Summary:
        //     Gets or sets the ending date and time of the selection range.
        //
        // Returns:
        //     The ending System.DateTime value of the range.
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.Date;
            }
        }

        //
        // Summary:
        //     Gets or sets the starting date and time of the selection range.
        //
        // Returns:
        //     The starting System.DateTime value of the range.
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.Date;
            }
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.SelectionRange class.
        public SelectionRange()
        {
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.SelectionRange class with
        //     the specified beginning and ending dates.
        //
        // Parameters:
        //   lower:
        //     The starting date in the System.Windows.Forms.SelectionRange.
        //
        //   upper:
        //     The ending date in the System.Windows.Forms.SelectionRange.
        public SelectionRange(DateTime lower, DateTime upper)
        {
            if (lower < upper)
            {
                start = lower.Date;
                end = upper.Date;
            }
            else
            {
                start = upper.Date;
                end = lower.Date;
            }
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.SelectionRange class with
        //     the specified selection range.
        //
        // Parameters:
        //   range:
        //     The existing System.Windows.Forms.SelectionRange.
        public SelectionRange(SelectionRange range)
        {
            start = range.start;
            end = range.end;
        }

        //
        // Summary:
        //     Returns a string that represents the System.Windows.Forms.SelectionRange.
        //
        // Returns:
        //     A string that represents the current System.Windows.Forms.SelectionRange.
        public override string ToString()
        {
            return "SelectionRange: Start: " + start.ToString() + ", End: " + end.ToString();
        }
    }
}

