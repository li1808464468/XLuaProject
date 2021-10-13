using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

namespace TDFramework.TDUI
{
    public class PageManager : MonoBehaviour
    {
        public enum PageManageType
        {
            Horizontal,
            Vertical
        }
    
        public enum ToggleType
        {
            Image,
            Label
        }
        
        public bool Interactable;
        public PageManageType direction = PageManageType.Horizontal;
        public ToggleType toggleType = ToggleType.Image;
        
        
        public PageView pageView;

        


        public GameObject toggle;
        private List<Toggle> m_PaginationChildren = new List<Toggle>();


        private void OnDestroy()
        {
            if (pageView)
            {
                pageView.OnPageChanged -= SetToggleGraphics;
                pageView.AddItem -= AddToggleItem;
                pageView.RemoveItem -= RemoveToggleItem;
            }
        }
    
    
        // Start is called before the first frame update
        void Start()
        {
            this.addLayoutGroup();
            
    
            if (pageView == null || toggle == null)
            {
                return;
            }
            
            this.addChildItem();
            
    
            pageView.OnPageChanged += SetToggleGraphics;
            pageView.AddItem += AddToggleItem;
            pageView.RemoveItem += RemoveToggleItem;

        }

        private void addChildItem()
        {
            
            List<GameObject> listGameObjects = new List<GameObject>();
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                listGameObjects.Add(transform.GetChild(i).gameObject);
            }



            for (int i = 0; i < pageView.ChildObjects.Length + pageView._content.childCount; i++)
            {

                GameObject gameObject = createToggleItem();
                if (i == 0)
                {
                    ChangeToggleState(gameObject.GetComponent<Toggle>(), true);
                }
                else
                {
                    ChangeToggleState(gameObject.GetComponent<Toggle>(), false);
                }


            }

            
            foreach (var gameObject in listGameObjects)
            {
                Destroy(gameObject);
            }
            listGameObjects.Clear();
        }
        
        
        private void addLayoutGroup()
        {

            if (gameObject.GetComponent<LayoutGroup>())
            {
                return;
            }
            
            
            
            if (direction == PageManageType.Horizontal)
            {
                HorizontalLayoutGroup layoutGorup = gameObject.AddComponent<HorizontalLayoutGroup>();
                layoutGorup.childControlHeight = false;
                layoutGorup.childControlWidth = false;
                layoutGorup.childAlignment = TextAnchor.MiddleCenter;
            }
            else
            {
                VerticalLayoutGroup layoutGorup = gameObject.AddComponent<VerticalLayoutGroup>();
                layoutGorup.childControlHeight = false;
                layoutGorup.childControlWidth = false;
                layoutGorup.childAlignment = TextAnchor.MiddleCenter;
                gameObject.transform.localEulerAngles = new Vector3(180, 0, 0);
            }
            
            
            
        }

        private GameObject createToggleItem()
        {
            GameObject gameObject = Instantiate(this.toggle);
            Toggle toggle = gameObject.GetComponent<Toggle>();
            toggle.group = GetComponent<ToggleGroup>();
            gameObject.transform.SetParent(transform);
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;
            toggle.onValueChanged.AddListener(ToggleClick);
            toggle.isOn = false;
            if (!Interactable)
            {
                toggle.interactable = false;
            }
            
            if (toggleType != ToggleType.Image)
            {
                Transform background = gameObject.transform.Find("Background");
                background.GetComponent<Image>().color = new Color(1,1,1,0);

                Transform check = gameObject.transform.Find("Background/Checkmark");
                check.GetComponent<Image>().color = new Color(1,1,1,0);
                
                gameObject.GetComponent<PageManagerItem>().SetLabelGameObject(true);
                gameObject.GetComponent<PageManagerItem>().SetLabelText((m_PaginationChildren.Count + 1).ToString());
            }

            
            m_PaginationChildren.Add(toggle);
            return gameObject;
        }

        private void ChangeToggleState(Toggle toggle, bool value)
        {

            if (value)
            {
                toggle.isOn = true;
            }
            
            
            if (toggleType != ToggleType.Label)
            {
                return;
            }
            
       
            toggle.GetComponent<PageManagerItem>().SetLabelTextDisable(value);


        }
        
        
        private void SetToggleGraphics(int pageNo)
        {


            for (int i = 0; i < m_PaginationChildren.Count; i++)
            {
                if (i == pageNo)
                {
                    ChangeToggleState(m_PaginationChildren[i], true);
                }
                else
                {
                    ChangeToggleState(m_PaginationChildren[i], false);
                }
            }
            
       
        }
        
        private void ToggleClick(bool toggle)
        {
            if (toggle)
            {
                for (int i = 0; i < m_PaginationChildren.Count; i++)
                {
                    if (m_PaginationChildren[i].isOn)
                    {
                        pageView.PageTo(i,true);
                        break;
                    }
                }
            }
        }


        private void AddToggleItem()
        {
            this.createToggleItem();
            if (m_PaginationChildren.Count == 1)
            {
                m_PaginationChildren[0].isOn = true;
            }
        }


        private void RemoveToggleItem(int index)
        {
            if (index >= m_PaginationChildren.Count || m_PaginationChildren.Count == 0)
            {
                return;
            }
            Destroy(m_PaginationChildren[index].gameObject);
            m_PaginationChildren.Remove(m_PaginationChildren[index]);
        }
        
    }

}

