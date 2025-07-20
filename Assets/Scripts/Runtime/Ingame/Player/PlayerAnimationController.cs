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
        /// <param name="dir"></param>
        public void SetMoveDirParam(Vector2 dir)
        {
            dir.Normalize();
            _animator.SetFloat(_moveXHash, dir.x);
            _animator.SetFloat(_moveYHash, dir.y);
        }

        private readonly Animator _animator;
        private readonly int _moveXHash = Animator.StringToHash("MoveX");
        private readonly int _moveYHash = Animator.StringToHash("MoveY");
    }
}
