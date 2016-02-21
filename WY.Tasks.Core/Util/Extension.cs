using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Util {
    public static class Extension {
        public static bool IsNullOrWhiteSpace(this string s) {
            if (s == null) {
                return true;
            }
            return s.Trim() == "" ? true : false;
        }
    }
}
