using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeighingScale : MonoBehaviour
{
    [SerializeField] private TMP_Text weightText;
    [SerializeField] private string unitSuffix = "kg";

    private readonly HashSet<Transform> rocksOnScale = new HashSet<Transform>();
    private float totalWeightKg;

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!TryGetRockWeight(other, out var rockWeight, out var rockTransform))
            return;

        if (rocksOnScale.Add(rockTransform))
        {
            totalWeightKg += rockWeight.WeightInKg;
            UpdateDisplay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!TryGetRockWeight(other, out var rockWeight, out var rockTransform))
            return;

        if (rocksOnScale.Remove(rockTransform))
        {
            totalWeightKg -= rockWeight.WeightInKg;
            UpdateDisplay();
        }
    }

    private bool TryGetRockWeight(Collider other, out RockWeight rockWeight, out Transform rockTransform)
    {
        rockWeight = other.GetComponentInParent<RockWeight>();

        if (rockWeight == null)
        {
            rockWeight = other.GetComponent<RockWeight>();
        }

        rockTransform = rockWeight != null ? rockWeight.transform : null;
        return rockWeight != null;
    }

    private void UpdateDisplay()
    {
        if (weightText != null)
        {
            weightText.text = $"{totalWeightKg:0.0} {unitSuffix}";
        }
        else
        {
            Debug.LogWarning("Weight Text is not assigned to the Weighing Scale.");
        }
    }
}