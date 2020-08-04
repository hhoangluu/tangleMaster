//using UnityEngine;
//using System.Collections;
//#pragma warning disable

//[RequireComponent(typeof(AudioSource))]
//public class MyMemory : MonoBehaviour
//{
//private void Awake()
//{
//    Application.targetFrameRate = 120;
//}
//void Update()
//{
//    Time.timeScale = 0.5f;
//}
//}

//void Test()
//{
//    var ind = Instantiate(null) as GameObject;
//    ind.hideFlags |= HideFlags.HideInHierarchy;
//    0011 
//    |= 
//    0110
//    0111
//     & 
//    var b = 1 << 0;
//    bit b = 0001
//     b = 0001 << 1
//    b = 0010
//    0011 
//    0110
//    0010
//}

//public void ABC(Card card, List<Card> listCards, bool isOpenned = false, System.Action callback = null)
//{
//    ABC(1, 2, true, () => { });
//    ABC(1, 2);
//}

//int i = 6;
//Stopwatch stw = Stopwatch.StartNew();
//decimal result = i.Factorial();
////yield return new WaitForEndOfFrame();
//stw.Stop();
//UnityEngine.Debug.LogError("RecursiveMode: " + i + "! = " + result + " - In Time: " + stw.ElapsedMilliseconds.ToString());

//Stopwatch stw2 = Stopwatch.StartNew();
//decimal result2 = MathExtension.MFactorial(i);
////yield return new WaitForEndOfFrame();
//stw2.Stop();
//UnityEngine.Debug.LogError("BasicMode: " + i + "! = " + result2 + " - In Time: " + stw2.ElapsedMilliseconds.ToString());