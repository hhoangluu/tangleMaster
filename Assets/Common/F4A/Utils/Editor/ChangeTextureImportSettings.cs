﻿using UnityEngine;
using UnityEditor;
 
// /////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Batch Texture import settings modifier.
//
// Modifies all selected textures in the project window and applies the requested modification on the 
// textures. Idea was to have the same choices for multiple files as you would have if you open the 
// import settings of a single texture. Put this into Assets/Editor and once compiled by Unity you find
// the new functionality in Custom -> Texture. Enjoy! :-)
// 
// Based on the great work of benblo in this thread: 
// http://forum.unity3d.com/viewtopic.php?t=16079&start=0&postdays=0&postorder=asc&highlight=textureimporter
// 
// Developed by Martin Schultz, Decane in August 2009
// e-mail: ms@decane.net
//
// /////////////////////////////////////////////////////////////////////////////////////////////////////////
public class ChangeTextureImportSettings : ScriptableObject {
 
	[MenuItem ("Custom/Texture/Change Texture Format/Auto")]
    static void ChangeTextureFormat_Auto() { 
		SelectedChangeTextureFormatSettings(TextureImporterFormat.Automatic);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Format/RGB Compressed DXT1")]
    static void ChangeTextureFormat_RGB_DXT1() { 
		SelectedChangeTextureFormatSettings(TextureImporterFormat.DXT1);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Format/RGB Compressed DXT5")]
    static void ChangeTextureFormat_RGB_DXT5() { 
		SelectedChangeTextureFormatSettings(TextureImporterFormat.DXT5);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Format/RGB 16 bit")]
    static void ChangeTextureFormat_RGB_16bit() { 
		SelectedChangeTextureFormatSettings(TextureImporterFormat.RGB16);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Format/RGB 24 bit")]
    static void ChangeTextureFormat_RGB_24bit() { 
		SelectedChangeTextureFormatSettings(TextureImporterFormat.RGB24);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Format/Alpha 8 bit")]
    static void ChangeTextureFormat_Alpha_8bit() { 
		SelectedChangeTextureFormatSettings(TextureImporterFormat.Alpha8);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Format/RGBA 16 bit")]
    static void ChangeTextureFormat_RGBA_16bit() { 
		SelectedChangeTextureFormatSettings(TextureImporterFormat.ARGB16);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Format/RGBA 32 bit")]
    static void ChangeTextureFormat_RGBA_32bit() { 
		SelectedChangeTextureFormatSettings(TextureImporterFormat.ARGB32);
	}
 
	// ----------------------------------------------------------------------------
 
	[MenuItem ("Custom/Texture/Change Texture Size/Change Max Texture Size/32")]
    static void ChangeTextureSize_32() { 
		SelectedChangeMaxTextureSize(32);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Size/Change Max Texture Size/64")]
    static void ChangeTextureSize_64() { 
		SelectedChangeMaxTextureSize(64);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Size/Change Max Texture Size/128")]
    static void ChangeTextureSize_128() { 
		SelectedChangeMaxTextureSize(128);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Size/Change Max Texture Size/256")]
    static void ChangeTextureSize_256() { 
		SelectedChangeMaxTextureSize(256);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Size/Change Max Texture Size/512")]
    static void ChangeTextureSize_512() { 
		SelectedChangeMaxTextureSize(512);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Size/Change Max Texture Size/1024")]
    static void ChangeTextureSize_1024() { 
		SelectedChangeMaxTextureSize(1024);
	}
 
	[MenuItem ("Custom/Texture/Change Texture Size/Change Max Texture Size/2048")]
    static void ChangeTextureSize_2048() { 
		SelectedChangeMaxTextureSize(2048);
	}
 
	// ----------------------------------------------------------------------------
 
	[MenuItem ("Custom/Texture/Change MipMap/Enable MipMap")]
    static void ChangeMipMap_On() { 
		SelectedChangeMimMap(true);
	}
 
	[MenuItem ("Custom/Texture/Change MipMap/Disable MipMap")]
    static void ChangeMipMap_Off() { 
		SelectedChangeMimMap(false);
	}
 
	// ----------------------------------------------------------------------------
 
	static void SelectedChangeMimMap(bool enabled) { 
 
		Object[] textures = GetSelectedTextures(); 
		Selection.objects = new Object[0];
		foreach (Texture2D texture in textures)  {
			string path = AssetDatabase.GetAssetPath(texture); 
			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter; 
			textureImporter.mipmapEnabled = enabled;	
			AssetDatabase.ImportAsset(path); 
		}
	}
 
	static void SelectedChangeMaxTextureSize(int size) { 
 
		Object[] textures = GetSelectedTextures(); 
		Selection.objects = new Object[0];
		foreach (Texture2D texture in textures)  {
			string path = AssetDatabase.GetAssetPath(texture); 
			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter; 
			textureImporter.maxTextureSize = size;	
			AssetDatabase.ImportAsset(path); 
		}
	}
 
	static void SelectedChangeTextureFormatSettings(TextureImporterFormat newFormat) { 
 
		Object[] textures = GetSelectedTextures(); 
		Selection.objects = new Object[0];
		foreach (Texture2D texture in textures)  {
			string path = AssetDatabase.GetAssetPath(texture); 
			//Debug.Log("path: " + path);
			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter; 
			textureImporter.textureFormat = newFormat;	
			AssetDatabase.ImportAsset(path); 
		}
	}
 
	static Object[] GetSelectedTextures() 
	{ 
		return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets); 
	}
}