package com.example.chatbot.notification;

import android.app.Notification;
import android.os.Build;
import android.os.Bundle;
import android.service.notification.NotificationListenerService;
import android.service.notification.StatusBarNotification;
import android.text.SpannableString;
import android.util.Log;

import com.example.chatbot.AppContext;
import com.example.chatbot.interactor.ClientMessageReceiver;
import com.example.chatbot.interactor.MessageDetail;

public class NotificationListener extends NotificationListenerService {

    private final String TAG = "NotificationListener";
    private ClientMessageReceiver clientMessageReceiver;
    private NotificationContextStorage notificationContextStorage;

    @Override
    public void onCreate() {
        super.onCreate();

        AppContext appContext = AppContext.getInstance();
        clientMessageReceiver = appContext.getApp().getClientMessageReceiver();
        notificationContextStorage = appContext.getNotificationContextStorage();
    }

    @Override
    public void onNotificationPosted(StatusBarNotification sbn) {
        super.onNotificationPosted(sbn);

        if (sbn.getPackageName().equals("com.kakao.talk")) {
            try {
                Notification.WearableExtender wExt = new Notification.WearableExtender(sbn.getNotification());
                for (Notification.Action act : wExt.getActions()) {
                    if (act.getRemoteInputs() != null && act.getRemoteInputs().length > 0) {
                        if (act.title.toString().toLowerCase().contains("reply") ||
                                act.title.toString().toLowerCase().contains("답장")) {
                            Bundle data = sbn.getNotification().extras;
                            String room, sender, msg;
                            boolean isGroupChat = data.get("android.text") instanceof SpannableString;
                            if (Build.VERSION.SDK_INT > 23) {
                                room = data.getString("android.summaryText");
                                if (room == null) isGroupChat = false;
                                else isGroupChat = true;
                                sender = data.get("android.title").toString();
                                msg = data.get("android.text").toString();
                            } else {
                                room = data.getString("android.subText");
                                msg = data.getString("android.text");
                                sender = data.getString("android.title");
                                if (room == null) isGroupChat = false;
                                else isGroupChat = true;
                            }
                            if (room == null) room = sender;
                            chatHook(sender, msg.trim(), room, isGroupChat);
                            saveSession(room, act);
                        }
                    }
                }
            } catch (Exception e) {
                Log.e(TAG,"fail to get notification", e);
            }
        }
    }

    private void saveSession(String room, Notification.Action context) {
        notificationContextStorage.saveContext(room, context);
    }

    private void chatHook(String sender, String msg, String room, boolean isGroupChat) {
        MessageDetail messageDetail = new MessageDetail(room, sender, msg, isGroupChat);
        clientMessageReceiver.onMessageReceivedFromClient(messageDetail);
    }

}
