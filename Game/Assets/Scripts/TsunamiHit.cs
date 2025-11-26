using UnityEngine;
using UnityEngine.SceneManagement;

public class TsunamiHit : MonoBehaviour
{
    public string GameoverSceneName = "GameOverScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(GameoverSceneName);
        }
    }
}
