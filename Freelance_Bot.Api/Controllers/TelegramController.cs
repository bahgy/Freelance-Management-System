//using Microsoft.AspNetCore.Mvc;
//using Telegram.Bot;
//using Telegram.Bot.Types;

//namespace YourProject.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TelegramController : ControllerBase
//    {
//        private readonly TelegramBotClient _bot;

//        public TelegramController()
//        {
//            _bot = new TelegramBotClient("8798795105:AAECwYFsch9LYmjfR0UbFXfmtg3b_brBSxQ");
//        }

//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody] Update update)
//        {
//            if (update.Message is not null)
//            {
//                var chatId = update.Message.Chat.Id;

//                await _bot.SendTextMessageAsync(
//                    chatId: chatId,
//                    text: "اهلا بيك 🔥 البوت شغال يمعلم"
//                );
//            }

//            return Ok();
//        }
//    }
//}
using FreelanceOS.Bot.Handlers;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace FreelanceOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelegramController : ControllerBase
{
    private readonly BotUpdateHandler _handler;

    public TelegramController(BotUpdateHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        await _handler.HandleAsync(update);
        return Ok();
    }
}