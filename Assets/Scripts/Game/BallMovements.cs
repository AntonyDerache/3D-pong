using UnityEngine;

public class BallMovements : MonoBehaviour
{
    public float speed = 1f;
    [HideInInspector] public bool hasSpeedBonus = false;
    [HideInInspector] public PlayerSide lastPlayerHit;
    [SerializeField] private GameObject explodeEffectPrefab;
    private Vector3 _dir;
    private GameManager _gm;
    private CameraShake _cam;
    private int mask;
    private int powerupsMask;
    private int BlockersMask;
    private Rigidbody rb;
    private float timeSinceRoundStart = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mask = ~LayerMask.GetMask("Ball", "Ignore Raycast");
        powerupsMask = LayerMask.NameToLayer("Powerups");
        BlockersMask = LayerMask.NameToLayer("Blocker");
        GameObject obj = GameObject.Find("GameManager");
        if (obj) {
            _gm = obj.gameObject.GetComponent<GameManager>();
        } else {
            Debug.LogError("GameManager is missing in the scene");
        }
        GameObject cam = GameObject.Find("Main Camera");
        if (obj) {
            _cam = cam.gameObject.GetComponent<CameraShake>();
        } else {
            Debug.LogError("GameManager is missing in the scene");
        }
    }

    void Start()
    {
        GetRandomizedDirection();
        rb.velocity = Vector3.zero;
    }

    void GetRandomizedDirection()
    {
        Vector3 newDir;
        if (UnityEngine.Random.value > .5f) {
            newDir = Vector3.left;
        } else {
            newDir = Vector3.right;
        }
        float angle = 0;
        while (angle < 0.1f && angle > -0.1f) {
            angle = UnityEngine.Random.Range(-0.50f, 0.50f);
        }
        newDir.z = angle;
        _dir = newDir;
    }

    private void Update() {
        if (!_gm.isPlaying)
            return;
        timeSinceRoundStart += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!_gm.isPlaying)
            return;
        Vector3 movements = Vector3.zero;
        if (hasSpeedBonus) {
            movements = _dir * Time.fixedDeltaTime * (speed + (timeSinceRoundStart * 10) + 250);
        } else {
            movements = _dir * Time.fixedDeltaTime * (speed + (timeSinceRoundStart * 10));
        }
        movements.y = 0;
        rb.velocity = movements;
    }

    private void OnCollisionEnter(Collision other) {
        switch (other.transform.tag) {
            case "RightGoal":
                lastPlayerHit = PlayerSide.NONE;
                HasScored(PlayerSide.RIGHT);
                break;
            case "LeftGoal":
                lastPlayerHit = PlayerSide.NONE;
                HasScored(PlayerSide.LEFT);
                break;
            case "LeftPlayer":
                lastPlayerHit = PlayerSide.LEFT;
                _dir = Vector3.Reflect(_dir, other.contacts[0].normal);
                if (AudioManager.instance) AudioManager.instance.PlaySfxBallSound();
                StartCoroutine(_cam.Shake(.2f, .05f));
                break;
            case "RightPlayer":
                lastPlayerHit = PlayerSide.RIGHT;
                _dir = Vector3.Reflect(_dir, other.contacts[0].normal);
                if (AudioManager.instance) AudioManager.instance.PlaySfxBallSound();
                StartCoroutine(_cam.Shake(.2f, .05f));
                break;
            default:
                if (other.transform.gameObject.layer == BlockersMask) {
                    Destroy(other.transform.gameObject);
                }
                _dir = Vector3.Reflect(_dir, other.contacts[0].normal);
                AudioManager.instance?.PlaySfxBallSound();
                StartCoroutine(_cam.Shake(.2f, .05f));
                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.layer == powerupsMask) {
            if (lastPlayerHit != PlayerSide.NONE) {
                Destroy(other.transform.gameObject);
                AudioManager.instance?.PlaySfxPowerupSound();
                _gm.ActivePowerups(other.transform.tag, lastPlayerHit);
            }
        }
    }

    private void HasScored(PlayerSide side)
    {
        GameObject obj = Instantiate(explodeEffectPrefab, transform.position, Quaternion.identity);
        Destroy(obj, 1f);
        transform.position = new Vector3(0, transform.position.y, 0);
        _gm.IncreaseScore(1, side);
        timeSinceRoundStart = 0;
        GetRandomizedDirection();
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }
}
