using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Assessment.Resources
{
    public class DateUtils
    {
        public static bool startBeforeEnd(DateTime start, DateTime end)
        {
            if (start <= end)
            {
                return true;
            }
            else
                return false;
        }

        public static bool isInDateRange(DateTime checkDate, DateTime start, DateTime end)
        {
            if (checkDate >= start && checkDate <= end)
            {
                return true;
            }
            else
                return false;
        }
    }
}