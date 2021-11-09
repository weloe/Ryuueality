using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    //private Animator Animi;
    private Collider2D Coll;
    public LayerMask Ground;
    public Transform leftpoint,rightpoint;//��������left.right������������λ��
    
    private bool Faceleft = true;//true��ʼ������
    public float Speed,JumpForce;//public,Speed�ɸ���
    private float leftx,rightx;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();//rb����������
        //Animi = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();

        transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);//�������ҵ�
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Movement()
    {
        if(Faceleft)//������
        {

            if (Coll.IsTouchingLayers(Ground))
            {
                Animi.SetBool("jumping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
                
            }
            
            if(transform.position.x<leftx)//if(transform.position.x<leftpoint.position.x)
            {
                transform.localScale = new Vector3(-1,1,1);//x=-1������
                Faceleft = false;
            }
        }
        else
        {
            if (Coll.IsTouchingLayers(Ground))
            {
                Animi.SetBool("jumping", true);
                rb.velocity = new Vector2(Speed, JumpForce);

            }
            if (transform.position.x>rightx)//if(transform.position.x>rightpoint.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);//x=1������
                Faceleft = true;
            }
        }


    }

    void SwitchAnim()
    {
        if(Animi.GetBool("jumping"))
        {
            if(rb.velocity.y<0.1)//y���ٶ�С��0
            {
                Animi.SetBool("jumping", false);
                Animi.SetBool("falling", true);

            }
        }
        if(Coll.IsTouchingLayers(Ground)&&Animi.GetBool("falling"))
        {
            Animi.SetBool("falling", false);
        }
    }
    //void Death()
    //{
    //    Animi.SetTrigger("death");
    //}
    //public void JumpOn()
    //{
    //    Destroy(gameObject);
    //}
    //void Death()
    //{
    //    Destroy(gameObject);
    //}
    //public void JumpOn()
    //{
    //    Animi.SetTrigger("death");
    //}
}
