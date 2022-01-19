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

    private bool isHurt;//Ĭ����false
    private bool isGround;
    private bool jumpPressed;

    private int extraJump;//Ĭ��ֵ��0
    public AudioSource jumpAudio, hurtAudio, cheeryAudio;//��Ƶ

    [Header("Dash����")]
    public float dashTime;//���ʱ��
    private float dashTimeLeft;//���ʣ��ʱ��
    private float lastDash = -10f;//�ϴγ���ʱ��
    public float dashCoolDown;
    public float dashSpeed;

    [Header("Gun����")]
    public float gunTime;
    private float gunTimeLeft;
    private float lastGun = -10f;
    public float gunCoolDown;
    public float gunSpeed;

    [Header("CD��UI���")]
    public Image DashcdImage;
    public Image GuncdImage;


    public bool isDash, isGun;
    private float facedirection;


    public GameObject bulletPrefab1;

    [Header("�Ų����߼��")]
    public float footOffset =  0.2f;
    public float groundDistance = 0.959139f;//�����֮��ľ���
    public bool isOnGround;
    public LayerMask groundLayer;
    [Header("ͷ�����߼��")]
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
        //д��FixedUpdate��Ϊ��ÿ֡�����
        //isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, ground);

        //�Ų����߼��
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
        //ͷ��
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

        //��Ծ���
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            jumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            //�ж�CD��
            if (Time.time >= (lastDash + dashCoolDown))
            {
                ReadyToDash();//ִ��dash

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
                ReadyToGun();//ִ��gun

            }
        }



        DashcdImage.fillAmount -= 1.0f / dashCoolDown * Time.deltaTime;
        GuncdImage.fillAmount -= 1.0f / gunCoolDown * Time.deltaTime;
    }

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        facedirection = Input.GetAxisRaw("Horizontal");


        //��ɫ�ƶ�
        //if (horizontalMove != 0)
        //{
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        animi.SetFloat("running", Mathf.Abs(facedirection));
        //}



        //����ķ���
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }


    }
    //����ת��
    void SwitchAnim()
    {
        animi.SetBool("idle", false);//��ȥ

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
            if (Mathf.Abs(rb.velocity.x) < 0.1f)//x���ٶ�С��0.1
            {
                animi.SetBool("hurt", false);
                animi.SetBool("idle", true);//��ȥ
                isHurt = false;
            }
        }
        else if (isOnGround)
        {
            animi.SetBool("falling", false);
            animi.SetBool("idle", true);

        }


    }
    //��ײ���
    private void OnTriggerEnter2D(Collider2D collision)//����һ��������2D�����˴������������onTriggerEnter2D
    {
        //�ռ���Ʒ
        if (collision.tag == "Collection")
        {
            cheeryAudio.Play();
            //Destroy(collision.gameObject);
            //Cheery += 1;
            collision.GetComponent<Animator>().Play("isGot");
            //CheeryNum.text = Cheery.ToString();
        }

        //���������ж�
        if (collision.tag == "DeadLine")
        {

            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }

    // public Ҫ����һ�������е���CheeryCount()
    public void CheeryCount()
    {
        Cheery += 1;
    }

    //���ó���
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //�������
    private void OnCollisionEnter2D(Collision2D collision)//����ײЧ������ʱ
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

    //�¶�
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

    //��ɫ��Ծ
    /*void Jump()
    {
        
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.deltaTime);
            animi.SetBool("jumping", true);
            jumpAudio.Play();
        }
    }*/

    //������
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

        lastDash = Time.time;//ÿdashһ�μ�¼һ��

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

                //��� �����ӵ�
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
            // ��Ϊ��2D���ò���z�ᡣʹ��z���ֵΪ0�����������������(x,y)ƽ������
            m_mousePosition.z = 0;
            transform.right = m_mousePosition - transform.position;
            GameObject b = Instantiate(bulletPrefab1, m_mousePosition, Quaternion.identity);
            float m_fireAngle = Vector2.Angle(b.transform.position - this.transform.position, Vector2.up);

            // �ٶ�
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