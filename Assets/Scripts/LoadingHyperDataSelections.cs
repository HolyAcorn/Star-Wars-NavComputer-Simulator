using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadingHyperDataSelections : MonoBehaviour
{

    public StringRuntimeSet hyperLaneDataOptions;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/StreamingAssets/Data/";
        string[] files = Directory.GetFiles(path);
        foreach (string file in files) Debug.Log(file);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
