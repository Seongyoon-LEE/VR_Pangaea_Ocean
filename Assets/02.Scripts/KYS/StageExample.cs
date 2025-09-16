using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageExample : MapManager
{
    private readonly string puzzleStr = "Puzzle";
    private readonly string turtleStr = "Turtle";

    public bool[] puzzleSolved = { false, false, false };
    //public bool puzzle1Solved = false;
    //public bool puzzle2Solved = false;
    //public bool puzzle3Solved = false;

    public bool[] turtleSolved = { false, false, false };
    //public bool turtle1Solved = false;
    //public bool turtle2Solved = false;
    //public bool turtle3Solved = false;
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
            foreach(var puzzle in DataManager.Instance.dicPuzzle)
            {
                ArraySetting(puzzleStr, puzzleSolved, puzzle);
                ArraySetting(turtleStr, turtleSolved, puzzle);
            }
        }
    }

    private void ArraySetting(string str, bool[] solved, KeyValuePair<string, bool> puzzle)
    {
        if (puzzle.Key.StartsWith(str))
        {
            if (int.TryParse(puzzle.Key.Replace(str, ""), out int result))
            {
                solved[result - 1] = puzzle.Value;
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
        //this.puzzle1Solved = false;
        //this.puzzle2Solved = false;
        //this.puzzle3Solved = false;
        Array.Fill(this.puzzleSolved, false);
    }
}
