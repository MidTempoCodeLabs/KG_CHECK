using Adapters.Shared.Constants;
using DeclaredPersonsAdapter.Application.Enums;
using DeclaredPersonsAnalyzer.Models;
using FluentValidation;
using Shared.Extensions;

namespace DeclaredPersonsAnalyzer.Validations.DeclaredPersonAnalyserCmdArguments;

public class DeclaredPersonAnalyzerCmdArgumentsValidator : AbstractValidator<DeclaredPersonAnalyzerCmdArguments>
{
    public DeclaredPersonAnalyzerCmdArgumentsValidator()
    {
        var currentYear = DateTime.Now.Year;

        RuleFor(x => x.Source)
            .NotEmpty()
            .NotNull()
            .Must(x => x!.StartsWith(ODataSourceConstants.Epakalpojumi.DataSourceUriString))
            .WithMessage($"Url must start with '{ODataSourceConstants.Epakalpojumi.DataSourceUriString}'.")
            .Must(x => Uri.IsWellFormedUriString(x, UriKind.Absolute))
            .WithMessage("Url must be a well-formed URI.");

        RuleFor(x => x.Month)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12)
            .When(x => x.Month.HasValue)
            .WithMessage("Month must be between 1 and 12 if it is not null.");

        RuleFor(x => x.Day)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(31)
            .When(x => x.Day.HasValue)
            .WithMessage("Day must be between 1 and 31 if it is not null.");

        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(1900)
            .LessThanOrEqualTo(currentYear)
            .When(x => x.Year.HasValue)
            .WithMessage($"Year must be between 1900 and {currentYear} if it is not null.");

        RuleFor(x => x.Group)
            .NotNull()
            .When(x => x.Group != null)
            .Must(x =>
                x == DeclaredPersonsGroupingType.ByDay.GetDescription()
                || x == DeclaredPersonsGroupingType.ByMonth.GetDescription()
                || x == DeclaredPersonsGroupingType.ByYear.GetDescription()
                || x == DeclaredPersonsGroupingType.ByYearAndMonth.GetDescription()
                || x == DeclaredPersonsGroupingType.ByYearAndDay.GetDescription()
                || x == DeclaredPersonsGroupingType.ByMonthAndDay.GetDescription())
            .WithMessage($"Group must be {DeclaredPersonsGroupingType.ByDay.GetDescription()}," +
                         $" {DeclaredPersonsGroupingType.ByMonth.GetDescription()}," +
                         $" {DeclaredPersonsGroupingType.ByYear.GetDescription()}," +
                         $" {DeclaredPersonsGroupingType.ByYearAndMonth.GetDescription()}," +
                         $" {DeclaredPersonsGroupingType.ByYearAndDay.GetDescription()}," +
                         $" {DeclaredPersonsGroupingType.ByMonthAndDay.GetDescription()}," +
                         $" if it is not null.");
    }
}