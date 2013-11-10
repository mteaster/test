using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using test.Models.Band;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;


namespace test.Models.Calendar
{
    [Table("CalendarEvent")]
    public class CalendarEvent
    {
        public CalendarEvent() {}

        [Key]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [Required]
        public string EventTitle { get; set; }

        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual BandProfile BandProfile { get; set; }

        [Required]
        public DateTime EventTime { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string EventDescription { get; set; }
    }

    public class CalendarEventModel
    {
        public CalendarEventModel() {}

        public CalendarEventModel(CalendarEvent calendarEvent)
        {
            this.EventTitle = calendarEvent.EventTitle;
            this.EventDescription = calendarEvent.EventDescription;
            this.EventMonth = calendarEvent.EventTime.Month;
            this.EventDay = calendarEvent.EventTime.Day;
            this.EventYear = calendarEvent.EventTime.Year;

            if (calendarEvent.EventTime.Hour > 12)
            {
                this.EventHour = calendarEvent.EventTime.Hour - 12;
                this.EventPeriod = "PM";
            }
            else
            {
                this.EventHour = calendarEvent.EventTime.Hour;
                this.EventPeriod = "AM";
            }

            this.EventMinute = calendarEvent.EventTime.Minute;
        }

        [Required]
        [Display(Name = "Title")]
        public string EventTitle { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string EventDescription { get; set; }

        [Display(Name = "Month (Number)")]
        public int EventMonth { get; set; }

        [Display(Name = "Day (Number)")]
        public int EventDay { get; set; }

        [Display(Name = "Year (Number)")]
        public int EventYear { get; set; }

        [Required]
        [Display(Name = "Hour (Number)")]
        public int EventHour { get; set; }

        [Required]
        [Display(Name = "Minute (Number)")]
        public int EventMinute { get; set; }

        [Required]
        [Display(Name = "Period (AM or PM)")]
        public string EventPeriod { get; set; }

    }

    public class MonthModel
    {
        public MonthModel() {}
        public MonthModel(int month, int year)
        {
            MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            DaysInMonth = DateTime.DaysInMonth(year, month);

            FirstDayOfWeek = (int)(new DateTime(year, month, 1).DayOfWeek);

            CurrentMonth = month;
            CurrentMonthYear = year;

            if (month == 1)
            {
                PreviousMonth = 12;
                PreviousMonthYear = year - 1;
            }
            else
            {
                PreviousMonth = month - 1;
                PreviousMonthYear = year;
            }

            if (month == 12)
            {
                NextMonth = 1;
                NextMonthYear = year + 1;
            }
            else
            {
                NextMonth = month + 1;
                NextMonthYear = year;
            }
        }

        public string MonthName { get; set; }
        public int DaysInMonth { get; set; }
        public int FirstDayOfWeek { get; set; }

        public int CurrentMonth { get; set; }
        public int CurrentMonthYear { get; set; }

        public int PreviousMonth { get; set; }
        public int PreviousMonthYear { get; set; }

        public int NextMonth { get; set; }
        public int NextMonthYear { get; set; }

        public IEnumerable<test.Models.Calendar.CalendarEvent> Events;
    }
}