using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Extensions.Service
{
    

    public interface INavigationService
    {
        void Push(long chatId, string page);

        string? Back(long chatId);
    }

    public class NavigationService : INavigationService
    {
        private readonly Dictionary<long, Stack<string>>
            _history = new();

        public void Push(
            long chatId,
            string page)
        {
            if (!_history.ContainsKey(chatId))
            {
                _history[chatId] = new();
            }

            _history[chatId].Push(page);
        }

        public string? Back(long chatId)
        {
            if (!_history.ContainsKey(chatId))
                return null;

            var stack = _history[chatId];

            if (stack.Count <= 1)
                return "HOME";

            stack.Pop();

            return stack.Peek();
        }
    }
}
