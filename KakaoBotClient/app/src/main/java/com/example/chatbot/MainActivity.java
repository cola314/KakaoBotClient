package com.example.chatbot;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import com.example.chatbot.interactor.App;
import com.example.chatbot.notification.NotificationContextStorage;
import com.example.chatbot.socket.SocketIOClient;
import com.example.chatbot.wearos.WearOsMessageSender;


public class MainActivity extends AppCompatActivity {

    private final String TAG = "MainActivity";
    private SocketIOClient socketIOClient;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        NotificationContextStorage notificationContextStorage = new NotificationContextStorage();
        WearOsMessageSender clientMessageSender = new WearOsMessageSender(getApplicationContext(), notificationContextStorage);
        socketIOClient = new SocketIOClient();
        App app = App.builder()
                .setClientMessageSender(clientMessageSender)
                .setServerMessageSender(socketIOClient)
                .build();

        socketIOClient.setServerMessageReceiver(app.getServerMessageReceiver());

        AppContext appContext = AppContext.getInstance();
        appContext.setApp(app);
        appContext.setNotificationContextStorage(notificationContextStorage);
    }

    public void start(View v) {
        startActivity(new Intent("android.settings.ACTION_NOTIFICATION_LISTENER_SETTINGS"));

        String serverIp = ((EditText)findViewById(R.id.serverIp)).getText().toString();

        boolean connectResult = socketIOClient.connect(serverIp, "4321");
        if (connectResult) {
            Toast.makeText(this, "연결 성공", Toast.LENGTH_SHORT).show();
        } else {
            Toast.makeText(this, "연결 실패", Toast.LENGTH_SHORT).show();
        }
    }
}