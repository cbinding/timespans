using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.Rx
{
    public class RxColour: Matcher<int> // <System.Drawing.KnownColor>
    {
        // TODO: prefixes and combinations? e.g. light/dark/deep/pale/medium, YellowGreen (but leads to nonsense like WhiteBlack) 
        // HTML4 predefined colours - 16
        // HTML5 predefined colours - 140 - orient to these? defined in System.Drawing.KnownColor
           // this class not used yet   
        private static string[] patterns_cy = 
        { 
            @"du",
            @"llwyd",
            @"brown",
            @"[cg]och",
            @"gwyrdd",
            @"glas"
        };
        private static string[] patterns_en = 
        { 
            @"black",
            @"grey",
            @"brown",
            @"red",
            @"green",
            @"blue"
        };

    }
}