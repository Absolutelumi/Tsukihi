using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Tsukihi.Chess;
using Tsukihi.Chess.Chess_Agents;

namespace Tsukihi.Services
{
    internal class ChessService
    {
        Board Board { get; set; }

        IUserMessage ChessMessage { get; set; }

        private (IUser User, PlayerType Side) White { get; set; } // White side player

        private (IUser User, PlayerType Side) Black { get; set; } // Black side player

        private bool AI { get; set; }

        private (ChessAgent Agent, PlayerType Side) Bot { get; set; }

        private Regex PositionRegex = new Regex("([A-H])([1-8]) (to )?([A-H])([1-8])", RegexOptions.IgnoreCase);

        public ChessService(IMessageChannel channel, IUser user, bool ai) // ai bool should eventually be string option for random, ..., intermediate
        {
            Board = new Board();

            White = (user, PlayerType.White);

            AI = ai;

            if (AI) Bot = (new Intermediate(PlayerType.Black), PlayerType.Black); 

            ChessMessage = channel.SendMessageAsync(embed: GetEmbed(channel.Id.ToString())).Result;

            Tsukihi.Client.MessageReceived += HandleChessCommand;
        }

        private async Task HandleChessCommand(SocketMessage message)
        {
            // If no black side user yet, any chess command will make other user black side
            if (!AI
                && Black.User == null 
                && Board.Turn == PlayerType.Black 
                && message.Author != White.User
                && PositionRegex.IsMatch(message.Content))
                Black = (message.Author, PlayerType.Black);

            if (message.Author != White.User && !AI && message.Author != Black.User) return;
            if ((message.Author == White.User ? White : Black).Side != Board.Turn) return; 
            if (!PositionRegex.IsMatch(message.Content)) return;

            Match positionMatch = PositionRegex.Match(message.Content);

            int x1 = ConvertLetterToCoord(positionMatch.Groups[1].Value) - 1;
            int x2 = ConvertLetterToCoord(positionMatch.Groups[4].Value) - 1;

            int y1 = Convert.ToInt32(positionMatch.Groups[2].Value) - 1;
            int y2 = Convert.ToInt32(positionMatch.Groups[5].Value) - 1;

            Position origin = new Position(x1, y1);
            Position destination = new Position(x2, y2);

            if (!Board.Move(origin, destination)) 
            {
                await ChessMessage.DeleteAsync();
                await message.DeleteAsync();
                ChessMessage = message.Channel.SendMessageAsync(embed: GetEmbed(message.Channel.Id.ToString(), "You cannot make that move!")).Result;
                return; 
            }

            await ChessMessage.DeleteAsync();
            ChessMessage = message.Channel.SendMessageAsync(embed: GetEmbed(message.Channel.Id.ToString())).Result;

            await message.DeleteAsync(); 

            if (AI)
            {
                Board = Bot.Agent.ChooseMove(Board);

                await ChessMessage.DeleteAsync();
                ChessMessage = message.Channel.SendMessageAsync(embed: GetEmbed(message.Channel.Id.ToString())).Result;
            }
        }

        private Embed GetEmbed(string channelId, string errorMsg = "")
        {
            var user = Board.Turn == PlayerType.White ? White.User : Black.User;
            bool inCheck = Board.InCheck().Check;

            // Message for when the player is in check
            string checkMsg =
                inCheck ? $"{(Board.Turn == PlayerType.White ? White.User.Mention : Black.User.Mention)} " +
                $"is in check! They can only make a move to get out of check! \n"
                : "";
            errorMsg += inCheck ? "" : "\n";

            string footerMsg =
                AI && Board.Turn == PlayerType.Black ? "Intermediate is thinking!" :
                $"It is {(user == null ? "someone" : user.Username)}'s turn!";

            return new EmbedBuilder()
                .WithTitle($"{checkMsg}{errorMsg}")
                .WithImageUrl(Extensions.GetPictureUrl(Board.UpdateBoardImage(channelId)))
                .WithFooter(footerMsg, user?.GetAvatarUrl())
                .WithColor(Board.Turn == PlayerType.White ? new Color(250, 250, 250) : new Color(0, 0, 0))
                .Build();
        }

        private int ConvertLetterToCoord(string letter)
        {
            switch (letter.ToUpper())
            {
                case "A":
                    return 1;

                case "B":
                    return 2;

                case "C":
                    return 3;

                case "D":
                    return 4;

                case "E":
                    return 5;

                case "F":
                    return 6;

                case "G":
                    return 7;

                case "H":
                    return 8;

                default:
                    return 0; 
            }
        }
    }
}
