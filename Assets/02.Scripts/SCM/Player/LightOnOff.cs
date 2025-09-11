using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightOnOff : MonoBehaviour
{
    public InputActionProperty mainInput;
    private Light _light;
    void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        mainInput.action.started += x => OnOff();
    }
    private void OnDisable()
    {
        mainInput.action.started -= x => OnOff();
    }

    private void OnOff()
    {
        _light.enabled = !_light.enabled;
    }
}
