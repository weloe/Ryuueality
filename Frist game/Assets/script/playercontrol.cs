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
    public Transform CellingCheck, GroundCheck;
    public int Cheery = 0;
    public Text CheeryNum;

    private bool isHurt;//默认是false
    private bool isGround;
    private bool jumpPressed;

    private int extraJump;//默认值是0
    public AudioSource jumpAudio, hurtAudio, cheeryAudio;//音频

    [Header("Dash参数")]
    public float dashTime;//冲锋时间
    private float dashTimeLeft;//冲锋剩余时间
    private float lastDash = -10f;//上次冲锋的时间
    public float dashCoolDown;
    public float dashSpeed;

    [Header("Gun参数")]
    public float gunTime;
    private float gunTimeLeft;
    private float lastGun = -10f;
    public float gunCoolDown;
    public float gunSpeed;

    [Header("CD的UI组件")]
    public Image DashcdImage;
    public Image GuncdImage;


    public bool isDash, isGun;
    private float facedirection;


    public GameObject bulletPrefab1;

    [Header("脚部射线检测")]
    public float footOffset =  0.2f;
    public float groundDistance = 0.959139f;//与地面之间的距离
    public bool isOnGround;
    public LayerMask groundLayer;
    [Header("头部射线检测")]
    public bool isHeadBlocked;
    public float headClearance = 0.8f;

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }



    // Start is called before the first frame update
    void Start()
    {
        DashcdImage.fillAmount = 0;
        GuncdImage.fillAmount = 0;
    }

    // Update is called once per frame
    //void update()
    void FixedUpdate()
    {
        //写在FixedUpdate中为了每帧检测多次
        //isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, ground);

        //脚部射线检测
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        if (leftCheck || rightCheck)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
        //头部
        RaycastHit2D headCheck = Raycast(new Vector2(0f,0), Vector2.up, headClearance,groundLayer);

        if (headCheck)
        {
            isHeadBlocked = true;
        }
        else
        {
            isHeadBlocked = false;
        }




        Gun();

        Dash();
        if (isDash)
            return;

        if (!isHurt)
        {
            Movement();
        }

        SwitchAnim();


    }

    private void Update()
    {
        //xiyin();

        //Jump();
        newJump();
        Crouch();
        CheeryNum.text = Cheery.ToString();

        //跳跃检测
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            jumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            //判断CD过
            if (Time.time >= (lastDash + dashCoolDown))
            {
                ReadyToDash();//执行dash

            }
        }


        if (Input.GetKeyDown(KeyCode.J))
        {

            //Gun();


            //gunTimeLeft = gunTime;

            //lastGun = Time.time;

            //GuncdImage.fillAmount = 1;
            if (Time.time >= (lastGun + gunCoolDown))
            {
                ReadyToGun();//执行gun

            }
        }



        DashcdImage.fillAmount -= 1.0f / dashCoolDown * Time.deltaTime;
        GuncdImage.fillAmount -= 1.0f / gunCoolDown * Time.deltaTime;
    }

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        facedirection = Input.GetAxisRaw("Horizontal");


        //角色移动
        //if (horizontalMove != 0)
        //{
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        animi.SetFloat("running", Mathf.Abs(facedirection));
        //}



        //面向的方向
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }


    }
    //动画转换
    void SwitchAnim()
    {
        animi.SetBool("idle", false);//可去

        if (rb.velocity.y < 0.1f && !isOnGround)
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
        else if (isHurt)
        {
            animi.SetBool("hurt", true);
            animi.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)//x轴速度小于0.1
            {
                animi.SetBool("hurt", false);
                animi.SetBool("idle", true);//可去
                isHurt = false;
            }
        }
        else if (isOnGround)
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

        //死亡重置判断
        if (collision.tag == "DeadLine")
        {

            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }

    // public 要在另一个代码中调用CheeryCount()
    public void CheeryCount()
    {
        Cheery += 1;
    }

    //重置场景
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)//当碰撞效果发生时
    {
        if (collision.gameObject.tag == "Enemy")//
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (animi.GetBool("falling") && transform.position.y > collision.gameObject.transform.position.y)//
            {
                //Destroy(collision.gameObject);
                enemy.JumpOn();

                rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                animi.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.transform.position.x)
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

    //下蹲
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CellingCheck.position, 0.2f, ground))//!isHeadBlocked)//!Physics2D.OverlapCircle(CellingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch")) //GetButtonDown
            {
                if (animi.GetFloat("running") > 0.1)
                {
                    animi.SetBool("crouching", true);
                    DisColl.enabled = false;
                }

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

    //二段跳
    void newJump()
    {
        if (isOnGround)
        {
            extraJump = 1;

        }
        if (jumpPressed && isOnGround)
        {

            rb.velocity = Vector2.up * JumpForce;//new Vector2(0,1)
            extraJump--;
            jumpPressed = false;
            animi.SetBool("jumping", true);
            jumpAudio.Play();
        }
        else if (jumpPressed && extraJump > 0 && !isOnGround)
        {
            rb.velocity = Vector2.up * JumpForce;
            extraJump--;
            jumpPressed = false;
            animi.SetBool("jumping", true);
            jumpAudio.Play();
        }
    }

    void ReadyToDash()
    {
        isDash = true;

        dashTimeLeft = dashTime;

        lastDash = Time.time;//每dash一次记录一次

        DashcdImage.fillAmount = 1;
    }

    void Dash()
    {
        if (isDash)
        {
            if (dashTimeLeft > 0)
            {

                if (rb.velocity.y > 0 && !isOnGround)
                {
                    rb.velocity = new Vector2(dashSpeed * transform.localScale.x, JumpForce);
                }
                rb.velocity = new Vector2(dashSpeed * transform.localScale.x, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFromPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDash = false;
                if (!isOnGround)
                {
                    rb.velocity = new Vector2(dashSpeed * facedirection, JumpForce);
                }

            }
        }
    }


    void ReadyToGun()
    {
        isGun = true;

        gunTimeLeft = gunTime;

        lastGun = Time.time;

        GuncdImage.fillAmount = 1;
    }

    void Gun()
    {
        if (isGun)
        {

            if (gunTimeLeft > 0)
            {

                gunTimeLeft -= Time.deltaTime;

                //填充 发射子弹
                BulletPool.instance.GetFromPool();

            }
            if (gunTimeLeft <= 0)
            {
                isGun = false;

            }
        }
    }

    /*void xiyin()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 m_mousePosition = Input.mousePosition;
            m_mousePosition = Camera.main.ScreenToWorldPoint(m_mousePosition);
            // 因为是2D，用不到z轴。使将z轴的值为0，这样鼠标的坐标就在(x,y)平面上了
            m_mousePosition.z = 0;
            transform.right = m_mousePosition - transform.position;
            GameObject b = Instantiate(bulletPrefab1, m_mousePosition, Quaternion.identity);
            float m_fireAngle = Vector2.Angle(b.transform.position - this.transform.position, Vector2.up);

            // 速度
            float BulletSpeed = 4;

            rb.velocity = ((b.transform.position - transform.position).normalized * BulletSpeed);
            

            if (m_mousePosition.x > this.transform.position.x)
            {
                m_fireAngle = -m_fireAngle;
            }
            transform.eulerAngles = new Vector3(0, 0, m_fireAngle);
        }




    }*/

}