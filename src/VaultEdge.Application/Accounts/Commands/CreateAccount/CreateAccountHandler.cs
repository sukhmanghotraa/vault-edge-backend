using ErrorOr;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Repositories;
using VaultEdge.Domain.Account;
using VaultEdge.Domain.Common.Errors;
using VaultEdge.Domain.Enums;
using VaultEdge.Domain.ValueObjects;

namespace VaultEdge.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountHandler : ICommandHandler<CreateAccountCommand, Guid>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;

        public CreateAccountHandler(IAccountRepository accountRepository, ICustomerRepository customerRepository)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if(customer is null)
            {
                return CustomerErrors.Customer.NotFound(request.CustomerId);
            }

            if(!customer.CanOpenNewAccount())
            {
                return AccountErrors.Account.CustomerCannotOpenAccount;
            }

            var existingAccount = await _accountRepository.GetByAccountNumberAsync(request.AccountNumber);

            if (existingAccount is not null)
            {
                return AccountErrors.Account.DuplicateAccountNumber;
            }

            if (!Enum.TryParse<AccountType>(request.Type, ignoreCase: true, out var accountType))
            {
                return AccountErrors.Account.InvalidCredentials;
            }

            var currency = Currency.FromCode(request.CurrencyCode);
            if (currency is null)
            {
                return AccountErrors.Account.InvalidCurrency;
            }

            Money? dailyLimit = null;
            if (request.DailyWithdrawalLimit.HasValue)
            {
                if (request.DailyWithdrawalLimit.Value <= 0)
                {
                    return AccountErrors.Account.InvalidLimit;
                }

                var limitCurrency = Currency.FromCode(
                    request.DailyWithdrawalLimitCurrency ?? request.CurrencyCode
                );

                if (limitCurrency is null)
                {
                    return AccountErrors.Account.InvalidCurrency;
                }

                if (limitCurrency != currency)
                {
                    return AccountErrors.Account.LimitCurrencyMismatch;
                }

                dailyLimit = Money.Create(request.DailyWithdrawalLimit.Value, limitCurrency);
            }

            Money? monthlyLimit = null;
            if (request.MonthlyWithdrawalLimit.HasValue)
            {
                if (request.MonthlyWithdrawalLimit.Value <= 0)
                {
                    return AccountErrors.Account.InvalidLimit;
                }

                var limitCurrency = Currency.FromCode(
                    request.MonthlyWithdrawalLimitCurrency ?? request.CurrencyCode
                );

                if (limitCurrency is null)
                {
                    return AccountErrors.Account.InvalidCurrency;
                }

                if (limitCurrency != currency)
                {
                    return AccountErrors.Account.LimitCurrencyMismatch;
                }

                monthlyLimit = Money.Create(request.MonthlyWithdrawalLimit.Value, limitCurrency);

                if (dailyLimit is not null && monthlyLimit < dailyLimit)
                {
                    return AccountErrors.Account.MonthlyLimitLessThanDailyLimit;
                }
            }

            var newAccount = Account.Create(
                request.CustomerId,
                request.AccountNumber,
                accountType,
                currency,
                dailyWithdrawalLimit: dailyLimit,
                monthlyWithdrawalLimit: monthlyLimit
            );

            await _accountRepository.AddAsync(newAccount);
            await _accountRepository.SaveChangesAsync();
            return newAccount.Id;
        }
    }
}
