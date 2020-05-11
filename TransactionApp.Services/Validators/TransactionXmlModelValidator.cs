using FluentValidation;
using TransactionApp.Services.Services.Transactions.Parsers.Models;

namespace TransactionApp.Services.Validators
{
    public class PaymentDetailsValidator : AbstractValidator<PaymentDetails>
    {
        public PaymentDetailsValidator()
        {
            RuleFor(x => x.Amount).NotNull().MinimumLength(1).Must(BeDecimal).DependentRules(() =>
            {
                RuleFor(x => x.CurrencyCode).NotNull().MinimumLength(3).MaximumLength(3);
            });
        }
        private bool BeDecimal(string arg)
        {
            var isDecimal = decimal.TryParse(arg, out _);
            return isDecimal;
        }
    }
    public class TransactionValidator : AbstractValidator<TransactionItem>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.PublicId).NotNull().MinimumLength(1).MaximumLength(50);
            RuleFor(x => x.Status).NotNull().MinimumLength(1);
            RuleFor(x => x.PaymentDetails).NotNull().SetValidator(new PaymentDetailsValidator());
        }
    }

       

        public class TransactionXmlModelValidator:AbstractValidator<XmlTransactionsDto>
    {
        public TransactionXmlModelValidator()
        {
            RuleForEach(c => c.Transaction).SetValidator(new TransactionValidator());
        }
    }
}