using UnityEngine;                    // Provides Unity's core engine functionality.
using UnityEngine.SceneManagement;    // Provides functions for loading and managing scenes.
using UnityEngine.InputSystem;        // Enables use of Unity's New Input System for keyboard input.

// The ResetGame class allows the user to restart the current scene
// by pressing the R key.
//
// Reloading the active scene returns all GameObjects to their
// original state, making it useful for restarting the simulation
// without closing and reopening the application.
//
// This script should be attached to an active GameObject
// within the scene.
public class ResetGame : MonoBehaviour
{
    // Update() is automatically called once every rendered frame.
    //
    // It continuously monitors the keyboard for the R key.
    private void Update()
    {
        // Detect whether the R key was pressed during
        // the current frame.
        //
        // wasPressedThisFrame becomes true only once when
        // the key transitions from released to pressed,
        // preventing the scene from being reloaded repeatedly
        // while the key is held down.
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            // Restart the current scene.
            ResetScene();
        }
    }

    // Reloads the currently active scene.
    //
    // Reloading the scene resets all GameObjects,
    // scripts, physics objects, UI elements, and variables
    // to their original state as defined when the scene
    // was first loaded.
    private void ResetScene()
    {
        // Retrieve information about the scene
        // that is currently active.
        //
        // Scene is a Unity structure containing
        // properties such as the scene's name,
        // build index, and loading status.
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the current scene using its name.
        //
        // Loading the active scene again effectively
        // restarts the simulation from the beginning.
        //
        // Example:
        // If the current scene is named "MainGame",
        // Unity reloads "MainGame".
        SceneManager.LoadScene(currentScene.name);
    }
}