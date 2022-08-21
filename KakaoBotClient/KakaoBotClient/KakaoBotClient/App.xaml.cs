using KakaoBotClient.Core.Application.Messages;
using KakaoBotClient.Model.ApplicationEvent;
using KakaoBotClient.Model.ApplicationService;
using KakaoBotClient.Model.MessageServer;
using KakaoBotClient.Model.Storage;
using KakaoBotClient.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;

namespace KakaoBotClient
{
    public partial class App : Application
    {
        private IApplicationEventQueue _eventQueue;

        public static IServiceProvider ServiceProvider { get; private set; }

        public App(Action<IServiceCollection> addPlatformServices = null)
        {
            InitializeComponent();

            SetupServices(addPlatformServices);

            MainPage = new MainPage
            {
                BindingContext = ServiceProvider.GetService<MainViewModel>()
            };
        }

        private void SetupServices(Action<IServiceCollection> addPlatformServices = null)
        {
            var services = new ServiceCollection();

            addPlatformServices?.Invoke(services);

            services.AddSingleton<ClientMessageFacade>();
            services.AddSingleton<IMessageObserver>(x => x.GetRequiredService<ClientMessageFacade>());
            services.AddSingleton<IApplicationEventQueue, MessagingCenterEventQueue>();
            services.AddSingleton<ApplicationStorage>();
            services.AddTransient<IMessageServerClient, GrpcMessageServerClient>();

            services.AddTransient<MainViewModel>();

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStart()
        {
            _eventQueue = ServiceProvider.GetService<IApplicationEventQueue>();
            _eventQueue.Send(new LifeCycleEvents.OnStart());
        }

        protected override void OnSleep()
        {
            _eventQueue.Send(new LifeCycleEvents.OnSleep());
        }

        protected override void OnResume()
        {
            _eventQueue.Send(new LifeCycleEvents.OnResume());
        }
    }
}
