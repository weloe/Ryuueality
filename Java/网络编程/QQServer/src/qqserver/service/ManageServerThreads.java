package qqserver.service;

import java.util.HashMap;

//该类用于管理和客户端通信的线程
public class ManageServerThreads {
    private static HashMap<String,ServerConnectClientThread> hm = new HashMap<>();

    //添加线程对象到hm集合
    public static void addServerThread(String userId,ServerConnectClientThread serverConnectClientThread){
        hm.put(userId, serverConnectClientThread);
    }

    //根据userId返回ServerConnectClientThread
    public static ServerConnectClientThread getServerConnectClientThread(String userId){
        return hm.get(userId);
    }



}
