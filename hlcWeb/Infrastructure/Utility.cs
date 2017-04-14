using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hlcWeb.Infrastructure
{
    public static class Utility
    {
        public static string IconCheckOrX(string text) => text.ToUpper().StartsWith("NOT") ? "close" : "check";
        public static string IconCheckOrX(bool value) => value ? "check" : "close";

    }
}