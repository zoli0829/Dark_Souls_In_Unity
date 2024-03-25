using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class LowerLeftArmModelChanger : MonoBehaviour
    {
        public List<GameObject> armModels;

        private void Awake()
        {
            GetAllArmModels();
        }

        private void GetAllArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                armModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllArmModels()
        {
            foreach (GameObject item in armModels)
            {
                item.SetActive(false);
            }
        }

        public void EquiparmModelByName(string armName)
        {
            for (int i = 0; i < armModels.Count; i++)
            {
                if (armModels[i].name == armName)
                {
                    armModels[i].SetActive(true);
                }
            }
        }
    }
}
