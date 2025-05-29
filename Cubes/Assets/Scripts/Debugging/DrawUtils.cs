using UnityEngine;

public class DrawUtils
{
    public static void DrawLine(Texture2D texture, Vector2 start, Vector2 end, Color color)
    {
        int x0 = Mathf.RoundToInt(start.x);
        int y0 = Mathf.RoundToInt(start.y);
        int x1 = Mathf.RoundToInt(end.x);
        int y1 = Mathf.RoundToInt(end.y);

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            texture.SetPixel(x0, y0, color);
            if (x0 == x1 && y0 == y1)
            {
                break;
            }

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        texture.Apply();
    }

    public static void DrawCircle(Texture2D texture, Vector2 position, int radius, Color color)
    {
        var numSegments = Mathf.CeilToInt(radius * Mathf.PI);

        Vector2 end = position + new Vector2(radius, 0.0f);

        for (int i = 0; i < numSegments; i++)
        {
            Vector2 start = end;
            float angle = 2f * Mathf.PI * (i + 1) / numSegments;
            end = position + new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
            DrawLine(texture, start, end, color);
        }
    }

    public static void DrawMaze(Texture2D texture, GridTopology maze, Color color, int edgeLength, Vector2Int offset,
        bool drawNodes = false)
    {
        int radius = 2;

        for (int y = 0; y < maze.Size().y; y++)
        {
            for (int x = 0; x < maze.Size().x; x++)
            {
                var start = edgeLength * new Vector2(x, y) + offset;
                if (drawNodes)
                {
                    DrawCircle(texture, start, radius, color);
                }


                if (x < maze.Size().x - 1 && maze.GetEdge(new Vector2Int(x, y), Direction.Right))
                {
                    var end = edgeLength * new Vector2(x + 1, y) + offset;
                    DrawLine(texture, start, end, color);
                }

                if (y < maze.Size().y - 1 && maze.GetEdge(new Vector2Int(x, y), Direction.Up))
                {
                    var end = edgeLength * new Vector2(x + 0, y + 1) + offset;
                    DrawLine(texture, start, end, color);
                }
            }
        }
    }

    public static Texture2D CreateMazeTexture(GridTopology maze, Color color, int edgeLength)
    {
        int width = (maze.Size().x + 1) * edgeLength;
        int height = (maze.Size().y + 1) * edgeLength;
        var texture = new Texture2D(width, height);

        Color[] fillColorArray = texture.GetPixels();

        for (int i = 0; i < fillColorArray.Length; i++)
        {
            fillColorArray[i] = Color.black;
        }

        int radius = 2;

        DrawMaze(texture, maze, color, edgeLength, new Vector2Int(edgeLength, edgeLength), true);

        return texture;
    }
}