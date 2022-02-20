using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using KakaoBotClient.Model.Messages.Messages;
using System;

namespace KakaoBotClient.Droid.Services
{
    internal class MessageSendService : IMessageSendService
    {
        private string Tag => nameof(MessageSendService);

        private readonly ActionStorage _actionStorage;
        private readonly Context _context;

        public MessageSendService(ActionStorage storage, Context context)
        {
            this._actionStorage = storage ?? throw new ArgumentNullException(nameof(storage));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Send(string roomName, string content)
        {
            var action = _actionStorage.GetAction(roomName);

            var sendIntent = new Intent();
            var bundle = new Bundle();
            foreach (var input in action.GetRemoteInputs())
            {
                bundle.PutCharSequence(input.ResultKey, content);
            }
            RemoteInput.AddResultsToIntent(action.GetRemoteInputs(), sendIntent, bundle);

            try
            {
                action.ActionIntent.Send(_context, 0, sendIntent);
            }
            catch (PendingIntent.CanceledException e)
            {
                Log.Error(Tag, "Fail to send message {0}", e.ToString());
            }
        }
    }
}