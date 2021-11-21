using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;//多余
    public float Speed;
    private float Topy, Bottony;
    public Transform top, botton;
    private bool isUp = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();//为了得到Animator的组件
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        Topy = top.position.y;
        Bottony = botton.position.y;
        Destroy(top.gameObject);
        Destroy(botton.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if(transform.position.y>Topy)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);
            if(transform.position.y<Bottony)
            {
                isUp = true;
            }
        }
    }
}
