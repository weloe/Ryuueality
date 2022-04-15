package qqserver.service;

//该类对应的一个对象和某个客户端保持通信

import qqcommon.Message;

import java.io.IOException;
import java.io.ObjectInput;
import java.io.ObjectInputStream;
import java.net.Socket;

public class ServerConnectClientThread extends Thread{

    private Socket socket;
    private String userId;//连接到服务端的用户id

    public ServerConnectClientThread(Socket socket, String userId) {
        this.socket = socket;
        this.userId = userId;
    }

    @Override
    public void run() {//这里线程处于运行的状态，可以发送/接受消息
        while (true){
            try {
                System.out.println("服务端和客户端"+userId + "保持通信，读取数据...");
                ObjectInput ois = new ObjectInputStream(socket.getInputStream());
                Message message = (Message)ois.readObject();
            } catch (IOException e) {
                e.printStackTrace();
            } catch (ClassNotFoundException e) {
                e.printStackTrace();
            }
        }
    }
}
