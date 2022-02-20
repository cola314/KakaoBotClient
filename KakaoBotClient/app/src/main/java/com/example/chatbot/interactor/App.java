package com.example.chatbot.interactor;

import java.util.function.Function;

public class App {

    private ClientMessageReceiver clientMessageReceiver;
    private ClientMessageSender clientMessageSender;
    private ServerMessageReceiver serverMessageReceiver;
    private ServerMessageSender serverMessageSender;

    public ClientMessageReceiver getClientMessageReceiver() {
        return clientMessageReceiver;
    }

    public ClientMessageSender getClientMessageSender() {
        return clientMessageSender;
    }

    public ServerMessageReceiver getServerMessageReceiver() {
        return serverMessageReceiver;
    }

    public ServerMessageSender getServerMessageSender() {
        return serverMessageSender;
    }

    public static Builder builder() {
        return new Builder();
    }

    public static class Builder {

        private MessageInteractor messageInteractor;
        private ServerMessageSender serverMessageSender;
        private ClientMessageSender clientMessageSender;

        private Builder() {
            messageInteractor = new MessageInteractor();
        }

        public App build() throws NullPointerException {
            if (clientMessageSender == null)
                throw new NullPointerException("clientMessageReceiver should't be null");
            if (serverMessageSender == null)
                throw new NullPointerException("serverMessageSender should't be null");

            App app = new App();
            MessageInteractor messageInteractor =  new MessageInteractor();
            messageInteractor.setClientMessageSender(clientMessageSender);
            messageInteractor.setServerMessageSender(serverMessageSender);
            app.clientMessageSender = clientMessageSender;
            app.serverMessageSender = serverMessageSender;
            app.clientMessageReceiver = messageInteractor;
            app.serverMessageReceiver = messageInteractor;

            return app;
        }

        public Builder setClientMessageSender(ClientMessageSender clientMessageSender) {
            this.clientMessageSender = clientMessageSender;
            return this;
        }

        public Builder setServerMessageSender(ServerMessageSender serverMessageSender) {
            this.serverMessageSender = serverMessageSender;
            return this;
        }

    }

}
