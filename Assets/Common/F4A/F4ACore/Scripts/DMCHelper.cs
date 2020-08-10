namespace com.F4A.MobileThird
{
    using DG.Tweening;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEngine;

    using Random = UnityEngine.Random;

    public static class DMCHelper
    {
        public static float ScaleToFitSize(float width, float height, float boundWidth, float boundHeight)
        {
            float widthRatio = float.MaxValue;
            float heightRatio = float.MaxValue;

            if (boundWidth > 0)
            {
                widthRatio = boundWidth / width;
            }

            if (boundHeight > 0)
            {
                heightRatio = boundHeight / height;
            }

            float ratio = widthRatio < heightRatio ? widthRatio : heightRatio;
            return ratio;
        }

        public static Vector2 ClampeSizeDelta(Vector2 current, Vector2 bound)
        {
            Vector2 result = Vector2.zero;

            if (current.x <= bound.x && current.y <= bound.y)
            {
                result = current;
            }
            else if (current.x > bound.x && current.y > bound.y)
            {
                result = bound;
            }
            else if (current.x > bound.x && current.y <= bound.y)
            {
                float ratio = current.y / current.x;
                result.x = bound.x;
                result.y = result.x * ratio;
            }
            else if (current.x <= bound.x && current.y > bound.y)
            {
                float ratio = current.x / current.y;
                result.y = bound.y;
                result.x = result.y * ratio;
            }

            return result;
        }

        public static Vector3 GetRatio(Vector3 current, Vector3 target)
        {
            Vector3 result = Vector3.zero;
            result.x = target.x / current.x;
            result.y = target.y / current.y;
            result.z = target.z / current.z;
            return result;
        }

        /// <summary>
        /// Return a random integer number between min [inclusive] and max [exclusive], ignore number in result
        /// </summary>
        public static int RandomIgnoreNumber(int min, int max, params int[] ignoreNumbers)
        {
            List<int> list = new List<int>();

            for (int counter = min; counter < max; counter++)
            {
                if (!ignoreNumbers.Contains(counter))
                {
                    list.Add(counter);
                }
            }

            int rnd = Random.Range(0, list.Count);
            return list[rnd];
        }

        public static int RandomIgnoreNumber(int min, int max, List<int> ignoreNumbers)
        {
            List<int> list = new List<int>();

            for (int counter = min; counter < max; counter++)
            {
                if (!ignoreNumbers.Contains(counter))
                {
                    list.Add(counter);
                }
            }

            int rnd = Random.Range(0, list.Count);
            return list[rnd];
        }

        /// <summary>
        /// Return a random list of integer number between min [inclusive] and max [exclusive], ignore number in result
        /// </summary>
        public static List<int> RandomListIgnoreNumber(int min, int max, int count, params int[] ignoreNumbers)
        {
            List<int> result = new List<int>();
            List<int> baseList = new List<int>();

            for (int counter = min; counter < max; counter++)
            {
                if (!ignoreNumbers.Contains(counter))
                {
                    baseList.Add(counter);
                }
            }

            for (int counter = 0; counter < count; counter++)
            {
                int rnd = Random.Range(0, baseList.Count);
                int value = baseList[rnd];
                baseList.RemoveAt(rnd);
                result.Add(value);
            }

            return result;
        }

        public static List<int> RandomListIgnoreNumber(int min, int max, int count, List<int> ignoreNumbers)
        {
            List<int> result = new List<int>();
            List<int> baseList = new List<int>();

            for (int counter = min; counter < max; counter++)
            {
                if (!ignoreNumbers.Contains(counter))
                {
                    baseList.Add(counter);
                }
            }

            for (int counter = 0; counter < count; counter++)
            {
                int rnd = Random.Range(0, baseList.Count);
                int value = baseList[rnd];
                baseList.RemoveAt(rnd);
                result.Add(value);
            }

            return result;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Shuffle<T>(this T[,] array)
        {
            int lengthRow = array.GetLength(1);

            for (int i = array.Length - 1; i > 0; i--)
            {
                int i0 = i / lengthRow;
                int i1 = i % lengthRow;

                int j = Random.Range(0, i + 1);
                int j0 = j / lengthRow;
                int j1 = j % lengthRow;

                T temp = array[i0, i1];
                array[i0, i1] = array[j0, j1];
                array[j0, j1] = temp;
            }
        }

        public static Vector2[] RotateUV(Vector2[] uv, float rotationRadians)
        {
            Vector2[] result = uv;

            float rotMatrix00 = Mathf.Cos(rotationRadians);
            float rotMatrix01 = -Mathf.Sin(rotationRadians);
            float rotMatrix10 = Mathf.Sin(rotationRadians);
            float rotMatrix11 = Mathf.Cos(rotationRadians);

            Vector2 halfVector = new Vector2(0.5f, 0.5f);

            for (int counter = 0; counter < result.Length; counter++)
            {
                // Switch coordinates to be relative to center of the plane
                result[counter] = result[counter] - halfVector;

                // Apply the rotation matrix
                float u = rotMatrix00 * result[counter].x + rotMatrix01 * result[counter].y;
                float v = rotMatrix10 * result[counter].x + rotMatrix11 * result[counter].y;
                result[counter].x = u;
                result[counter].y = v;

                // Switch back coordinates to be relative to edge
                result[counter] = result[counter] + halfVector;
            }

            return result;
        }

        public static Texture2D LoadTextureFromFile(string path)
        {
            string realPath = path;

            if (!File.Exists(realPath))
            {
                realPath = path + ".png";

                if (!File.Exists(realPath))
                {
                    realPath = path + ".jpg";

                    if (!File.Exists(realPath))
                        return null;
                }
            }

            byte[] fileData = File.ReadAllBytes(realPath);
            Texture2D result = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            result.LoadImage(fileData);
            return result;
        }

        public static void SaveTextureToFile(Texture2D texture, string path)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
        }

        public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
            result.filterMode = source.filterMode;

            Color[] colorArray = new Color[targetWidth * targetHeight];

            float stepX = source.width / (float)targetWidth;
            float stepY = source.height / (float)targetHeight;

            for (int i = 0; i < targetWidth; i++)
            {
                for (int j = 0; j < targetHeight; j++)
                {
                    int x = Mathf.RoundToInt(i * stepX);
                    int y = Mathf.RoundToInt(j * stepY);
                    colorArray[i + j * targetWidth] = source.GetPixel(x, y);
                }
            }

            result.SetPixels(colorArray);
            result.Apply();
            return result;
        }

        public static Texture2D DeCompress(this Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

        public static DateTime CalculateTomorrow(DateTime current)
        {
            return current.AddDays(1).Date;
        }

        public static Vector3 GetRelativePosition(Transform parent, Transform item)
        {
            Transform oldParent = item.parent;
            int oldSibingIndex = item.GetSiblingIndex();

            item.SetParent(parent);
            Vector3 result = item.localPosition;

            item.SetParent(oldParent);
            item.SetSiblingIndex(oldSibingIndex);

            return result;
        }

        public static void Reset(this Tween target)
        {
            if(target != null)
            {
                target.Kill();
                target = null;
            }
        }
    }
}