using System;
using UnityEngine.UI;

////TODO: have updateBindingUIEvent receive a control path string, too (in addition to the device layout name)

namespace UnityEngine.InputSystem.Samples.RebindUI
{
    /// <summary>
    /// This is an example for how to override the default display behavior of bindings. The component
    /// hooks into <see cref="RebindActionUI.updateBindingUIEvent"/> which is triggered when UI display
    /// of a binding should be refreshed. It then checks whether we have an icon for the current binding
    /// and if so, replaces the default text display with an icon.
    /// </summary>
    public class GamepadIconsExample : MonoBehaviour
    {
        public GamepadIcons xbox;
        public GamepadIcons ps4;
        public KeyboardAndMouseIcons keyboardAndMouse;

        protected void OnEnable()
        {
            // Hook into all updateBindingUIEvents on all RebindActionUI components in our hierarchy.
            var rebindUIComponents = transform.GetComponentsInChildren<RebindActionUI>();
            foreach (var component in rebindUIComponents)
            {
                component.updateBindingUIEvent.AddListener(OnUpdateBindingDisplay);
                component.UpdateBindingDisplay();
            }
        }

        protected void OnUpdateBindingDisplay(RebindActionUI component, string bindingDisplayString, string deviceLayoutName, string controlPath)
        {
            if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
                return;

            var icon = default(Sprite);
            if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad"))
                icon = ps4.GetSprite(controlPath);
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad"))
                icon = xbox.GetSprite(controlPath);
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Keyboard&Mouse"))
                icon = keyboardAndMouse.GetSprite(controlPath);

            var textComponent = component.bindingText;

            // Grab Image component.
            var imageGO = textComponent.transform.parent.Find("ActionBindingIcon");
            var imageComponent = imageGO.GetComponent<Image>();

            if (icon != null)
            {
                textComponent.gameObject.SetActive(false);
                imageComponent.sprite = icon;
                imageComponent.gameObject.SetActive(true);
            }
            else
            {
                textComponent.gameObject.SetActive(true);
                imageComponent.gameObject.SetActive(false);
            }
        }

        [Serializable]
        public struct GamepadIcons
        {
            public Sprite buttonSouth;
            public Sprite buttonNorth;
            public Sprite buttonEast;
            public Sprite buttonWest;
            public Sprite startButton;
            public Sprite selectButton;
            public Sprite leftTrigger;
            public Sprite rightTrigger;
            public Sprite leftShoulder;
            public Sprite rightShoulder;
            public Sprite dpad;
            public Sprite dpadUp;
            public Sprite dpadDown;
            public Sprite dpadLeft;
            public Sprite dpadRight;
            public Sprite leftStick;
            public Sprite rightStick;
            public Sprite leftStickPress;
            public Sprite rightStickPress;

            public Sprite GetSprite(string controlPath)
            {
                // From the input system, we get the path of the control on device. So we can just
                // map from that to the sprites we have for gamepads.
                switch (controlPath)
                {
                    case "buttonSouth": return buttonSouth;
                    case "buttonNorth": return buttonNorth;
                    case "buttonEast": return buttonEast;
                    case "buttonWest": return buttonWest;
                    case "start": return startButton;
                    case "select": return selectButton;
                    case "leftTrigger": return leftTrigger;
                    case "rightTrigger": return rightTrigger;
                    case "leftShoulder": return leftShoulder;
                    case "rightShoulder": return rightShoulder;
                    case "dpad": return dpad;
                    case "dpad/up": return dpadUp;
                    case "dpad/down": return dpadDown;
                    case "dpad/left": return dpadLeft;
                    case "dpad/right": return dpadRight;
                    case "leftStick": return leftStick;
                    case "rightStick": return rightStick;
                    case "leftStickPress": return leftStickPress;
                    case "rightStickPress": return rightStickPress;
                }
                return null;
            }
        }

        [Serializable]
        public struct KeyboardAndMouseIcons
        {
            public Sprite aKey;
            public Sprite bKey;
            public Sprite cKey;
            public Sprite dKey;
            public Sprite eKey;
            public Sprite fKey;
            public Sprite gKey;
            public Sprite hKey;
            public Sprite iKey;
            public Sprite jKey;
            public Sprite kKey;
            public Sprite lKey;
            public Sprite mKey;
            public Sprite nKey;
            public Sprite oKey;
            public Sprite pKey;
            public Sprite qKey;
            public Sprite rKey;
            public Sprite sKey;
            public Sprite tKey;
            public Sprite uKey;
            public Sprite vKey;
            public Sprite wKey;
            public Sprite xKey;
            public Sprite yKey;
            public Sprite zKey;
            public Sprite num1Key;
            public Sprite num2Key;
            public Sprite num3Key;
            public Sprite num4Key;
            public Sprite num5Key;
            public Sprite num6Key;
            public Sprite num7Key;
            public Sprite num8Key;
            public Sprite num9Key;
            public Sprite num0Key;
            public Sprite upArrowKey;
            public Sprite downArrowKey;
            public Sprite leftArrowKey;
            public Sprite rightArrowKey;
            public Sprite spaceKey;
            public Sprite shiftKey;
            public Sprite leftShiftKey;
            public Sprite enterKey;
            public Sprite tabKey;
            public Sprite ctrlKey;
            public Sprite f1Key;
            public Sprite f2Key;
            public Sprite f3Key;
            public Sprite f4Key;
            public Sprite f5Key;
            public Sprite f6Key;
            public Sprite f7Key;
            public Sprite f8Key;
            public Sprite f9Key;
            public Sprite f10Key;
            public Sprite f11Key;
            public Sprite f12Key;
            public Sprite leftMouseKey;
            public Sprite rightMouseKey;
            public Sprite middleMouseKey;

            public Sprite GetSprite(string controlPath)
            {
                // From the input system, we get the path of the control on device. So we can just
                // map from that to the sprites we have for gamepads.
                switch (controlPath)
                {
                    case "a": return aKey;
                    case "b": return bKey;
                    case "c": return cKey;
                    case "d": return dKey;
                    case "e": return eKey;
                    case "f": return fKey;
                    case "g": return gKey;
                    case "h": return hKey;
                    case "i": return iKey;
                    case "j": return jKey;
                    case "k": return kKey;
                    case "l": return lKey;
                    case "m": return mKey;
                    case "n": return nKey;
                    case "o": return oKey;
                    case "p": return pKey;
                    case "q": return qKey;
                    case "r": return rKey;
                    case "s": return sKey;
                    case "t": return tKey;
                    case "u": return uKey;
                    case "v": return vKey;
                    case "w": return wKey;
                    case "x": return xKey;
                    case "y": return yKey;
                    case "z": return zKey;
                    case "1": return num1Key;
                    case "2": return num2Key;
                    case "3": return num3Key;
                    case "4": return num4Key;
                    case "5": return num5Key;
                    case "6": return num6Key;
                    case "7": return num7Key;
                    case "8": return num8Key;
                    case "9": return num9Key;
                    case "0": return num0Key;
                    case "upArrow": return upArrowKey;
                    case "downArrow": return downArrowKey;
                    case "leftArrow": return leftArrowKey;
                    case "rightArrow": return rightArrowKey;
                    case "space": return spaceKey;
                    case "shift": return shiftKey;
                    case "leftShift": return leftShiftKey;
                    case "enter": return enterKey;
                    case "tab": return tabKey;
                    case "ctrl": return ctrlKey;
                    case "f1": return f1Key;
                    case "f2": return f2Key;
                    case "f3": return f3Key;
                    case "f4": return f4Key;
                    case "f5": return f5Key;
                    case "f6": return f6Key;
                    case "f7": return f7Key;
                    case "f8": return f8Key;
                    case "f9": return f9Key;
                    case "f10": return f10Key;
                    case "f11": return f11Key;
                    case "f12": return f12Key;
                    case "leftButton": return leftMouseKey;
                    case "rightButton": return rightMouseKey;
                    case "middleButton": return middleMouseKey;
                }
                return null;
            }
        }
    }
}
