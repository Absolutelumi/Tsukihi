using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Tsukihi.Services;

namespace Tsukihi.Modules
{
    public class Games : ModuleBase<CommandContext>
    {
        public static IEmote Emote = new Emoji("3️⃣");

        public static int Order = 3;

        [Command("Chess")]
        [Summary("Play a game of chess")]
        public async Task PlayChess(bool ai = false) => new ChessService(Context.Channel, Context.User, ai);

        //[Command("Blackjack"), Summary("Play a game of blackjack")]
        //public async Task PlayBlackJack() => return;
    }
}
