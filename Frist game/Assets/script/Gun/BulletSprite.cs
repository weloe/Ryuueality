using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSprite : MonoBehaviour
{

    private Transform player;//��public,��Bullet����ʱ�����޷����ֵ��Ҳ�����Ժ����playerҲ���Զ����

    private SpriteRenderer thisSprite;
    private Rigidbody2D rb;
    private Collider2D coll;
    public float speed;


    private Color color;
    [Header("ʱ����Ʋ���")]
    public float activeTime;//��ʾʱ��
    public float activeStart;//��ÿ�ʼ��ʾ��ʱ��

    [Header("��͸���ȿ���")]
    private float alpha;//����ʱ�䲻�ϸı��ֵ
    public float alphaSet;//���ò�͸���ȳ�ʼֵ
    public float alphaMultiplier;
    public float Bullet_facedirection;



    //����
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        Bullet_facedirection = GameObject.FindGameObjectWithTag("Player").transform.localScale.x;

        

        alpha = alphaSet;

        
        transform.position = player.position;
        transform.localScale = player.localScale;//�ѽ�ɫ�����ҷ�תֵ����
        transform.rotation = player.rotation;

        activeStart = Time.time;//��ʼ��ʾʱ��Ϊ��ǰʱ���

        //rb.velocity = new Vector2(speed * Bullet_facedirection, 0);
        

    }
    // Update is called once per frame
    void Update()
    {
        //alpha *= alphaMultiplier;

        color = new Color(1, 1, 1, 1);

        thisSprite.color = color;

        //����ʱ��>��ʼʱ��+����ʱ��
        if (Time.time >= activeStart + activeTime)
        {
            //���ض����
            BulletPool.instance.ReturnPool(this.gameObject);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision != null)
        //{
            if (collision.tag == "Enemy")
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.JumpOn();
                Destroy(gameObject);
                //BulletPool.instance.FillPool();
                
            }

        //}

    }

    
}
