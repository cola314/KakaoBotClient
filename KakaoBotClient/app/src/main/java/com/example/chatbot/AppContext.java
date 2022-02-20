package com.example.chatbot;

import com.example.chatbot.interactor.App;
import com.example.chatbot.notification.NotificationContextStorage;
import com.example.chatbot.socket.SocketIOClient;

public class AppContext {

    private static AppContext instance;

    public static synchronized AppContext getInstance() {
        if (instance == null) {
            instance = new AppContext();
        }
        return instance;
    }

    private App app;
    private NotificationContextStorage notificationContextStorage;

    public App getApp() {
        return app;
    }

    public void setApp(App app) {
        this.app = app;
    }

    public NotificationContextStorage getNotificationContextStorage() {
        return notificationContextStorage;
    }

    public void setNotificationContextStorage(NotificationContextStorage notificationContextStorage) {
        this.notificationContextStorage = notificationContextStorage;
    }
}
