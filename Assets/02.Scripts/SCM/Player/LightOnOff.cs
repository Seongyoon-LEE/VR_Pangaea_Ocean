using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightOnOff : MonoBehaviour
{
    public InputActionProperty mainInput;
    private Light _light;
    void Start()
    {
        _light = GetComponent<Light>();
        mainInput.action.started += x => OnOff();
    }

    private void OnOff()
    {
        _light.enabled = !_light.enabled;
    }
}
