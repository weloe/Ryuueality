package service;

import qqcommon.Message;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.net.Socket;

public class ClientConnectServerThread extends Thread{
    //该线程需要持有Socket(实现通信的依靠
    private Socket socket;

    //构造器可以接受一个Socket对象
    public ClientConnectServerThread(Socket socket){
        this.socket = socket;
    }

    //为了更方便的得到Socket方法
    public Socket getSocket() {
        return socket;
    }

    //
    @Override
    public void run() {
        //因为线程需要在后台和服务器通信，因此要while循环
        while (true){

            try {
                System.out.println("客户端线程，等待读取从服务端发送的消息");
                ObjectInputStream ois = new ObjectInputStream(socket.getInputStream());
                //如果没有从服务端发来的消息，线程就会阻塞在这
                //阻塞性网络编程，相对来说效率比较低
                Message mes = (Message)ois.readObject();

            } catch (Exception e) {
                e.printStackTrace();
            }

        }
    }
}
