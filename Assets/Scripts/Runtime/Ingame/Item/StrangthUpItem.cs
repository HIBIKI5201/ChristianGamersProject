using ChristianGamers.Ingame.Player;
using System;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    public class StrangthUpItem : ItemBase, IUseble
    {
        private static float _timer;

        [SerializeField, Min(1), Tooltip("スピード強化倍率")]
        private float _strangthUpScale = 1;

        [SerializeField, Tooltip("効果時間")]
        private float _duration = 1;

        [SerializeField, Tooltip("再発動可能までの時間")]
        private float _cooldown = 1;

        public bool Use(PlayerManager player)
        {
            //クールダウン中なら失敗
            if (Time.time < _timer) return false;
            _timer = Time.time + _cooldown;

            BuffOperationAsync(player);

            return true;
        }

        private async void BuffOperationAsync(PlayerManager player)
        {
            player.RegisterStrangthBuff(StrangthBuff);

            try
            {
                await Awaitable.WaitForSecondsAsync(_duration, destroyCancellationToken);
            }
            catch (OperationCanceledException) { return; }

            player.UnregisterStrangthBuff(StrangthBuff);

            Destroy(gameObject);
        }

        private float StrangthBuff(float value) => value * _strangthUpScale;
    }
}
