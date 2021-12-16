package com.example.chatbot.interactor;

public class Message {

    private String room;
    private String content;

    public Message(String room, String content) {
        this.room = room;
        this.content = content;
    }

    public String getRoom() {
        return room;
    }

    public String getContent() {
        return content;
    }

}
