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

        //playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        //thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;//�ѽ�ɫ�����ҷ�תֵ����Shadow
        transform.rotation = player.rotation;

        activeStart = Time.time;//��ʼ��ʾʱ��Ϊ��ǰʱ���

        rb.velocity = new Vector2(speed * Bullet_facedirection, rb.velocity.y);
        


    }
    // Update is called once per frame
    void Update()
    {
        //alpha *= alphaMultiplier;

        color = new Color(1, 1, 1, 1);

        thisSprite.color = color;

        //����ʱ����ڿ�ʼʱ��+����ʱ��
        if (Time.time >= activeStart + activeTime)
        {
            //���ض����
            BulletPool.instance.ReturnPool(this.gameObject);
        }
        
    }
    private void OnCollisionEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.tag == "Enemy")
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.JumpOn();
                
            }
            Destroy(gameObject);
        }

    }


}
