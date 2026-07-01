using UnityEngine;
using UnityEngine.InputSystem;

public class HelpUI : MonoBehaviour
{
    [SerializeField] private GameObject helpPanel;

    private bool isOpen;


    private void Start()
    {
        helpPanel.SetActive(false);
    }


    private void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            ToggleHelp();
        }
    }


    private void ToggleHelp()
    {
        isOpen = !isOpen;

        helpPanel.SetActive(isOpen);
    }
}