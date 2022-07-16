using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Hyperdrive", menuName = "Scriptable Objects/Objects/Hyperdrive")]
public class HyperDrive : ScriptableObject
{
    public string Name;
    [Range(0f, 15f)]
    public float Class;
}
