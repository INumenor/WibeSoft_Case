using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TilemapGridSystem : MonoBehaviour
{
    public static TilemapGridSystem Instance;

    public GridLayout GridLayout;
    public Tilemap MainTileMap;
    public Tilemap TempTileMap;

    private static Dictionary<TileType,TileBase> tileBases = new Dictionary<TileType,TileBase>();

    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White,Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green,Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red,Resources.Load<TileBase>(tilePath + "red"));
        foreach (TileBase tile in tileBases.Values)
        {
            Debug.Log(tile.name + " " +tile.GetInstanceID());
        }
    }

    private static TileBase[] GetTilesBlock(BoundsInt area,Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x,v.y,0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }
    private static void SetTilesBlock(BoundsInt area,TileType type ,Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray,type);
        tilemap.SetTilesBlock(area, tileArray);
    }
    private static void FillTiles(TileBase[] tileArray,TileType type)
    {
        for (int i = 0; i < tileArray.Length;i++)
        {
            tileArray[i] = tileBases[type];
        }
    }

    public void InitializeWithBuilding(GameObject building)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        temp = Instantiate(building, worldPosition, Quaternion.identity).GetComponent<Building>();
        FollowBuilding();
    }

    public void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTileMap.SetTilesBlock(prevArea,toClear);
    }
    public void FollowBuilding()
    {
        ClearArea();

        temp.area.position = GridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;
        TileBase[] baseArray = GetTilesBlock(buildingArea,MainTileMap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];
        
        for (int i = 0; i < size; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }
        TempTileMap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTileMap);
        foreach (TileBase tileBase in baseArray)
        {
            if(tileBase != tileBases[TileType.White])
            {
                return false;
            }
        }
        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTileMap);
        SetTilesBlock(area, TileType.Green, MainTileMap);
    }

    private void Update()
    {
        if(!temp)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }
            if (!temp.Placed)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = GridLayout.LocalToCell(touchPos);

                if(prevPos != cellPos)
                {
                    temp.transform.localPosition = GridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f,.5f,0));
                    prevPos = cellPos;  
                    FollowBuilding();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearArea();
            Destroy(temp.gameObject);
        }
    }
}

public enum TileType
{
    Empty,
    White,
    Green,
    Red
}
