using FluentValidation;
using StudentApi.Models;

namespace StudentApi.Validators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator() 
        {
            RuleFor(x => x.Name)
                    .NotEmpty()
                    .MinimumLength(2);
            RuleFor(x=>x.Major)
                .NotEmpty();
            RuleFor(x => x.Age)
                .GreaterThan(17);
        }
    }
}
