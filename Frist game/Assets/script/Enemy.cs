using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator Animi;
    protected virtual void Start()
    {
        Animi = GetComponent<Animator>();
    }

    public void Death()
    {
        Destroy(gameObject);
    }
    public void JumpOn()
    {
        Animi.SetTrigger("death");
    }
}
