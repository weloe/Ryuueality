using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playercontrol : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    public Animator animi;
    public LayerMask ground;
    public Collider2D coll;
    public int Cheery = 0;
    public Text CheeryNum;
    private bool isHurt;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    //void update()
    void FixedUpdate()
    {
        
        if(!isHurt)
        {
            Movement();//函数调用
        }

        SwitchAnim();
    }

    void Movement()
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");


        //角色移动
        if (horizontalmove != 0)
        {
            rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime, rb.velocity.y);
            animi.SetFloat("running", Mathf.Abs(facedirection));

        }
        //面向的方向
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }
        //角色跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
            animi.SetBool("jumping", true);
        }

    }

    void SwitchAnim()//动画转换
    {
        animi.SetBool("idle", false);

        if(rb.velocity.y<0.1f && !coll.IsTouchingLayers(ground))
        {
            animi.SetBool("falling", true);
        }
        if (animi.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                animi.SetBool("jumping", false);
                animi.SetBool("falling", true);
            }
        }
        else if(isHurt)
        {
            animi.SetBool("hurt", true);
            animi.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)//x轴速度小于0.1
            {
                animi.SetBool("hurt", false);
                animi.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            animi.SetBool("falling", false);
            animi.SetBool("idle", true);

        }

    }
    //收集物品
    private void OnTriggerEnter2D(Collider2D collision)//若另一个配置器2D进入了触发器，则调用onTriggerEnter2D
    {
        if (collision.tag == "Collection")//吃Cheery
        {
            Destroy(collision.gameObject);
            Cheery += 1;
            CheeryNum.text = Cheery.ToString();
        }
    }
    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)//当碰撞效果发生时
    {
        if (collision.gameObject.tag == "Enemy")//
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (animi.GetBool("falling"))//
            {
                //Destroy(collision.gameObject);
                enemy.JumpOn();
                
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                animi.SetBool("jumping", true);
            }
            else if(transform.position.x<collision.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > collision.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                isHurt = true;
            }
        }
    }
}

