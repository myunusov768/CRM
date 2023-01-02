using CrmAppNew.Enums;
using CrmAppNew.Model;
using System;
/*2.) В CRM требуется добавить возможность получать статистику:

 - Кол-во пользователей которых принял модератор.
 - Кол-во пользовтелей которым было отказано в регистрации и причина отказа.*/
namespace CrmAppNew.ManagerCrm
{
    sealed class StatisticService
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

        

        public void CalculationStatistic()
        {
            if(_users != null)
                foreach (var user in _users)
                {
                    if (user.moderatorCheck.Equals(ModeratorCheckType.Accept))
                        _countAcceptUsers++;
                    else if (user.moderatorCheck.Equals(ModeratorCheckType.NotAccept))
                        _countNotAcceptUsers++;
                    else if (user.moderatorCheck.Equals(ModeratorCheckType.Pending))
                        _countPendingUsers++;
                    else
                        throw new Exception("Oooops:(");
                }
            else
                throw new ArgumentNullException(nameof(_users));
        }
    }
}
