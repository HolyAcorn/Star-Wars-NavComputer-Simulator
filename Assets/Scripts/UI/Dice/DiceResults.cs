using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using System;
using static UnityEngine.UI.Button;
using UnityEngine.Events;

namespace SwNavComp
{
    public class DiceResults : MonoBehaviour
    {
        [SerializeField] DiceResultsRuntimeSet diceResultsRuntimeSet;
        [SerializeField] GameObject resultPanel;
        [SerializeField] GameObject resultPrefab;
        [SerializeField] Sprite[] resultSprites;

        private void Start()
        {
            ClearList();
            gameObject.SetActive(false);
        }

        DiceValue GetDiceValue(int index)
        {
            return (DiceValue)index;
        }

        public void AddToList(int index)
        {
            DiceValue diceValue = GetDiceValue(index);
            diceResultsRuntimeSet.Add(diceValue, true);

            PresentResults();
        }

        public void RemoveFromList(int index)
        {
            DiceValue diceValue = GetDiceValue(index);
            diceResultsRuntimeSet.Remove(diceValue);
            PresentResults();
        }

        public void ClearList()
        {
            diceResultsRuntimeSet.Clear();
            PresentResults();
        }



        private void PresentResults()
        {

            for (int i = resultPanel.transform.childCount-1; i >= 0; i--)
            {
                Destroy(resultPanel.transform.GetChild(i).gameObject);
            }

            foreach (DiceValue result in diceResultsRuntimeSet.items)
            {
                PresentOne(result);
            }


            void PresentOne(DiceValue diceRollResult)
            {
                GameObject instance = Instantiate(resultPrefab);
                instance.name = diceRollResult.ToString();
                instance.GetComponent<Image>().sprite = resultSprites[(int)diceRollResult - 1];
                instance.transform.parent = resultPanel.transform;
                instance.transform.localScale = Vector3.one;
            }
        }


    }
}
