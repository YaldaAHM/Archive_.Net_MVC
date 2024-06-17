using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;


/// <summary>
/// Summary description for fpub
/// </summary>
public static class fpub
{
    public static readonly int diff = 0;


    public static string ymmd(string shams)
    {
        string y = shams.Substring(0, 4);
        string m = shams.Substring(5, 2);
        string d = shams.Substring(8, 2);
        byte bm = byte.Parse(m);

        switch (bm)
        {
            case 1: m = "فروردین"; break;
            case 2: m = "اردیبهشت"; break;
            case 3: m = "خرداد"; break;
            case 4: m = "تیر"; break;
            case 5: m = "مرداد"; break;
            case 6: m = "شهریور"; break;
            case 7: m = "مهر"; break;
            case 8: m = "آبان"; break;
            case 9: m = "آذر"; break;
            case 10: m = "دی"; break;
            case 11: m = "بهمن"; break;
            case 12: m = "اسفند"; break;
        }
        return d + " " + m + " " + y;
    }
    public static DateTime convert2miladi(string shamsi)
    {
        if (shamsi == string.Empty)
        {
            return DateTime.Now.AddMinutes(diff);
        }
        else
        {
            //try
            //{
            //System.Globalization.PersianCalendar Mdate = new System.Globalization.PersianCalendar();
            //DateTime shamsidate;
            //string[] prdate = shamsi.Split('/');
            //   shamsidate = Mdate.ToDateTime(Convert.ToInt32((prdate[0])), Convert.ToInt32(prdate[1]), Convert.ToInt32(prdate[2]), 1, 1, 1, 1, System.Globalization.GregorianCalendar.ADEra);
            //   DateTime d=   Convert.ToDateTime(shamsidate.ToShortDateString())  ;

            // return d;

            //}
            //catch { return DateTime.Now.AddMinutes(diff); }

            string[] prdate = shamsi.Split('/');
            int y, m, d;
            PersianCalendar pc = new PersianCalendar();
            if (prdate[0].Length > 2)
            {
                y = Convert.ToInt32(prdate[0]);
                m = Convert.ToInt32(prdate[1]);
                d = Convert.ToInt32(prdate[2].Substring(0, 2));
            }
            else
            {
                m = Convert.ToInt32(prdate[0]);
                d = Convert.ToInt32(prdate[1]);
                y = Convert.ToInt32(prdate[2].Substring(0, 4));
            }
            DateTime dd = Convert.ToDateTime(pc.ToDateTime(y, m, d, 0, 0, 0, 0).ToShortDateString());
            return dd;

            //PersianCalendar pdate = new PersianCalendar();

        }


    }



    public static DateTime convert2shamsi(DateTime mainInput)
    {
        // PersianCalendar pc = new PersianCalendar();
        //int year = pc.GetYear(DateTime.Now);
        //int month = pc.GetMonth(DateTime.Now);
        //int day = pc.GetDayOfMonth(DateTime.Now);
        //label1.Text = year + "/" + month + "/" + day;

        PersianCalendar pdate = new PersianCalendar();

        //DateTime nT = new DateTime();

        //nT = DateTime.Now;

        string s = String.Format("{0}/{1}/{2}", pdate.GetYear(mainInput), pdate.GetMonth(mainInput),

                                      pdate.GetDayOfMonth(mainInput));
        var p= Convert.ToDateTime(s);
        return Convert.ToDateTime(s);
    }

    public static string convert2shamsiS(DateTime mainInput)
    {
        // PersianCalendar pc = new PersianCalendar();
        //int year = pc.GetYear(DateTime.Now);
        //int month = pc.GetMonth(DateTime.Now);
        //int day = pc.GetDayOfMonth(DateTime.Now);
        //label1.Text = year + "/" + month + "/" + day;

        PersianCalendar pdate = new PersianCalendar();

        //DateTime nT = new DateTime();

        //nT = DateTime.Now;

        string s = String.Format("{0}/{1}/{2}", pdate.GetYear(mainInput), pdate.GetMonth(mainInput),

                                      pdate.GetDayOfMonth(mainInput));
        return Convert.ToDateTime(s).ToString("yyyy/MM/dd");
    }

    public static string convert2shamsiSTime(DateTime mainInput)
    {
        // PersianCalendar pc = new PersianCalendar();
        //int year = pc.GetYear(DateTime.Now);
        //int month = pc.GetMonth(DateTime.Now);
        //int day = pc.GetDayOfMonth(DateTime.Now);
        //label1.Text = year + "/" + month + "/" + day;

        PersianCalendar pdate = new PersianCalendar();

        //DateTime nT = new DateTime();

        //nT = DateTime.Now;

        string s = String.Format("{0}/{1}/{2} {3}:{4}:{5}", pdate.GetYear(mainInput), pdate.GetMonth(mainInput),

                                      pdate.GetDayOfMonth(mainInput),mainInput.Hour,
                                      mainInput.Minute,mainInput.Second);
     
        return Convert.ToDateTime(s).ToString("yyyy/MM/dd HH:mm:ss");
    }

    public static string nowshamsi()
    {
        PersianCalendar pc = new PersianCalendar();
        int y = pc.GetYear(System.DateTime.Now.AddMinutes(diff));
        int m = pc.GetMonth(System.DateTime.Now.AddMinutes(diff));
        int d = pc.GetDayOfMonth(System.DateTime.Now.AddMinutes(diff));
        string shm = (y + "/" + (m > 9 ? m.ToString() : ("0" + m)) + "/" + (d > 9 ? d.ToString() : ("0" + d)));
        return shm;
    }
    public static string nowshamsi2()
    {
        PersianCalendar pc = new PersianCalendar();
        int y = pc.GetYear(System.DateTime.Now.AddMinutes(diff));
        int m = pc.GetMonth(System.DateTime.Now.AddMinutes(diff));
        int d = pc.GetDayOfMonth(System.DateTime.Now.AddMinutes(diff));
        string shm = (y + (m > 9 ? m.ToString() : ("0" + m)) + (d > 9 ? d.ToString() : ("0" + d)));
        return shm;
    }
    public static DateTime nmiladit()
    {
        return DateTime.Now.AddMinutes(diff);
    }
    public static string nowtime()
    {
        string tme;
        int h = System.DateTime.Now.AddMinutes(diff).Hour;
        int m = System.DateTime.Now.AddMinutes(diff).Minute;
        int s = System.DateTime.Now.AddMinutes(diff).Second;

        tme = ((h > 9 ? h.ToString() : ("0" + h)) + ":" + (m > 9 ? m.ToString() : ("0" + m)) + ":" + (s > 9 ? s.ToString() : ("0" + s)));
        return tme;
    }
    public static string SetDefaultDate()
    {
        return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

    }
    public static string SetDefaultTime()
    {
        return DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
    }
    public static string nowtime2()
    {
        string tme;
        int h = System.DateTime.Now.AddMinutes(diff).Hour;
        int m = System.DateTime.Now.AddMinutes(diff).Minute;
        int s = System.DateTime.Now.AddMinutes(diff).Second;

        tme = ((h > 9 ? h.ToString() : ("0" + h)) + (m > 9 ? m.ToString() : ("0" + m)) + (s > 9 ? s.ToString() : ("0" + s)));
        return tme;
    }
    public static bool is_friday()
    {
        System.DateTime dt = System.DateTime.Now.AddMinutes(diff);
        if (dt.DayOfWeek == DayOfWeek.Friday)
            return true;
        else
            return false;

    }
    public static string minnowtime(int i)
    {
        string tme;
        System.DateTime dt = System.DateTime.Now.AddMinutes(diff).AddMinutes(-1 * i);
        int h = dt.Hour;
        int m = dt.Minute;
        int s = dt.Second;
        //
        PersianCalendar pc = new PersianCalendar();
        int y = pc.GetYear(dt);
        int mm = pc.GetMonth(dt);
        int d = pc.GetDayOfMonth(dt);
        string shm = (y + "/" + (mm > 9 ? mm.ToString() : ("0" + mm)) + "/" + (d > 9 ? d.ToString() : ("0" + d)));

        //
        tme = ((h > 9 ? h.ToString() : ("0" + h)) + ":" + (m > 9 ? m.ToString() : ("0" + m)) + ":" + (s > 9 ? s.ToString() : ("0" + s)));
        tme = shm + " " + tme;
        return tme;
    }
    public static double age(string shams)
    {
        DateTime d1 = convert2miladi(shams);
        DateTime d2 = DateTime.Now.AddMinutes(diff);
        double l = Math.Truncate(DateDiff(DateInterval.Day, d1, d2) / 365.25);
        return l;
    }
    public enum DateInterval
    {
        Day,
        DayOfYear,
        Hour,
        Minute,
        Month,
        Quarter,
        Second,
        Weekday,
        WeekOfYear,
        Year
    }
    public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2)
    {
        return DateDiff(interval, dt1, dt2, System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
    }
    private static int GetQuarter(int nMonth)
    {
        if (nMonth <= 3)
            return 1;
        if (nMonth <= 6)
            return 2;
        if (nMonth <= 9)
            return 3;
        return 4;
    }
    public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2, DayOfWeek eFirstDayOfWeek)
    {
        if (interval == DateInterval.Year)
            return dt2.Year - dt1.Year;

        if (interval == DateInterval.Month)
            return (dt2.Month - dt1.Month) + (12 * (dt2.Year - dt1.Year));

        TimeSpan ts = dt2 - dt1;

        if (interval == DateInterval.Day || interval == DateInterval.DayOfYear)
            return Round(ts.TotalDays);

        if (interval == DateInterval.Hour)
            return Round(ts.TotalHours);

        if (interval == DateInterval.Minute)
            return Round(ts.TotalMinutes);

        if (interval == DateInterval.Second)
            return Round(ts.TotalSeconds);

        if (interval == DateInterval.Weekday)
        {
            return Round(ts.TotalDays / 7.0);
        }

        if (interval == DateInterval.WeekOfYear)
        {
            while (dt2.DayOfWeek != eFirstDayOfWeek)
                dt2 = dt2.AddDays(-1);
            while (dt1.DayOfWeek != eFirstDayOfWeek)
                dt1 = dt1.AddDays(-1);
            ts = dt2 - dt1;
            return Round(ts.TotalDays / 7.0);
        }

        if (interval == DateInterval.Quarter)
        {
            double d1Quarter = GetQuarter(dt1.Month);
            double d2Quarter = GetQuarter(dt2.Month);
            double d1 = d2Quarter - d1Quarter;
            double d2 = (4 * (dt2.Year - dt1.Year));
            return Round(d1 + d2);
        }

        return 0;

    }
    private static long Round(double dVal)
    {
        if (dVal >= 0)
            return (long)Math.Floor(dVal);
        return (long)Math.Ceiling(dVal);
    }

    public static TimeSpan tmdiff(string stm, string btm)
    {
        DateTime dt1 = DateTime.ParseExact(btm, "HH:mm:ss", new DateTimeFormatInfo());
        DateTime dt2 = DateTime.ParseExact(stm, "HH:mm:ss", new DateTimeFormatInfo());
        TimeSpan ts1 = dt1.Subtract(dt2);
        return ts1;
    }
    public static string shamsitxt()
    {

        PersianCalendar pc = new PersianCalendar();
        int y = pc.GetYear(System.DateTime.Now.AddMinutes(diff));
        int m = pc.GetMonth(System.DateTime.Now.AddMinutes(diff));
        int d = pc.GetDayOfMonth(System.DateTime.Now.AddMinutes(diff));
        DayOfWeek dw = DateTime.Now.AddMinutes(diff).DayOfWeek;
        string daytxt = "";
        string monthtxt = "";
        switch (dw)
        {
            case DayOfWeek.Friday: daytxt = "جمعه"; break;
            case DayOfWeek.Saturday: daytxt = "شنبه"; break;
            case DayOfWeek.Sunday: daytxt = "یکشنبه"; break;
            case DayOfWeek.Thursday: daytxt = "پنج شنبه"; break;
            case DayOfWeek.Tuesday: daytxt = "سه شنبه"; break;
            case DayOfWeek.Wednesday: daytxt = "چهار شنبه"; break;
            case DayOfWeek.Monday: daytxt = "دوشنبه"; break;
        }
        switch (m)
        {
            case 1: monthtxt = "فروردین"; break;
            case 2: monthtxt = "اردیبهشت"; break;
            case 3: monthtxt = "خرداد"; break;
            case 4: monthtxt = "تیر"; break;
            case 5: monthtxt = "مرداد"; break;
            case 6: monthtxt = "شهریور"; break;
            case 7: monthtxt = "مهر"; break;
            case 8: monthtxt = "آبان"; break;
            case 9: monthtxt = "آذر"; break;
            case 10: monthtxt = "دی"; break;
            case 11: monthtxt = "بهمن"; break;
            case 12: monthtxt = "اسفند"; break;
        }

        string shm = daytxt + " " + d.ToString() + " " + monthtxt + " " + y.ToString();
        return shm;

    }

    public static string GetDayOfWeekName(this DateTime date)
    {
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Saturday: return "شنبه";
            case DayOfWeek.Sunday: return "يک شنبه";
            case DayOfWeek.Monday: return "دوشنبه";
            case DayOfWeek.Tuesday: return "سه‏ شنبه";
            case DayOfWeek.Wednesday: return "چهارشنبه";
            case DayOfWeek.Thursday: return "پنجشنبه";
            case DayOfWeek.Friday: return "جمعه";
            default: return "";
        }
    }
}


