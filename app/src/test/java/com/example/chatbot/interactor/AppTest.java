package com.example.chatbot.interactor;

import org.junit.Test;

import static org.junit.Assert.*;

public class AppTest {

    @Test
    public void AppBuilder_테스트() {

        ClientMessageSender mockClientMessageSender = new ClientMessageSender() {
            @Override
            public void sendMessage(Message message) {
            }
        };
        ServerMessageSender mockServerMessageSender = new ServerMessageSender() {
            @Override
            public void sendMessage(MessageDetail messageDetail) {
            }
        };

        App app = App.builder()
                .setClientMessageSender(mockClientMessageSender)
                .setServerMessageSender(mockServerMessageSender)
                .build();

        assertEquals(app.getClientMessageSender(), mockClientMessageSender);
        assertEquals(app.getServerMessageSender(), mockServerMessageSender);
        assertNotNull(app.getClientMessageReceiver());
        assertNotNull(app.getServerMessageReceiver());
    }

    @Test
    public void 클라이언트가_메시지_받았을때_서버로_전달되는지_테스트() {
        MessageDetail sendMessage = new MessageDetail("테스트방", "테스트_보내는_사람",
                "테스트_메시지", false);

        App app = App.builder()
                .setClientMessageSender(new ClientMessageSender() {
                    @Override
                    public void sendMessage(Message message) {
                    }
                })
                .setServerMessageSender(new ServerMessageSender() {
                    @Override
                    public void sendMessage(MessageDetail messageDetail) {
                        assertEquals(sendMessage, messageDetail);
                    }
                })
                .build();

        app.getClientMessageReceiver()
                .onMessageReceivedFromClient(sendMessage);
    }

    @Test
    public void 서버에서_메시지_받았을때_클라이언트로_전달되는지_테스트() {
        Message messageFromServer = new Message("방이름", "메시지");

        App app = App.builder()
                .setClientMessageSender(new ClientMessageSender() {
                    @Override
                    public void sendMessage(Message message) {
                        assertEquals(messageFromServer, message);
                    }
                })
                .setServerMessageSender(new ServerMessageSender() {
                    @Override
                    public void sendMessage(MessageDetail messageDetail) {
                    }
                })
                .build();

        app.getServerMessageReceiver()
                .onMessageReceivedFromServer(messageFromServer);
    }
}
