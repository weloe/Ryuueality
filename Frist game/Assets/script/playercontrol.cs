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
    public Transform CellingCheck;

    public int Cheery = 0;
    public Text CheeryNum;
    private bool isHurt;
    public AudioSource jumpAudio,hurtAudio,cheeryAudio;//��Ծ,����,ӣ����Ƶ
    

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
            Movement();//��������
        }

        SwitchAnim();
    }

    private void Update()
    {
        Jump();
        Crouch();
        CheeryNum.text = Cheery.ToString();

    }

    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");


        //��ɫ�ƶ�
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            animi.SetFloat("running", Mathf.Abs(facedirection));

        }
        //����ķ���
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }
        //��ɫ��Ծ
        //if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.fixedDeltaTime);
        //    animi.SetBool("jumping", true);
        //    jumpAudio.Play();
        //}

        
    }

    void SwitchAnim()//����ת��
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
            if (Mathf.Abs(rb.velocity.x) < 0.1f)//x���ٶ�С��0.1
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

        //��������
        if(collision.tag == "DeadLine")
        {

            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }
    //�������
    private void OnCollisionEnter2D(Collision2D collision)//����ײЧ������ʱ
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
    //��ɫ��Ծ
    void Jump()
    {
        
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.deltaTime);
            animi.SetBool("jumping", true);
            jumpAudio.Play();
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // public Ҫ����һ�������е���CheeryCount()
    public void CheeryCount()
    {
        Cheery += 1;
    }

}

