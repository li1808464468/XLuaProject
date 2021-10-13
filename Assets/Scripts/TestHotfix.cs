using System.Collections;
using System.Collections.Generic;
using LuaFramework;
using UnityEngine;
using XLua;

namespace LuaFramework.Hotfix
{ 
//    [Hotfix]
    public class TestHotfix : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
//        LuaManager.Instance.LoadScript("HotfixScript");
        
            this.setFont();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public void setFont()
        {
            GetComponent<UILabel>().text = "11111111";
        }
    
    }


}
