//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Samples/Input System/PlayerInputActions.inputactions
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

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""EarthPlayerDefault"",
            ""id"": ""966d5caa-72aa-4065-bd25-131c7c92cb41"",
            ""actions"": [
                {
                    ""name"": ""Walk"",
                    ""type"": ""Value"",
                    ""id"": ""43db4cfc-7821-4a25-98e3-5b4088b2bae9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PickTree"",
                    ""type"": ""Button"",
                    ""id"": ""495a34ab-3f11-4936-b56e-af29a9bccb07"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PickGrass"",
                    ""type"": ""Button"",
                    ""id"": ""b9e78fa6-71d3-466d-bd61-ae801e14870c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PickFlower"",
                    ""type"": ""Button"",
                    ""id"": ""5d3b6a75-ebc5-4669-b5ff-271a74081743"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Remove Building"",
                    ""type"": ""Button"",
                    ""id"": ""514d0af8-d2f9-4fd1-9f44-2409bf99d06b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""a01b538b-153a-45fc-8c9e-8aae773c39d3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Debug Tileflip"",
                    ""type"": ""Button"",
                    ""id"": ""7b88c43f-dd52-49f5-b524-0eb0d63f124c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""811510da-74ec-48dd-b739-5ba28b806dbf"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickTree"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbe9e24c-63fc-46bc-923a-e4b3daa1c9ee"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickTree"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9d638ee-8c65-47ee-b44b-6a86993f259a"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickGrass"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9de986a9-5537-44dc-bdd5-ad17691ded38"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickGrass"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db31b04e-9491-4154-a099-82a2f8533792"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickFlower"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""819a3489-f125-4f9d-b03a-212eb3fefb7b"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickFlower"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fda73281-8b58-4c1e-b653-3a8799fa97fd"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Remove Building"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b83cc183-82f3-464e-92e6-91577a02b239"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Remove Building"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Controller"",
                    ""id"": ""aa1559d0-f32b-4667-9710-717974944141"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.2,max=1)"",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f82b9738-ef29-4a13-98da-c536560ccbae"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""aae35a44-e8c2-4f5d-9826-54b674f81a4d"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ea58e246-29ce-4712-b0b1-4d0285e9e343"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bb29ec5e-2264-475a-b254-30867cf1943f"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""9640d80c-cdbe-4cfa-af27-699424941762"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c5b6b3b0-e28d-4b09-b687-6a04f4479b4e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5583e7e6-b8f4-4782-8ec5-0a40d651f849"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e26fdfd1-924c-4ed0-9067-6cfd96b4abed"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6a880111-8b2f-49f9-8563-ade3df2d9fb9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""30582203-2784-484e-a200-e74f58a21e92"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f74824a8-a3ac-43ab-b042-5ae0c2631b16"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fddf9087-6640-4efc-8d88-3cd3b5f639f5"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Debug Tileflip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlantIsSelected"",
            ""id"": ""bcc7bf38-e7f8-48eb-8d1b-58fd81b0eff0"",
            ""actions"": [
                {
                    ""name"": ""Plant plant"",
                    ""type"": ""Button"",
                    ""id"": ""9491d659-6adc-47b3-9a57-a00f6d07d1bb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel planting"",
                    ""type"": ""Button"",
                    ""id"": ""fb33cbf5-e211-45bc-af25-fa98fed3c34b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""f135b41f-ef8f-4405-a125-3b506d4c8835"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Cursor Move"",
                    ""type"": ""Value"",
                    ""id"": ""4d61e0a9-39e3-4fef-a2b0-5a829350c262"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""01c1f3cd-ffcc-4b6a-bef0-c6b6d6646149"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Plant plant"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f0a3964-fbde-44be-b691-60e18e3ce6df"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Plant plant"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c777e4d2-f813-469c-9feb-9f11070ae7a8"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel planting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc566b5a-0681-41bf-b55e-61a009eed32d"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel planting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81770430-cad4-4575-82bc-9a65fc9e432b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e5a0abd-321d-48c8-8082-951276b2f4d1"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cursor Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ac3baf0-6ffa-4f19-9b7e-bc90895b03b4"",
                    ""path"": ""<VirtualMouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cursor Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ebd3025-3762-43e5-a6e5-740d068858bb"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cursor Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // EarthPlayerDefault
        m_EarthPlayerDefault = asset.FindActionMap("EarthPlayerDefault", throwIfNotFound: true);
        m_EarthPlayerDefault_Walk = m_EarthPlayerDefault.FindAction("Walk", throwIfNotFound: true);
        m_EarthPlayerDefault_PickTree = m_EarthPlayerDefault.FindAction("PickTree", throwIfNotFound: true);
        m_EarthPlayerDefault_PickGrass = m_EarthPlayerDefault.FindAction("PickGrass", throwIfNotFound: true);
        m_EarthPlayerDefault_PickFlower = m_EarthPlayerDefault.FindAction("PickFlower", throwIfNotFound: true);
        m_EarthPlayerDefault_RemoveBuilding = m_EarthPlayerDefault.FindAction("Remove Building", throwIfNotFound: true);
        m_EarthPlayerDefault_Interact = m_EarthPlayerDefault.FindAction("Interact", throwIfNotFound: true);
        m_EarthPlayerDefault_DebugTileflip = m_EarthPlayerDefault.FindAction("Debug Tileflip", throwIfNotFound: true);
        // PlantIsSelected
        m_PlantIsSelected = asset.FindActionMap("PlantIsSelected", throwIfNotFound: true);
        m_PlantIsSelected_Plantplant = m_PlantIsSelected.FindAction("Plant plant", throwIfNotFound: true);
        m_PlantIsSelected_Cancelplanting = m_PlantIsSelected.FindAction("Cancel planting", throwIfNotFound: true);
        m_PlantIsSelected_Move = m_PlantIsSelected.FindAction("Move", throwIfNotFound: true);
        m_PlantIsSelected_CursorMove = m_PlantIsSelected.FindAction("Cursor Move", throwIfNotFound: true);
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

    // EarthPlayerDefault
    private readonly InputActionMap m_EarthPlayerDefault;
    private List<IEarthPlayerDefaultActions> m_EarthPlayerDefaultActionsCallbackInterfaces = new List<IEarthPlayerDefaultActions>();
    private readonly InputAction m_EarthPlayerDefault_Walk;
    private readonly InputAction m_EarthPlayerDefault_PickTree;
    private readonly InputAction m_EarthPlayerDefault_PickGrass;
    private readonly InputAction m_EarthPlayerDefault_PickFlower;
    private readonly InputAction m_EarthPlayerDefault_RemoveBuilding;
    private readonly InputAction m_EarthPlayerDefault_Interact;
    private readonly InputAction m_EarthPlayerDefault_DebugTileflip;
    public struct EarthPlayerDefaultActions
    {
        private @PlayerInputActions m_Wrapper;
        public EarthPlayerDefaultActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Walk => m_Wrapper.m_EarthPlayerDefault_Walk;
        public InputAction @PickTree => m_Wrapper.m_EarthPlayerDefault_PickTree;
        public InputAction @PickGrass => m_Wrapper.m_EarthPlayerDefault_PickGrass;
        public InputAction @PickFlower => m_Wrapper.m_EarthPlayerDefault_PickFlower;
        public InputAction @RemoveBuilding => m_Wrapper.m_EarthPlayerDefault_RemoveBuilding;
        public InputAction @Interact => m_Wrapper.m_EarthPlayerDefault_Interact;
        public InputAction @DebugTileflip => m_Wrapper.m_EarthPlayerDefault_DebugTileflip;
        public InputActionMap Get() { return m_Wrapper.m_EarthPlayerDefault; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EarthPlayerDefaultActions set) { return set.Get(); }
        public void AddCallbacks(IEarthPlayerDefaultActions instance)
        {
            if (instance == null || m_Wrapper.m_EarthPlayerDefaultActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_EarthPlayerDefaultActionsCallbackInterfaces.Add(instance);
            @Walk.started += instance.OnWalk;
            @Walk.performed += instance.OnWalk;
            @Walk.canceled += instance.OnWalk;
            @PickTree.started += instance.OnPickTree;
            @PickTree.performed += instance.OnPickTree;
            @PickTree.canceled += instance.OnPickTree;
            @PickGrass.started += instance.OnPickGrass;
            @PickGrass.performed += instance.OnPickGrass;
            @PickGrass.canceled += instance.OnPickGrass;
            @PickFlower.started += instance.OnPickFlower;
            @PickFlower.performed += instance.OnPickFlower;
            @PickFlower.canceled += instance.OnPickFlower;
            @RemoveBuilding.started += instance.OnRemoveBuilding;
            @RemoveBuilding.performed += instance.OnRemoveBuilding;
            @RemoveBuilding.canceled += instance.OnRemoveBuilding;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @DebugTileflip.started += instance.OnDebugTileflip;
            @DebugTileflip.performed += instance.OnDebugTileflip;
            @DebugTileflip.canceled += instance.OnDebugTileflip;
        }

        private void UnregisterCallbacks(IEarthPlayerDefaultActions instance)
        {
            @Walk.started -= instance.OnWalk;
            @Walk.performed -= instance.OnWalk;
            @Walk.canceled -= instance.OnWalk;
            @PickTree.started -= instance.OnPickTree;
            @PickTree.performed -= instance.OnPickTree;
            @PickTree.canceled -= instance.OnPickTree;
            @PickGrass.started -= instance.OnPickGrass;
            @PickGrass.performed -= instance.OnPickGrass;
            @PickGrass.canceled -= instance.OnPickGrass;
            @PickFlower.started -= instance.OnPickFlower;
            @PickFlower.performed -= instance.OnPickFlower;
            @PickFlower.canceled -= instance.OnPickFlower;
            @RemoveBuilding.started -= instance.OnRemoveBuilding;
            @RemoveBuilding.performed -= instance.OnRemoveBuilding;
            @RemoveBuilding.canceled -= instance.OnRemoveBuilding;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @DebugTileflip.started -= instance.OnDebugTileflip;
            @DebugTileflip.performed -= instance.OnDebugTileflip;
            @DebugTileflip.canceled -= instance.OnDebugTileflip;
        }

        public void RemoveCallbacks(IEarthPlayerDefaultActions instance)
        {
            if (m_Wrapper.m_EarthPlayerDefaultActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IEarthPlayerDefaultActions instance)
        {
            foreach (var item in m_Wrapper.m_EarthPlayerDefaultActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_EarthPlayerDefaultActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public EarthPlayerDefaultActions @EarthPlayerDefault => new EarthPlayerDefaultActions(this);

    // PlantIsSelected
    private readonly InputActionMap m_PlantIsSelected;
    private List<IPlantIsSelectedActions> m_PlantIsSelectedActionsCallbackInterfaces = new List<IPlantIsSelectedActions>();
    private readonly InputAction m_PlantIsSelected_Plantplant;
    private readonly InputAction m_PlantIsSelected_Cancelplanting;
    private readonly InputAction m_PlantIsSelected_Move;
    private readonly InputAction m_PlantIsSelected_CursorMove;
    public struct PlantIsSelectedActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlantIsSelectedActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Plantplant => m_Wrapper.m_PlantIsSelected_Plantplant;
        public InputAction @Cancelplanting => m_Wrapper.m_PlantIsSelected_Cancelplanting;
        public InputAction @Move => m_Wrapper.m_PlantIsSelected_Move;
        public InputAction @CursorMove => m_Wrapper.m_PlantIsSelected_CursorMove;
        public InputActionMap Get() { return m_Wrapper.m_PlantIsSelected; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlantIsSelectedActions set) { return set.Get(); }
        public void AddCallbacks(IPlantIsSelectedActions instance)
        {
            if (instance == null || m_Wrapper.m_PlantIsSelectedActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlantIsSelectedActionsCallbackInterfaces.Add(instance);
            @Plantplant.started += instance.OnPlantplant;
            @Plantplant.performed += instance.OnPlantplant;
            @Plantplant.canceled += instance.OnPlantplant;
            @Cancelplanting.started += instance.OnCancelplanting;
            @Cancelplanting.performed += instance.OnCancelplanting;
            @Cancelplanting.canceled += instance.OnCancelplanting;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @CursorMove.started += instance.OnCursorMove;
            @CursorMove.performed += instance.OnCursorMove;
            @CursorMove.canceled += instance.OnCursorMove;
        }

        private void UnregisterCallbacks(IPlantIsSelectedActions instance)
        {
            @Plantplant.started -= instance.OnPlantplant;
            @Plantplant.performed -= instance.OnPlantplant;
            @Plantplant.canceled -= instance.OnPlantplant;
            @Cancelplanting.started -= instance.OnCancelplanting;
            @Cancelplanting.performed -= instance.OnCancelplanting;
            @Cancelplanting.canceled -= instance.OnCancelplanting;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @CursorMove.started -= instance.OnCursorMove;
            @CursorMove.performed -= instance.OnCursorMove;
            @CursorMove.canceled -= instance.OnCursorMove;
        }

        public void RemoveCallbacks(IPlantIsSelectedActions instance)
        {
            if (m_Wrapper.m_PlantIsSelectedActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlantIsSelectedActions instance)
        {
            foreach (var item in m_Wrapper.m_PlantIsSelectedActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlantIsSelectedActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlantIsSelectedActions @PlantIsSelected => new PlantIsSelectedActions(this);
    public interface IEarthPlayerDefaultActions
    {
        void OnWalk(InputAction.CallbackContext context);
        void OnPickTree(InputAction.CallbackContext context);
        void OnPickGrass(InputAction.CallbackContext context);
        void OnPickFlower(InputAction.CallbackContext context);
        void OnRemoveBuilding(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnDebugTileflip(InputAction.CallbackContext context);
    }
    public interface IPlantIsSelectedActions
    {
        void OnPlantplant(InputAction.CallbackContext context);
        void OnCancelplanting(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnCursorMove(InputAction.CallbackContext context);
    }
}
