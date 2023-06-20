using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SwitchToggle : MonoBehaviour
    {
        [SerializeField] private RectTransform handleRt;

        private Vector2 handlePositon;
        // Update is called once per frame
        void Awake()
        {
            Toggle toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnSwitch);

            handlePositon = -handleRt.localPosition;
        }

        void OnSwitch(bool on)
        {
            if (on)
            {
                handleRt.localPosition = handlePositon * -1;
            }
            else
            {
                handleRt.localPosition = handlePositon;
            }
        }
    }
}
