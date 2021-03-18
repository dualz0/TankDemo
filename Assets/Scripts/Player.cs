using UnityEngine;

public class Player : MonoBehaviour
{
    // 属性值
    public float moveSpeed = 3;
    private Vector3 bulletEulerAngles;
    private float defendTimeVal = 3;    // 出生后3s无敌
    private bool isDefended = true;
    private float timeVal;

    // 引用
    private SpriteRenderer sr;
    public Sprite[] tankSprite; // 上右下左
    public GameObject bulletPrefeb;
    public GameObject explosionPrefab;
    public GameObject defendEffectPrefab;
    public AudioSource moveAudio;
    public AudioClip[] tankAudio;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveAudio.volume = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        // 重生无敌状态
        if (isDefended) {
            defendEffectPrefab.SetActive(true);
            defendTimeVal -= Time.deltaTime;
            if (defendTimeVal <= 0) {
                isDefended = false;
                defendEffectPrefab.SetActive(false);
            }
        }

        // 攻击CD
        if (timeVal >= 0.4f) {
            Attack();
        }
        else {
            timeVal += Time.deltaTime;
        }

        
    }
    private void FixedUpdate() {
        if (PlayerManager.Instance.isDefeat) {
            return;
        }
        Move();
    }

    private void Attack() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Instantiate(bulletPrefeb, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
            timeVal = 0;
        }
    }

    private void Move() {
        float h = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);   // 以世界坐标轴移动
        if (h < 0) {
            sr.sprite = tankSprite[3];
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0) {
            sr.sprite = tankSprite[1];
            bulletEulerAngles = new Vector3(0, 0, -90);
        }

        if (Mathf.Abs(h) > 0.05f) {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying) {
                moveAudio.Play();
            }
        }
        else {
            moveAudio.clip = tankAudio[0];
            if (!moveAudio.isPlaying) {
                moveAudio.Play();
            }
        }

        if (h != 0) {    // 设置优先级
            return;
        }

        float v = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);
        if (v < 0) {
            sr.sprite = tankSprite[2];
            bulletEulerAngles = new Vector3(0, 0, 180);
        }
        else if (v > 0) {
            sr.sprite = tankSprite[0];
            bulletEulerAngles = new Vector3(0, 0,  0);
        }

        if (Mathf.Abs(v) > 0.05f) {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying) {
                moveAudio.Play();
            }
        }
        else {
            moveAudio.clip = tankAudio[0];
            if (!moveAudio.isPlaying) {
                moveAudio.Play();
            }
        }
    }

    private void Die() {

        if (isDefended) {
            return;
        }

        PlayerManager.Instance.isDead = true;

        // 爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // 死亡
        Destroy(gameObject);
    }
}
