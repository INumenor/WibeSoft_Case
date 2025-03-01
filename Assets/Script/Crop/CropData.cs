using UnityEngine;

[CreateAssetMenu(fileName = "New Crop Data", menuName = "Crop System/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropName;
    public Sprite[] growthStages;
    public float growthTime;
    public int harvestReward;
}
