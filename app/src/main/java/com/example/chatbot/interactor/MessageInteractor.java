package com.example.chatbot.interactor;

class MessageInteractor implements ClientMessageReceiver, ServerMessageReceiver {

    private ClientMessageSender clientMessageSender;
    private ServerMessageSender serverMessageSender;

    public ClientMessageSender getClientMessageSender() {
        return clientMessageSender;
    }

    public void setClientMessageSender(ClientMessageSender clientMessageSender) {
        this.clientMessageSender = clientMessageSender;
    }

    public ServerMessageSender getServerMessageSender() {
        return serverMessageSender;
    }

    public void setServerMessageSender(ServerMessageSender serverMessageSender) {
        this.serverMessageSender = serverMessageSender;
    }

    @Override
    public void onMessageReceivedFromClient(MessageDetail messageDetail) {
        serverMessageSender.sendMessage(messageDetail);
    }

    @Override
    public void onMessageReceivedFromServer(Message message) {
        clientMessageSender.sendMessage(message);
    }

}
