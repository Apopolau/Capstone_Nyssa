//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Controllers & Systems/Input Systems/CelestialPlayerInputActions.inputactions
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

public partial class @CelestialPlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CelestialPlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CelestialPlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""CelestialPlayerDefault"",
            ""id"": ""fd55c840-f4fd-4a8b-a6f2-f8278c296c0e"",
            ""actions"": [
                {
                    ""name"": ""CelestialWalk"",
                    ""type"": ""Value"",
                    ""id"": ""a4e29042-cd0b-47ae-a6f8-845de2e6571d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MakeRain"",
                    ""type"": ""Button"",
                    ""id"": ""d31d8289-53b7-497e-97ab-003d87e38b48"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MakeColdSnap"",
                    ""type"": ""Button"",
                    ""id"": ""abe50024-f9fd-430e-97bb-714ba157b059"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MakeLighteningStrike"",
                    ""type"": ""Button"",
                    ""id"": ""dd920340-10f7-48ce-9c68-9a1673690858"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MakeSmog"",
                    ""type"": ""Button"",
                    ""id"": ""d3ebafe9-90c6-44be-b922-ccd15c656c95"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MakeSunBeam"",
                    ""type"": ""Button"",
                    ""id"": ""a1268b76-f0a0-4d53-ab24-df712bd1790d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MakeDodge"",
                    ""type"": ""Button"",
                    ""id"": ""4deb8876-8554-4b2e-9552-fbfbcdbde72c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Controller"",
                    ""id"": ""6d015667-5eff-46f8-b4d6-b05081afdacf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b433d833-df6f-4362-80a0-66bc16502076"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""15520ac7-25a8-44cb-a7d0-7a3f7b12bea7"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""65a9990c-9c27-4707-bb32-74b154cded0c"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""112fe192-9d7e-4f31-a5ff-44e8fe7fc262"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""aca82e20-b5cd-4d1e-bd09-4a73034ab324"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fb5e921c-01e1-455a-956b-e016538af1ff"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0d93a4ce-c54a-4cdb-ac25-2de93e14606e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""76ef8127-2714-48dc-ac4b-8a6bc69fc290"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""30a910c2-5a7f-4e3d-acc6-3d12993967f3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CelestialWalk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5888b0e5-3e5f-470c-81b0-6642e8da71de"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeRain"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c5b4593-75b6-49f5-b8dd-2594130a33a9"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeColdSnap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f84d75e-4084-4f06-b426-1b0e736e6988"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeSmog"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9913192c-1396-455e-a9cf-b8ae097318ad"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeSunBeam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87cc3060-d82e-475b-b20b-b934a328021f"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeDodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""KeyBoard"",
                    ""id"": ""a089a833-2bca-4360-a49f-f6642ea2aede"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeRain"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""dcddc6f7-ad6a-4f51-ace2-eb48c93b816a"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeRain"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""KeyBoard"",
                    ""id"": ""12dc3e48-704a-4528-afe8-8f86f8d8c6ee"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeColdSnap"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0e13d44f-1373-491c-8784-f1cb6ceaa6b2"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeColdSnap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""KeyBoard"",
                    ""id"": ""9833fa81-63f8-4aea-a888-b0ed52c7804b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeSmog"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""622b1fc4-3eb3-4c78-bb18-6ac5f837c262"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeSmog"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""KeyBoard"",
                    ""id"": ""f7e4d591-2154-4416-bf20-2b37e88dd6c6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeSunBeam"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ed15ce8e-9d55-4e31-8a2b-6be237758022"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeSunBeam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""KeyBoard"",
                    ""id"": ""550a50cf-e31b-45b1-8b9e-89f2f233f3b6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeDodge"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""836d3692-3388-475d-a2b7-4e06a88be618"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeDodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d95e9c81-4c47-4fab-b047-b2ad46eb3e46"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeLighteningStrike"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""KeyBoard"",
                    ""id"": ""8d308be7-5956-4a22-ab5b-0ce4164bdb6c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeLighteningStrike"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bea95190-6609-4444-a7b3-ae329e903299"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MakeLighteningStrike"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""MenuControls"",
            ""id"": ""ed706210-954b-498f-9499-82c477473bfd"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""5fa4e077-2c25-4af5-b10b-32044342f299"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3e1781bc-7c92-4e79-8012-49dc85979875"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""DialogueControls"",
            ""id"": ""8657378d-3483-4078-9f19-c32deff41834"",
            ""actions"": [
                {
                    ""name"": ""Continue"",
                    ""type"": ""Button"",
                    ""id"": ""832fe578-41e0-42e8-ac4e-7e2d5dbafed6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skip"",
                    ""type"": ""Button"",
                    ""id"": ""440eb206-42f3-4595-a22b-797778b7eee4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1263d104-e2bf-4317-8798-ea99ab85ba4a"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9469a737-c776-4334-8a4b-abf5af463337"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae11d5c3-b112-4d73-ac5c-9e473371f8d2"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24f105ae-b7f1-44cf-9029-79909122978e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CelestialPlayerDefault
        m_CelestialPlayerDefault = asset.FindActionMap("CelestialPlayerDefault", throwIfNotFound: true);
        m_CelestialPlayerDefault_CelestialWalk = m_CelestialPlayerDefault.FindAction("CelestialWalk", throwIfNotFound: true);
        m_CelestialPlayerDefault_MakeRain = m_CelestialPlayerDefault.FindAction("MakeRain", throwIfNotFound: true);
        m_CelestialPlayerDefault_MakeColdSnap = m_CelestialPlayerDefault.FindAction("MakeColdSnap", throwIfNotFound: true);
        m_CelestialPlayerDefault_MakeLighteningStrike = m_CelestialPlayerDefault.FindAction("MakeLighteningStrike", throwIfNotFound: true);
        m_CelestialPlayerDefault_MakeSmog = m_CelestialPlayerDefault.FindAction("MakeSmog", throwIfNotFound: true);
        m_CelestialPlayerDefault_MakeSunBeam = m_CelestialPlayerDefault.FindAction("MakeSunBeam", throwIfNotFound: true);
        m_CelestialPlayerDefault_MakeDodge = m_CelestialPlayerDefault.FindAction("MakeDodge", throwIfNotFound: true);
        // MenuControls
        m_MenuControls = asset.FindActionMap("MenuControls", throwIfNotFound: true);
        m_MenuControls_Newaction = m_MenuControls.FindAction("New action", throwIfNotFound: true);
        // DialogueControls
        m_DialogueControls = asset.FindActionMap("DialogueControls", throwIfNotFound: true);
        m_DialogueControls_Continue = m_DialogueControls.FindAction("Continue", throwIfNotFound: true);
        m_DialogueControls_Skip = m_DialogueControls.FindAction("Skip", throwIfNotFound: true);
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

    // CelestialPlayerDefault
    private readonly InputActionMap m_CelestialPlayerDefault;
    private List<ICelestialPlayerDefaultActions> m_CelestialPlayerDefaultActionsCallbackInterfaces = new List<ICelestialPlayerDefaultActions>();
    private readonly InputAction m_CelestialPlayerDefault_CelestialWalk;
    private readonly InputAction m_CelestialPlayerDefault_MakeRain;
    private readonly InputAction m_CelestialPlayerDefault_MakeColdSnap;
    private readonly InputAction m_CelestialPlayerDefault_MakeLighteningStrike;
    private readonly InputAction m_CelestialPlayerDefault_MakeSmog;
    private readonly InputAction m_CelestialPlayerDefault_MakeSunBeam;
    private readonly InputAction m_CelestialPlayerDefault_MakeDodge;
    public struct CelestialPlayerDefaultActions
    {
        private @CelestialPlayerInputActions m_Wrapper;
        public CelestialPlayerDefaultActions(@CelestialPlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @CelestialWalk => m_Wrapper.m_CelestialPlayerDefault_CelestialWalk;
        public InputAction @MakeRain => m_Wrapper.m_CelestialPlayerDefault_MakeRain;
        public InputAction @MakeColdSnap => m_Wrapper.m_CelestialPlayerDefault_MakeColdSnap;
        public InputAction @MakeLighteningStrike => m_Wrapper.m_CelestialPlayerDefault_MakeLighteningStrike;
        public InputAction @MakeSmog => m_Wrapper.m_CelestialPlayerDefault_MakeSmog;
        public InputAction @MakeSunBeam => m_Wrapper.m_CelestialPlayerDefault_MakeSunBeam;
        public InputAction @MakeDodge => m_Wrapper.m_CelestialPlayerDefault_MakeDodge;
        public InputActionMap Get() { return m_Wrapper.m_CelestialPlayerDefault; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CelestialPlayerDefaultActions set) { return set.Get(); }
        public void AddCallbacks(ICelestialPlayerDefaultActions instance)
        {
            if (instance == null || m_Wrapper.m_CelestialPlayerDefaultActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CelestialPlayerDefaultActionsCallbackInterfaces.Add(instance);
            @CelestialWalk.started += instance.OnCelestialWalk;
            @CelestialWalk.performed += instance.OnCelestialWalk;
            @CelestialWalk.canceled += instance.OnCelestialWalk;
            @MakeRain.started += instance.OnMakeRain;
            @MakeRain.performed += instance.OnMakeRain;
            @MakeRain.canceled += instance.OnMakeRain;
            @MakeColdSnap.started += instance.OnMakeColdSnap;
            @MakeColdSnap.performed += instance.OnMakeColdSnap;
            @MakeColdSnap.canceled += instance.OnMakeColdSnap;
            @MakeLighteningStrike.started += instance.OnMakeLighteningStrike;
            @MakeLighteningStrike.performed += instance.OnMakeLighteningStrike;
            @MakeLighteningStrike.canceled += instance.OnMakeLighteningStrike;
            @MakeSmog.started += instance.OnMakeSmog;
            @MakeSmog.performed += instance.OnMakeSmog;
            @MakeSmog.canceled += instance.OnMakeSmog;
            @MakeSunBeam.started += instance.OnMakeSunBeam;
            @MakeSunBeam.performed += instance.OnMakeSunBeam;
            @MakeSunBeam.canceled += instance.OnMakeSunBeam;
            @MakeDodge.started += instance.OnMakeDodge;
            @MakeDodge.performed += instance.OnMakeDodge;
            @MakeDodge.canceled += instance.OnMakeDodge;
        }

        private void UnregisterCallbacks(ICelestialPlayerDefaultActions instance)
        {
            @CelestialWalk.started -= instance.OnCelestialWalk;
            @CelestialWalk.performed -= instance.OnCelestialWalk;
            @CelestialWalk.canceled -= instance.OnCelestialWalk;
            @MakeRain.started -= instance.OnMakeRain;
            @MakeRain.performed -= instance.OnMakeRain;
            @MakeRain.canceled -= instance.OnMakeRain;
            @MakeColdSnap.started -= instance.OnMakeColdSnap;
            @MakeColdSnap.performed -= instance.OnMakeColdSnap;
            @MakeColdSnap.canceled -= instance.OnMakeColdSnap;
            @MakeLighteningStrike.started -= instance.OnMakeLighteningStrike;
            @MakeLighteningStrike.performed -= instance.OnMakeLighteningStrike;
            @MakeLighteningStrike.canceled -= instance.OnMakeLighteningStrike;
            @MakeSmog.started -= instance.OnMakeSmog;
            @MakeSmog.performed -= instance.OnMakeSmog;
            @MakeSmog.canceled -= instance.OnMakeSmog;
            @MakeSunBeam.started -= instance.OnMakeSunBeam;
            @MakeSunBeam.performed -= instance.OnMakeSunBeam;
            @MakeSunBeam.canceled -= instance.OnMakeSunBeam;
            @MakeDodge.started -= instance.OnMakeDodge;
            @MakeDodge.performed -= instance.OnMakeDodge;
            @MakeDodge.canceled -= instance.OnMakeDodge;
        }

        public void RemoveCallbacks(ICelestialPlayerDefaultActions instance)
        {
            if (m_Wrapper.m_CelestialPlayerDefaultActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICelestialPlayerDefaultActions instance)
        {
            foreach (var item in m_Wrapper.m_CelestialPlayerDefaultActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CelestialPlayerDefaultActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CelestialPlayerDefaultActions @CelestialPlayerDefault => new CelestialPlayerDefaultActions(this);

    // MenuControls
    private readonly InputActionMap m_MenuControls;
    private List<IMenuControlsActions> m_MenuControlsActionsCallbackInterfaces = new List<IMenuControlsActions>();
    private readonly InputAction m_MenuControls_Newaction;
    public struct MenuControlsActions
    {
        private @CelestialPlayerInputActions m_Wrapper;
        public MenuControlsActions(@CelestialPlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_MenuControls_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_MenuControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuControlsActions set) { return set.Get(); }
        public void AddCallbacks(IMenuControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_MenuControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MenuControlsActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        private void UnregisterCallbacks(IMenuControlsActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        public void RemoveCallbacks(IMenuControlsActions instance)
        {
            if (m_Wrapper.m_MenuControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMenuControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_MenuControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MenuControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MenuControlsActions @MenuControls => new MenuControlsActions(this);

    // DialogueControls
    private readonly InputActionMap m_DialogueControls;
    private List<IDialogueControlsActions> m_DialogueControlsActionsCallbackInterfaces = new List<IDialogueControlsActions>();
    private readonly InputAction m_DialogueControls_Continue;
    private readonly InputAction m_DialogueControls_Skip;
    public struct DialogueControlsActions
    {
        private @CelestialPlayerInputActions m_Wrapper;
        public DialogueControlsActions(@CelestialPlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Continue => m_Wrapper.m_DialogueControls_Continue;
        public InputAction @Skip => m_Wrapper.m_DialogueControls_Skip;
        public InputActionMap Get() { return m_Wrapper.m_DialogueControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DialogueControlsActions set) { return set.Get(); }
        public void AddCallbacks(IDialogueControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_DialogueControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DialogueControlsActionsCallbackInterfaces.Add(instance);
            @Continue.started += instance.OnContinue;
            @Continue.performed += instance.OnContinue;
            @Continue.canceled += instance.OnContinue;
            @Skip.started += instance.OnSkip;
            @Skip.performed += instance.OnSkip;
            @Skip.canceled += instance.OnSkip;
        }

        private void UnregisterCallbacks(IDialogueControlsActions instance)
        {
            @Continue.started -= instance.OnContinue;
            @Continue.performed -= instance.OnContinue;
            @Continue.canceled -= instance.OnContinue;
            @Skip.started -= instance.OnSkip;
            @Skip.performed -= instance.OnSkip;
            @Skip.canceled -= instance.OnSkip;
        }

        public void RemoveCallbacks(IDialogueControlsActions instance)
        {
            if (m_Wrapper.m_DialogueControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDialogueControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_DialogueControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DialogueControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DialogueControlsActions @DialogueControls => new DialogueControlsActions(this);
    public interface ICelestialPlayerDefaultActions
    {
        void OnCelestialWalk(InputAction.CallbackContext context);
        void OnMakeRain(InputAction.CallbackContext context);
        void OnMakeColdSnap(InputAction.CallbackContext context);
        void OnMakeLighteningStrike(InputAction.CallbackContext context);
        void OnMakeSmog(InputAction.CallbackContext context);
        void OnMakeSunBeam(InputAction.CallbackContext context);
        void OnMakeDodge(InputAction.CallbackContext context);
    }
    public interface IMenuControlsActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
    public interface IDialogueControlsActions
    {
        void OnContinue(InputAction.CallbackContext context);
        void OnSkip(InputAction.CallbackContext context);
    }
}
