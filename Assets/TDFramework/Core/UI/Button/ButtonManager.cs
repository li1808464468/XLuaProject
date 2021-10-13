using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace TDFramework.TDUI
{
    
    public class ButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
    
        private enum ButtonAnimType
        { 
            Normal,
            Long,
            LongLong,
            Short,
        };
    
        private enum ButtonColorType
        {
            Normal,
            Black,
        }
        
        public Image _targetImage;
        
    
        // 按下动作类型
        [SerializeField] private ButtonAnimType _animType;
        // 按下颜色变化类型
        [SerializeField] private ButtonColorType _colorType;
        
        // [SerializeField] private Image _buttonImage;
        public Sprite _clickChangeSprite;
        private Sprite _normalSprite;
        private Image _changeImge;
        private Transform _mTransform;
        private bool _pointTag = false;
        private bool _disable;
        
    
        // Start is called before the first frame update
        void Start()
        {
            _mTransform = transform;
            _changeImge = transform.GetComponent<Image>();
            if (!_changeImge)
            {
                _changeImge = transform.GetComponentInChildren<Image>();
            }

        }
    
        
        public void AddEventTrigger(Transform insObject, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> myFunction)//泛型委托
        {
            
            EventTrigger trigger = insObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(myFunction);
            trigger.triggers.Add(entry);
        }
    
        public void OnPointerDown(PointerEventData eventData)
        {
    
            if (_disable)
            {
                return;
            }
            
            if (_pointTag)
            {
                return;
            }
    
            _pointTag = true;
            if (_targetImage && _clickChangeSprite)
            {
                _normalSprite = _targetImage.sprite;
                _targetImage.sprite = _clickChangeSprite;
                _targetImage.SetNativeSize();
            }
    
            _mTransform.DOKill();
            _mTransform.localScale = Vector3.one;
    
            
            
            if (_changeImge)
            {
                _changeImge.color = Color.white;
            }
            
            
            Vector3 scaleType  = new Vector3(0.95f, 0.95f, 1);
    
            switch (_animType)
            {
                case ButtonAnimType.Long:
                    scaleType.x = 0.98f;
    
                    break;
                case ButtonAnimType.LongLong:
                    scaleType.x = 0.99f;
                    scaleType.y = 0.94f;
                    
                    break;
                
                case ButtonAnimType.Short:
                    scaleType.x = 0.85f;
                    scaleType.y = 0.85f;
                    break;
            }
            
            
    
    
            _mTransform.DOScale(scaleType, 0.1f);
            
            Color colorType = new Color(0.819f, 0.737f, 0.737f);
    
            switch (_colorType)
            {
                case ButtonColorType.Black:
                    colorType = new Color(0.392f, 0.235f, 0.235f);
                    break;
            }
            
            
            if (_changeImge)
            {
                _changeImge.DOColor(colorType, 0.1f);
            }
    
        }
    
        public void OnPointerUp(PointerEventData eventData)
        {
    
            if (_disable)
            {
                return;
            }
    
            _pointTag = false;
    
            if (_targetImage && _clickChangeSprite)
            {
                _targetImage.sprite = _normalSprite;
                _targetImage.SetNativeSize();
            }
    
            _mTransform.DOScale(Vector3.one, 0.17f);
            if (_changeImge)
            {
                _changeImge.DOColor(Color.white, 0.16f);
            }
            
        }
    
        public void DisableButton()
        {
            _disable = true;
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.GetComponent<Button>().interactable = false;
    
            if (_targetImage)
            {
                _changeImge.DOKill();
            }
            
            
        }
    
    
        public void UnDisableButton()
        {
            _disable = false;
            transform.GetComponent<Button>().interactable = true;
        }
    }
}

