using UnityEngine;

public class RockWeight : MonoBehaviour
{
    [SerializeField] private float weightInKg = 1f;

    public float WeightInKg => weightInKg;
}