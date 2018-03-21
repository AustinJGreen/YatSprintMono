using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YatSprint.UI
{
    public class ScreenSort : IComparer<Screen>
    {
        public int Compare(Screen x, Screen y)
        {
            if (x.Index < y.Index)
                return -1;
            if (x.Index > y.Index)
                return 1;
            return 0;
        }
    }
}
