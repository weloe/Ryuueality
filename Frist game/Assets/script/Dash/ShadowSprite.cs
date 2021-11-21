using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;//��public,��Shadow����ʱ�����޷����ֵ��Ҳ�����Ժ����playerҲ���Զ����

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;//��ý�ɫͼ��

    private Color color;

    public float activeTime;//��ʾʱ��
    public float activeStart;//��ÿ�ʼ��ʾ��ʱ��

    private float alpha;
    public float alphaSet;//���ò�͸���ȳ�ʼֵ
    public float alphaMultiplier;

    //����Shadow,��Ӱ����
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;//�ѽ�ɫ�����ҷ�תֵ����Shadow
        transform.rotation = player.rotation;

        activeStart = Time.time;

    }
    // Update is called once per frame
    void Update()
    {
        alpha *= alphaMultiplier;

        color = new Color(1,1,1,alpha);

        thisSprite.color = color;

        if(Time.time>=activeStart+activeTime)
        {
            //���ض����
            ShadowPool.instance.ReturnPool(this.gameObject);
        }

    }




}
