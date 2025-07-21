using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class ViewSeqence : MonoBehaviour
    {
        [SerializeField] ResultView _resultView;
        [SerializeField] RankingDrawer _drawer;
        [SerializeField] private float _waitTime = 2f;
        [SerializeField] private float _endTime = 10;

        void Start()
        {
            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            yield return new WaitForSeconds(_waitTime);
            _resultView.View();
            _drawer.RankingView();

            yield return new WaitForSeconds(_endTime);

            if (TryGetComponent(out SceneLoad component))
            {
                component.LoadScene();
            }
        }
    }
}
