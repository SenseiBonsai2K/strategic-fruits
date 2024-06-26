using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Rank { get; set; }
    public string Suit { get; set; }
    public Material Content { get; set; }

    public bool HasToken { get; set; } = false;
}

public class GridManager : MonoBehaviour
{
    public GameObject CellPrefab;
    private Cell[,] _grid;
    private readonly string[] _suits = { "Apple", "Pear", "Banana", "Orange" };

    private void Start()
    {
        CreateGrid(4, 5);
    }

    private void CreateGrid(int width, int height)
    {
        _grid = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            string suit = _suits[x % _suits.Length];
            for (int y = 0; y < height; y++)
            {
                GameObject cellObject = CreateCell(x, y, suit, y + 1);
                _grid[x, y] = cellObject.GetComponent<Cell>();
            }
        }
    }

    private GameObject CreateCell(int x, int y, string suit, int rank)
    {
        GameObject cellObject = Instantiate(CellPrefab, GetCellPosition(x, y), transform.rotation);
        cellObject.transform.parent = transform;
        cellObject.tag = transform.tag;

        Cell cell = cellObject.AddComponent<Cell>();
        cell.Rank = rank;
        cell.Suit = suit;

        //Material material = Resources.Load<Material>($"Materials/Grids/{rank}/{rank}_{suit}");
        //Renderer renderer = cellObject.GetComponent<Renderer>();
        //if (renderer != null)
        //{
        //  renderer.material = material;
        //}

        //cell.Content = material;
        cellObject.name = $"{cell.Rank} of {cell.Suit}";

        return cellObject;
    }

    private Vector3 GetCellPosition(int x, int y)
    {
        Vector3 relativePosition = new Vector3(x * CellPrefab.transform.localScale.x,
            y * CellPrefab.transform.localScale.y, 0);
        return transform.TransformDirection(relativePosition) + transform.position;
    }
}