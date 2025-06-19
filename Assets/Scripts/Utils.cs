using UnityEngine;

public static class  Utils
{

    public static Vector3 GridToWorld(Vector2Int MapPos, float cellSize, int mapWidth, int mapHeight)
    {
        float worldWidth = LeverManager.main.worldWidth;
        float menuWidth = LeverManager.main.menuWidth;
        float offsetX = -worldWidth / 2 + (menuWidth / Screen.width) * worldWidth + cellSize / 2;
        float offsetY = -(mapHeight * cellSize) / 2 + cellSize / 2;

        return new Vector3(offsetX + MapPos.x * cellSize, offsetY + MapPos.y * cellSize, 0);
    }

    public static Vector2Int WorldToGrid(Vector3 worldPos, float cellSize, int mapWidth, int mapHeight)
    {
        float worldWidth = LeverManager.main.worldWidth;
        float menuWidth = LeverManager.main.menuWidth;
        float offsetX = -worldWidth / 2 + (menuWidth / Screen.width) * worldWidth + cellSize / 2;
        float offsetY = -(mapHeight * cellSize) / 2 + cellSize / 2;

        int x = Mathf.RoundToInt((worldPos.x - offsetX) / cellSize);
        int y = Mathf.RoundToInt((worldPos.y - offsetY) / cellSize);

        return new Vector2Int(x, y);
    }
}
