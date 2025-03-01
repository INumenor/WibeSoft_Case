using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour
{
    
         public bool Placed {  get; private set; }
         public BoundsInt area;

    public bool CanBePlaced()
    {
        Vector3Int positionInt = TilemapGridSystem.Instance.GridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (TilemapGridSystem.Instance.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = TilemapGridSystem.Instance.GridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        TilemapGridSystem.Instance.TakeArea(areaTemp);
    }
}
