using System;
using UnityEngine;

[Serializable]
public class Vector2Reference
{
    public bool UseConstant = true;
    public Vector2 ConstantValue;
    public Vector2Variable Variable;

    public Vector2 Value
    {
        get
        {
            return UseConstant ? ConstantValue :
                                  Variable.Value;
        }
    }
}
