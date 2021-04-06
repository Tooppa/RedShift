// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Surface"",
            ""id"": ""917b69c6-c317-4755-af81-7e188084bf2b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""f2e6a78a-109e-4b3d-9c78-52ab5c74c403"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenHud"",
                    ""type"": ""Button"",
                    ""id"": ""1a478481-d51c-4887-9fd1-e16506cef9c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""b3fd68d1-9ee0-4221-bf90-86174348857c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Flashlight"",
                    ""type"": ""Button"",
                    ""id"": ""4ec23875-d2c6-4f29-b84e-ddbd340c88cc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e110194a-af62-4c2b-b4a9-b860392ee48a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""b2d1093d-b8ce-4b6b-a777-4639101df3aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""529b5223-2885-41f1-a494-7759b7047426"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ef0f5b07-6a12-4e8d-906e-3eff433706b5"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenHud"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4b8a0ab-be50-474f-ac30-4855eece9747"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7b09a95-dac2-49f5-a2bb-d0a407842ae2"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flashlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""507c6f41-c058-4090-9150-c601229e66f9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""069c1748-91e5-40f4-9a96-8749cdc5afd8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6bcecabd-7805-4562-9cb7-3e530b92e933"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b9a660e5-46d6-46c1-8a57-b9c5e324dd87"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fee75e84-1147-4fdb-888a-5fe011b3bfc1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1f2e19d9-5572-4547-9c06-e2b39fd9c401"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e961b69-f16d-43d5-af9b-ad5b305e102d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32415fc1-317c-4d8b-82f1-9af43f439866"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Surface
        m_Surface = asset.FindActionMap("Surface", throwIfNotFound: true);
        m_Surface_Move = m_Surface.FindAction("Move", throwIfNotFound: true);
        m_Surface_OpenHud = m_Surface.FindAction("OpenHud", throwIfNotFound: true);
        m_Surface_Interact = m_Surface.FindAction("Interact", throwIfNotFound: true);
        m_Surface_Flashlight = m_Surface.FindAction("Flashlight", throwIfNotFound: true);
        m_Surface_Jump = m_Surface.FindAction("Jump", throwIfNotFound: true);
        m_Surface_Dash = m_Surface.FindAction("Dash", throwIfNotFound: true);
        m_Surface_Shoot = m_Surface.FindAction("Shoot", throwIfNotFound: true);
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

    // Surface
    private readonly InputActionMap m_Surface;
    private ISurfaceActions m_SurfaceActionsCallbackInterface;
    private readonly InputAction m_Surface_Move;
    private readonly InputAction m_Surface_OpenHud;
    private readonly InputAction m_Surface_Interact;
    private readonly InputAction m_Surface_Flashlight;
    private readonly InputAction m_Surface_Jump;
    private readonly InputAction m_Surface_Dash;
    private readonly InputAction m_Surface_Shoot;
    public struct SurfaceActions
    {
        private @PlayerControls m_Wrapper;
        public SurfaceActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Surface_Move;
        public InputAction @OpenHud => m_Wrapper.m_Surface_OpenHud;
        public InputAction @Interact => m_Wrapper.m_Surface_Interact;
        public InputAction @Flashlight => m_Wrapper.m_Surface_Flashlight;
        public InputAction @Jump => m_Wrapper.m_Surface_Jump;
        public InputAction @Dash => m_Wrapper.m_Surface_Dash;
        public InputAction @Shoot => m_Wrapper.m_Surface_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Surface; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SurfaceActions set) { return set.Get(); }
        public void SetCallbacks(ISurfaceActions instance)
        {
            if (m_Wrapper.m_SurfaceActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnMove;
                @OpenHud.started -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnOpenHud;
                @OpenHud.performed -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnOpenHud;
                @OpenHud.canceled -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnOpenHud;
                @Interact.started -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnInteract;
                @Flashlight.started -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnFlashlight;
                @Flashlight.performed -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnFlashlight;
                @Flashlight.canceled -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnFlashlight;
                @Jump.started -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnJump;
                @Dash.started -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnDash;
                @Shoot.started -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_SurfaceActionsCallbackInterface.OnShoot;
            }
            m_Wrapper.m_SurfaceActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @OpenHud.started += instance.OnOpenHud;
                @OpenHud.performed += instance.OnOpenHud;
                @OpenHud.canceled += instance.OnOpenHud;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Flashlight.started += instance.OnFlashlight;
                @Flashlight.performed += instance.OnFlashlight;
                @Flashlight.canceled += instance.OnFlashlight;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
            }
        }
    }
    public SurfaceActions @Surface => new SurfaceActions(this);
    public interface ISurfaceActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnOpenHud(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnFlashlight(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
}
