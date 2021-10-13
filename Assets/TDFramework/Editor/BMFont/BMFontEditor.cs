using System;
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// 使用ShoeBox 将美术字转为xml和对应的图片
public class BMFontEditor : EditorWindow
{
	[MenuItem("Tools/BMFont Maker")]
	static public void OpenBMFontMaker()
	{
		EditorWindow.GetWindow<BMFontEditor>(false, "BMFont Maker", true).Show();
	}

	[SerializeField]
	private Font targetFont;
	[SerializeField]
	private TextAsset fntData;
	[SerializeField]
	private Material fontMaterial;
	[SerializeField]
	private Texture2D fontTexture;

	private BMFont bmFont = new BMFont();
	private string debugLog = "";

	public BMFontEditor()
	{
	}

	void OnGUI()
	{
		GUILayout.Space(10);
		fntData = EditorGUILayout.ObjectField("Fnt Data", fntData, typeof(TextAsset), false) as TextAsset;
		GUILayout.Space(10);
		fontTexture = EditorGUILayout.ObjectField("Font Texture", fontTexture, typeof(Texture2D), false) as Texture2D;
		
		GUILayout.Space(15);
		if (GUILayout.Button("Create BMFont"))
		{
			this.CreateFont();
		}
		
		
		GUILayout.Space(100);
		GUI.skin.label.fontSize = 12;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label(debugLog);
	}


	private void CreateFont()
	{
		if (fntData == null)
		{
			debugLog = "Target font Data is null!";
			return;
		}

		if (fontTexture == null)
		{
			debugLog = "Target font texture is null!";
			return;
		}
		
		var path = AssetDatabase.GetAssetPath(fntData);
		if (!System.IO.File.Exists(path))
		{
			debugLog = "Path not font!";
			return;
		}

		path = path.Substring(0, path.LastIndexOf("/"));
		
		fontMaterial = new Material(Shader.Find("GUI/Text Shader"));
		fontMaterial.name = fntData.name;
		ProjectWindowUtil.CreateAsset (fontMaterial, path + "/"+ fontMaterial.name + ".mat");
		fontMaterial.mainTexture = fontTexture;
		
		targetFont = new Font(fntData.name);
		ProjectWindowUtil.CreateAsset (targetFont, path + "/"+targetFont.name + ".fontsettings");
		targetFont.material = fontMaterial;
		
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(fntData.text);
 
 
		int totalWidth = Convert.ToInt32(xmlDocument["font"]["common"].Attributes["scaleW"].InnerText);
		int totalHeight = Convert.ToInt32(xmlDocument["font"]["common"].Attributes["scaleH"].InnerText);
 
		XmlElement xml = xmlDocument["font"]["chars"];
		ArrayList characterInfoList = new ArrayList();
 
 
		for(int i = 0; i < xml.ChildNodes.Count; ++i)
		{
			XmlNode node = xml.ChildNodes[i];
			if(node.Attributes == null)
			{
				continue;
			}
			int index = Convert.ToInt32(node.Attributes["id"].InnerText);
			int x = Convert.ToInt32(node.Attributes["x"].InnerText);
			int y = Convert.ToInt32(node.Attributes["y"].InnerText);
			int width = Convert.ToInt32(node.Attributes["width"].InnerText);
			int height = Convert.ToInt32(node.Attributes["height"].InnerText);
			int xOffset = Convert.ToInt32(node.Attributes["xoffset"].InnerText);
			int yOffset = Convert.ToInt32(node.Attributes["yoffset"].InnerText);
			int xAdvance = Convert.ToInt32(node.Attributes["xadvance"].InnerText);
			CharacterInfo info = new CharacterInfo();
			Rect uv = new Rect();
			uv.x = (float)x / totalWidth;
			uv.y = (float)(totalHeight - y - height) / totalHeight;
			uv.width = (float)width / totalWidth;
			uv.height = (float)height / totalHeight;
			info.index = index;
			info.uvBottomLeft = new Vector2(uv.xMin, uv.yMin);
			info.uvBottomRight = new Vector2(uv.xMax, uv.yMin);
			info.uvTopLeft = new Vector2(uv.xMin, uv.yMax);
			info.uvTopRight = new Vector2(uv.xMax, uv.yMax);
			info.minX = xOffset;
			info.maxX = xOffset + width;
			info.minY = -yOffset - height;
			info.maxY = -yOffset;
			info.advance = xAdvance;
			info.glyphWidth = width;
			info.glyphHeight = height;
			characterInfoList.Add(info);
		}
		targetFont.characterInfo = characterInfoList.ToArray(typeof(CharacterInfo)) as CharacterInfo[];
		

		debugLog = "create font <" + targetFont.name + "> success";
		
		Debug.Log("create font <" + targetFont.name + "> success!");

		Close();
	}
	
}
