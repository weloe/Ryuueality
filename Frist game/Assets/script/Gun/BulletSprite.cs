using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSprite : MonoBehaviour
{

    private Transform player;//用public,在Bullet启动时容易无法获得值，也方便以后更换player也能自动获得

    private SpriteRenderer thisSprite;
    private Rigidbody2D rb;
    private Collider2D coll;
    public float speed;


    private Color color;
    [Header("时间控制参数")]
    public float activeTime;//显示时间
    public float activeStart;//获得开始显示的时间

    [Header("不透明度控制")]
    private float alpha;//根据时间不断改变的值
    public float alphaSet;//设置不透明度初始值
    public float alphaMultiplier;
    public float Bullet_facedirection;



    //启动
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        Bullet_facedirection = GameObject.FindGameObjectWithTag("Player").transform.localScale.x;

        

        alpha = alphaSet;

        
        transform.position = player.position;
        transform.localScale = player.localScale;//把角色的左右翻转值传到
        transform.rotation = player.rotation;

        activeStart = Time.time;//开始显示时间为当前时间点

        //rb.velocity = new Vector2(speed * Bullet_facedirection, 0);
        

    }
    // Update is called once per frame
    void Update()
    {
        //alpha *= alphaMultiplier;

        color = new Color(1, 1, 1, 1);

        thisSprite.color = color;

        //运行时间>开始时间+启动时间
        if (Time.time >= activeStart + activeTime)
        {
            //返回对象池
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
