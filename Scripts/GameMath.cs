using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMath
{
    public static Quaternion GlobalQuatToLocal(Transform parent, Quaternion globalQuat)
    {
        Quaternion parentQuaternion = Quaternion.Inverse(parent.rotation);
        Quaternion localQuat = parentQuaternion * globalQuat;
        return localQuat;
    }

    public static Vector2[] ConvertDouble2ToVector2(double[,] doubleArray)
    {
        int length = doubleArray.Length / 2;
        Vector2[] result = new Vector2[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = new Vector2((float)doubleArray[i, 0], (float)doubleArray[i, 1]);
        }
        
        return result;
    }

    public static Vector3[] ConvertVector2ArrayToVector3Array(Vector2[] array)
    {
        Vector3[] result = new Vector3[array.Length];
        for (int i = 0; i < array.Length; i++) result[i] = new Vector3(array[i].x, 0f, array[i].y);
        return result;
    }
}
