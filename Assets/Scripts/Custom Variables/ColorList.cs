using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ColorList", menuName ="Scriptable Objects/Settings/Color List")]
public class ColorList : ScriptableObject
{
    public List<Color> Colors;
}
