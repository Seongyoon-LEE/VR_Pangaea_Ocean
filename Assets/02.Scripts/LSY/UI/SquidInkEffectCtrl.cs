using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquidInkEffectCtrl : MonoBehaviour
{
    [Header("오징어 먹물 UI")]
    [SerializeField] Image squidInkImage;
    [SerializeField] int inkDuration = 3;
    [SerializeField] float fadeDuration = 0.5f;

    private void OnEnable()
    {
        BigTrashTakeHit.OnBigTrashBroken += TrySquidInkAttack;
    }
    private void OnDisable()
    {
        BigTrashTakeHit.OnBigTrashBroken -= TrySquidInkAttack;
    }
    void TrySquidInkAttack()
    {
        //if(Random.value < 0.33f)
        //{
            print("오징어 먹물 공격!");
            StartCoroutine(ShowInkSplat());
        //}
        //else
        //{
            print("오징어 먹물 공격 실패!");
        //}
    }
    IEnumerator ShowInkSplat()
    {
        // 부드럽게 (페이드인) 나타나기
        squidInkImage.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            Color color = squidInkImage.color;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            squidInkImage.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(inkDuration); // 3초동안 보여주기

        // 부드럽게 (페이드아웃) 사라지기
        timer = 0f;
        while(timer < fadeDuration)
        {
            timer += Time.deltaTime;
            Color color = squidInkImage.color;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            squidInkImage.color = color;
            yield return null;
        }
        squidInkImage.gameObject.SetActive(false);
    }
}
