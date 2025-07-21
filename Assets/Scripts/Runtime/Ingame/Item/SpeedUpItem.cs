using ChristianGamers.Ingame.Item;
using ChristianGamers.Ingame.Player;
using System;
using UnityEngine;

namespace ChristianGamers
{
    public class SpeedUpItem : ItemBase, IUseble
    {
        private static float _timer;

        [SerializeField, Min(1), Tooltip("スピード強化倍率")]
        private float _speedUpScale = 1;

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
            player.RegisterSpeedBuff(SpeedUpBuff);

            try
            {
                await Awaitable.WaitForSecondsAsync(_duration, destroyCancellationToken);
            }
            catch (OperationCanceledException) { return; }

            player.UnregisterSpeedBuff(SpeedUpBuff);

            Destroy(gameObject);
        }

        private float SpeedUpBuff(float value) => value * _speedUpScale;
    }
}
