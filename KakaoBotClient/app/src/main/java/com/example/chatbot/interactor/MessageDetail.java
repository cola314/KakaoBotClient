package com.example.chatbot.interactor;

public class MessageDetail {

    private String room;
    private String sender;
    private String content;
    private boolean isGroupRoom;

    public MessageDetail(String room, String sender, String content, boolean isGroupRoom) {
        this.room = room;
        this.sender = sender;
        this.content = content;
        this.isGroupRoom = isGroupRoom;
    }

    public String getRoom() {
        return room;
    }

    public String getSender() {
        return sender;
    }

    public String getContent() {
        return content;
    }

    public boolean isGroupRoom() {
        return isGroupRoom;
    }
}
