using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.ManagerCrm
{
    public sealed class StatisticService
    {
        private readonly List<User> _users;
        public StatisticService(List<User> users) { _users = users; }
        private int _countAcceptUsers = 0;
        private int _countNotAcceptUsers = 0;
        private int _countPendingUsers = 0;
        private int _allUsers = 0;

        public int CountAcceptUsers { get => _countAcceptUsers; }
        public int CountNotAcceptUsers { get => _countNotAcceptUsers; }
        public int CountPendingUsers { get => _countPendingUsers; }
        public int AllUsers { get => _allUsers; }

        public double PercentAcceptUsers { get => (double)_countAcceptUsers/_allUsers; }
        public double PercentNotAcceptUsers { get => (double)_countNotAcceptUsers /_allUsers; }
        public double PercentPendingUsers { get => (double)_countPendingUsers /_allUsers; }

        

        public Result<bool> CalculationStatistic()
        {
            if (_users != null)
            {
                foreach (var user in _users)
                {
                    if (user.moderatorCheck.Equals(ModeratorCheckType.Accept))
                        _countAcceptUsers++;
                    else if (user.moderatorCheck.Equals(ModeratorCheckType.NotAccept))
                        _countNotAcceptUsers++;
                    else
                        _countPendingUsers++;
                }
                var result = new Result<bool>() 
                { 
                    IsSuccessfully = true, 
                    Message = "sheet parsed", 
                    Payload = true 
                };
                return result;
            }
            else 
            {
                var result = new Result<bool>() 
                { 
                    Error = Error.SheetIsEmpty, 
                    IsSuccessfully = false, 
                    Message = "The sheet you want to analyze is empty!" 
                };
                return result;
            }
        }
    }
}
