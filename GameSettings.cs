using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public int rounds;
    public GameObject plane;
    public int diff;
    public bool showCountryName;

    public void SetProperties(int r, GameObject p, int d, bool s)
    {
        rounds = r;
        plane = p;
        diff = d;
        showCountryName = s;
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}