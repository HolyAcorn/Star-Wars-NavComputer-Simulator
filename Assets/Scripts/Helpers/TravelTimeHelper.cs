using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace SwNavComp
{
    public static class TravelTimeHelper
    {
        static float hoursInDay = 24;
        static float daysInWeeks = 7;

        public static float CalculateTimeRequiredInHours(float distance, float parsecsPerHour, float hyperDriveRating, float modifier)
        {
            return ((distance / parsecsPerHour) * hyperDriveRating) + modifier;
        }

        public static string DecimalToTime(float inputHours)
        {
            TimeSpan ts = TimeSpan.FromHours(inputHours);

            int days = ts.Days;
            int hours = ts.Hours;
            int minutes = ts.Minutes;

            string output = "";

            if(days > 0)
            {
                if (days > 1) output += days + " days";
                else output += days + " day";


                if (minutes <= 0) output += " and ";
                else if (hours > 0) output += ", ";
            }

            if(hours > 0)
            {
                if (hours > 1) output += hours + " hours ";
                else output += hours + " hour";
            }
            
            if(minutes > 0)
            {
                output += " and ";
                if (minutes > 1) output += minutes + " minutes";
                else if (minutes > 0) output += +minutes + " minute";
            }

            output += ".";

            return output;

        }
    }
}
