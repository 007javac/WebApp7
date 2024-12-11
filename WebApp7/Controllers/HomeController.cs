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
            // ������� ������ ������
            model.Subjects = model.Subjects
                .Where(s => !string.IsNullOrWhiteSpace(s.SubjectName))
                .ToList();

            // ��������� ������� �������� ���������
            if (!model.Subjects.Any())
            {
                ModelState.AddModelError("", "���������� ������ ���� �� ���� �������");
                return View("Index", model);
            }

            // ��������� ������� ��������
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

            // ��������� ���������� ������
            var colorValidationContext = new ValidationContext(model.Colors);
            var colorValidationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model.Colors, colorValidationContext, colorValidationResults, true))
            {
                foreach (var validationResult in colorValidationResults)
                {
                    ModelState.AddModelError("", validationResult.ErrorMessage);
                }
            }

            // ���� ���� ������, ���������� ����� �����
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // ������ �������� �����
            model.AverageGrade = model.Subjects.Average(s => s.Grade);
            model.HasValidSubjects = true;

            return View(model);
        }
    }
}