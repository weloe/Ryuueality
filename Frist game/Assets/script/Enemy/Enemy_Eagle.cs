using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;//����
    public float Speed;
    private float Topy, Bottony;
    public Transform top, botton;
    private bool isUp = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();//Ϊ�˵õ�Animator�����
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
