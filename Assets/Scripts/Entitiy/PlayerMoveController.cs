using System.Collections.Generic;
using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerMoveController : MonoBehaviour
    {
        private static PlayerMoveController _instance;

        private GloballyPlayerInputHandler _globallyPlayerInputHandler;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            if (_instance)
            {
                return;
            }
            GameObject gameObject = new GameObject("Player Move Controller", typeof(PlayerMoveController));
            _instance = gameObject.GetComponent<PlayerMoveController>();
            Debug.Log("Player Move Controller initialized");
        }

        private void Start()
        {
            _globallyPlayerInputHandler = GloballyPlayerInputHandler.Instance;
            _globallyPlayerInputHandler.AddAction(new KeyboardInputAction());
            Debug.Log("Keyboard action attached");
        }

        private class KeyboardInputAction : IKeyboardInputAction
        {
            public void OnKeyDown(IList<KeyCode> keyCodes)
            {
                if (keyCodes.Contains(KeyCode.W))
                {
                    Debug.Log("W key pressed");
                }
                if (keyCodes.Contains(KeyCode.S))
                {
                    Debug.Log("S key pressed");
                }
            }
        }
    }
}
