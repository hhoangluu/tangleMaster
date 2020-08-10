// --
// The MIT License
// 
// Copyright (c) 2013 tatsuhiko yamamura
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ---
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using com.F4A.MobileThird.MiniJSON;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;

public class ImageToUnitySprites : AssetPostprocessor
{
	static void OnPostprocessAllAssets (
		string[] importedAssets, 
		string[] deletedAssets, 
		string[] movedAssets, 
		string[] movedFromAssetPaths)
	{
		foreach (var assetPath in importedAssets) {
            string extension = Path.GetExtension(assetPath);
            if (extension.Equals(".plist"))
            {
                var pngPath = assetPath.Substring(0, assetPath.Length - 6) + ".png";
                if (File.Exists(pngPath))
                    SplitSpriteWithPlist(pngPath, assetPath);
            }
            //else if (extension.Equals(".txt"))
            //{
            //    TextAsset json = AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset)) as TextAsset;
            //    var pngPath = assetPath.Substring(0, assetPath.Length - 4);
            //    if (File.Exists(pngPath))
            //        SplitSpriteWithJson(pngPath, json.text);

            //}
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imagePath"></param>
    /// <param name="plistPath"></param>
    static void SplitSpriteWithPlist(string imagePath, string plistPath)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(new StreamReader(plistPath));
        var frames = xmlDocument.DocumentElement.SelectSingleNode("dict").SelectNodes("dict")[0];
        var dictInfoFrame = frames.SelectNodes("dict");
        var keys = frames.SelectNodes("key");
        int len = keys.Count;

        TextureImporter importer = AssetImporter.GetAtPath(imagePath) as TextureImporter;
        int wImage = 0, hImage = 0;
        if (importer != null)
        {
            object[] args = new object[2] { 0, 0 };
            MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(importer, args);

            wImage = (int)args[0];
            hImage = (int)args[1];
        }

        Debug.Log(wImage + "/" + hImage);

        List<SpriteMetaData> metas = new List<SpriteMetaData>();
        string patten = "([0-9]+)";
        Regex regex = new Regex(patten);
        MatchCollection match;
        for (int i = 0; i < len; i++)
        {
            string str = dictInfoFrame[i].SelectNodes("string")[0].InnerText;
            match = regex.Matches(str);
            int X = System.Convert.ToInt32(match[0].Value);
            int Y = System.Convert.ToInt32(match[1].Value);
            int Width = System.Convert.ToInt32(match[2].Value);
            int Height = System.Convert.ToInt32(match[3].Value);
            SpriteMetaData metadata = new SpriteMetaData();
            metadata.rect = new Rect(X, hImage - Y - Height, Width, Height);
            metadata.name = keys[i].InnerText.Replace("/", "").Split('.')[0];
            metas.Add(metadata);
        }

        // import
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritesheet = metas.ToArray();

        // apply
        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(imagePath, ImportAssetOptions.ForceUncompressedImport);
    }


    static void SplitSpriteWithJson (string imagePath, string json)
	{
		// check texture type.
		var importer = (TextureImporter)TextureImporter.GetAtPath (imagePath);
		if( importer.textureType != TextureImporterType.Sprite){
			return;
		}

		// read frame
		var jsonData = (Dictionary<string, object>)Json.Deserialize (json);
		var spriteFrames = (Dictionary<string, object>)jsonData ["frames"];
		
		// read meta file
		var meta = (Dictionary<string, object>)jsonData ["meta"];
		var size = (Dictionary<string, object>)meta ["size"];
		var height = int.Parse (size ["h"].ToString ());
		
		// json to meta
		List<SpriteMetaData> metas = new List<SpriteMetaData> ();
		foreach (string spriteName in spriteFrames.Keys) {
			var dic = (Dictionary<string, object>)spriteFrames [spriteName];
			var metadata = JsonToSpriteMetadata (spriteName, height, (Dictionary<string, object>)dic ["frame"]);
			metas.Add (metadata);
		}
		// import
		importer.spriteImportMode = SpriteImportMode.Multiple;
		importer.spritesheet = metas.ToArray ();
		
		// appry
		EditorUtility.SetDirty (importer);
		AssetDatabase.ImportAsset (imagePath, ImportAssetOptions.ForceUncompressedImport);
	}

	static SpriteMetaData JsonToSpriteMetadata (string spriteName, int height, Dictionary<string, object> frame)
	{
		SpriteMetaData meta = new SpriteMetaData ();
		int x = int.Parse (frame ["x"].ToString ());
		int y = int.Parse (frame ["y"].ToString ());
		int w = int.Parse (frame ["w"].ToString ());
		int h = int.Parse (frame ["h"].ToString ());
		meta.name = spriteName;
		meta.rect = new Rect (x, height - (y + h), w, h);
		meta.pivot = new Vector2 (0.5f, 0.5f);
		return meta;
	}

    [MenuItem("F4A/ImageToUnitySprite/All")]
    static void ImageToUnitySprite()
    {
        Object[] selectedTextures = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets);
        if (selectedTextures.Length == 0)
        {
            EditorUtility.DisplayDialog("ImageToUnitySprite", "Please select texture", "OK");
            return;
        }

        foreach (Object activeObject in selectedTextures)
        {
            Texture2D selectedTexture = activeObject as Texture2D;
            if (selectedTexture != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(selectedTexture);
                string textPath = assetPath.Substring(0, assetPath.Length - 4) + ".plist";
                Debug.Log("export sprite sheet \"" + assetPath + "\"");
                Debug.Log("plist \"" + textPath + "\"");
                SplitSpriteWithPlist(assetPath, textPath);
            } // if (selectedTexture != null)
        } // foreach (Object activeObject in selectedTextures)
    }
}