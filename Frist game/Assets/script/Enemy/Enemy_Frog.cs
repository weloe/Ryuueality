using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    //private Animator Animi;
    private Collider2D Coll;
    public LayerMask Ground;
    public Transform leftpoint,rightpoint;//拖入物体left.right获得左右物体的位置
    
    private bool Faceleft = true;//true开始面向左
    public float Speed,JumpForce;//public,Speed可更改
    private float leftx,rightx;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();//rb获得组件刚体
        //Animi = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();

        transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);//销毁左右点
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Movement()
    {
        if(Faceleft)//面向左
        {

            if (Coll.IsTouchingLayers(Ground))
            {
                Animi.SetBool("jumping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
                
            }
            
            if(transform.position.x<leftx)//if(transform.position.x<leftpoint.position.x)
            {
                transform.localScale = new Vector3(-1,1,1);//x=-1面向右
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
                transform.localScale = new Vector3(1, 1, 1);//x=1面向左
                Faceleft = true;
            }
        }


    }

    void SwitchAnim()
    {
        if(Animi.GetBool("jumping"))
        {
            if(rb.velocity.y<0.1)//y轴速度小于0
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
