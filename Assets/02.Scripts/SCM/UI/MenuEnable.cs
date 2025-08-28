using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuEnable : ShowCanvas
{
    public InputActionProperty mainInput; // 사용하는 버튼 - 드래그 앤 드롭
    
    protected override void Start()
    {
        canvas = transform.GetChild(0).gameObject;
        base.Start();
    }

    private void OnEnable()
    {
        mainInput.action.started += x => UIEnable(!canvas.activeSelf);
    }
    private void OnDisable()
    {
        mainInput.action.started -= x => UIEnable(!canvas.activeSelf);
        mainInput.action.Disable();
    }
}
