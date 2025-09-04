using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VacuumCleaner : MonoBehaviour
{
    [Header("û�ұ� �߻�")]
    [SerializeField] ParticleSystem inhalationPS;
    [SerializeField] LayerMask layerMask; // �������� �浹�� ���̾ �����մϴ�.
    [SerializeField] Transform FirePos; // �������� �߻�Ǵ� ��ġ
    [SerializeField] float rayDistance = 10f; // �������� �ִ� �Ÿ�
    [SerializeField] InputActionProperty rightTriggerAction; // ������ Ʈ���� �׼�
    bool isFiring;
    public Action onCleanAction;

    private void OnEnable()
    {
        if (rightTriggerAction != null && rightTriggerAction.action != null)
        {
            rightTriggerAction.action.performed += OnTriggerPressed;
            rightTriggerAction.action.canceled += OnTriggerReleased;
        }
    }
        void OnDisable()
        {
            if (rightTriggerAction != null && rightTriggerAction.action != null)
            {
                rightTriggerAction.action.performed -= OnTriggerPressed;
                rightTriggerAction.action.canceled -= OnTriggerReleased;
            }
    }
    void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        // �����ϱ����� ���԰� 300�� ������ �߻� �ȵ�
        if (DataManager.Instance.PlayerData.weight >= 300)
        {
            StopShoot();
            return;
        }

        if (inhalationPS != null && !isFiring)
        {
            inhalationPS.Play();
            isFiring = true;
        }
    }
        void OnTriggerReleased(InputAction.CallbackContext ctx)
        {
            StopShoot();
        }
        void Start()
        {
            if (inhalationPS != null)
                inhalationPS.Stop();
            print("Stop");
        FirePos = transform.GetChild(2);
    }

    void Update()
    {
        if (isFiring) RaycastCheck();
        inhalationPS.Simulate(10, true, false); // ��ƼŬ �ý����� �ùķ��̼��մϴ�.
    }

    public void StopShoot()
    {
        isFiring = false; // �߻� ����
        // ���� ��� ���� �ٷ� ��ƼŬ �ܿ��� �������
        if(inhalationPS)
            inhalationPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);        
    }
    void RaycastCheck()
    {
        if (!FirePos) return;
        if (Physics.Raycast(FirePos.position, FirePos.forward, out var hit, rayDistance, layerMask))
        {
            Debug.DrawLine(FirePos.position, hit.point, Color.red); // �������� �浹�� ������ �ð������� ǥ���մϴ�.
            hit.collider.GetComponent<TrashData>().Clean();
            hit.collider.GetComponent<TrashData>().DisActivate(); // �������� �浹�� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            DataManager.Instance.PlayerData.weight += hit.collider.GetComponent<TrashData>().Info.weight;
            if(DataManager.Instance.PlayerData.weight >= 300)
            {
                StopShoot();
            }
            onCleanAction();
        }
    }
}


