using System.ComponentModel.DataAnnotations;

namespace _1221kr.Models
{
    public enum ConsultationType
    {
        [Display(Name = "Текстовая консультация")]
        Text,
        [Display(Name = "Аудио консультация")]
        Audio,
        [Display(Name = "Видео консультация")]
        Video,
        [Display(Name = "Личная встреча")]
        InPerson
    }
}