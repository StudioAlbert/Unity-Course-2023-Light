using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCGDungeon
{
    public static class MapPainter
    {
        public static void MapPaint(IEnumerable<Vector2Int> positions, Tilemap map, TileBase tile)
        {
            foreach (Vector2Int position in positions)
            {
                CellPaint(position, map, tile);
            }

        }

        public static void MapPaint(IEnumerable<Vector2Int> positions, Tilemap map, RuleTile tile)
        {
            foreach (Vector2Int position in positions)
            {
                CellPaint(position, map, tile);
            }
        }

        public static void CellPaint(Vector2Int position, Tilemap map, TileBase tile)
        {
            var mapPosition = map.WorldToCell((Vector3Int)position);
            map.SetTile(mapPosition, tile);
        }

        public static void CellPaint(Vector2Int position, Tilemap map, TileBase tile, Color color)
        {
            var mapPosition = map.WorldToCell((Vector3Int)position);
            map.SetTile(mapPosition, tile);
            map.SetColor(mapPosition, color);
        }

        public static void CellPaint(Vector2Int position, Tilemap map, RuleTile tile)
        {
            var mapPosition = map.WorldToCell((Vector3Int)position);
            map.SetTile(mapPosition, tile);
        }
        
        public static TileBase PickGradientColor(List<TileBase> gradient, int index)
        {
            int gradientIdx = index % gradient.Count;
            return gradient[gradientIdx];
        }
        public static TileBase PickGradientColorClamp(List<TileBase> gradient, int index)
        {
            if (gradient.Count > 0)
            {
                if (index < 0)
                    return gradient[0];
                else if (index < gradient.Count)
                    return gradient[index];
                else
                    return gradient[gradient.Count - 1];
            }
            else
            {
                return new Tile();
            }
        }
    }
}
