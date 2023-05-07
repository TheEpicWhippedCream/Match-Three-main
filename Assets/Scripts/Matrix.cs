using System;
using UnityEngine;

[RequireComponent(typeof(MatrixSpawner))]
public class Matrix : MonoBehaviour
{
    private MatrixSpawner m_MatrixSpawner;

    private Cell[,] m_Cells;
    private Cell m_SelectedCell;

    private void Awake()
    {
        m_MatrixSpawner = GetComponent<MatrixSpawner>();
    }

    private void Start()
    {
        SpawnMatrix();
    }

    private void SpawnMatrix()
    {
        m_Cells = m_MatrixSpawner.Spawn();
    }

    public void HandleCellClicked(Cell cell)
    {
        if (cell == null)
        {
            SelectCell(null);
            return;
        }

        if (m_SelectedCell == null || !CanSwap(m_SelectedCell, cell))
        {
            SelectCell(cell);
        }
        else
        {
            SwapCells(m_SelectedCell, cell);
            SelectCell(null);
        }
    }

    private bool CanSwap(Cell cell1, Cell cell2)
    {
        if (!AreNeighbours(cell1, cell2))
            return false;
        return true;
    }

    private bool AreNeighbours(Cell cell1, Cell cell2)
    {
        return Mathf.Abs(cell1.Row - cell2.Row) + Mathf.Abs(cell1.Column - cell2.Column) == 1;
    }


    private void CheckMatchThreeVertical(Cell cell)
    {
        int CellUp = 0, pozUp;
        int CellDown = 0, pozDown;

        int i = cell.Row + 1;
        while (i < m_MatrixSpawner.m_NoRows && cell.Type == m_Cells[i, cell.Column].Type)
        {
            i++;
            CellUp++;
        }
        pozUp = i - 1;

        i = cell.Row - 1;
        while (i >= 0 && cell.Type == m_Cells[i, cell.Column].Type)
        {
            i--;
            CellDown++;
        }
        pozDown = i + 1;
        if (CellDown + CellUp >= 2)
        {
            Debug.Log($"Matched vertical {cell}");
            for (int poz = pozDown; poz <= pozUp; ++poz)
            {
                /*
                Cell prefab = MatrixSpawner.m_CellPrefabs[Random.Range(0, 6)];

                Vector2 position;
                position.x = poz;
                position.y = cell.Column;

                Cell copy = Instantiate(prefab, position, Quaternion.identity, transform);
                copy.transform.localScale *= m_CellSize;
                m_Cells[poz, cell.Column] = copy;
                m_MatrixSpawner[poz, cell.Column] = copy;
                */
            }
        }

    }

    private void CheckMatchThreeHorizontal(Cell cell)
    {
        int CellRight = 0, pozRight;
        int CellLeft = 0, pozLeft;

        int i = cell.Column + 1;
        while (i < m_MatrixSpawner.m_NoColumns && cell.Type == m_Cells[cell.Row, i].Type)
        {
            i++;
            CellRight++;
        }
        pozRight = i - 1;

        i = cell.Column - 1;
        while (i >= 0 && cell.Type == m_Cells[cell.Row, i].Type)
        {
            i--;
            CellLeft++;
        }
        pozLeft = i + 1;
        if (CellLeft + CellRight >= 2)
        {
            Debug.Log($"Matched horizontal {cell}");
            for (int poz = pozLeft; poz <= pozRight; ++poz)
            {
                /*
                Cell prefab = MatrixSpawner.m_CellPrefabs[Random.Range(0, 6)];

                Vector2 position;
                position.x = cell.Row;
                position.y = poz;

                Cell copy = Instantiate(prefab, position, Quaternion.identity, transform);
                copy.transform.localScale *= m_CellSize;
                m_Cells[cell.Row, poz] = copy;
                m_MatrixSpawner[cell.Row, poz] = copy;
                */
            }
        }

    }
    

    private void SwapCells(Cell cell1, Cell cell2)
    {
        m_Cells[cell1.Row, cell1.Column] = cell2;
        m_Cells[cell2.Row, cell2.Column] = cell1;

        m_MatrixSpawner.Swap(cell1, cell2);

        if (cell1.Row == cell2.Row)
        {
            CheckMatchThreeVertical(cell1);
            CheckMatchThreeVertical(cell2);
        }
        else
        {
            CheckMatchThreeHorizontal(cell1);
            CheckMatchThreeHorizontal(cell2);
        }
    }

    private void SelectCell(Cell cell)
    {
        if (m_SelectedCell != null)
        {
            Debug.Log($"Deselecting {m_SelectedCell.name}", m_SelectedCell);
            m_SelectedCell.Highlight(false);
        }

        m_SelectedCell = cell;

        if (m_SelectedCell != null)
        {
            Debug.Log($"Selecting {m_SelectedCell.name}", m_SelectedCell);
            m_SelectedCell.Highlight(true);
        }
    }
}
