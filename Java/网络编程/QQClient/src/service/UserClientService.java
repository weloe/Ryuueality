package service;

import qqcommon.Message;
import qqcommon.MessageType;
import qqcommon.User;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.InetAddress;
import java.net.Socket;

//该类完成用户登陆验证和用户注册等功能
public class UserClientService {
    private User user = new User();//因为我们可能在其他地方使用user信息，因此作成一个成员属性
    //socket在其他地方也可能使用，所有作为属性
    private Socket socket;

    //根据userId和pwd到服务器验证该用户是否合法
    public boolean checkUser(String userId, String pwd) {
        boolean b = false;
        user.setUserId(userId);
        user.setPasswd(pwd);

        //连接到服务端，发送user对象
        try {
            socket = new Socket(InetAddress.getByName("127.0.0.1"), 9999);
            //得到ObjectOutputStream对象
            ObjectOutputStream oos = new ObjectOutputStream(socket.getOutputStream());
            oos.writeObject(user);//发送user对象给服务端

            //会收到服务端的回复
            //读取从服务端恢复的Message对象
            ObjectInputStream ois = new ObjectInputStream(socket.getInputStream());
            Message ms = (Message) ois.readObject();
            if (ms.getMesType().equals(MessageType.MESSAGE_LOGIN_SUCCEED)) {//登陆成功

                //创建一个和服务器端保持通信的线程->创建一个类 ClientConnectServerThread
                ClientConnectServerThread clientConnectServerThread = new ClientConnectServerThread(socket);

                //启动客户端的线程
                clientConnectServerThread.start();

                //客户端可能要与服务端有多条连接，需要多个socket，也就需要多给线程，为方便管理，将线程放入集合中管理
                ManageClientConnectServerThread.addClientConnectServerThread(userId, clientConnectServerThread);

                b = true;

            } else {//登陆失败,就不能启动和服务端通信的线程,但是socket开启了，所以要关闭
                socket.close();
            }

        } catch (Exception e) {
            e.printStackTrace();
        }

        return b;

    }
}
