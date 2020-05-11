using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using TransactionApp.Common.Constants;
using TransactionApp.DomainModel.Models;
using TransactionApp.Services.Helpers;
using TransactionApp.Services.Services.Transactions.Parsers.Models;

namespace TransactionApp.Services.Validators
{
    public class TransactionCreateModelValidator : AbstractValidator<TransactionCreateModel>
    {
        public TransactionCreateModelValidator()
        {
            RuleFor(x => x.Amount).NotNull().WithMessage(ValidationMessages.DecimalTypeError).OverridePropertyName("Amount");
            RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationMessages.EmptyItemError).DependentRules(() =>
            {
                RuleFor(x => x.Code).Must(BeIsoFormat).WithMessage(ValidationMessages.CodeFormatError);
            });
            RuleFor(x => x.Date).NotEmpty().WithMessage(ValidationMessages.DateFormatError);
            RuleFor(x => x.PublicId).NotEmpty().WithMessage(ValidationMessages.EmptyItemError).OverridePropertyName("Id").MaximumLength(50).WithMessage(ValidationMessages.MaxLengthError);
            RuleFor(x => x.Status).NotEmpty().WithMessage(ValidationMessages.EmptyItemError).DependentRules(() =>
            {
                RuleFor(x => x.Status).Must(BeCorrectStatus).WithMessage(ValidationMessages.InappropriateStatusError);
            });
        }

        private bool BeIsoFormat(string code)
        {
            return code.Length.Equals(3) && IsAllUpper(code);
        }
        private bool IsAllUpper(string input)
        {
            return input.All(t => !char.IsLetter(t) || char.IsUpper(t));
        }
        private bool BeCorrectStatus(string status)
        {
            return StatusHelper.IsAppropriateStatus(status);
        }
    }

    public class ParseResultsValidator : AbstractValidator<ParseResults>
    {
        public ParseResultsValidator()
        {
            RuleForEach(x => x.Transactions).SetValidator(new TransactionCreateModelValidator()).OverridePropertyName("TransactionItem");
        }
    }
}