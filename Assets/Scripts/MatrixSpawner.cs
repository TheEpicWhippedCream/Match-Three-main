using UnityEngine;

public class MatrixSpawner : MonoBehaviour
{
    [SerializeField] public int m_NoRows;
    [SerializeField] public int m_NoColumns;
    [SerializeField] private float m_CellSize;
    [SerializeField] private float m_Offset;
    [SerializeField] public Cell[] m_CellPrefabs;

    public Vector2 m_StartPosition;

    public void Awake()
    {
        m_StartPosition = ComputeStartPosition(m_NoRows, m_NoColumns);
    }

    public Cell[,] Spawn()
    {
        if (m_CellPrefabs == null || m_CellPrefabs.Length == 0)
        {
            Debug.LogError("No prefabs to spawn.");
            return null;
        }

        Cell[,] matrix = new Cell[m_NoRows, m_NoColumns];
        for (int row = 0; row < m_NoRows; ++row)
            for (int column = 0; column < m_NoColumns; ++column)
            {
                matrix[row, column] = SpawnRandomCell(ComputePosition(row, column));
                matrix[row, column].SetMatrixPosition(row, column);
            }

        Debug.Log($"Spawned {m_NoRows} rows and {m_NoColumns} columns.");

        return matrix;
    }

    public void Swap(Cell cell1, Cell cell2)
    {
        // Swap world positions
        cell1.transform.localPosition = ComputePosition(cell2.Row, cell2.Column);
        cell2.transform.localPosition = ComputePosition(cell1.Row, cell1.Column);

        // Swap matrix positions
        int cell1Row, cell1Column;

        cell1Row = cell1.Row;
        cell1Column = cell1.Column;
        cell1.SetMatrixPosition(cell2.Row, cell2.Column);
        cell2.SetMatrixPosition(cell1Row, cell1Column);
    }

    public Vector2 ComputePosition(int row, int column)
    {
        return m_StartPosition + new Vector2(column * (m_CellSize + m_Offset), row * (m_CellSize + m_Offset));
    }

    public Vector2 ComputeStartPosition(int noRows, int noColumns)
    {
        return new Vector2(-ComputeStartPosition(noColumns), -ComputeStartPosition(noRows));
    }

    public float ComputeStartPosition(int count)
    {
        float position = 0.5f * (count - 1) * (m_Offset + m_CellSize);
        if (count % 2 == 0)
            position += 0.5f * (m_Offset + m_CellSize);

        return position;
    }

    public Cell SpawnRandomCell(Vector2 position)
    {
        Cell prefab = m_CellPrefabs[Random.Range(0, m_CellPrefabs.Length)];
        Cell copy = Instantiate(prefab, position, Quaternion.identity, transform);
        copy.transform.localScale *= m_CellSize;
        return copy;
    }
}
