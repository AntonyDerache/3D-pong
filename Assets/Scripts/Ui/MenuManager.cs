using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start() => AudioManager.instance.PlayMenuMusic();

    public void LaunchGame() => SceneManager.LoadScene("Arena");

    public void OnApplicationQuit() => Application.Quit();
}
