using UnityEngine;
using UnityEngine.EventSystems;

public class CropPlacer : MonoBehaviour
{
    public GameObject cropPrefab;
    private GameObject currentCrop;
    private bool isDragging = false;
    public CropData cropData;

    public void StartDragging()
    {
        isDragging = true;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        currentCrop = Instantiate(cropPrefab, worldPosition, Quaternion.identity);
    }

    public void StopDragging()
    {
        isDragging = false;
        DropCrop();
    }

    void Update()
    {
        if (isDragging && currentCrop != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            currentCrop.transform.position = worldPosition;
        }
    }

    private void DropCrop()
    {
        Collider2D hit = Physics2D.OverlapPoint(currentCrop.transform.position);
        if (hit != null && hit.CompareTag("FarmLand"))
        {
            Crop crop = hit.GetComponent<Crop>();
            if (crop != null)
            {
                crop.Plant(cropData); 
            }
        }
        else
        {
            Destroy(currentCrop);
        }

        Destroy(currentCrop);
        currentCrop = null;
    }
}
