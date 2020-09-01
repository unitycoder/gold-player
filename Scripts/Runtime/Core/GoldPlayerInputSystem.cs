﻿#if !ENABLE_INPUT_SYSTEM || !GOLD_PLAYER_NEW_INPUT
#define OBSOLETE
#endif

#if OBSOLETE && !UNITY_EDITOR // If it's obsolete and not in the editor, remove it.
#define STRIP
#endif

#if !STRIP
using UnityEngine;
using UnityEngine.Serialization;
#if !OBSOLETE
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace Hertzole.GoldPlayer
{
#if OBSOLETE
    [System.Obsolete("You're not using the new Input System so this component will be useless.")]
    [AddComponentMenu("")]
#else
    [AddComponentMenu("Gold Player/Gold Player Input System", 1)]
    [DisallowMultipleComponent]
#endif
    public class GoldPlayerInputSystem : MonoBehaviour, IGoldInput
    {
#if !OBSOLETE
        [System.Serializable]
        public struct InputItem
        {
#pragma warning disable CA2235 // Mark all non-serializable fields
            public string actionName;
            public InputActionReference action;

            public InputItem(string actionName, InputActionReference action)
            {
                this.actionName = actionName;
                this.action = action;
            }

            public override bool Equals(object obj)
            {
                return obj != null && obj is InputItem item ? item.actionName == actionName && item.action == action : false;
            }

            public override int GetHashCode()
            {
                return (action.action.name + "." + actionName).GetHashCode();
            }

            public static bool operator ==(InputItem left, InputItem right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(InputItem left, InputItem right)
            {
                return !(left == right);
            }
#pragma warning restore CA2235 // Mark all non-serializable fields
        }

        [SerializeField]
        [FormerlySerializedAs("input")]
        private InputActionAsset inputAsset = null;
        [SerializeField]
        private InputItem[] actions = null;
#endif
        [SerializeField]
        private bool autoEnableInput = true;
        [SerializeField]
        private bool autoDisableInput = true;

        private bool enabledInput = false;

#if !OBSOLETE
#if UNITY_EDITOR
        [System.Obsolete("Use 'InputAsset' instead. This will be removed on build.", true)]
        public InputActionAsset Input { get { return InputAsset; } set { InputAsset = value; } }
#endif
        public InputActionAsset InputAsset { get { return inputAsset; } set { inputAsset = value; } }
        private Dictionary<string, InputAction> actionsDictionary;
#endif

        public bool EnabledInput { get { return enabledInput; } }
        public bool AutoEnableInput { get { return autoEnableInput; } set { autoEnableInput = value; } }
        public bool AutoDisableInput { get { return autoDisableInput; } set { autoDisableInput = value; } }

        private void Start()
        {
#if OBSOLETE
            Debug.LogError(gameObject.name + " has GoldPlayerInputSystem added. It does not work on the legacy input manager.");
#else
            UpdateActions();
#endif
        }

        public void EnableInput()
        {
#if !OBSOLETE
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].action != null)
                {
                    actions[i].action.action.Enable();
                }
            }

#endif
            enabledInput = true;
        }

        public void DisableInput()
        {
#if !OBSOLETE
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].action != null)
                {
                    actions[i].action.action.Disable();
                }
            }
#endif
            enabledInput = false;
        }

#if !OBSOLETE
        private void OnEnable()
        {
            if (autoEnableInput)
            {
                EnableInput();
            }
        }

        private void OnDisable()
        {
            if (autoDisableInput)
            {
                DisableInput();
            }
        }

        private void UpdateActions()
        {
            actionsDictionary = new Dictionary<string, InputAction>();
            for (int i = 0; i < actions.Length; i++)
            {
                actionsDictionary.Add(actions[i].actionName, actions[i].action);
            }
        }
#endif

        public bool GetButton(string buttonName)
        {
#if !OBSOLETE
            if (inputAsset == null)
            {
                Debug.LogWarning("There is no input asset on " + gameObject.name + ".", gameObject);
                return false;
            }

            if (actionsDictionary == null)
            {
                UpdateActions();
            }

            if (actionsDictionary.TryGetValue(buttonName, out InputAction inputAction))
            {
                if (inputAction == null)
                {
                    return false;
                }

                return inputAction.activeControl is ButtonControl button && button.isPressed;
            }
            else
            {
                Debug.LogError("Can't find action '" + buttonName + "' in " + inputAsset.name + "!");
                return false;
            }
#else
            return false;
#endif
        }

        public bool GetButtonDown(string buttonName)
        {
#if !OBSOLETE
            if (inputAsset == null)
            {
                Debug.LogWarning("There is no input asset on " + gameObject.name + ".", gameObject);
                return false;
            }

            if (actionsDictionary == null)
            {
                UpdateActions();
            }

            if (actionsDictionary.TryGetValue(buttonName, out InputAction inputAction))
            {
                if (inputAction == null)
                {
                    return false;
                }

                return inputAction.activeControl is ButtonControl button && button.wasPressedThisFrame;
            }
            else
            {
                Debug.LogError("Can't find action '" + buttonName + "' in " + inputAsset.name + "!");
                return false;
            }
#else
            return false;
#endif
        }

        public bool GetButtonUp(string buttonName)
        {
#if !OBSOLETE
            if (inputAsset == null)
            {
                Debug.LogWarning("There is no input asset on " + gameObject.name + ".", gameObject);
                return false;
            }

            if (actionsDictionary == null)
            {
                UpdateActions();
            }

            if (actionsDictionary.TryGetValue(buttonName, out InputAction inputAction))
            {
                if (inputAction == null)
                {
                    return false;
                }

                return inputAction.activeControl is ButtonControl button && button.wasReleasedThisFrame;
            }
            else
            {
                Debug.LogError("Can't find action '" + buttonName + "' in " + inputAsset.name + "!");
                return false;
            }
#else
            return true;
#endif
        }

        public float GetAxis(string axisName)
        {
#if !OBSOLETE
            if (inputAsset == null)
            {
                Debug.LogWarning("There is no input asset on " + gameObject.name + ".", gameObject);
                return 0;
            }

            if (actionsDictionary.TryGetValue(axisName, out InputAction inputAction))
            {
                if (inputAction == null)
                {
                    return 0;
                }

                if (inputAction.activeControl is AxisControl axis)
                {
                    return axis.ReadValue();
                }
                else
                {
                    Debug.LogError(axisName + " is not an axis type.");
                    return 0;
                }
            }
            else
            {
                Debug.LogError("Can't find action '" + axisName + "' in " + inputAsset.name + "!");
                return 0;
            }
#else
            return 0;
#endif
        }

        public float GetAxisRaw(string axisName)
        {
#if !OBSOLETE
            if (inputAsset == null)
            {
                Debug.LogWarning("There is no input asset on " + gameObject.name + ".", gameObject);
                return 0;
            }

            if (actionsDictionary.TryGetValue(axisName, out InputAction inputAction))
            {
                if (inputAction == null)
                {
                    return 0;
                }

                if (inputAction.activeControl is AxisControl axis)
                {
                    return axis.ReadUnprocessedValue();
                }
                else
                {
                    Debug.LogError(axisName + " is not an axis type.");
                    return 0;
                }
            }
            else
            {
                Debug.LogError("Can't find action '" + axisName + "' in " + inputAsset.name + "!");
                return 0;
            }
#else
            return 0;
#endif
        }

        public Vector2 GetVector2(string action)
        {
#if !OBSOLETE
            if (inputAsset == null)
            {
                Debug.LogWarning("There is no input asset on " + gameObject.name + ".", gameObject);
                return Vector2.zero;
            }

            if (actionsDictionary == null)
            {
                UpdateActions();
            }

            if (actionsDictionary.TryGetValue(action, out InputAction inputAction))
            {
                if (inputAction == null)
                {
                    return Vector2.zero;
                }

                return inputAction.ReadValue<Vector2>();
            }
            else
            {
                Debug.LogError("Can't find action '" + action + "' in " + inputAsset.name + "!");
                return Vector2.zero;
            }
#else
            return Vector2.zero;
#endif
        }

#if UNITY_EDITOR && !OBSOLETE
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                UpdateActions();

                if (enabledInput)
                {
                    EnableInput();
                }
            }
        }

        private void Reset()
        {
            GoldPlayerController gp = GetComponent<GoldPlayerController>();
#if !GOLD_PLAYER_DISABLE_INTERACTION
            GoldPlayerInteraction gi = GetComponent<GoldPlayerInteraction>();
#endif

            actions = new InputItem[]
            {
                new InputItem(gp != null ? gp.Camera.LookInput : "Look", null),
                new InputItem(gp != null ? gp.Movement.MoveInput : "Move", null),
                new InputItem(gp != null ? gp.Movement.JumpInput : "Jump", null),
                new InputItem(gp != null ? gp.Movement.RunInput : "Run", null),
                new InputItem(gp != null ? gp.Movement.CrouchInput : "Crouch", null),
#if !GOLD_PLAYER_DISABLE_INTERACTION
                new InputItem(gi != null ? gi.InteractInput : "Interact", null)
#endif
            };
        }
#endif
    }
}
#endif