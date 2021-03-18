using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public bool isPlayerBullet; // 默认是敌人子弹

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch (collision.tag) {
            case "Tank":
                if (!isPlayerBullet) {
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                }
                break;
            
            case "Heart":
                collision.SendMessage("Die");
                Destroy(gameObject);
                break;

            case "Enemy":
                if (isPlayerBullet) {
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                }
                break;

            case "Wall":
                Destroy(collision.gameObject);  // 墙消失
                Destroy(gameObject);    // 子弹消失
                break;

            case "Barrier":
                if (isPlayerBullet)
                    collision.SendMessage("PlayAudio");
                Destroy(gameObject);
                break;

            default:
                break;
        }
    }
}
