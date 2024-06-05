using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SwNavComp
{
    namespace HLEditor
    {
        


        public class HyperlaneListPresenter : MonoBehaviour
        {

            [SerializeField] HyperLaneRuntimeSet hyperLaneList;
            [SerializeField] TMP_Dropdown dropdown;


            public void AddOptions()
            {
                if (hyperLaneList == null && hyperLaneList.Count() == 0) return;
                List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();

                for (int i = 0; i < hyperLaneList.Count(); i++)
                {
                    print(hyperLaneList.Get(i).name);
                    TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(hyperLaneList.Get(i).name);
                    optionDataList.Add(option);
                }
                dropdown.AddOptions(optionDataList);
            }
        }
    }
}
