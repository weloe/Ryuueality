using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject bulletPrefab1;
    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;


        Vector3 m_mousePosition = Input.mousePosition;
        m_mousePosition = Camera.main.ScreenToWorldPoint(m_mousePosition);
        // 因为是2D，用不到z轴。使将z轴的值为0，这样鼠标的坐标就在(x,y)平面上了
        m_mousePosition.z = 0;
        transform.right = m_mousePosition - transform.position;



        

        if (Input.GetMouseButtonDown(0))
        {


            // 子弹角度
            float m_fireAngle = Vector2.Angle(m_mousePosition - this.transform.position, Vector2.up);

            if (m_mousePosition.x > this.transform.position.x)
            {
                m_fireAngle = -m_fireAngle;
            }


            // 计时器归零
            //m_nextFire = 0;

            // 生成子弹
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            //设为GunBullet的子物体
            GameObject gunBullet = GameObject.Find("GunBullet");
            bullet.transform.SetParent(gunBullet.transform);

            // 速度
            float BulletSpeed = 50;
            bullet.GetComponent<Rigidbody2D>().velocity = ((m_mousePosition - transform.position).normalized * BulletSpeed);

            // 角度
            bullet.transform.eulerAngles = new Vector3(0, 0, m_fireAngle);

        }

        if(Input.GetMouseButtonDown(1))
        {
            GameObject b = Instantiate(bulletPrefab1, m_mousePosition, Quaternion.identity);
            //float m_fireAngle = Vector2.Angle(b.transform.position - this.transform.position, Vector2.up);

            // 速度
            float BulletSpeed = 4;
            
            player.GetComponent<Rigidbody2D>().velocity = ((b.transform.position - player.transform.position).normalized * BulletSpeed);
            //player.transform.Translate((b.transform.position - player.transform.position).normalized * BulletSpeed);
            
            // 角度
            //player.transform.eulerAngles = new Vector3(0, 0, m_fireAngle);

            //float speed = 1;

            //player.transform.Translate(Vector3.forward * Time.deltaTime * speed);
            //player.transform.position = Vector3.MoveTowards(player.transform.position, b.transform.position, speed);


            
        }

    }
}
