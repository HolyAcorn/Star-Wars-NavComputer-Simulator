using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int height;
    public int width;
    private Vector2 step;

    public GameObject prefab;
    public GameEvent gameEvent;

    private SpriteRenderer spriteRenderer;
    private Sprite sprite;

    [SerializeField] FloatReference currentCameraSize;
    [SerializeField] Sprite[] sprites;

    [SerializeField] GameEvent ReadJson;
    [SerializeField] GameEvent LoadHyperLanes;
    
    [SerializeField] StyleSettings styleSettings;
 
    private void Start()
    {
        spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        if (sprite != null)
        {
            step = new Vector2(sprite.texture.width, sprite.texture.height) / sprite.pixelsPerUnit;
        }
        gameEvent.Raise();
        ReadJson.Raise();
        LoadHyperLanes.Raise();
        
    }

    public void GenerateGrid()
    {
        transform.position = new Vector2(width * -40, height * -20);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cell = Instantiate(prefab);
                cell.GetComponent<SpriteRenderer>().color = styleSettings.GridColor;
                cell.name = "grid_" + x + y;
                cell.transform.parent = transform;
                cell.transform.localPosition = new Vector2(step.x * x, step.y * y);
                
            }
        }
    }

    public void CheckCameraSize()
    {
        if (currentCameraSize.Value < 29.0f)
        {
            ChangeSize(0);
        }
        if (currentCameraSize.Value > 29.0f && currentCameraSize.Value < 49.0f)
        {
            ChangeSize(1);
        }
        if (currentCameraSize.Value > 49.0f && currentCameraSize.Value < 69.0f)
        {
            ChangeSize(2);
        }
        if (currentCameraSize.Value > 69.0f)
        {
            ChangeSize(3);
        }

    }

    private void ChangeSize(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.GetComponent<SpriteRenderer>().sprite = sprites[index];
        }
    }
}
