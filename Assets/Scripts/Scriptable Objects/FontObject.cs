using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Font Object", menuName ="Scriptable Objects/Font/Font Object")]
public class FontObject : ScriptableObject
{
    public Font Font;
    public TMP_FontAsset fontAsset;
    public int Index;
}
