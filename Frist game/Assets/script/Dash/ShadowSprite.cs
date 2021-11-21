using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;//用public,在Shadow启动时容易无法获得值，也方便以后更换player也能自动获得

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;//获得角色图像

    private Color color;

    public float activeTime;//显示时间
    public float activeStart;//获得开始显示的时间

    private float alpha;
    public float alphaSet;//设置不透明度初始值
    public float alphaMultiplier;

    //启动Shadow,残影生成
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;//把角色的左右翻转值传到Shadow
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
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);
        }

    }




}
