//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Scripts/Input/Inputs.inputactions
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

public partial class @Inputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""Input"",
            ""id"": ""72462321-4107-4cad-bd7d-269c9150a246"",
            ""actions"": [
                {
                    ""name"": ""LookAt"",
                    ""type"": ""Value"",
                    ""id"": ""5242b132-15e1-4311-9e62-dd4b6696ad33"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Value"",
                    ""id"": ""aadd64c2-d96e-457c-b178-86274265aa08"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9af4595c-8d28-49ca-827c-c7a23b9f47cd"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookAt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1cdfeff0-abe0-43d7-851c-e694617c4ea9"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""IsometricInput"",
            ""id"": ""df222970-edfb-4d4a-9460-3fdfe358647b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""10e255ad-a3b5-4ae3-a2df-a835c2cd4c49"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Button"",
                    ""id"": ""1b2d04a4-e083-4e76-907c-12c8ac8839bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Value"",
                    ""id"": ""8a64b36a-e457-46fa-881c-77848ddb9dce"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Value"",
                    ""id"": ""def77380-53a3-44b4-a643-12980ae3c52f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""0c3c4a2e-6d67-478c-89b3-16106a53c93c"",
                    ""path"": ""1DAxis(minValue=-0.5)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e7fe01ed-0438-488e-bab8-764200941d13"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""491d3bbd-57db-43d4-bc46-589eed8389b9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""c41e6af9-f785-4987-884f-eb81dc849900"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""b46fee00-b99b-4af0-b08d-a8ac9060fa61"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""a82ce684-6804-45ec-8e32-a1f4917a0d1d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5786308b-28a7-4613-9694-bd7515845d12"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""849d9582-e307-4b58-bfe8-947cde6bb4f3"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""FirstPersonInput"",
            ""id"": ""68cf310e-5714-4010-8b95-e5e387ac0954"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Value"",
                    ""id"": ""5e317b3d-9f2b-458b-8961-eef5013e1959"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""35b369e4-4724-4838-aec4-e6630f2cc398"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3f87bc21-beb7-4cd6-9824-81ecc890a19a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""659df79c-927f-496e-a74c-793f6c494c85"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.05,y=0.05)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Input
        m_Input = asset.FindActionMap("Input", throwIfNotFound: true);
        m_Input_LookAt = m_Input.FindAction("LookAt", throwIfNotFound: true);
        m_Input_Reload = m_Input.FindAction("Reload", throwIfNotFound: true);
        // IsometricInput
        m_IsometricInput = asset.FindActionMap("IsometricInput", throwIfNotFound: true);
        m_IsometricInput_Move = m_IsometricInput.FindAction("Move", throwIfNotFound: true);
        m_IsometricInput_Rotation = m_IsometricInput.FindAction("Rotation", throwIfNotFound: true);
        m_IsometricInput_Run = m_IsometricInput.FindAction("Run", throwIfNotFound: true);
        m_IsometricInput_Interact = m_IsometricInput.FindAction("Interact", throwIfNotFound: true);
        // FirstPersonInput
        m_FirstPersonInput = asset.FindActionMap("FirstPersonInput", throwIfNotFound: true);
        m_FirstPersonInput_Fire = m_FirstPersonInput.FindAction("Fire", throwIfNotFound: true);
        m_FirstPersonInput_Look = m_FirstPersonInput.FindAction("Look", throwIfNotFound: true);
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

    // Input
    private readonly InputActionMap m_Input;
    private List<IInputActions> m_InputActionsCallbackInterfaces = new List<IInputActions>();
    private readonly InputAction m_Input_LookAt;
    private readonly InputAction m_Input_Reload;
    public struct InputActions
    {
        private @Inputs m_Wrapper;
        public InputActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @LookAt => m_Wrapper.m_Input_LookAt;
        public InputAction @Reload => m_Wrapper.m_Input_Reload;
        public InputActionMap Get() { return m_Wrapper.m_Input; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InputActions set) { return set.Get(); }
        public void AddCallbacks(IInputActions instance)
        {
            if (instance == null || m_Wrapper.m_InputActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InputActionsCallbackInterfaces.Add(instance);
            @LookAt.started += instance.OnLookAt;
            @LookAt.performed += instance.OnLookAt;
            @LookAt.canceled += instance.OnLookAt;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
        }

        private void UnregisterCallbacks(IInputActions instance)
        {
            @LookAt.started -= instance.OnLookAt;
            @LookAt.performed -= instance.OnLookAt;
            @LookAt.canceled -= instance.OnLookAt;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
        }

        public void RemoveCallbacks(IInputActions instance)
        {
            if (m_Wrapper.m_InputActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInputActions instance)
        {
            foreach (var item in m_Wrapper.m_InputActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InputActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InputActions @Input => new InputActions(this);

    // IsometricInput
    private readonly InputActionMap m_IsometricInput;
    private List<IIsometricInputActions> m_IsometricInputActionsCallbackInterfaces = new List<IIsometricInputActions>();
    private readonly InputAction m_IsometricInput_Move;
    private readonly InputAction m_IsometricInput_Rotation;
    private readonly InputAction m_IsometricInput_Run;
    private readonly InputAction m_IsometricInput_Interact;
    public struct IsometricInputActions
    {
        private @Inputs m_Wrapper;
        public IsometricInputActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_IsometricInput_Move;
        public InputAction @Rotation => m_Wrapper.m_IsometricInput_Rotation;
        public InputAction @Run => m_Wrapper.m_IsometricInput_Run;
        public InputAction @Interact => m_Wrapper.m_IsometricInput_Interact;
        public InputActionMap Get() { return m_Wrapper.m_IsometricInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(IsometricInputActions set) { return set.Get(); }
        public void AddCallbacks(IIsometricInputActions instance)
        {
            if (instance == null || m_Wrapper.m_IsometricInputActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_IsometricInputActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Rotation.started += instance.OnRotation;
            @Rotation.performed += instance.OnRotation;
            @Rotation.canceled += instance.OnRotation;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
        }

        private void UnregisterCallbacks(IIsometricInputActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Rotation.started -= instance.OnRotation;
            @Rotation.performed -= instance.OnRotation;
            @Rotation.canceled -= instance.OnRotation;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
        }

        public void RemoveCallbacks(IIsometricInputActions instance)
        {
            if (m_Wrapper.m_IsometricInputActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IIsometricInputActions instance)
        {
            foreach (var item in m_Wrapper.m_IsometricInputActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_IsometricInputActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public IsometricInputActions @IsometricInput => new IsometricInputActions(this);

    // FirstPersonInput
    private readonly InputActionMap m_FirstPersonInput;
    private List<IFirstPersonInputActions> m_FirstPersonInputActionsCallbackInterfaces = new List<IFirstPersonInputActions>();
    private readonly InputAction m_FirstPersonInput_Fire;
    private readonly InputAction m_FirstPersonInput_Look;
    public struct FirstPersonInputActions
    {
        private @Inputs m_Wrapper;
        public FirstPersonInputActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_FirstPersonInput_Fire;
        public InputAction @Look => m_Wrapper.m_FirstPersonInput_Look;
        public InputActionMap Get() { return m_Wrapper.m_FirstPersonInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FirstPersonInputActions set) { return set.Get(); }
        public void AddCallbacks(IFirstPersonInputActions instance)
        {
            if (instance == null || m_Wrapper.m_FirstPersonInputActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_FirstPersonInputActionsCallbackInterfaces.Add(instance);
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
        }

        private void UnregisterCallbacks(IFirstPersonInputActions instance)
        {
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
        }

        public void RemoveCallbacks(IFirstPersonInputActions instance)
        {
            if (m_Wrapper.m_FirstPersonInputActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IFirstPersonInputActions instance)
        {
            foreach (var item in m_Wrapper.m_FirstPersonInputActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_FirstPersonInputActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public FirstPersonInputActions @FirstPersonInput => new FirstPersonInputActions(this);
    public interface IInputActions
    {
        void OnLookAt(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
    }
    public interface IIsometricInputActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
    public interface IFirstPersonInputActions
    {
        void OnFire(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
}
