using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public GameObject enterTalk;
    public GameObject npcTalk;
    public GameObject npcTalkone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enterTalk.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        enterTalk.SetActive(false);
        npcTalk.SetActive(false);
        npcTalkone.SetActive(false);
        

    }

    private void Update()
    {
        if(enterTalk.activeSelf&&Input.GetKeyDown(KeyCode.E))
        {
            enterTalk.SetActive(false);
            npcTalk.SetActive(true);

        }
        if (enterTalk.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            enterTalk.SetActive(false);
            npcTalkone.SetActive(true);
        }
    }
}
