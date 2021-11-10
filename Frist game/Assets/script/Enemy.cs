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
        //deathAudio.Play();加在这里后老鹰无法销毁，会留着个静态动画上下移动
        Destroy(gameObject);
    }
    public void JumpOn()
    {
        Animi.SetTrigger("death");
        deathAudio.Play();
    }
}
