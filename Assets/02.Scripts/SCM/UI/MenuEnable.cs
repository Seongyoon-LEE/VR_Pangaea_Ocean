using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuEnable : ShowCanvas
{
    public InputActionProperty mainInput; // 사용하는 버튼 - 드래그 앤 드롭
    public InputActionProperty subInput; // 사용하지 않는 버튼 - 드래그 앤 드롭
    
    protected override void Start()
    {
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

    void Update()
    {
        FollowUI();
    }

    protected override void FollowUI()
    {
        base.FollowUI();
        if (canvas.activeSelf)
        {
            // 다른 메뉴버튼 누르면 사라지게하기
            if (subInput.action.WasPressedThisFrame())
                canvas.SetActive(false);
        }
    }

    protected override void UIEnable(bool isEnable)
    {
        base.UIEnable(isEnable);
    }

    // 버튼
    public override void Close()
    {
        base.Close();
    }

    
}
