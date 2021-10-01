using FluentValidation;
using Planner.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.DAL.FluentValidations
{
    public class StageValidator : AbstractValidator<Stage>
    {
        public StageValidator()
        {
            RuleFor(x => x.StageId).Null();
            RuleFor(x => x.ProjectId).Null();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Title).Length(0, 50);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Description).Length(0, 500);
            RuleFor(x => x.Owner).Length(2, 25);

            RuleFor(x => x.ReportDate).NotEmpty()
                    .Must(date => date != default(DateTime))
                    .WithMessage("Report Date must be present.");

            RuleFor(x => x.DueDate).NotEmpty()
                    .Must(date => date != default(DateTime))
                    .WithMessage("Due Date must be present.");

            RuleFor(x => x.DueDate).NotEmpty()
                    .Must(date => date > DateTime.Now)
                    .WithMessage("Due Date has to be in the future.");

            RuleFor(x => x.DueDate.Value).NotEmpty()
                    .GreaterThanOrEqualTo(date => date.ReportDate.Value)
                    .WithMessage("Due Date has to be after Report date.");
        }
    }
}
