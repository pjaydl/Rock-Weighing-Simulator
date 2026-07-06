using UnityEngine; // Provides access to Unity's core engine classes and MonoBehaviour.

// The RockWeight class stores the weight value of a rock.
//
// Every rock in the simulation should have this component attached.
// Other scripts (such as the weighing scale) can read this value
// to determine how much the rock contributes to the total weight.
//
// This class only stores data—it does not perform any calculations
// or physics operations.
public class RockWeight : MonoBehaviour
{
    // Stores the weight of the rock in kilograms.
    //
    // [SerializeField] makes this private variable visible in the
    // Unity Inspector while keeping it inaccessible to other classes.
    //
    // This allows each rock to have a different weight that can
    // easily be configured without modifying the source code.
    //
    // Default value:
    // 1f = 1 kilogram
    [SerializeField] private float weightInKg = 1f;

    // Public read-only property that provides access to the rock's weight.
    //
    // The expression-bodied property (=>) simply returns the value
    // stored in weightInKg.
    //
    // A property is used instead of making the variable public because:
    // • Other scripts can read the weight.
    // • Other scripts cannot accidentally modify the weight.
    // • Data encapsulation is maintained.
    //
    // Example:
    // float weight = rock.WeightInKg;
    public float WeightInKg => weightInKg;
}