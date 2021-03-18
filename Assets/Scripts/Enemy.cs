using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 属性值
    public float moveSpeed = 3;
    private Vector3 bulletEulerAngles;
    private float v=-1;
    private float h;

    // 计时器
    private float timeVal = 0;
    private float timeValChangeDirection;

    // 引用
    private SpriteRenderer sr;
    public Sprite[] tankSprite; // 上右下左
    public GameObject bulletPrefeb;
    public GameObject explosionPrefab;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 攻击时间间隔
        if (timeVal >= 3) {
            Attack();
            timeVal = 0;
        }
        else {
            timeVal += Time.deltaTime;
        }
    }
    private void FixedUpdate() {
        Move();
    }

    private void Attack() {
        Instantiate(bulletPrefeb, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
    }

    private void Move() {
        if (timeValChangeDirection >= 3) {
            int num = Random.Range(0, 8);
            if (num > 5) {
                v = -1;
                h = 0;
            }
            else if (num == 0) {
                v = 1;
                h = 0;
            }
            else if (num > 0 && num <= 2) {
                h = -1;
                v = 0;
            }
            else if (num > 2 && num <= 4) {
                h = 1;
                v = 0;
            }
            timeValChangeDirection = 0;
        }
        else {
            timeValChangeDirection += Time.fixedDeltaTime;
        }

        
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);   // 以世界坐标轴移动
        if (h < 0) {
            sr.sprite = tankSprite[3];
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0) {
            sr.sprite = tankSprite[1];
            bulletEulerAngles = new Vector3(0, 0, -90);
        }

        if (h != 0) {    // 设置优先级
            return;
        }
        
        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (v < 0) {
            sr.sprite = tankSprite[2];
            bulletEulerAngles = new Vector3(0, 0, 180);
        }
        else if (v > 0) {
            sr.sprite = tankSprite[0];
            bulletEulerAngles = new Vector3(0, 0,  0);
        }
    }

    private void Die() {
        PlayerManager.Instance.playerScore++;

        // 爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // 死亡
        Destroy(gameObject);
    }

    // 敌人碰到敌人时转圈
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            timeValChangeDirection = 4;
        }
    }
}
