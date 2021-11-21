using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator Animi;
    protected AudioSource deathAudio;
    protected virtual void Start()
    {
        Animi = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    public void Death()
    {

        GetComponent<Collider2D>().enabled = false;//½ûÓÃcollider2D

        Destroy(gameObject);

    }
    public void JumpOn()
    {
        Animi.SetTrigger("death");
        deathAudio.Play();
    }
}
