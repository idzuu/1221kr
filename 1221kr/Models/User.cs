using _1221kr.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _1221kr.Models
{
    // Основной класс Identity пользователя
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        // Дополнительные свойства для юристов
        [Display(Name = "Специализация")]
        public string Specialization { get; set; }

        [Display(Name = "Опыт работы")]
        public int? ExperienceYears { get; set; }

        [Display(Name = "Образование")]
        public string Education { get; set; }

        [Display(Name = "Сертификаты")]
        public string Certifications { get; set; }

        [Display(Name = "О себе")]
        public string About { get; set; }

        [Display(Name = "Фото")]
        public string PhotoPath { get; set; }

        [Display(Name = "Подтвержден")]
        public bool IsVerified { get; set; } = false;

        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Навигационное свойство для анкет
        public virtual ICollection<LawyerQuestionnaire> Questionnaires { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    // Класс анкеты юриста
    public class LawyerQuestionnaire
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Связь с ApplicationUser

        [Required]
        [Display(Name = "Тип анкеты")]
        public QuestionnaireType Type { get; set; }

        [Required]
        [Display(Name = "Данные анкеты")]
        public string QuestionnaireData { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Дата обновления")]
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }

    public enum QuestionnaireType
    {
        BasicInfo,
        Specialization,
        Experience,
        Pricing,
        Availability
    }
    public class Consultation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; } = true;

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Required]
        [Range(15, 240)]
        public int DurationMinutes { get; set; }

        [Required]
        public ConsultationType Type { get; set; } // Используем enum напрямую

        public string AvailableDays { get; set; }
        public string WorkingHours { get; set; }
        public string VideoConferenceId { get; set; }
        public string ConsultationLink { get; set; }
        public virtual ICollection<ConsultationBooking> Bookings { get; set; }

        [Required]
        public string LawyerId { get; set; }

        [ForeignKey("LawyerId")]
        
        public virtual ApplicationUser Lawyer { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    }
}




public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext()
        : base("DefaultConnection", throwIfV1Schema: false)
    {
    }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<ConsultationBooking> ConsultationBookings { get; set; }
    public DbSet<LawyerQuestionnaire> LawyerQuestionnaires { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка связи один-ко-многим
        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Questionnaires)
            .WithRequired(q => q.User)
            .HasForeignKey(q => q.UserId);
        modelBuilder.Entity<ConsultationBooking>()
          .HasRequired(b => b.Consultation)
          .WithMany(c => c.Bookings)
          .HasForeignKey(b => b.ConsultationId);

    }

    public static ApplicationDbContext Create()
    {
        return new ApplicationDbContext();
    }
}
  
    


