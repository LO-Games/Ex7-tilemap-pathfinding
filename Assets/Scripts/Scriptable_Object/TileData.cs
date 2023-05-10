using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "New Tile Data", menuName = "Tile Data")]
public class TileData : ScriptableObject
{
    public bool isWalkable = true;
    public TileBase tile;
}