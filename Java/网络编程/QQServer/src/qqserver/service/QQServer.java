package qqserver.service;


import qqcommon.Message;
import qqcommon.MessageType;
import qqcommon.User;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.HashMap;

//服务端，监听9999，等待客户端的连接，并保持通信
public class QQServer {
    private ServerSocket serverSocket = null;

    //创建一个集合，存放多个用户，如果是这些用户登陆，就认为是合法
    //这里也可以使用ConcurrentHashMap，可以处理并发的集合，没有线程安全问题
    //HashMap没有处理线程安全，因此在多线程情况下是不安全
    //ConcurrentHashMap处理的线程安全，即线程同步处理，在多线程情况下是安全的
    private static HashMap<String, User> validUsers = new HashMap<>();

    static {//在静态代码块，初始化validUsers

        validUsers.put("100", new User("100", "123456"));
        validUsers.put("200", new User("200", "123456"));
        validUsers.put("300", new User("300", "123456"));
        validUsers.put("zzb", new User("zzb", "123456"));
        validUsers.put("zxxz", new User("zxxz", "123456"));
        validUsers.put("pt", new User("ot", "123456"));

    }

    //验证用户是否有效的方法
    private boolean checkUser(String userId, String passwd) {

        User user = validUsers.get(userId);
        if (user == null) {//说明userId没有存在在validUsers的key中
            return false;
        }
        if (!user.getPasswd().equals(passwd)) {//userId正确但密码错误
            return false;
        }

        return true;
    }

    public QQServer() {

        //注意：端口可以写在配置文件中
        try {
            System.out.println("服务端在9999端口监听...");
            serverSocket = new ServerSocket(9999);
            while (true) {//当和某个客户端连接后会继续监听,因此用循环
                Socket socket = serverSocket.accept();//如果没有客户端连接就会阻塞
                //得到socket关联的对象输入流
                ObjectInputStream ois =
                        new ObjectInputStream(socket.getInputStream());
                User user = (User) ois.readObject();//读取客户端发送的User对象

                //得到socket关联的对象输出流
                ObjectOutputStream oos =
                        new ObjectOutputStream(socket.getOutputStream());


                //创建一个Message对象，准备回复客户端
                Message message = new Message();
                //验证用户
                if (checkUser(user.getUserId(), user.getPasswd())) {//登陆成功
                    message.setMesType(MessageType.MESSAGE_LOGIN_SUCCEED);
                    //将message对象回复给客户端
                    oos.writeObject(message);
                    //创建一个线程，和客户端保持通信
                    ServerConnectClientThread serverConnectClientThread = new ServerConnectClientThread(socket, user.getUserId());
                    //启动该线程
                    serverConnectClientThread.start();
                    //把该线程对象放入到一个集合中进行管理
                    ManageServerThreads.addServerThread(user.getUserId(), serverConnectClientThread);

                } else {//登陆失败,返回fail message
                    System.out.println("用户id=" + user.getUserId() + "pwd=" + user.getPasswd() + "验证失败");
                    message.setMesType(MessageType.MESSAGE_LOGIN_FAIL);
                    oos.writeObject(message);
                    //关闭socket
                    socket.close();
                }

            }
        } catch (IOException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } finally {
            //如果服务端退出了while循环，说明服务端不在监听，因此需要关闭ServerSocket
            try {
                serverSocket.close();
            } catch (IOException e) {
                e.printStackTrace();
            }

        }
    }
}
