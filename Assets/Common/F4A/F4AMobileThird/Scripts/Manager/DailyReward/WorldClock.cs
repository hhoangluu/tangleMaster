using UnityEngine;
using System;
using System.Collections;
using System.Globalization;

namespace com.F4A.MobileThird
{
    /**
     * Representation of the world clock JSON Object as shown at http://worldclockapi.com/
     **/
    [Serializable]
    public class WorldClock
    {
        public string currentDateTime;
        public string utcOffset;
        public string dayOfTheWeek;
        public string timeZoneName;
        public long currentFileTime;
        public string ordinalDate;
        public string serviceResponse;
    }
}