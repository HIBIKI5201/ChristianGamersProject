using UnityEngine;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class ItemHotBarSlot : MonoBehaviour
    {
        public Image Self => _self;
        public Image Child => _child;
        public Text Price => _price;

        [SerializeField]
        private Image _self;

        [SerializeField]
        private Image _child;

        [SerializeField]
        private Text _price;
    }
}
