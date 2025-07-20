using UnityEngine;

namespace ChristianGamers.Ingame.Player
{
    public class PlayerAnimationController
    {
        public PlayerAnimationController(Animator animator)
        {
            _animator = animator;
        }

        /// <summary>
        ///     速度パラメータを設定
        /// </summary>
        /// <param name="velocity"></param>
        public void SetVelocityParam(Vector2 velocity)
        {
            _animator.SetFloat(_velocityXHash, velocity.x);
            _animator.SetFloat(_velocityYHash, velocity.y);
        }

        private readonly Animator _animator;
        private readonly int _velocityXHash = Animator.StringToHash("VelocityX");
        private readonly int _velocityYHash = Animator.StringToHash("VelocityY");
    }
}
