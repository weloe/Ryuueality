using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheery : MonoBehaviour
{
    public void Death()
    {
        FindObjectOfType<playercontrol>().CheeryCount();
        Destroy(gameObject);
    }
}
