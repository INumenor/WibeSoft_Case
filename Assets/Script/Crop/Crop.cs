using UnityEngine;
using System.Collections;

public class Crop : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public int currentStage = 0;
    public bool isPlanted = false;

    private CropData _cropData;

    public void Plant(CropData cropData)
    {
        if (!isPlanted)
        {
            _cropData = cropData;
            isPlanted = true;
            spriteRenderer.sprite = _cropData.growthStages[0];
            StartCoroutine(GrowCrop());
        }
    }

    private IEnumerator GrowCrop()
    {
        while (currentStage < _cropData.growthStages.Length - 1)
        {
            yield return new WaitForSeconds(_cropData.growthTime / _cropData.growthStages.Length);
            currentStage++;
            spriteRenderer.sprite = _cropData.growthStages[currentStage];
        }
    }

    public bool IsFullyGrown()
    {
        return currentStage == _cropData.growthStages.Length - 1;
    }

    public void Harvest()
    {
        if (IsFullyGrown())
        {
            Debug.Log("Congrulations : " + _cropData.harvestReward);
            spriteRenderer.sprite = null;
            isPlanted = false;
            currentStage = 0;
            _cropData = null;
        }
    }

    private void OnMouseDown()
    {
        if (IsFullyGrown())
        {
            Harvest();
        }
    }
}
