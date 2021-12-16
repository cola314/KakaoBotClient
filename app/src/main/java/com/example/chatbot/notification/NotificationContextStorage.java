package com.example.chatbot.notification;

import android.app.Notification;

import java.util.HashMap;

public class NotificationContextStorage {

    private HashMap<String, Notification.Action> contextMap = new HashMap<>();

    public void saveContext(String room, Notification.Action context) {
        contextMap.put(room, context);
    }

    public Notification.Action getContextByRoom(String room) {
        return contextMap.get(room);
    }

}
