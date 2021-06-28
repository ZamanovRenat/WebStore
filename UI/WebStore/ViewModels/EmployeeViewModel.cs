using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Не указана фамилия")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "В фамилии должно быть не меньше 2х символов")]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Не указано имя")]
        [Display(Name = "Имя")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "В имени должно быть не меньше 2х символов")]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Не указано отчество")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "В отчестве должно быть не меньше 2х символов")]
        public string Patronymic { get; set; }
        [Display(Name = "Возраст")]
        [Range(1, 65, ErrorMessage = "Недопустимый возраст")]
        public int Age { get; set; }
    }
}
