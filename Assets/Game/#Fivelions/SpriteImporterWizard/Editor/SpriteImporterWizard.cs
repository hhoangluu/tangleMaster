// Creates a simple wizard that lets you create a Light GameObject
// or if the user clicks in "Apply", it will set the color of the currently
// object selected to red

using UnityEditor;
using UnityEngine;
using System.Threading;
//using KR;

public class SpriteImporterWizard : ScriptableWizard
{
    //public float range = 500;
    //public Color color = Color.red;
    public Sprite spriteSheet;

    [MenuItem("Fivelions/Sprite Importer")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<SpriteImporterWizard>("Sprite Light", "Convert To FBF");
        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
    }

    private void OnWizardCreate()
    {
        EditorUtility.DisplayProgressBar("Creating Multiple Sprite", "Extracting sprite", 0);

        var path = AssetDatabase.GetAssetPath(spriteSheet);
        var objs = AssetDatabase.LoadAllAssetsAtPath(path);

        float totals = objs.Length;

        for (int i = 0; i < objs.Length; i++)
        {
            var s = objs[i] as Sprite;

            if (s != null)
            { // this is a sprite object
                var rect = s.textureRect;

                var pixels = s.texture.GetPixels(Mathf.RoundToInt(rect.x), Mathf.RoundToInt(rect.y), Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height));

                int width = Mathf.RoundToInt(s.textureRect.width);
                int height = Mathf.RoundToInt(s.textureRect.height);
                string outputName = path.Replace(".png", "_") + i + ".png";
                EditorUtility.DisplayProgressBar("Progressing asset", outputName, i / totals);
                CreateSprite(outputName, width, height, pixels);
            }
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }

    private void CreateSprite(string path, int width, int height, Color[] colors)
    {
        Texture2D tex2D = new Texture2D(width, height);
        Color[] outputColors = new Color[width * height];
        int i = 0;
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                outputColors[i] = colors[i];
                i++;
            }
        }

        tex2D.SetPixels(colors);
        tex2D.Apply();
        var bytes = tex2D.EncodeToPNG();

        Thread thread = new Thread(() =>
        {
            System.IO.File.WriteAllBytes(path, bytes);

        });

        thread.Start();
        thread.Join();
    }

    private void OnWizardUpdate()
    {
        helpString = "Input sprite sheet image.";
    }

    // When the user presses the "Apply" button OnWizardOtherButton is called.
    private void OnWizardOtherButton()
    {
        //if (Selection.activeTransform != null)
        //{
        //    Light lt = Selection.activeTransform.GetComponent<Light>();
        //    if (lt != null)
        //    {
        //        lt.color = Color.red;
        //    }
        //}
    }
}
