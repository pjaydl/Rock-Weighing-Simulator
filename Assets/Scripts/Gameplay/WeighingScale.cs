using System.Collections.Generic; // Provides collection classes such as HashSet.
using TMPro;                      // Allows use of TextMeshPro UI components.
using UnityEngine;                // Provides Unity's core engine functionality.

// The WeighingScale class is responsible for detecting rocks placed
// on the weighing scale, calculating their combined weight, and
// displaying the total weight on the user interface.
//
// This script should be attached to the weighing scale GameObject,
// which must contain a Trigger Collider. Whenever a rock enters or
// leaves the trigger area, the displayed weight is automatically updated.
public class WeighingScale : MonoBehaviour
{
    // Reference to the TextMeshPro text component that displays
    // the current total weight on the screen.
    //
    // [SerializeField] allows this private field to appear in the
    // Unity Inspector so it can be assigned without making it public.
    [SerializeField] private TMP_Text weightText;

    // Stores the measurement unit displayed after the numeric value.
    //
    // Example:
    // 5.0 kg
    //
    // This can easily be changed in the Inspector if another unit
    // (such as "g" or "lbs") is preferred.
    [SerializeField] private string unitSuffix = "kg";

    // Stores every rock currently resting on the weighing scale.
    //
    // A HashSet is used because:
    // • It automatically prevents duplicate entries.
    // • Searching, adding, and removing objects is very efficient.
    //
    // Each rock is identified by its Transform component,
    // ensuring the same rock cannot be counted twice.
    private readonly HashSet<Transform> rocksOnScale = new HashSet<Transform>();

    // Stores the current combined weight of all rocks on the scale.
    //
    // This value increases when a rock enters the trigger
    // and decreases when a rock exits.
    private float totalWeightKg;

    // Start() is called once before the first frame update.
    //
    // It initializes the display so the UI immediately shows
    // the correct starting weight (normally 0.0 kg).
    private void Start()
    {
        UpdateDisplay();
    }

    // Automatically called by Unity when another Collider enters
    // this object's Trigger Collider.
    //
    // This method checks whether the entering object is a rock,
    // and if so, adds its weight to the total.
    private void OnTriggerEnter(Collider other)
    {
        // Attempt to locate a RockWeight component.
        //
        // If no RockWeight component exists,
        // immediately exit the function.
        if (!TryGetRockWeight(other, out var rockWeight, out var rockTransform))
            return;

        // Add the rock's Transform into the HashSet.
        //
        // HashSet.Add() returns:
        // true  -> rock was not previously on the scale
        // false -> rock already exists in the collection
        //
        // This prevents duplicate weight calculations.
        if (rocksOnScale.Add(rockTransform))
        {
            // Increase the total weight using the value
            // stored in the RockWeight component.
            totalWeightKg += rockWeight.WeightInKg;

            // Refresh the UI so the new total is displayed.
            UpdateDisplay();
        }
    }

    // Automatically called by Unity when a Collider exits
    // this object's Trigger Collider.
    //
    // If a rock leaves the weighing area, its weight is
    // subtracted from the running total.
    private void OnTriggerExit(Collider other)
    {
        // Attempt to retrieve the RockWeight component.
        //
        // If the object is not a rock, stop executing.
        if (!TryGetRockWeight(other, out var rockWeight, out var rockTransform))
            return;

        // Remove the rock from the HashSet.
        //
        // HashSet.Remove() returns:
        // true  -> rock existed and was removed
        // false -> rock was never stored
        if (rocksOnScale.Remove(rockTransform))
        {
            // Subtract the rock's weight from the total.
            totalWeightKg -= rockWeight.WeightInKg;

            // Update the displayed weight.
            UpdateDisplay();
        }
    }

    // Attempts to locate a RockWeight component from the collider
    // involved in the trigger event.
    //
    // Parameters:
    // other          -> Collider involved in the trigger event.
    // rockWeight     -> Returns the RockWeight component if found.
    // rockTransform  -> Returns the Transform of that rock.
    //
    // Returns:
    // true  -> RockWeight component found.
    // false -> Object is not a valid rock.
    private bool TryGetRockWeight(Collider other, out RockWeight rockWeight, out Transform rockTransform)
    {
        // First, search the collider's parent hierarchy.
        //
        // This is useful when the collider belongs to a child object
        // while the RockWeight script is attached to the parent object.
        rockWeight = other.GetComponentInParent<RockWeight>();

        // If no parent component was found,
        // check the collider's own GameObject.
        if (rockWeight == null)
        {
            rockWeight = other.GetComponent<RockWeight>();
        }

        // If a RockWeight component exists,
        // retrieve its Transform.
        //
        // Otherwise assign null.
        rockTransform = rockWeight != null ? rockWeight.transform : null;

        // Return true only when a valid RockWeight component exists.
        return rockWeight != null;
    }

    // Updates the weight displayed on the user interface.
    //
    // This method is called whenever the total weight changes,
    // ensuring the UI always reflects the latest value.
    private void UpdateDisplay()
    {
        // Verify that a TextMeshPro UI object has been assigned.
        if (weightText != null)
        {
            // Display the total weight using one decimal place.
            //
            // Format:
            // {totalWeightKg:0.0}
            //
            // Examples:
            // 0.0 kg
            // 3.5 kg
            // 12.8 kg
            //
            // The unit suffix is appended after the number.
            weightText.text = $"{totalWeightKg:0.0} {unitSuffix}";
        }
        else
        {
            // Display a warning in the Unity Console if the
            // UI Text component has not been assigned.
            //
            // This helps identify setup mistakes during development.
            Debug.LogWarning("Weight Text is not assigned to the Weighing Scale.");
        }
    }
}