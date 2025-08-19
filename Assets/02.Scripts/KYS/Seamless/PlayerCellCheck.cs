using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCellCheck : MonoBehaviour
{
    Vector2Int _currentCell;
    Vector2Int CurrentCell
    {
        get
        {
            return _currentCell;
        }
        set
        {
            _currentCell = value;
            //Cell이 바뀔 때 쓰래기 로딩하는 내용
            TerrainTrashDataManager.Instance.CellMove(value); // Cell이 바뀌면 해당 Cell에 있는 쓰레기를 로드하는 함수 호출
        }
    }
    private void Start()
    {
        this.CurrentCell = TerrainTrashDataManager.Instance.GetCellFromPosition(this.transform.position);
    }
    private void Update()
    {
        Vector2Int newCell = TerrainTrashDataManager.Instance.GetCellFromPosition(this.transform.position);
        if (newCell != CurrentCell)
        {
            CurrentCell = newCell;
            Debug.Log($"Player is now in cell: {CurrentCell.x}, {CurrentCell.y}");

        }
    }
}