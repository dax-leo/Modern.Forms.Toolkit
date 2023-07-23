using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Forms.MonthCalendar
{
    public class DateRangeEventArgs : EventArgs
    {
        private readonly DateTime start;

        private readonly DateTime end;

        //
        // Summary:
        //     Gets the first date/time value in the range that the user has selected.
        //
        // Returns:
        //     A System.DateTime that represents the first date in the date range that the user
        //     has selected.
        public DateTime Start => start;

        //
        // Summary:
        //     Gets the last date/time value in the range that the user has selected.
        //
        // Returns:
        //     A System.DateTime that represents the last date in the date range that the user
        //     has selected.
        public DateTime End => end;

        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.DateRangeEventArgs class.
        //
        // Parameters:
        //   start:
        //     The first date/time value in the range that the user has selected.
        //
        //   end:
        //     The last date/time value in the range that the user has selected.
        public DateRangeEventArgs(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
