老鹰移动（类似青蛙

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    public float Speed;
    public Transform top, botton;
    private float Topy, Bottony;
    private bool isUp = true;

    // Start is called before the first frame update
    void Start()
    {
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
```

设置死亡动画

Make Transition

条件设置 Trigger

playercontroller中调用Enemy_frog动画效果