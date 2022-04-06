using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tsukihi.Objects
{
    internal class ReactionListener : IDisposable
    {
        public enum Action { Added, Removed }

        public delegate Task OnReactionChangedHandler(IEmote emote, Action action);

        public event OnReactionChangedHandler OnReactionChanged;

        private IUserMessage Message;
        private Dictionary<int, IEmote> Emotes;

        public ReactionListener(IUserMessage message, Dictionary<int, IEmote> emotes)
        {
            Message = message;
            Emotes = emotes;
        }

        public async void Initialize()
        {
            await SendReactions();
            Tsukihi.Client.ReactionAdded += OnReactionAdded;
            Tsukihi.Client.ReactionRemoved += OnReactionRemoved;
        }

        private async Task OnReactionAdded(
            Cacheable<IUserMessage, ulong> message, 
            Cacheable<IMessageChannel, ulong> channel, 
            Discord.WebSocket.SocketReaction reaction)
        {
            if (message.Id == Message.Id && reaction.UserId != Tsukihi.Client.CurrentUser.Id && Emotes.ContainsValue(reaction.Emote))
            {
                await OnReactionChanged(reaction.Emote, Action.Added);
            }
        }

        private async Task OnReactionRemoved(
            Cacheable<IUserMessage, ulong> message, 
            Cacheable<IMessageChannel, ulong> channel, 
            Discord.WebSocket.SocketReaction reaction)
        {
            if (message.Id == Message.Id && reaction.UserId != Tsukihi.Client.CurrentUser.Id && Emotes.ContainsValue(reaction.Emote))
            {
                await OnReactionChanged(reaction.Emote, Action.Removed);
            }
        }

        private async Task SendReactions()
        {
            for (int i = 0; i < Emotes.Count; i++)
            {
                IEmote emote;
                Emotes.TryGetValue(i + 1, out emote);
                await Task.Delay(250);
                await Message.AddReactionAsync(emote);
            }
        }

        public void Dispose()
        {
            Tsukihi.Client.ReactionAdded -= OnReactionAdded;
            Tsukihi.Client.ReactionRemoved -= OnReactionRemoved;
        }
    }
}