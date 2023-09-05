using System;
using UnityEngine;

[Serializable]
public class Grid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private TGridObject[,] gridArray;
    [SerializeField] private Vector3 originPosition;

    //public Grid(int w, int h, float cellsize, Vector3 origin,
    //    Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    //{
    //    width = w;
    //    height = h;
    //    cellSize = cellsize;
    //    gridArray = new TGridObject[w, h];
    //    originPosition = origin;

    //    for (int x = 0; x < gridArray.GetLength(0); x++)
    //    {
    //        for (int y = 0; y < gridArray.GetLength(1); y++)
    //        {
    //            gridArray[x, y] = createGridObject(this, x, y);
    //        }
    //    }

    //}

    public Grid(int w, int h, float cellsize, Vector3 origin,
        Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, bool debugGizmo = false)
    {
        width = w;
        height = h;
        cellSize = cellsize;
        gridArray = new TGridObject[w, h];
        originPosition = origin;

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        if (debugGizmo)
            DrawDebugGizmo();

    }

    private void DrawDebugGizmo()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    public void GetXZ(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPos - originPosition).z / cellSize);
    }

    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < 0 && y < 0)
        {
            gridArray[x, y] = value;
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerOnValueChanged(int x, int y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    public void SetValue(Vector3 pos, TGridObject value)
    {
        int x, y;
        GetXZ(pos, out x, out y);
        SetValue(x, y, value);
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetValue(Vector3 pos)
    {
        int x, y;
        GetXZ(pos, out x, out y);
        return GetValue(x, y);
    }

}
