using System.Linq;
using System.Collections.Generic;

namespace Five.Math
{
    public static class ProbabilityAndMathematicalStatistics
    {
        /// <summary>
        /// Factorial - Recursive - n!
        /// <para>EX: 5! = 5 x 4 x 3 x 2 x 1 = 120</para> 
        /// <para>n >= 0 | 0! = 1</para>
        /// </summary>
        /// <returns>Factorial.</returns>
        /// <param name="this">This.</param>
        public static double Factorial(this int @this) { return @this <= 1 ? 1 : @this * (@this - 1).Factorial(); }

        /// <summary>
        /// Arrangement/Permutation selections of elements from a collection (Chinh Hop A)
        /// <para>BY ORDER</para>
        /// <para>n >= k >= 1</para>
        /// </summary>
        /// <returns>The arrangement.</returns>
        /// <param name="n">N - The Collection of Elements.</param>
        /// <param name="k">K - Number of Elements in choose.</param>
        public static double Arrangement(this int n, int k) { return k > n ? 0 : n.Factorial() / (n - k).Factorial(); }

        /// <summary>
        /// Combination selections of elements from a collection (To Hop C)
        /// <para>THE ORDER OF SELECTIONS DOES NOT MATTER (Unlike Arrangement|Permutations)</para>
        /// <para>n >= k >= 1</para>
        /// </summary>
        /// <returns>The Combination.</returns>
        /// <param name="n">N - The Collection of Elements.</param>
        /// <param name="k">K - Number of Elements in choose.</param>
        public static double Combination(this int n, int k) { return k > n ? 0 : n.Factorial() / ((n - k).Factorial() * k.Factorial()); }

        /// <summary>
        /// Total Combination Selections
        /// <para>EX: 3 -> 3C1 + 3C2 + 3C3 = 3 + 3 + 1 = 7</para>
        /// </summary>
        /// <returns>Total Combination Selections.</returns>
        /// <param name="this">This Int.</param>
        public static double CombinationTotalSelections(this int @this)
        {
            double result = 0;
            for (int i = 1; i <= @this; i++)
                result += @this.Combination(i);
            return result;
        }

        /// <summary>
        /// Matrix of Selections (Combination)
        /// <para>EX: 4T2 -> (T1-T2) | (T1-T3) | (T1-T4) | (T2-T3) | (T2-T4) | (T3-T4)</para>
        /// <para>List<T> >= 0 | <paramref name="numberSelect"/> > 0 ; <paramref name="numberSelect"/> < List<T>.Count</para>
        /// </summary>
        /// <returns>The selections.</returns>
        /// <param name="this">This List.</param>
        /// <param name="numberSelect">Number of Selections in List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<List<T>> MatrixSelections<T>(this List<T> @this, int numberSelect)
        {
            //Debug.LogError(("MatrixSelections: ").Color(Color.magenta) + @this.Count.ToString().Color(Color.red) + " select " + numberSelect.ToString().Color(Color.green));
            if (@this.Count == 0 || numberSelect == 0 || numberSelect > @this.Count)
                return new List<List<T>>();

            List<List<T>> result = new List<List<T>>();
            List<T> selectedListT = new List<T>();
            selectedListT = @this.GetRange(0, numberSelect);
            if (numberSelect == @this.Count)
            {
                result.Add(selectedListT);
                goto RESULT;
            }
            for (int i = 0; i < @this.Count.Combination(numberSelect); i++)
            {
                List<T> newResult = new List<T>();
                newResult.AddRange(selectedListT);
                result.Add(newResult);

                if (@this.IndexOf(selectedListT.Last()) < @this.Count - 1)
                {
                    selectedListT[selectedListT.Count - 1] = @this[@this.IndexOf(selectedListT.Last()) + 1];
                    goto CONTINUE;
                }
                if (@this.IndexOf(selectedListT.Last()) == @this.Count - 1)
                {
                    if (selectedListT.Count == 1)
                        goto RESULT;

                    for (int j = @this.Count - 1 - 1; j >= 0; j--)
                    {
                        if (!selectedListT.Contains(@this[j]))
                            continue;

                        if (selectedListT.Contains(@this[j]) && !selectedListT.Contains(@this[j + 1]))
                        {
                            selectedListT[selectedListT.IndexOf(@this[j])] = @this[j + 1];

                            List<T> reset = selectedListT.FindAll(t => selectedListT.IndexOf(t) > selectedListT.IndexOf(@this[j + 1]));
                            for (int y = 0; y < reset.Count; y++)
                            {
                                selectedListT[selectedListT.IndexOf(reset[y])] = @this[@this.IndexOf(@this[j + 1]) + y + 1];
                            }
                            goto CONTINUE;
                        }
                    }
                }
            CONTINUE:
                continue;
            }
        RESULT:
            //Debug.LogError(("MatrixSelections Count: " + result.Count).Color(Color.grey));
            //foreach (var lT in result)
            //{
            //    Debug.Log(("Result: " + result.IndexOf(lT) + " - " + lT.Count).Color(Color.red));
            //    foreach (var t in lT)
            //    {
            //        Debug.Log((lT.IndexOf(t) + " - " + t).Color(Color.cyan));
            //    }
            //}
            return result;
        }
    }

    public static class General
    {
        /// <summary>
        /// Check if the number are a Even numbers?
        ///<para><see langword="true"/> if EVEN | <see langword="false"/> if ODD.</para>
        /// </summary>
        /// <returns><c>True</c> if EVEN | <c>False</c> if ODD.</returns>
        /// <param name="this">This.</param>
        public static bool isEven(this int @this) { return @this % 2 == 0; }
    }

    public static class Percent
    {
        public static int GetHundredPercent(this int @this, float percent) { return (int)((@this / 100f) * percent); }
        public static float GetHundredPercent(this float @this, float percent) { return (@this / 100f) * percent; }
        public static double GetHundredPercent(this double @this, float percent) { return (@this / 100f) * percent; }
        public static decimal GetHundredPercent(this decimal @this, float percent) { return (@this / 100) * (decimal)percent; }
    }
}
