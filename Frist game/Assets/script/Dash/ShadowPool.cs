using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    public GameObject shadowPrefab;

    public int shadowCount;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();


    void Awake()
    {
        instance = this;

        //初始化对象池
        FillPool();
    }


    public void FillPool()
    {
        for(int i = 0;i<shadowCount;i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);//让他成为空物体的子级

            //取消启用,回到对象池,
            ReturnPool(newShadow);
        }
    }

    //取消启用,回到对象池,
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availableObjects.Enqueue(gameObject);//把gameObject添加到队列的末端

    }

    public GameObject GetFromPool()
    {

        if(availableObjects.Count==0)
        {
            FillPool();
        }
        var outShadow = availableObjects.Dequeue();//从开头取出 

        outShadow.SetActive(true);

        return outShadow;
    }
}
