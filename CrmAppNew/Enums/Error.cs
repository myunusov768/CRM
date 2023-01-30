using System;

namespace CrmAppNew.Enums
{
    public enum Error
    {
        UserIsNotFound,
        TextIsNull,
        MessageNotFound,
        InputParameterIsEmpty,
        ThisLoginIsAlreadyTaken,
        YourRequestForConsideration,
        YourRequestHasBeenDenied,
        ModeratorDidNotAcceptYourRequest,
        LoanIsNotFound,
        SheetIsEmpty,
        AmountZeroOrLessThanZero,
        LimitExhausted,
        IncomingIdIsEmpty,
        OtherErrors,
        AdvancePaymentMoreThanSalary
    }
}
