using UnityEngine;                   // Provides Unity's core engine functionality.
using UnityEngine.SceneManagement;   // Provides access to Unity's scene management system.

// The IntroScreen class controls the introductory or splash screen
// shown when the application starts.
//
// Its primary responsibility is to display the intro scene for a
// specified amount of time before automatically loading the next scene.
//
// This script should be attached to a GameObject in the Intro Scene.
public class IntroScreen : MonoBehaviour
{
    // Determines how long the intro screen remains visible
    // before transitioning to the next scene.
    //
    // [SerializeField] allows this private variable to be edited
    // directly in the Unity Inspector without exposing it publicly.
    //
    // Default value:
    // 3f = 3 seconds
    [SerializeField] private float duration = 3f;

    // Stores the name of the scene that will be loaded
    // after the intro screen finishes.
    //
    // The value entered here must exactly match the scene name
    // included in the Build Settings.
    //
    // Default:
    // "MainScene" = the scene named "MainScene" will be loaded next.
    [SerializeField] private string nextScene = "MainScene";

    // Start() is automatically called by Unity once,
    // just before the first frame is rendered.
    //
    // It begins the countdown for loading the next scene.
    private void Start()
    {
        // Print a message to the Unity Console.
        //
        // This is useful for debugging and confirming that
        // the intro scene has successfully started.
        Debug.Log("Intro Started");

        // Schedule the LoadNextScene() method to execute
        // after the number of seconds specified by 'duration'.
        //
        // nameof(LoadNextScene) is used instead of writing
        // "LoadNextScene" as a string because:
        //
        // • It reduces typing mistakes.
        // • The compiler checks that the method exists.
        // • Renaming the method automatically updates this reference.
        Invoke(nameof(LoadNextScene), duration);
    }

    // Loads the next scene after the intro timer expires.
    //
    // This method is called automatically by Invoke().
    private void LoadNextScene()
    {
        // Print a message to the Unity Console indicating
        // which scene is about to be loaded.
        //
        // Example output:
        // Loading Scene: MainGame
        Debug.Log("Loading Scene: " + nextScene);

        // Load the specified scene immediately.
        //
        // SceneManager.LoadScene() unloads the current scene
        // and replaces it with the scene whose name matches
        // the value stored in 'nextScene'.
        //
        // The scene must be added to the Build Settings,
        // otherwise Unity will generate an error.
        SceneManager.LoadScene(nextScene);
    }
}