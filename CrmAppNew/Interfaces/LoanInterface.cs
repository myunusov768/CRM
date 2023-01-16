using CrmAppNew.Abstracts;
using CrmAppNew.Model;
using System;

namespace CrmAppNew.Interfaces
{
    public class LoanInterface
    {
        public static void RequestLoan(User user, ILoanService service)
        {
            Console.WriteLine("Введите сумма кредита!");
            int amount = int.Parse(Console.ReadLine());
            var result = service.CreateLoan(user.Id, amount);
            Console.WriteLine(result.Message);
        }
        public static void AllLoans(User user, ILoanService service)
        {
            var loans = service.GetAllLoansUser(user.Id);
            if (!loans.IsSuccessfully)
                Console.WriteLine(loans.Message);
            else
                Console.WriteLine(loans.Payload);
        }
        public static void RepaymentLoan(User user, ILoanService service)
        {
            AllLoans(user, service);
            Console.WriteLine("Введите Id транша!");
            int _id = int.Parse(Console.ReadLine());
            var loan = service.GetSpecificLoan(_id);
            if (loan.IsSuccessfully)
            {
                Console.WriteLine($"Сумма: {loan.Payload.LoanAmount}");
                var result = service.RepaymentLoan(_id, Program.AmountInput());
                Console.WriteLine(result.Message);
            }
            else
                Console.WriteLine("Не правельный транш!");

        }

    }
}
