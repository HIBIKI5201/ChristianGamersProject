using ChristianGamers.Ingame.Player;
using SymphonyFrameWork.System;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;

namespace ChristianGamers.Ingame.Sequence
{
    [DefaultExecutionOrder(1000)]
    public class IngameSequence : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector _startTimeLineDirector;

        [SerializeField]
        private AudioClip[] _bgms;

        private CancellationTokenSource _bgmCancellationTokenSource;
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

                _bgmCancellationTokenSource = new CancellationTokenSource();
                AudioSource source = AudioManager.GetAudioSource(AudioGroupTypeEnum.BGM.ToString());
                BGMLoop(source, _bgms, _bgmCancellationTokenSource.Token);
            }

            void HandleTimeUp()
            {
                timer.Stop();
                player.SetActiveInputHandle(false);
                _bgmCancellationTokenSource.Cancel();

                if (TryGetComponent(out SceneLoad component))
                    component.LoadScene();
            }
        }

        private async void BGMLoop(AudioSource source, AudioClip[] clips, CancellationToken token = default)
        {
            source.loop = false;

            int index = 0;
            while (true)
            {
                //新しいクリップに変更
                source.Stop();
                source.clip = clips[index];
                source.Play();

                try
                {
                    //再生中は待機
                    while (source.isPlaying)
                    {
                        await Awaitable.NextFrameAsync(token);
                    }
                }
                catch (OperationCanceledException) { break; }

                index = ++index % clips.Length;
            }
            source.loop = true;
        }
    }
}
