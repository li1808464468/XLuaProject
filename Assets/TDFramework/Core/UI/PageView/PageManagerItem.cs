using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDFramework.TDUI
{
    public class PageManagerItem : MonoBehaviour
    {

        public GameObject LabelGameObject;
    
        public GameObject LabelText1;

        public GameObject LabelText2;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }


        public void SetLabelGameObject(bool value)
        {
        
            LabelGameObject.SetActive(value);
        
        }

        public void SetLabelText(string str)
        {
            
            setLabelText(LabelText1, str);
            setLabelText(LabelText2, str);
        }

        public void SetLabelTextDisable(bool value)
        {
        
        
            if (value)
            {
               setLabelDisable(LabelText1, false);
               setLabelDisable(LabelText2, true);
                
            
            }
            else
            {
                
                setLabelDisable(LabelText1, true);
                setLabelDisable(LabelText2, false);
            }
        }

        private void setLabelText(GameObject label, string str)
        {
            if (label == null)
            {
                return;
            }
            if (label.GetComponent<Text>())
            {
                label.GetComponent<Text>().text = str;
            }
            else if (label.GetComponent<TextMeshProUGUI>())
            {
                label.GetComponent<TextMeshProUGUI>().text = str;
            }

        }

        private void setLabelDisable(GameObject label, bool value)
        {
            if (label == null)
            {
                return;
            }
            
            label.SetActive(value);
        }


    }

}
