using UnityEngine;

namespace ChristianGamers.Utility
{
    public static class TransformUtility
    {
        /// <summary>
        ///     指定されたTransformの親をたどりながら、指定された型のコンポーネントを検索します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T FindTypeByParents<T>(Transform target) where T : Component
        {
            while (target != null)
            {
                if (target.TryGetComponent(out T item))
                {
                    return item; // 見つかったら即返す
                }

                target = target.parent; // 親をたどる
            }

            return null; // 最後まで見つからなかった場合
        }
    }
}
