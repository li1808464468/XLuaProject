using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TDFramework.TDUI
{
    public class TDUI
    {
        
        private const string kUILayerName = "UI";
        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kMaskPath = "UI/Skin/UIMask.psd";
        private const string kBackgroundSpriteResourcePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
        
        private static GameObject SecurityCheck()
        {
            GameObject gameObject;
            var canvas =  Object.FindObjectOfType<Canvas>();
            if (!canvas)
            {
                gameObject = NewGameObject("Canvas",typeof(Canvas));
                if (!Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>())
                {
                    GameObject eventSystem = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem));
                }
            }
            else
            {
                gameObject = canvas.gameObject;
            }
    
            return gameObject;
    
        }
    
        #region Page View
        
    
        [MenuItem("GameObject/UI/TDUI/Vertical Page View")]
        public static void CreateVerticalPageView(MenuCommand menuCommand)
        {
            CreatePageView(menuCommand, true, false);
        }
        
        [MenuItem("GameObject/UI/TDUI/Horizontal Page View")]
        public static void CreateHorizontalPageView(MenuCommand menuCommand)
        {
            CreatePageView(menuCommand, false, true);
        }
    
        private static void CreatePageView(MenuCommand menuCommand, bool vertical, bool horizontal)
        {
            Vector2 contentSize = new Vector2(400, 200);
            string gameObjectName = "Page View";
            if (vertical)
            {
                gameObjectName = "Vertical Page View";
                contentSize = new Vector2(200, 400);
            }
            else
            {
                gameObjectName = "Horizontal Page View";
            }
            
            GameObject pageView = NewGameObject(gameObjectName, typeof(ScrollRect), typeof(Image), typeof(PageView));
            pageView.GetComponent<RectTransform>().sizeDelta = contentSize;
            Image image = pageView.GetComponent<Image>();
            image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
            image.type = Image.Type.Sliced;
            
            ScrollRect scrollRect = pageView.GetComponent<ScrollRect>();
            scrollRect.horizontal = horizontal;
            scrollRect.vertical = vertical;
    
            if (vertical)
            {
                pageView.GetComponent<PageView>().direction = PageView.ScrollDirection.Vertical;
            }
            else
            {
                pageView.GetComponent<PageView>().direction = PageView.ScrollDirection.Horizontal;
            }
    
    
            GameObject viewPort = NewGameObject("View Port", typeof(Mask));
            Image viewImage =  viewPort.AddComponent<Image>();
            viewImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPath);
            viewImage.type = Image.Type.Sliced;
            
            RectTransform viewPortTransform =  viewPort.GetComponent<RectTransform>();
            ChangeParent(pageView.transform, viewPortTransform);
            
            GameObject content = NewGameObject("Content", typeof(RectTransform));
            ChangeParent(viewPort.transform, content.transform);
            RectTransform conteTransform = content.GetComponent<RectTransform>();
            conteTransform.sizeDelta = contentSize;
            
            viewPortTransform.pivot = new Vector2(0, 1);
            viewPortTransform.anchorMin = Vector2.zero;
            viewPortTransform.anchorMax = Vector2.one;
            viewPortTransform.offsetMin = Vector2.zero;
            viewPortTransform.offsetMax = Vector2.zero;
            viewPort.GetComponent<Mask>().showMaskGraphic = false;
            
            
            GameObject testImage = NewGameObject("TestImage", typeof(Image));
            ChangeParent(content.transform, testImage.transform);
            RectTransform testImageTransform = testImage.GetComponent<RectTransform>();
            testImageTransform.sizeDelta = contentSize - new Vector2(10, 5);
            testImage.GetComponent<Image>().color = new Color(0.62f,0.62f,0.62f, 1);
    
            GameObject testLabel = NewGameObject("TestLabel", typeof(RectTransform));
            ChangeParent(testImage.transform, testLabel.transform);
            Text label = testLabel.AddComponent<Text>();
            label.text = "Item_0";
            label.fontSize = 40;
            RectTransform labelRectTransform = label.GetComponent<RectTransform>();
            labelRectTransform.sizeDelta = new Vector2(120, 50);
    
            scrollRect.content = conteTransform;
            scrollRect.viewport = viewPortTransform;
            pageView.GetComponent<PageView>()._content = conteTransform;
            
            
            
            AddCanvas(pageView);
            
            GameObject pageManager = CreatePageManager(menuCommand);
            pageManager.GetComponent<PageManager>().pageView = pageView.GetComponent<PageView>();
            pageManager.transform.localPosition =  new Vector3(0, - contentSize.y , 0);
            if (vertical)
            {
                pageManager.GetComponent<PageManager>().direction = PageManager.PageManageType.Vertical;
                pageManager.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 400);
                pageManager.transform.localPosition = new Vector3(300, 0);
            }
            else
            {
                pageManager.GetComponent<PageManager>().direction = PageManager.PageManageType.Horizontal;
            }
        }
        
        
        
        #endregion
    
    
        #region PageManager
        
        [MenuItem("GameObject/UI/TDUI/PageManager")]
        public static GameObject CreatePageManager(MenuCommand menuCommand)
        {
            
            GameObject pageManager =
                NewGameObject("PageManager", typeof(ToggleGroup), typeof(RectTransform), typeof(PageManager));
            pageManager.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 100);
    
            
            
            GameObject toggleObject = NewGameObject("Toggle");
            RectTransform rectTransform = toggleObject.AddComponent<RectTransform>();
            PageManagerItem managerItem = toggleObject.AddComponent<PageManagerItem>();
            rectTransform.sizeDelta = new Vector2(50, 50);
            Toggle toggle = toggleObject.AddComponent<Toggle>();
            toggle.isOn = true;
            toggle.group = pageManager.GetComponent<ToggleGroup>();
            Selectable selectable = toggle.GetComponent<Selectable>();
            var selectableColors = selectable.colors;
            selectableColors.disabledColor = Color.white;
            selectable.colors = selectableColors;
    
            ChangeParent(pageManager.transform, toggle.transform);
            
            GameObject background = NewGameObject("Background", typeof(Image));
            Image backgroundImage = background.GetComponent<Image>();
            backgroundImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
            backgroundImage.type = Image.Type.Sliced;
            backgroundImage.GetComponent<RectTransform>().sizeDelta = rectTransform.sizeDelta;
            ChangeParent(toggleObject.transform, background.transform);
            toggle.targetGraphic = backgroundImage;
            
            
            GameObject checkmark = NewGameObject("Checkmark", typeof(Image));
            Image checkmarkImage = checkmark.GetComponent<Image>();
            checkmarkImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);
            checkmarkImage.GetComponent<RectTransform>().sizeDelta = rectTransform.sizeDelta;
            ChangeParent(background.transform, checkmark.transform);
            toggle.graphic = checkmarkImage;
    
    
            GameObject labelGameObject = NewGameObject("LabelGameObject", typeof(RectTransform));
            ChangeParent(toggleObject.transform, labelGameObject.transform);
            managerItem.LabelGameObject = labelGameObject;
            
            GameObject label1 = NewGameObject("Label1", typeof(Text));
            ChangeParent(labelGameObject.transform, label1.transform);
            managerItem.LabelText1 = label1;
            
            GameObject label2 = NewGameObject("Label2", typeof(Text));
            ChangeParent(labelGameObject.transform, label2.transform);
            label2.GetComponent<Text>().color = Color.red;
            managerItem.LabelText2 = label2;
            
            label2.SetActive(false);
            labelGameObject.SetActive(false);
    
            pageManager.GetComponent<PageManager>().toggle = toggleObject;
            
            AddCanvas(pageManager);
    
    
            return pageManager;
        }
        
        #endregion
        
     
        
        [MenuItem("GameObject/UI/Text")]
        public static void CreateTDText(MenuCommand menuCommand)
        {
            GameObject text = NewGameObject("Text", typeof(Text));
            text.GetComponent<Text>().raycastTarget = false;
            text.GetComponent<Text>().text = "New Text";
            AddCanvas(text);
        }

        [MenuItem("GameObject/UI/Text - TextMeshPro")]
        public static void CreateTDTextPro(MenuCommand menuCommand)
        {
            GameObject textPro = NewGameObject("Text (TMP)", typeof(TextMeshProUGUI));
            textPro.GetComponent<TextMeshProUGUI>().raycastTarget = false;
            AddCanvas(textPro);
        }
        
        [MenuItem("GameObject/UI/TDUI/Button")]
        public static void CreateMyButton(MenuCommand menuCommand)
        {

            GameObject gameObject = new GameObject("Button", typeof(Image),typeof(Button),typeof(ButtonManager));
        
            Button button = gameObject.GetComponent<Button>();
            // 设置点击动画为空
            button.transition = Selectable.Transition.None;
            AddCanvas(gameObject);

        }
        
        
        
        private static GameObject NewGameObject(string name, params System.Type[] components)
        {
    
            GameObject gameObject =  new GameObject(name, components);
            gameObject.layer = LayerMask.NameToLayer(kUILayerName);
            return gameObject;
        }
    
        private static void AddCanvas(GameObject gameObject)
        {
    
            var canvas = SecurityCheck();
            Transform parent = canvas.transform;
            if (!Selection.activeTransform)
            {
                gameObject.transform.SetParent(canvas.transform);
            }
            else
            {
                if (!Selection.activeTransform.GetComponentInParent<Canvas>())
                {
                    gameObject.transform.SetParent(canvas.transform);
                }
                else
                {
                    gameObject.transform.SetParent(Selection.activeTransform);
                    parent = Selection.activeTransform;
    
                }
            }
            Selection.activeGameObject = gameObject;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;
            
            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Undo.SetTransformParent(gameObject.transform, parent, "Parent " + gameObject.name);
        }
    
    
        private static void ChangeParent(Transform parent, Transform child)
        {
            child.SetParent(parent,true);
            child.gameObject.layer = parent.gameObject.layer;
        }
        
        
        
    
    }
    
}


