using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcProto;
using KakaoBotClient.Model.Messages;

namespace KakaoBotClient.Model.MessageServer
{
    internal class GrpcMessageServerClient : IMessageServerClient
    {
        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;
        public event EventHandler<ReciveMessageEventArgs> OnReceiveMessage;

        private string _apiKey;
        private CancellationTokenSource _cts;
        private GrpcChannel _channel;
        private KakaoClient.KakaoClientClient _client;

        public Task ConnectAsync(string address, string apiKey)
        {
            _cts = new CancellationTokenSource();
            _apiKey = apiKey;

            var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
            _channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions() {HttpHandler = httpHandler});
            _client = new KakaoClient.KakaoClientClient(_channel);
            
            _ = Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        Console.WriteLine("[ReadPushMessage]");
                        using (var call = _client.ReadPushMessage(new ReadPushMessageRequest() { ApiKey = apiKey }))
                        {
                            OnConnected?.Invoke(this, EventArgs.Empty);

                            while (await call.ResponseStream.MoveNext(CancellationToken.None))
                            {
                                var pushMessage = call.ResponseStream.Current;
                                Console.WriteLine(
                                    $"[Push Message] Room: {pushMessage.Room}, Message: {pushMessage.Message}");

                                OnReceiveMessage?.Invoke(this, new ReciveMessageEventArgs(pushMessage.Room, pushMessage.Message));
                            }
                        }
                    }
                    catch (RpcException ex)
                    {
                        if (ex.StatusCode == StatusCode.Unauthenticated)
                        {
                            Console.WriteLine("[Unauthenticated]" + ex.Message);
                            break;
                        }
                        Console.WriteLine("[ReadPushMessage RpcException]" + ex.Message);

                        await Task.Delay(1000);
                    }
                }
            });
            return Task.CompletedTask;
        }

        public async Task DisconnectAsync()
        {
            _cts.Cancel();
            await _channel.ShutdownAsync();

            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }

        public void SendMessage(Message message)
        {
            if (_cts.IsCancellationRequested)
                return;

            var reqeust = new SendReceivedMessageRequest()
            {
                ApiKey = _apiKey,
                IsGroupChat = message.IsGroupChat,
                Message = message.Content,
                Room = message.Room,
                Sender = message.Sender,
            };
            try
            {
                _ = _client?.SendReceivedMessageAsync(reqeust, cancellationToken: _cts.Token);
            }
            catch (RpcException ex)
            {
                if (ex.StatusCode == StatusCode.Unauthenticated)
                {
                    Console.WriteLine("[Unauthenticated]" + ex.Message);
                }
                Console.WriteLine("[SendReceivedMessageAsync RpcException]" + ex.Message);
            }
        }
    }
}