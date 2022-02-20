package com.example.chatbot.wearos;

import android.app.Notification;
import android.app.PendingIntent;
import android.app.RemoteInput;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.example.chatbot.interactor.ClientMessageSender;
import com.example.chatbot.interactor.Message;
import com.example.chatbot.notification.NotificationContextStorage;

public class WearOsMessageSender implements ClientMessageSender {

    private final String TAG = "WearOsMessageSender";
    private final Context context;
    private final NotificationContextStorage notificationContextStorage;

    public WearOsMessageSender(Context context, NotificationContextStorage notificationContextStorage) {
        this.context = context;
        this.notificationContextStorage = notificationContextStorage;
    }

    @Override
    public void sendMessage(Message message) {
        Log.i(TAG, "send message using wear os");
        Notification.Action action = notificationContextStorage.getContextByRoom(message.getRoom());

        Intent sendIntent = new Intent();
        Bundle msg = new Bundle();
        for (RemoteInput input : action.getRemoteInputs()) {
            msg.putCharSequence(input.getResultKey(), message.getContent());
        }
        RemoteInput.addResultsToIntent(action.getRemoteInputs(), sendIntent, msg);

        try {
            action.actionIntent.send(context, 0, sendIntent);
        } catch (PendingIntent.CanceledException e) {
            Log.e(TAG, "fail to send message using wear os", e);
        }
    }
}
