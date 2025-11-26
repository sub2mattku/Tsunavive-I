using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Button PlayButton;

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }
}
