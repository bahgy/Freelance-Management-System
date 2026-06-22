using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
namespace TelegramBot.Handlers.Interface
{
    


    public interface ICommandHandler
    {
        bool CanHandle(string input);
        Task HandleAsync(Message message);
    }
}
