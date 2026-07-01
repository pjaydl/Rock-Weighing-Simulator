using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ResetGame : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetScene();
        }
    }

    private void ResetScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}