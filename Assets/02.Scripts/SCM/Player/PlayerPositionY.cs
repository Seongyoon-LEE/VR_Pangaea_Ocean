using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionY : MonoBehaviour
{
    private WaitForSeconds ws;
    private IEnumerator Start()
    {
        // 로딩 될 때까지 대기
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }

        ws = new WaitForSeconds(0.5f);

        StartCoroutine(PlayerPosition());
    }

    IEnumerator PlayerPosition()
    {
        while(true)
        {
            yield return ws;

            if (transform.position.y > 0)
            {
                GameManager.Instance.state = GameManager.State.RECOVERY;
            }
            else
            {
                if (GameManager.Instance.state == GameManager.State.RECOVERY)
                {
                    GameManager.Instance.state = GameManager.State.NORMAL;
                }
            }
        }
    }
}
