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
        isEnter = !(transform.position.y > -1.3f);
        while (true)
        {
            yield return ws;
            
            if (transform.position.y > -1.3f)
            {
                isEnter = false;
                GameManager.Instance.state = GameManager.State.RECOVERY;
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
            else
            {
                water.rotation = new Quaternion(1, 0, 0, 0);
                if (!isEnter)
                {
                    isEnter = true;
                    GameManager.Instance.state = GameManager.State.NORMAL;
                }
            }
        }
    }
}
