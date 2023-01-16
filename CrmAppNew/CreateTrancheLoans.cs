
namespace CrmAppNew
{
    public static class CreateTrancheLoans
    {
        private static int _tranche = 0;
        public static int SetTranche() => ++_tranche;
    }
}
