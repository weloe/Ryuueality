using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;//����ģʽ���������������з���

    public GameObject bulletPrefab;

    public int bulletCount;

    //���ɶ���
    private Queue<GameObject> availableObjects = new Queue<GameObject>();


    void Awake()
    {
        instance = this;

        
        FillPool();
    }

    //��ʼ�������
    public void FillPool()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            var newBullet = Instantiate(bulletPrefab);//����prefab
            newBullet.transform.SetParent(transform);//������Ϊ��������Ӽ�

            
            ReturnPool(newBullet);//�������ɵ�����ص�����ز�ȡ������
        }
    }

    //ȡ������,�ص������,
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availableObjects.Enqueue(gameObject);//��gameObject��ӵ����е�ĩ��

    }

    //����Ԥ���岢��ʾ
    public GameObject GetFromPool()
    {

        if (availableObjects.Count == 0)
        {
            FillPool();
        }
        var outBullet = availableObjects.Dequeue();//�ӿ�ͷȡ��

        outBullet.SetActive(true);//���Ż��Զ�����BulletSprite�е�OnEnabled

        return outBullet;
    }
}
