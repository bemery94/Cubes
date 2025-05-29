
using System.Collections.Generic;
using UnityEngine;

public class ArrayUtils
{
    public static void FillConstant<T>(T[,,] array, T constant)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                for (int k = 0; k < array.GetLength(2); k++)
                {
                    array[i, j, k] = constant;
                }
            }
        }
    }
    
    public static bool SizeContains(Vector2Int size, Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < size.x && position.y < size.y;
    }
}
