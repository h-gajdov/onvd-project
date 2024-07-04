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
}
