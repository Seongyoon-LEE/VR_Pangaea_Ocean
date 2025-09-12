using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageExample : MapManager
{
    public bool puzzle1Solved = false;
    public bool puzzle2Solved = false;
    public bool puzzle3Solved = false;

    public bool turtle1Solved = false;
    public bool turtle2Solved = false;
    public bool turtle3Solved = false;
    private IEnumerator Start()
    {
        while (!DataManager.Instance.IsLoadingFinish) // 데이터 매니저가 로딩이 끝날 때까지 대기
        {
            yield return null;
        }
        if (!DataManager.Instance.IsTrashDataExist)
        {
            PuzzleReset();
        }
        else
        {
            // 퍼즐
            if (DataManager.Instance.dicPuzzle.Count != 0)
            {
                this.puzzle1Solved = DataManager.Instance.dicPuzzle["Puzzle1"];
                this.puzzle2Solved = DataManager.Instance.dicPuzzle["Puzzle2"];
                this.puzzle3Solved = DataManager.Instance.dicPuzzle["Puzzle3"];
            }

            // 터틀
            if (DataManager.Instance.dicPuzzle.Count != 0)
            {
                this.turtle1Solved = DataManager.Instance.dicPuzzle["Turtle1"];
                this.turtle2Solved = DataManager.Instance.dicPuzzle["Turtle2"];
                this.turtle3Solved = DataManager.Instance.dicPuzzle["Turtle3"];
            }
        }
    }

    public override void PuzzleReset()
    {
        //DataManager.Instance.dicPuzzle.Clear(); // 기존 데이터를 클리어

        // 거북이와 같은 딕셔너리를 사용하고 있어서 전체 지우기가 아니라 부분 지우기를 한다.
        DataManager.Instance.dicPuzzle.Remove("Puzzle1");
        DataManager.Instance.dicPuzzle.Remove("Puzzle2");
        DataManager.Instance.dicPuzzle.Remove("Puzzle3");

        //이하 테스트용
        this.puzzle1Solved = false;
        this.puzzle2Solved = false;
        this.puzzle3Solved = false;
    }
}
