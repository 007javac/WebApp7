using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SubjectsGradesApp.Models
{
    public class SubjectGrade
    {
        [Required(ErrorMessage = "Название предмета обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название предмета должно быть от 2 до 50 символов")]
        [Display(Name = "Название предмета")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Оценка обязательна")]
        [Range(2, 5, ErrorMessage = "Оценка должна быть от 2 до 5")]
        [Display(Name = "Оценка")]
        public int Grade { get; set; }
    }

    public class ColorSettings
    {
        [Required(ErrorMessage = "Первый цвет обязателен")]
        [Display(Name = "Первый цвет")]
        [RegularExpression(@"^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$", ErrorMessage = "Введите корректный HEX-код цвета")]
        public string Color1 { get; set; } = "#FF0000";

        [Required(ErrorMessage = "Второй цвет обязателен")]
        [Display(Name = "Второй цвет")]
        [RegularExpression(@"^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$", ErrorMessage = "Введите корректный HEX-код цвета")]
        public string Color2 { get; set; } = "#00FF00";

        [Required(ErrorMessage = "Третий цвет обязателен")]
        [Display(Name = "Третий цвет")]
        [RegularExpression(@"^#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{3})$", ErrorMessage = "Введите корректный HEX-код цвета")]
        public string Color3 { get; set; } = "#0000FF";
    }

    public class SubjectsViewModel
    {
        public List<SubjectGrade> Subjects { get; set; }
        public ColorSettings Colors { get; set; }
        public double AverageGrade { get; set; }
        public bool HasValidSubjects { get; set; }
    }
}
