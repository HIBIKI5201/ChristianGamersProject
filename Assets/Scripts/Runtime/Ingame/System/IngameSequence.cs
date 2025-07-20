using ChristianGamers.Ingame.Player;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.Playables;

namespace ChristianGamers.Ingame.Sequence
{
    [DefaultExecutionOrder(1000)]
    public class IngameSequence : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector _startTimeLineDirector;

        private void Start()
        {
            //必要な参照を取得
            IngameTimer timer = ServiceLocator.GetInstance<IngameTimer>();
            PlayerManager player = ServiceLocator.GetInstance<PlayerManager>();

            //一連のシークエンスのイベントを登録
            _startTimeLineDirector.stopped += d => HandleStart();
            timer.OnTimeUp += HandleTimeUp;

            try
            {
                _startTimeLineDirector.Play(); //タイムラインを開始
            }
            catch { HandleStart(); } //Playで問題が起こったら始める

            void HandleStart()
            {
                timer.Play();
                player.SetActiveInputHandle(true);
            }

            void HandleTimeUp()
            {
                timer.Stop();
                player.SetActiveInputHandle(false);
            }
        }
    }
}
