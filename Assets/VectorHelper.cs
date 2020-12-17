using UnityEngine;

public static class VectorHelper
{
    public static Vector3 RoundToInt(Vector3 vector, float gridSize = 1)
    {
        Vector3  returnVector = new Vector3(0,0,0);
        for (int i = 0; i < 3; i++)
        {
            returnVector[i] = Mathf.Round(vector[i]/gridSize)*gridSize;
        }
        return returnVector;
    }
}
