using ChristianGamers.Ingame.Item;
using ChristianGamers.Ingame.Player;
using System;
using UnityEngine;

namespace ChristianGamers
{
    public class SpeedUpItem : ItemBase, IUseble
    {
        [SerializeField, Min(1), Tooltip("スピード強化倍率")]
        private float _speedUpScale = 1;

        [SerializeField, Tooltip("効果時間")]
        private float _duration = 1;
        public async void Use(PlayerManager player)
        {
            player.RegisterSpeedBuff(SpeedUpBuff);
            
            try
            {
                await Awaitable.WaitForSecondsAsync(_duration, destroyCancellationToken);
            }
            catch (OperationCanceledException) { return; }

            player.UnregisterSpeedBuff(SpeedUpBuff);
        }

        private float SpeedUpBuff(float value) => value * _speedUpScale;
    }
}
