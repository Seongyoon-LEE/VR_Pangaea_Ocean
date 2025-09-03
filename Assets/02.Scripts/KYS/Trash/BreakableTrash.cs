using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct InitialTransform // Transform�� ���� �����ϱ� ���� ����ü
{
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;
    public InitialTransform(Transform transform)
    {
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;
        localScale = transform.localScale;
    }
}
public class BreakableTrash : TrashData
{
    [SerializeField]
    private List<GameObject> breakablePieces;
    private List<GameObject> breakables = new List<GameObject>();
    public Transform modelParent_Po;
    public Transform modelParent_Im;
    private List<InitialTransform> pieceTransformList = new List<InitialTransform>(); // �������� Transform�� ������ ����Ʈ
    private void Start()
    {
        // �������� �⺻ ��ġ�� ����, ������Ʈ Ǯ�� �ٽ� ����� �� �ش� ��ġ�� �ʱ�ȭ
        foreach (var piece in breakablePieces)
        {
            pieceTransformList.Add(new InitialTransform(piece.transform)); // �������� Transform�� ����
        }
        
    }
    public void SetInnerTrash(List<TrashInfo> infos)
    {
        //infos : ������ ������
        this.Info = infos[0]; // ù��° ������ �ʱ�ȭ, ��ġ �����
        for (int i = 0; i < this.Info.count; i++)
        {
            this.breakables.Add(this.breakablePieces[i]);
        }
        for (int i = 0; i < infos.Count; i++)
        {
            breakables[i].GetComponent<BreakedTrash>().Info = infos[i]; // info�� �־���
        }
        if (infos.Find(x => x.status == (int)TrashStatus.Clean) != null) // û�ҵ� ������ �ϳ��� �ִٸ�
        {
            this.Break();
            for (int i = 0; i < infos.Count; i++)
            {
                if (infos[i].status == (int)TrashStatus.Clean)
                {
                    this.breakables[i].SetActive(false); // û�ҵ� ������ ��Ȱ��ȭ
                }
            }
        }
    }
    public void Break()
    {
        this.GetComponent<BoxCollider>().enabled = false; // �ڽ� �ݶ��̴� ��Ȱ��ȭ
        foreach (var piece in breakables)
        {
            piece.transform.parent = this.transform; // ��Ȱ��ȭ�� �� ���� �� ������ �θ�� ����
            piece.GetComponent<Rigidbody>().useGravity = true;
            piece.GetComponent<MeshCollider>().enabled = true;
            piece.layer = 6; // �Ϲ� ������ ���̾�� ����
        }
        
        this.modelParent_Po.gameObject.SetActive(false);
        this.modelParent_Im.gameObject.SetActive(false);
    }
    public override void DisActivate()
    {
        // Ǯ�� ������ �������� �ٽ� modelParent �θ�� ���� ��������, ��ġ �����ߴ��� �־��ְ�, �߷��̶� �ݶ��̴� ����
        this.modelParent_Po.gameObject.SetActive(true); // �� �θ� Ȱ��ȭ
        this.modelParent_Im.gameObject.SetActive(true);
        this.GetComponent<BoxCollider>().enabled = true;
        for (int i = 0; i < breakables.Count; i++)
        {
            breakables[i].transform.parent = this.modelParent_Po; // �� �θ�� ����
            //�̰� OnDisable���� �θ� ���� ������ �����Ҽ��� ���, ���� ���� �̰� ���� �ϰ� �������ֵ��� �������

            breakables[i].transform.localPosition = pieceTransformList[i].localPosition; // �ʱ� ��ġ�� �ǵ���
            breakables[i].transform.localRotation = pieceTransformList[i].localRotation; // �ʱ� ȸ������ �ǵ���
            breakables[i].transform.localScale = pieceTransformList[i].localScale; // �ʱ� �����Ϸ� �ǵ���
            breakables[i].GetComponent<Rigidbody>().useGravity = false; // �߷� ��Ȱ��ȭ
            breakables[i].GetComponent<Rigidbody>().velocity = Vector3.zero; // �ӵ� �ʱ�ȭ
            breakables[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // ȸ���ӵ� �ʱ�ȭ
            breakables[i].GetComponent<MeshCollider>().enabled = false; // �ݶ��̴� ��Ȱ��ȭ

            breakables[i].SetActive(true); // �������� Ȱ��ȭ
            breakables[i].layer = 8; // ū ������ ���̾�� ����
        }
        breakables.Clear();
        base.DisActivate();
    }
}
