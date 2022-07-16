using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Planet", menuName = "Scriptable Objects/Planet")]
public class Planet : ScriptableObject
{
    public string Name;
    public int CoordX;
    public int CoordY;

    public List<string> HyperlaneRoutes;

}
