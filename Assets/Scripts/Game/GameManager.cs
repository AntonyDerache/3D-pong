using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isPlaying = false;
    [SerializeField] private PlayerMovements _leftPlayer;
    [SerializeField] private PlayerMovements _rightPlayer;
    [SerializeField] private BallMovements _ball;
    [SerializeField] private CameraShake _cam;
    private UiManager _uiManager;
    private int _rightScore;
    private int _leftScore;
    private Dictionary<string, Action<PlayerSide>> _powerupsAction = new Dictionary<string, Action<PlayerSide>>();
    private bool _isCountdownActive = false;
    private Coroutine _hideCursor;
    private Action _resetObjectsOnField = null;

    private void Awake()
    {
        GameObject canvasManager = GameObject.Find("CanvasManager");
        if (canvasManager) {
            _uiManager = canvasManager.GetComponent<UiManager>();
        }
        _powerupsAction.Add("Boost", Boost);
        _powerupsAction.Add("Death", Death);
        _powerupsAction.Add("Health", Health);
        _powerupsAction.Add("Heart", Heart);
        _powerupsAction.Add("Random", Random);
    }

    void Start() {
        if (AudioManager.instance) {
            AudioManager.instance.PlayGameMusic();
        }
        StartCoroutine(CounterBeforeStart());
    }

    void Update() {
        if (Input.GetAxis("Mouse X") == 0 && (Input.GetAxis("Mouse Y") == 0)) {
            if (_hideCursor == null) {
                _hideCursor = StartCoroutine(HideCursor());
            }
        } else {
            if (_hideCursor != null) {
                StopCoroutine(_hideCursor);
                _hideCursor = null;
                Cursor.visible = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _uiManager.ShowPauseMenu();
            PauseGame();
        }
    }

    private IEnumerator HideCursor()
    {
        yield return new WaitForSeconds(2f);
        Cursor.visible = false;
    }

    private IEnumerator CounterBeforeStart()
    {
        _isCountdownActive = true;
        yield return StartCoroutine(_uiManager.ActiveCountdown());
        _isCountdownActive = false;
        isPlaying = true;
    }

    public void IncreaseScore(int amount, PlayerSide side)
    {
        StartCoroutine(_cam.Shake(1f, .5f));
        if (AudioManager.instance) {
            AudioManager.instance.PlaySfxScoreSound();
        }
        if (side == PlayerSide.RIGHT) {
            Debug.Log("Right scored");
            Debug.Log("left additional life:" + _leftPlayer.additionalLife);
            if (_leftPlayer.additionalLife == 0) {
                _rightScore += amount;
                _uiManager.UpdateRightScore(_rightScore.ToString());
            } else {
                _leftPlayer.additionalLife--;
                _uiManager.UpdateLeftScore(_leftScore.ToString());
            }
        } else if (side == PlayerSide.LEFT) {
            Debug.Log("Left scored");
            Debug.Log("right additional life:" + _rightPlayer.additionalLife);
            if (_rightPlayer.additionalLife == 0) {
                _leftScore += amount;
                _uiManager.UpdateLeftScore(_leftScore.ToString());
            } else {
                _rightPlayer.additionalLife--;
                _uiManager.UpdateLeftScore(_rightScore.ToString());
            }
        }
        Reset();
    }

    private void Reset()
    {
        isPlaying = false;
        StartCoroutine(_leftPlayer.ResetPosition());
        StartCoroutine(_rightPlayer.ResetPosition());
        ResetPowerups();
        StartCoroutine(CounterBeforeStart());
        if (_resetObjectsOnField != null) _resetObjectsOnField();
        _ball.SetVelocity(Vector3.zero);
    }

    public void ActivePowerups(string powerupTag, PlayerSide playerSide) => _powerupsAction[powerupTag](playerSide);

    private void Boost(PlayerSide playerSide)
    {
        _ball.hasSpeedBonus = true;
        StartCoroutine(StopBoostSpeed());
    }

    private void Death(PlayerSide playerSide)
    {
        if (playerSide == PlayerSide.RIGHT) {
            if (_leftPlayer.hasImmuneBonus > 0) {
                _leftPlayer.hasImmuneBonus--;
                _uiManager.UpdateLeftHealth(_leftPlayer.hasImmuneBonus.ToString());
            } else {
                _leftPlayer.hasSlow = true;
            }
        } else {
            if (_rightPlayer.hasImmuneBonus > 0) {
                _rightPlayer.hasImmuneBonus--;
                _uiManager.UpdateRightHealth(_rightPlayer.hasImmuneBonus.ToString());
            } else {
                _rightPlayer.hasSlow = true;
            }
        }
        StartCoroutine(StopSlowMalus(playerSide));
    }

    private void Heart(PlayerSide playerSide)
    {
        if (playerSide == PlayerSide.RIGHT) {
            _rightPlayer.additionalLife++;
            _uiManager.UpdateRightHeart(_rightPlayer.additionalLife.ToString());
        } else if (playerSide == PlayerSide.LEFT){
            _leftPlayer.additionalLife++;
            _uiManager.UpdateLeftHeart(_leftPlayer.additionalLife.ToString());
        }
    }

    private void Health(PlayerSide playerSide)
    {
        if (playerSide == PlayerSide.RIGHT) {
            _rightPlayer.hasImmuneBonus++;
            _uiManager.UpdateRightHealth(_rightPlayer.hasImmuneBonus.ToString());
        } else if (playerSide == PlayerSide.LEFT) {
            _leftPlayer.hasImmuneBonus++;
            _uiManager.UpdateLeftHealth(_leftPlayer.hasImmuneBonus.ToString());
        }
    }

    private void Random(PlayerSide playerSide)
    {
        int rdmValue = UnityEngine.Random.Range(0, 4);
        switch (rdmValue) {
            case 0:
                Boost(playerSide);
                break;
            case 1:
                Death(playerSide);
                break;
            case 2:
                Health(playerSide);
                break;
            case 3:
                Heart(playerSide);
                break;
            default:
                break;
        }
    }

    private IEnumerator StopBoostSpeed()
    {
        yield return new WaitForSeconds(10f);
        _ball.hasSpeedBonus = false;
    }

    private IEnumerator StopSlowMalus(PlayerSide side)
    {
        yield return new WaitForSeconds(5f);
        if (side == PlayerSide.RIGHT) {
            _leftPlayer.hasSlow = false;
        } else if (side == PlayerSide.LEFT) {
            _rightPlayer.hasSlow = false;
        }
    }

    private void ResetPowerups()
    {
        _ball.hasSpeedBonus = false;
        _rightPlayer.hasSlow = false;
        _rightPlayer.additionalLife = 0;
        _rightPlayer.hasImmuneBonus = 0;
        _uiManager.UpdateRightHeart(_rightPlayer.additionalLife.ToString());
        _uiManager.UpdateRightHealth(_rightPlayer.hasImmuneBonus.ToString());
        _leftPlayer.hasSlow = false;
        _leftPlayer.additionalLife = 0;
        _leftPlayer.hasImmuneBonus = 0;
        _uiManager.UpdateLeftHeart(_leftPlayer.additionalLife.ToString());
        _uiManager.UpdateLeftHealth(_leftPlayer.hasImmuneBonus.ToString());
    }

    public void PauseGame()
    {
        isPlaying = false;
        _uiManager.isPaused = true;
    }

    public void ResumeGame()
    {
        if (!_isCountdownActive) {
            isPlaying = true;
        }
        _uiManager.isPaused = false;
    }

    public void SetResetObjectsOnField(Action func)
    {
        _resetObjectsOnField = func;
    }
}
