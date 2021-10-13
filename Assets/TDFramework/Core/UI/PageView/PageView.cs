using System;
using UnityEngine.UI;  
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace TDFramework.TDUI
{
    public class PageView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        
        public enum ScrollDirection
        {
            Horizontal,
            Vertical
        }
        
        private ScrollRect _scrollRect;    
        private RectTransform _rectTransform;
        //滑动的起始坐标  
        private float targethorizontal = 0;  
        private float _itemWidth;
        private float _itemHeight;
        //是否拖拽结束 
        private bool isDrag = false;  
        //求出每页的临界角，页索引从0开始
        private List<float> posList = new List<float>();     
        private int _currentPageIndex = -1;
        public Action<int> OnPageChanged;
        public Action AddItem;
        public Action<int> RemoveItem;

        public RectTransform _content;
        public int StartingScene = 0;
        private bool stopMove = true;
    //    [Header("滑动到指定页面速度")]
        public float SlidSpeed = 4;     
    //    [Header("滑动灵敏度判定")]
        public float Sensitivity = 0;
    //    [Header("滑动方向")]
        public ScrollDirection direction = ScrollDirection.Horizontal;
        private float startTime;
        private float startDragHorizontal;
        public GameObject[] ChildObjects;
        private List<GameObject> _childObjects = new List<GameObject>();
        
        void Start()
        {
            
            
            foreach (var obj in ChildObjects)
            {
                GameObject child = Instantiate(obj);
                child.transform.SetParent(_content);
                child.transform.localScale = Vector3.one;
                child.transform.localPosition = Vector3.zero;
            }
            
            
            for (int i = 0; i < _content.childCount; i++)
            {
                _childObjects.Add(_content.GetChild(i).gameObject);
            }
            
            _scrollRect = transform.GetComponent<ScrollRect>();
            
            
            _rectTransform = GetComponent<RectTransform>();
    
           
            _itemWidth = _rectTransform.rect.width;
            _itemHeight = _rectTransform.rect.height;
           
            
           
            if (direction == ScrollDirection.Horizontal)
            {
                _scrollRect.horizontal = true;
                _scrollRect.vertical = false;
                _content.pivot = new Vector2(0f, 0.5f);
                _content.anchorMin = new Vector2(0, 0.5f);
                _content.anchorMax = new Vector2(0, 0.5f);
                
            }
            else
            {
                _scrollRect.horizontal = false;
                _scrollRect.vertical = true;
                _content.pivot = new Vector2(0.5f, 0);
                _content.anchorMin = new Vector2(0.5f, 0f);
                _content.anchorMax = new Vector2(0.5f, 0f);
                
            }
    
    
            setItemDistance();
    
            if (StartingScene > 0)
            {
                PageTo(StartingScene, false);
            }
    
    
        }
    
    
    
        private void setItemDistance()
        {
    
            float horizontalLength = _itemWidth * _childObjects.Count  - _rectTransform.rect.width;
    
            if (direction == ScrollDirection.Horizontal)
            {
                _content.sizeDelta = new Vector2(_itemWidth * _childObjects.Count, _itemHeight);
            }
            else 
            { 
                _content.sizeDelta = new Vector2(_itemWidth, _itemHeight * _childObjects.Count);
                horizontalLength = _itemHeight * _childObjects.Count - _rectTransform.rect.height;
    
            }
            
            
            posList.Clear();
            for (int i = 0; i < _childObjects.Count; i++)
            {
                GameObject child = _childObjects[i];
                if (direction == ScrollDirection.Horizontal)
                {
                    setItemPosition(i, child.transform);
                    posList.Add(_itemWidth* i / horizontalLength);
    
                }
                else
                {
                    setItemPosition(i, child.transform);
                    posList.Add(_itemHeight * i / horizontalLength);
    
                }
                
            }
        }
    
        private void OnEnable()
        {
            
        }
    
        void setItemPosition(int index, Transform transform)
        {
            Vector3 childPositon = Vector3.zero;
    
            if (direction == ScrollDirection.Horizontal)
            {
    
                childPositon.x = _itemWidth * 0.5f + index * _itemWidth;
            }
            else
            {
                childPositon.y = _itemHeight * 0.5f + index * _itemHeight;
            }
    
            transform.localPosition = childPositon;
        }
    
    
        void Update()
        {
            if (!isDrag && !stopMove)
            {
                startTime += Time.deltaTime;
                float t = startTime * SlidSpeed;
                if (direction == ScrollDirection.Horizontal)
                {
                    _scrollRect.horizontalNormalizedPosition = Mathf.Lerp(_scrollRect.horizontalNormalizedPosition, targethorizontal, t);
                }
                else
                {
                    _scrollRect.verticalNormalizedPosition =
                        Mathf.Lerp(_scrollRect.verticalNormalizedPosition, targethorizontal, t);
                }
                
                
                if (t >= 1)
                    stopMove = true;
            }
        }
        
        
        public void PageTo(int index, bool slid)
        {
            if (index >= 0 && index < posList.Count)
            {
    
                if (!slid)
                {
                    if (direction == ScrollDirection.Horizontal)
                    {
                        _scrollRect.horizontalNormalizedPosition = posList[StartingScene];
                    }
                    else
                    {
                        _scrollRect.verticalNormalizedPosition = posList[StartingScene];
    
                    } 
                }
                else
                {
                    //            _scrollRect.horizontalNormalizedPosition = posList[index];
                    setPageIndex(index);
                    isDrag = false;
                    stopMove = false;
                    startTime = 0;
                    targethorizontal = posList[index];
                }
            }
        }
    
        private void setPageIndex(int index)
        {
            if (_currentPageIndex != index)
            {
                _currentPageIndex = index;
                this.changePageManagerView(index);
            }
        }


        private void changePageManagerView(int index)
        {
            if (OnPageChanged != null)
                OnPageChanged(index);
        }
    
    
        public void OnBeginDrag(PointerEventData eventData)
        {

            if (posList.Count == 0)
            {
                return;
            }
            
            isDrag = true;
            
            //开始拖动  
            startDragHorizontal = _scrollRect.horizontalNormalizedPosition;
            if (direction == ScrollDirection.Vertical)
            {
                startDragHorizontal = _scrollRect.verticalNormalizedPosition;
            }
            
        }
    
    
        public void OnEndDrag(PointerEventData eventData)
        {
            if (posList.Count == 0)
            {
                return;
            }
            
            float posX = _scrollRect.horizontalNormalizedPosition;
            if (direction == ScrollDirection.Vertical)
            {
                posX = _scrollRect.verticalNormalizedPosition;
            }
            posX += ((posX - startDragHorizontal) * Sensitivity);
            posX = posX < 1 ? posX : 1;
            posX = posX > 0 ? posX : 0;
            int index = 0;
            float offset = Mathf.Abs(posList[index] - posX);
            for (int i = 1; i < posList.Count; i++)
            {
                float temp = Mathf.Abs(posList[i] - posX);
             
                if (temp < offset)
                {
                    index = i;
                    offset = temp;
                }
    
            }
    
    
            PageTo(index, true);
    
        }
        
        /// <summary>
        /// 添加一个新的Item 到末尾
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddChild(GameObject gameObject)
        {
            
            gameObject.transform.SetParent(_content);
            gameObject.transform.localScale = Vector3.one;
            _childObjects.Add(gameObject);
            
            setItemDistance();

            if (AddItem != null)
            {
                AddItem();
            }
    
        }


        public void RemoveChild(int index, out GameObject gameObject)
        {
            index = _childObjects.Count - 1;
            
            gameObject = null;
            if (index >= _childObjects.Count || _childObjects.Count == 0)
            {
                return;
            }
            
            
            gameObject = _childObjects[index];
            gameObject.transform.SetParent(null);
            _childObjects.Remove(gameObject);
            setItemDistance();
            if (RemoveItem != null)
            {
                RemoveItem(index);
            }

            if (_currentPageIndex >= _childObjects.Count)
            {
                _currentPageIndex--;
            }
            
            this.changePageManagerView(_currentPageIndex);
        }
    
    
    }
    
    
    
}
