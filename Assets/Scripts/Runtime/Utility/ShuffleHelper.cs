using System;

namespace ChristianGamers.Utility
{
    /// <summary>
    ///     シャッフルを行うためのヘルパークラス
    /// </summary>
    public static class ShuffleHelper
    {
        /// <summary>
        /// Fisher-Yatesアルゴリズムを使用して配列をシャッフルします。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void FisherYatesShuffle<T>(T[] array)
        {
            Random rng = new Random(); // System.Randomを使用
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1); // 0以上i以下のランダムな整数

                // 要素を交換する
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}
