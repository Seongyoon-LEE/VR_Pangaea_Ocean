using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageExample : MapManager
{
    public bool puzzle1Solved = false;
    public bool puzzle2Solved = false;
    public bool puzzle3Solved = false;
    public bool puzzleAddSolved = false;
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
            this.puzzle1Solved = DataManager.Instance.dicPuzzle["Puzzle1"];
            this.puzzle2Solved = DataManager.Instance.dicPuzzle["Puzzle2"];
            this.puzzle3Solved = DataManager.Instance.dicPuzzle["Puzzle3"];
            this.puzzleAddSolved = DataManager.Instance.dicPuzzle["PuzzleAdd"];
        }
    }

    public override void PuzzleReset()
    {
        DataManager.Instance.dicPuzzle.Clear(); // 기존 데이터를 클리어
        DataManager.Instance.dicPuzzle.Add("Puzzle1", false);
        DataManager.Instance.dicPuzzle.Add("Puzzle2", false);
        DataManager.Instance.dicPuzzle.Add("Puzzle3", false);
        DataManager.Instance.dicPuzzle.Add("PuzzleAdd", false);

        //이하 테스트용
        this.puzzle1Solved = false;
        this.puzzle2Solved = false;
        this.puzzle3Solved = false;
        this.puzzleAddSolved = false;
    }
}
