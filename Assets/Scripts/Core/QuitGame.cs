using UnityEngine;                   // Provides Unity's core engine functionality.
using UnityEngine.InputSystem;       // Enables use of Unity's New Input System for keyboard input.

// The QuitGame class allows the user to exit the application
// by pressing the Escape (Esc) key.
//
// During development inside the Unity Editor, the script stops
// Play Mode instead of closing the Unity Editor.
//
// In a built (compiled) application, it closes the program
// completely.
public class QuitGame : MonoBehaviour
{
    // Update() is called once every rendered frame.
    //
    // It continuously checks whether the Escape key
    // has been pressed.
    void Update()
    {
        // First, verify that a keyboard device exists.
        //
        // Keyboard.current will be null if no keyboard
        // is detected by the Input System.
        //
        // Next, check whether the Escape key was pressed
        // during the current frame.
        //
        // wasPressedThisFrame returns true only once,
        // preventing the quit function from being called
        // repeatedly while the key remains held down.
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Execute the method responsible for
            // closing the application.
            TriggerQuit();
        }
    }

    // Handles quitting the application.
    //
    // The behavior depends on where the project is running.
    private void TriggerQuit()
    {
        // UNITY_EDITOR is a compiler directive provided by Unity.
        //
        // Code inside this block is compiled only when the project
        // is running inside the Unity Editor.
        //
        // This allows testing the quit functionality without
        // creating a standalone build.
#if UNITY_EDITOR

        // Stop Play Mode inside the Unity Editor.
        //
        // Instead of closing Unity itself, this simply ends
        // the current simulation and returns to Edit Mode.
        UnityEditor.EditorApplication.isPlaying = false;

#else

        // When running as a standalone application,
        // close the program.
        //
        // Application.Quit() has no visible effect while
        // inside the Unity Editor, which is why the
        // compiler directive above is necessary.
        Application.Quit();

#endif
    }
}