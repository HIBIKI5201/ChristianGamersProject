using System;
using UnityEngine;

namespace ChristianGamers
{
    [CreateAssetMenu(fileName = nameof(RouteBranchData), menuName = nameof(RouteBranchData))]
    public class RouteBranchData : MonoBehaviour
    {
        public SceneListEnum GetRoute(int score)
        {
            for (int i = _routeData.Length; 0 <= i; i--)
            {
                RouteData data = _routeData[i];
                if (data.RequireScore <= score)
                {
                    return data.TargetScene;
                }
            }

            Debug.LogError($"Score {score}の条件を満たすルートが存在しません");
            return SceneListEnum.Title;
        }

        private void OnEnable()
        {
            //必要スコアでソートする
            Array.Sort(_routeData, (a,b) => a.RequireScore.CompareTo(b.RequireScore));
        }

        [SerializeField]
        private RouteData[] _routeData;

        [Serializable]
        private struct RouteData
        {
            public int RequireScore => _requireScore;
            public SceneListEnum TargetScene => _targetScene;

            [SerializeField]
            private int _requireScore;
            [SerializeField]
            private SceneListEnum _targetScene;
        }
    }
}
