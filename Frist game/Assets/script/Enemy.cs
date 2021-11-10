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
        //deathAudio.Play();�����������ӥ�޷����٣������Ÿ���̬���������ƶ�
        Destroy(gameObject);
    }
    public void JumpOn()
    {
        Animi.SetTrigger("death");
        deathAudio.Play();
    }
}
