using UnityEngine;                   // Provides Unity's core engine functionality.
using UnityEngine.InputSystem;       // Enables use of Unity's New Input System for keyboard input.

// The HelpUI class controls the visibility of the Help Panel
// displayed during the simulation.
//
// Pressing the H key toggles the Help Panel between visible
// and hidden states, allowing users to quickly access
// instructions without permanently occupying screen space.
//
// This script should be attached to a GameObject responsible
// for managing the Help UI.
public class HelpUI : MonoBehaviour
{
    // Reference to the Help Panel GameObject.
    //
    // This panel typically contains instructions,
    // controls, or other helpful information for the user.
    //
    // [SerializeField] allows the panel to be assigned
    // through the Unity Inspector while keeping the
    // variable private.
    [SerializeField] private GameObject helpPanel;

    // Stores the current visibility state of the Help Panel.
    //
    // false = panel is hidden
    // true  = panel is visible
    //
    // This variable is updated each time the H key is pressed.
    private bool isOpen;

    // Start() is automatically called by Unity once,
    // before the first frame is rendered.
    //
    // It initializes the Help Panel so that it starts
    // in the hidden state when the application begins.
    private void Start()
    {
        // Disable the Help Panel.
        //
        // SetActive(false) makes the GameObject inactive,
        // preventing it from being rendered or interacted with.
        helpPanel.SetActive(false);
    }

    // Update() executes once every rendered frame.
    //
    // It continuously checks whether the user has pressed
    // the H key to toggle the Help Panel.
    private void Update()
    {
        // Detect the exact frame the H key is pressed.
        //
        // wasPressedThisFrame becomes true only once,
        // preventing repeated toggles while the key
        // remains held down.
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            // Change the visibility state of the Help Panel.
            ToggleHelp();
        }
    }

    // Toggles the Help Panel between visible and hidden.
    //
    // Each time this method is called:
    // • If the panel is hidden, it becomes visible.
    // • If the panel is visible, it becomes hidden.
    private void ToggleHelp()
    {
        // Reverse the current state.
        //
        // Example:
        // false -> true
        // true  -> false
        isOpen = !isOpen;

        // Apply the new state to the Help Panel.
        //
        // SetActive(true)  -> Show the panel.
        // SetActive(false) -> Hide the panel.
        helpPanel.SetActive(isOpen);
    }
}