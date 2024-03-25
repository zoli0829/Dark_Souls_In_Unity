using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class LeftLegModelChanger : MonoBehaviour
    {
        public List<GameObject> leftLegModels;

        private void Awake()
        {
            GetAllLeftLegModels();
        }

        private void GetAllLeftLegModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                leftLegModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllLeftLegModels()
        {
            foreach (GameObject item in leftLegModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipLeftLegModelByName(string leftLegName)
        {
            for (int i = 0; i < leftLegModels.Count; i++)
            {
                if (leftLegModels[i].name == leftLegName)
                {
                    leftLegModels[i].SetActive(true);
                }
            }
        }
    }
}
