using UnityEngine;

public class GameOverSceneUi : MonoBehaviour
{
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
