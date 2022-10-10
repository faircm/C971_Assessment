using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Assessment.Resources
{
    internal class DateUtils
    {
        public static bool startBeforeEnd(DateTime start, DateTime end)
        {
            if (start < end)
            {
                return true;
            }
            else
                return false;
        }
    }
}