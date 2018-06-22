using UnityEngine;

namespace SlimeBasher.CrossPlatformInput
{
    public class InputAxisScrollbar : MonoBehaviour
    {
        public string axis;

        void Update()
        {
        }

        public void HandleInput(float value)
        {
            CrossPlatformInputManager.SetAxis(axis, (value * 2f) - 1f);
        }
    }
}
