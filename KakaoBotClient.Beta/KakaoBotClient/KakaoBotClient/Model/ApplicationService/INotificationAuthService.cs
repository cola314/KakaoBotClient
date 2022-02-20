namespace KakaoBotClient.Model.ApplicationService
{
    public interface INotificationAuthService
    {
        bool IsAuthenticated();
        void RequestAuthentication();
    }
}
