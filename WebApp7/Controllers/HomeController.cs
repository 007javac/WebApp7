using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SubjectsGradesApp.Models;

namespace SubjectsGradesApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new SubjectsViewModel
            {
                Subjects = Enumerable.Range(1, 9)
                    .Select(_ => new SubjectGrade())
                    .ToList(),
                Colors = new ColorSettings()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ShowResults(SubjectsViewModel model)
        {
            // Удаляем пустые записи
            model.Subjects = model.Subjects
                .Where(s => !string.IsNullOrWhiteSpace(s.SubjectName))
                .ToList();

            // Проверяем наличие валидных предметов
            if (!model.Subjects.Any())
            {
                ModelState.AddModelError("", "Необходимо ввести хотя бы один предмет");
                return View("Index", model);
            }

            // Валидация каждого предмета
            foreach (var subject in model.Subjects)
            {
                var validationContext = new ValidationContext(subject);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(subject, validationContext, validationResults, true))
                {
                    foreach (var validationResult in validationResults)
                    {
                        ModelState.AddModelError("", validationResult.ErrorMessage);
                    }
                }
            }

            // Проверяем валидность цветов
            var colorValidationContext = new ValidationContext(model.Colors);
            var colorValidationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model.Colors, colorValidationContext, colorValidationResults, true))
            {
                foreach (var validationResult in colorValidationResults)
                {
                    ModelState.AddModelError("", validationResult.ErrorMessage);
                }
            }

            // Если есть ошибки, возвращаем форму ввода
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Расчет среднего балла
            model.AverageGrade = model.Subjects.Average(s => s.Grade);
            model.HasValidSubjects = true;

            return View(model);
        }
    }
}