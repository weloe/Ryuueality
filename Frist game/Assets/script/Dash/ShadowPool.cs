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

        //��ʼ�������
        FillPool();
    }


    public void FillPool()
    {
        for(int i = 0;i<shadowCount;i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);//������Ϊ��������Ӽ�

            //ȡ������,�ص������,
            ReturnPool(newShadow);
        }
    }

    //ȡ������,�ص������,
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availableObjects.Enqueue(gameObject);//��gameObject��ӵ����е�ĩ��

    }

    public GameObject GetFromPool()
    {

        if(availableObjects.Count==0)
        {
            FillPool();
        }
        var outShadow = availableObjects.Dequeue();//�ӿ�ͷȡ�� 

        outShadow.SetActive(true);

        return outShadow;
    }
}
