using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct InitialTransform // Transform을 깊은 복사하기 위한 구조체
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
    public List<GameObject> breakablePieces;
    public Transform modelParent;
    private List<InitialTransform> pieceTransformList = new List<InitialTransform>(); // 조각들의 Transform을 저장할 리스트
    private void Start()
    {
        // 조각들의 기본 위치를 저장, 오브젝트 풀로 다시 사용할 때 해당 위치로 초기화
        foreach (var piece in breakablePieces)
        {
            pieceTransformList.Add(new InitialTransform(piece.transform)); // 조각들의 Transform을 저장
        }
        
    }
    public void SetInnerTrash(List<TrashInfo> infos)
    {
        //infos : 내부의 쓰레기
        this.Info = infos[0]; // 첫번째 정보로 초기화, 위치 적용용
        for (int i = 0; i < infos.Count; i++)
        {
            breakablePieces[i].GetComponent<BreakedTrash>().Info = infos[i]; // info를 넣어줌
        }
        if (infos.Find(x => x.status == (int)TrashStatus.Clean) != null) // 청소된 조각이 하나라도 있다면
        {
            this.Break();
            for (int i = 0; i < infos.Count; i++)
            {
                if (infos[i].status == (int)TrashStatus.Clean)
                {
                    this.breakablePieces[i].SetActive(false); // 청소된 조각은 비활성화
                }
            }
        }
    }
    public void Break()
    {
        this.GetComponent<BoxCollider>().enabled = false; // 박스 콜라이더 비활성화
        foreach (var piece in breakablePieces)
        {
            piece.transform.parent = this.transform; // 비활성화할 모델 말고 그 상위를 부모로 설정
            piece.gameObject.GetComponent<Rigidbody>().useGravity = true;
            piece.gameObject.GetComponent<MeshCollider>().enabled = true;
            piece.gameObject.layer = 6; // 일반 쓰레기 레이어로 변경
        }
        this.modelParent.gameObject.SetActive(false);
    }
    public override void DisActivate()
    {
        // 풀에 넣을때 조각들을 다시 modelParent 부모로 갖게 돌려놓고, 위치 저장했던거 넣어주고, 중력이랑 콜라이더 끄고
        this.modelParent.gameObject.SetActive(true); // 모델 부모를 활성화
        for (int i = 0; i < breakablePieces.Count; i++)
        {
            breakablePieces[i].transform.parent = this.modelParent; // 모델 부모로 설정
            //이게 OnDisable에서 부모 설정 관련이 실행할수가 없어서, 따로 만들어서 이거 먼저 하고 꺼질수있도록 만들었다

            breakablePieces[i].transform.localPosition = pieceTransformList[i].localPosition; // 초기 위치로 되돌림
            breakablePieces[i].transform.localRotation = pieceTransformList[i].localRotation; // 초기 회전으로 되돌림
            breakablePieces[i].transform.localScale = pieceTransformList[i].localScale; // 초기 스케일로 되돌림
            breakablePieces[i].GetComponent<Rigidbody>().useGravity = false; // 중력 비활성화
            breakablePieces[i].GetComponent<Rigidbody>().velocity = Vector3.zero; // 속도 초기화
            breakablePieces[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // 회전속도 초기화
            breakablePieces[i].GetComponent<MeshCollider>().enabled = false; // 콜라이더 비활성화

            breakablePieces[i].SetActive(true); // 조각들을 활성화
            breakablePieces[i].layer = 8; // 큰 쓰레기 레이어로 변경
        }
        base.DisActivate();
    }
}
