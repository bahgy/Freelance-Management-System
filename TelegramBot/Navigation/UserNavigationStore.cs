using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Navigation
{

    public class UserNavigationStore
        : IUserNavigationStore
    {
        private readonly Dictionary<
            long,
            Stack<NavigationState>>
            _history = new();

        public void GoTo(
            long chatId,
            NavigationScreen screen,
            Guid? entityId = null)
        {
            if (!_history.ContainsKey(chatId))
            {
                _history[chatId] =
                    new Stack<NavigationState>();
            }

            _history[chatId]
                .Push(
                    new NavigationState
                    {
                        Screen = screen,
                        EntityId = entityId
                    });
        }

        public NavigationState GetCurrent(
            long chatId)
        {
            if (!_history.ContainsKey(chatId)
                || !_history[chatId].Any())
            {
                return new NavigationState
                {
                    Screen = NavigationScreen.Home
                };
            }

            return _history[chatId]
                .Peek();
        }

        public NavigationState Back(
            long chatId)
        {
            if (!_history.ContainsKey(chatId))
            {
                return new NavigationState
                {
                    Screen = NavigationScreen.Home
                };
            }

            var stack = _history[chatId];

            if (stack.Count > 1)
            {
                stack.Pop();
            }

            return stack.Peek();
        }
    }
}
