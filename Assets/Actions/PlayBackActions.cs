//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Actions/PlayBackActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayBackActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayBackActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayBackActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""61e9be48-9476-44da-8b9b-ccf1917b7812"",
            ""actions"": [
                {
                    ""name"": ""PausePlay"",
                    ""type"": ""Button"",
                    ""id"": ""42526720-52fb-43db-bf5d-7b107f419741"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Step"",
                    ""type"": ""Button"",
                    ""id"": ""6383857c-68c8-4085-b2f7-fcf7cf91a89a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""133627c2-277f-498e-a805-236a4ac97918"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PausePlay"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89326caa-7cdc-4b84-85eb-9f1ace1fabe9"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Step"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_PausePlay = m_Player.FindAction("PausePlay", throwIfNotFound: true);
        m_Player_Step = m_Player.FindAction("Step", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_PausePlay;
    private readonly InputAction m_Player_Step;
    public struct PlayerActions
    {
        private @PlayBackActions m_Wrapper;
        public PlayerActions(@PlayBackActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PausePlay => m_Wrapper.m_Player_PausePlay;
        public InputAction @Step => m_Wrapper.m_Player_Step;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @PausePlay.started += instance.OnPausePlay;
            @PausePlay.performed += instance.OnPausePlay;
            @PausePlay.canceled += instance.OnPausePlay;
            @Step.started += instance.OnStep;
            @Step.performed += instance.OnStep;
            @Step.canceled += instance.OnStep;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @PausePlay.started -= instance.OnPausePlay;
            @PausePlay.performed -= instance.OnPausePlay;
            @PausePlay.canceled -= instance.OnPausePlay;
            @Step.started -= instance.OnStep;
            @Step.performed -= instance.OnStep;
            @Step.canceled -= instance.OnStep;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnPausePlay(InputAction.CallbackContext context);
        void OnStep(InputAction.CallbackContext context);
    }
}
