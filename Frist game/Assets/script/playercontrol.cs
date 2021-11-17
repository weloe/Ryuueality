using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class playercontrol : MonoBehaviour
{
    public Rigidbody2D rb;



    public float speed;
    public float JumpForce;
    public Animator animi;
    public LayerMask ground;
    public Collider2D coll;

    public Collider2D DisColl;
    public Transform CellingCheck,GroundCheck;

    public int Cheery = 0;
    public Text CheeryNum;
    private bool isHurt;//默认是false
    private bool isGround;
    private int extraJump;//默认值是0
    public AudioSource jumpAudio,hurtAudio,cheeryAudio;//跳跃,受伤,樱桃音频
    

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

        //写在FixedUpdate中为了每帧检测多次
        isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, ground);
    }

    private void Update()
    {
        //Jump();
        newJump();
        Crouch();
        CheeryNum.text = Cheery.ToString();

    }

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");


        //角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            animi.SetFloat("running", Mathf.Abs(facedirection));

        }
        //面向的方向
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }
        //角色跳跃
        //if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.fixedDeltaTime);
        //    animi.SetBool("jumping", true);
        //    jumpAudio.Play();
        //}

        
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
    //碰撞检测
    private void OnTriggerEnter2D(Collider2D collision)//若另一个配置器2D进入了触发器，则调用onTriggerEnter2D
    {
        //收集物品
        if (collision.tag == "Collection")
        {
            cheeryAudio.Play();
            //Destroy(collision.gameObject);
            //Cheery += 1;
            collision.GetComponent<Animator>().Play("isGot");
            //CheeryNum.text = Cheery.ToString();
        }

        //死亡重置
        if(collision.tag == "DeadLine")
        {

            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
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
                
                rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.deltaTime);
                animi.SetBool("jumping", true);
            }
            else if(transform.position.x<collision.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                isHurt = true;
                hurtAudio.Play();
            }
            else if (transform.position.x > collision.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                isHurt = true;
                hurtAudio.Play();
            }
        }
    }

    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CellingCheck.position,0.2f,ground)) 
        {
            if (Input.GetButton("Crouch")) //GetButtonDown
            {
                animi.SetBool("crouching", true);
                DisColl.enabled = false;
            }
            else //else if (Input.GetButtonUp("Crouch"))
            {
                animi.SetBool("crouching", false);
                DisColl.enabled = true;
            } 
        }
    }
    //角色跳跃
    /*void Jump()
    {
        
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.deltaTime);
            animi.SetBool("jumping", true);
            jumpAudio.Play();
        }
    }*/

    void newJump()
    {
        if(isGround)
        {
            extraJump = 1;
        }
        if(Input.GetButtonDown("Jump") && extraJump>0)
        {
            rb.velocity = Vector2.up * JumpForce;//new Vector2(0,1)
            extraJump--;
            animi.SetBool("jumping", true);
        }
        if(Input.GetButtonDown("Jump") && extraJump==0 && isGround)
        {
            rb.velocity = Vector2.up * JumpForce;
            animi.SetBool("jumping", true);
        }
    }





    //重置场景
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // public 要在另一个代码中调用CheeryCount()
    public void CheeryCount()
    {
        Cheery += 1;
    }

}

