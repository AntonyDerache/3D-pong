using System.Collections;
using UnityEngine;

public enum PlayerSide
{
    NONE,
    RIGHT,
    LEFT,
}

public class PlayerMovements : MonoBehaviour
{
    [HideInInspector] public int additionalLife = 0;
    [HideInInspector] public int hasImmuneBonus = 0;
    [HideInInspector] public bool hasSlow = false;
    public float speed = 1f;
    [SerializeField] private PlayerSide _playerSide;
    private float _vertical;
    private int _wallLayer;
    private GameManager _gm;
    private Vector3 _initialPosition;

    void Start()
    {
        _wallLayer = ~LayerMask.NameToLayer("Wall");
        GameObject obj = GameObject.Find("GameManager");
        if (obj) {
            _gm = obj.gameObject.GetComponent<GameManager>();
        } else {
            Debug.LogError("GameManager is missing in the scene");
        }
        _initialPosition = transform.position;
    }

    void Update() => MovePlayer();

    void MovePlayer()
    {
        if (!_gm.isPlaying)
            return;
        _vertical = PlayerInputs.GetAxis(_playerSide);
        if (CheckIfCanMove()) {
            if (hasSlow) {
                gameObject.transform.position += new Vector3(0, 0, (_vertical * Time.deltaTime) * (speed - 6));
            } else {
                gameObject.transform.position += new Vector3(0, 0, (_vertical * Time.deltaTime) * speed);
            }
        }
    }

    bool CheckIfCanMove()
    {
        float distance = 2.1f;
        Vector3 dir1 = Vector3.forward;
        Vector3 dir2 = Vector3.back;

        if (_vertical > 0 && Physics.Raycast(transform.position, dir1, distance, _wallLayer)) {
            return false;
        }
        if (_vertical < 0 && Physics.Raycast(transform.position, dir2, distance, _wallLayer)) {
            return false;
        }
        return true;
    }

    public IEnumerator ResetPosition()
    {
        float t = 0;
        while (t < .5f) {
            yield return null;
            t += Time.deltaTime / .5f;
            transform.position = Vector3.Lerp(transform.position, _initialPosition, t);
        }
        transform.position = _initialPosition;
    }
}
