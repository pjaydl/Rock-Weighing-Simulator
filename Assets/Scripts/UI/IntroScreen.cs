using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private string nextScene = "MainGame";

    private void Start()
    {
        Debug.Log("Intro Started");
        Invoke(nameof(LoadNextScene), duration);
    }

    private void LoadNextScene()
    {
        Debug.Log("Loading Scene: " + nextScene);
        SceneManager.LoadScene(nextScene);
    }
}