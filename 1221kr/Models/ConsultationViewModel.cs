// ViewModels/ConsultationViewModel.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _1221kr.ViewModels
{
    public class ConsultationViewModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Цена")]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Длительность (мин)")]
        [Range(15, 240)]
        public int DurationMinutes { get; set; } = 60;

        [Required]
        [Display(Name = "Тип консультации")]
        public Models.ConsultationType Type { get; set; }

        [Display(Name = "Доступные дни")]
        public List<DayOfWeekOption> DaysOfWeek { get; set; }

        [Display(Name = "Дни (JSON)")]
        public string AvailableDaysJson { get; set; }

        [Display(Name = "Время работы")]
        public string WorkingHours { get; set; }

        public ConsultationViewModel()
        {
            DaysOfWeek = new List<DayOfWeekOption>
            {
                new DayOfWeekOption { Value = "Monday", Text = "Понедельник" },
                new DayOfWeekOption { Value = "Tuesday", Text = "Вторник" },
                new DayOfWeekOption { Value = "Wednesday", Text = "Среда" },
                new DayOfWeekOption { Value = "Thursday", Text = "Четверг" },
                new DayOfWeekOption { Value = "Friday", Text = "Пятница" },
                new DayOfWeekOption { Value = "Saturday", Text = "Суббота" },
                new DayOfWeekOption { Value = "Sunday", Text = "Воскресенье" }
            };
        }
    }

    public class DayOfWeekOption
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
   
}