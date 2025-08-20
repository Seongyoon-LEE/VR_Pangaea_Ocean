using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuEnable : ShowCanvas
{
    public InputActionProperty mainInput; // 사용할 손 - 드래그 앤 드롭
    public InputActionProperty subInput; // 사용하지 않는 손 - 드래그 앤 드롭
    
    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        mainInput.action.started += x => UIEnable();
    }
    private void OnDisable()
    {
        mainInput.action.started -= x => UIEnable();
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
                UIEnable();
        }
    }

    protected override void UIEnable()
    {
        base.UIEnable();
    }

    // 버튼 or 외부에서 사용
    public override void Close()
    {
        base.Close();
    }

    
    


    

    
}
