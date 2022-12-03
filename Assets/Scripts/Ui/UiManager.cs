using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManager : MonoBehaviour
{
    [HideInInspector] public bool isPaused = false;
    [SerializeField] private TextMeshProUGUI _countdown;
    [SerializeField] private TextMeshProUGUI _rightCounter;
    [SerializeField] private TextMeshProUGUI _leftCounter;
    [SerializeField] private TextMeshProUGUI _rightHeartCounter;
    [SerializeField] private TextMeshProUGUI _leftHeartCounter;
    [SerializeField] private TextMeshProUGUI _rightHealthCounter;
    [SerializeField] private TextMeshProUGUI _leftHealthCounter;
    [SerializeField] private GameObject _pauseMenu;

    public IEnumerator ActiveCountdown()
    {
        _countdown.text = "3";
        _countdown.gameObject.SetActive(true);
        while (isPaused) yield return null;
        yield return new WaitForSeconds(.5f);
        _countdown.text = "2";
        while (isPaused) yield return null;
        yield return new WaitForSeconds(.5f);
        _countdown.text = "1";
        while (isPaused) yield return null;
        yield return new WaitForSeconds(.5f);
        _countdown.text = "GO";
        while (isPaused) yield return null;
        yield return new WaitForSeconds(.5f);
        while (isPaused) yield return null;
        _countdown.gameObject.SetActive(false);
    }

    public void UpdateRightScore(string text)
    {
        _rightCounter.text = text;
    }

    public void UpdateLeftScore(string text)
    {
        _leftCounter.text = text;
    }

    public void UpdateLeftHeart(string text)
    {
        _leftHeartCounter.text = text;
    }

    public void UpdateRightHeart(string text)
    {
        _rightHeartCounter.text = text;
    }

    public void UpdateLeftHealth(string text)
    {
        _leftHealthCounter.text = text;
    }

    public void UpdateRightHealth(string text)
    {
        _rightHealthCounter.text = text;
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Arena");
    }

    public void ShowPauseMenu()
    {
        _pauseMenu.SetActive(true);
    }
}
