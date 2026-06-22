using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Navigation
{
    public interface IUserNavigationStore
    {
        void GoTo(
            long chatId,
            NavigationScreen screen,
            Guid? entityId = null);

        NavigationState GetCurrent(
            long chatId);

        NavigationState Back(
            long chatId);
    }
}
