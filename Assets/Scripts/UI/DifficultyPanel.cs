using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyPanel : MonoBehaviour
{
    [SerializeField] RawImage difficultyImage;
    [SerializeField] GameObject difficultyPanel;

    public void ToggleVisible(bool value) { gameObject.SetActive(value); }

    public void ToggleDamaged(bool value)
    {
        if (value) AddDifficultyDice();
        else RemoveDifficultyDice();
    }

    public void ToggleHastyCalculation(bool value)
    {
        if (value) AddDifficultyDice();
        else RemoveDifficultyDice();
    }

    private void AddDifficultyDice()
    {
        GameObject instance = Instantiate(difficultyImage.gameObject);
        instance.transform.parent = difficultyImage.transform.parent;
        instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y, 0);
        instance.transform.localScale = Vector3.one;
    }

    private void RemoveDifficultyDice()
    {
        Destroy(difficultyPanel.transform.GetChild(1).gameObject);
    }
}
