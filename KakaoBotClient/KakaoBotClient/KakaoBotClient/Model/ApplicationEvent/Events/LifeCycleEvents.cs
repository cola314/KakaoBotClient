namespace KakaoBotClient.Model.ApplicationEvent
{
    public class LifeCycleEvents
    {
        [EventKey("LifeCycleEvents/OnStart")]
        public class OnStart : IBaseApplicationEvent { }

        [EventKey("LifeCycleEvents/OnStart")]
        public class OnSleep : IBaseApplicationEvent { }

        [EventKey("LifeCycleEvents/OnResume")]
        public class OnResume : IBaseApplicationEvent { }
    }
}
