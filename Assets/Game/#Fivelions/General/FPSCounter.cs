using System.Collections;
using UnityEngine;
#pragma warning disable

public class FPSCounter : MonoBehaviour
{
    private string label = "";
    private float count;

    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            if (Time.timeScale == 1)
            {
                count = (1 / Time.deltaTime);
                label = "FPS :" + (Mathf.Round(count));
            }
            else
            {
                label = "Pause || Time.timeScale != 1";
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnGUI() { GUI.Label(new Rect(1f, 60f, 100f, 25f), label); }
}
