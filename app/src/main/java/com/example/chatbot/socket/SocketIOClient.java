package com.example.chatbot.socket;

import android.util.Log;

import com.example.chatbot.interactor.Message;
import com.example.chatbot.interactor.MessageDetail;
import com.example.chatbot.interactor.ServerMessageReceiver;
import com.example.chatbot.interactor.ServerMessageSender;

import org.json.JSONException;
import org.json.JSONObject;
import io.socket.client.IO;
import io.socket.client.Socket;

public class SocketIOClient implements ServerMessageSender {

    private final String TAG = "socketIOClient";
    private final String MESSAGE_RECEIVE_MESSAGE_EVENT = "push message";
    private final String REGISTER_CLIENT_EVENT = "register client";
    private final String SEND_MESSAGE_EVENT = "request message";

    private ServerMessageReceiver serverMessageReceiver;
    private Socket socket;

    public void setServerMessageReceiver(ServerMessageReceiver serverMessageReceiver) {
        this.serverMessageReceiver = serverMessageReceiver;
    }

    @Override
    public void sendMessage(MessageDetail messageDetail) {
        if (socket != null) {
            try {
                socket.emit(SEND_MESSAGE_EVENT, convertMessageDetailToJsonText(messageDetail));
            } catch (JSONException e) {
                Log.e(TAG, "convert message detail fail", e);
            }
        }
    }

    private String convertMessageDetailToJsonText(MessageDetail messageDetail) throws JSONException{
        JSONObject jsonObject = new JSONObject();
        jsonObject.put("sender", messageDetail.getSender());
        jsonObject.put("room", messageDetail.getRoom());
        jsonObject.put("msg", messageDetail.getContent());
        jsonObject.put("isGroupChat", messageDetail.isGroupRoom());
        return jsonObject.toString();
    }

    public boolean connect(String ip, String apiKey) {
        Log.i(TAG, "connect to server ip : "  + ip);
        try {
            if (socket != null)
                socket.disconnect();

            socket = IO.socket(ip);
            addSocketEventListener();
            socket.connect();
            return true;
        } catch (Exception e) {
            Log.e(TAG,  "connect fail", e);
        }
        return false;
    }

    public void disconnect() {
        Log.i(TAG, "disconnect socket server");
        if (socket != null) {
            socket.disconnect();
        }
    }

    private void addSocketEventListener() {
        socket.on(Socket.EVENT_CONNECT, args -> {
           Log.i(TAG,"socket connected");
           registerClient();
        });
        socket.on(MESSAGE_RECEIVE_MESSAGE_EVENT, args -> {
           Log.i(TAG, "receive message");

           if (args.length == 0) {
               Log.w(TAG, "argument length is 0");
               return;
           }

           Message receiveMessage = null;
           try {
               receiveMessage = convertJsonToMessage(args[0].toString());
           } catch (JSONException e) {
               Log.e(TAG, "fail to parse message", e);
           }

           if (receiveMessage != null) {
               Log.i(TAG, "receive message success");
               serverMessageReceiver.onMessageReceivedFromServer(receiveMessage);
           }
        });
    }

    private void registerClient() {
        socket.emit(REGISTER_CLIENT_EVENT);
    }

    private Message convertJsonToMessage(String plainJsonText) throws JSONException {
        JSONObject jsonObject = new JSONObject(plainJsonText);
        String room = jsonObject.getString("room");
        String content = jsonObject.getString("msg");
        return new Message(room, content);
    }
}
