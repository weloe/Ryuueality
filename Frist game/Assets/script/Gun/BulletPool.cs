using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;//单例模式，能在其他代码中访问

    public GameObject bulletPrefab;

    public int bulletCount;

    //生成队列
    private Queue<GameObject> availableObjects = new Queue<GameObject>();


    void Awake()
    {
        instance = this;

        
        FillPool();
    }

    //初始化对象池
    public void FillPool()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            var newBullet = Instantiate(bulletPrefab);//生成prefab
            newBullet.transform.SetParent(transform);//让他成为空物体的子级

            
            ReturnPool(newBullet);//让新生成的物体回到对象池并取消启用
        }
    }

    //取消启用,回到对象池,
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availableObjects.Enqueue(gameObject);//把gameObject添加到队列的末端

    }

    //生成预制体并显示
    public GameObject GetFromPool()
    {

        if (availableObjects.Count == 0)
        {
            FillPool();
        }
        var outBullet = availableObjects.Dequeue();//从开头取出

        outBullet.SetActive(true);//接着会自动启用BulletSprite中的OnEnabled

        return outBullet;
    }
}
