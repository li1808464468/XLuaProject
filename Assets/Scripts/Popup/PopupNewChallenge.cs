using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PopupNewChallenge : MonoBehaviour
{

    public GameObject popup;
    public UISprite icon;

    public UIAtlas popupAtlas;
    // Start is called before the first frame update
    void Start()
    {
        icon.GetComponent<UIWidget>().alpha = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClick()
    {
        Debug.LogWarning("111111111111111");
        popup.gameObject.SetActive(false);
    }


    public void ChangeImage()
    {
        icon.spriteName = "img_game_play";

        icon.MakePixelPerfect();

    }
}
