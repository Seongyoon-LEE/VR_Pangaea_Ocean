using ESSW.Editorcontroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionY : MonoBehaviour
{
    private WaitForSeconds ws;
    private Transform water;
    private bool isEnter = false;
    private IEnumerator Start()
    {
        // 로딩 될 때까지 대기
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }

        ws = new WaitForSeconds(0.5f);

        water = GameObject.FindFirstObjectByType<WaterShaderController>().transform;

        StartCoroutine(PlayerPosition());
    }

    IEnumerator PlayerPosition()
    {
        // 처음 시작 했을 때 위치에 대한 값으로 결정
        isEnter = !(transform.position.y > -1.3f);
        while (true)
        {
            yield return ws;
            
            if (transform.position.y > -1.3f) // 물 밖 있을 때
            {
                if (!isEnter) continue; // 물 밖에 계속 있었을 때 continue
                isEnter = false; // 처음 물 밖에 나왔을 때 상태 변경
                
                GameManager.Instance.state = GameManager.State.RECOVERY; // 회복 상태로 변경
                // 물 회전 값 변경
                // 스테이지 변경시 물 오브젝트가 사라져 다시 찾아준다.
                try
                {
                    water.rotation = new Quaternion(0, 0, 0, 0);
                }
                catch
                {
                    water = GameObject.FindFirstObjectByType<WaterShaderController>().transform;
                    water.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
            else // 물 속에 있을 때
            {
                if (isEnter) continue; // 물 속에 있을 때 

                isEnter = true; // 물 속에 있는 상태로
                water.rotation = new Quaternion(1, 0, 0, 0); // 물 회전
                GameManager.Instance.state = GameManager.State.NORMAL; // 일반 상태로 변경
            }
        }
    }
}
