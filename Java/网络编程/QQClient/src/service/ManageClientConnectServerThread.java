package service;

import java.util.HashMap;

//管理客户端连接到服务端的线程的类
public class ManageClientConnectServerThread {
    //把多个线程存入hashmap集合中，key就是用户id，value就是一个线程
    private static HashMap<String,ClientConnectServerThread> hm = new HashMap<>();

    //将某个线程加入到集合中
    public static void addClientConnectServerThread(String userId,ClientConnectServerThread clientConnectServerThread){
        hm.put(userId, clientConnectServerThread);
    }

    //通过userId可以得到对应的线程
    public static ClientConnectServerThread getClientConnectServerThread(String userId){
        return hm.get(userId);
    }

}
