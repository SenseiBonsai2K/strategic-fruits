using UnityEngine;

public class Cell : MonoBehaviour
{
    // The rank of the card represented by the cell.
    public int Rank { get; set; }

    // The suit of the card represented by the cell.
    public string Suit { get; set; }

    // The material used to visually represent the cell.
    public Material Content { get; set; }

    // A flag indicating whether the cell currently contains a token.
    public bool HasToken { get; set; } = false;
}

public class GridCreator : MonoBehaviour
{
    // The prefab used to instantiate new cells.
    public GameObject CellPrefab;

    // An array of the possible suits a cell can have.
    private readonly string[] _suits = { "Apple", "Pear", "Banana", "Orange" };

    // A 2D array representing the grid of cells.
    private Cell[,] _grid;

    // This method is called at the start of the game.
    private void Start()
    {
        // Creates a grid of 4x5 cells.
        CreateGrid(4, 5);
    }

    /// <summary>
    /// Creates a grid of cells with the specified width and height.
    /// </summary>
    /// <param name="width">The width of the grid.</param>
    /// <param name="height">The height of the grid.</param>
    private void CreateGrid(int width, int height)
    {
        _grid = new Cell[width, height];

        for (var x = 0; x < width; x++)
        {
            var suit = _suits[x % _suits.Length];
            for (var y = 0; y < height; y++)
            {
                var cellObject = CreateCell(x, y, suit, y + 1);
                _grid[x, y] = cellObject.GetComponent<Cell>();
            }
        }
    }

    /// <summary>
    /// Creates a cell with the specified coordinates, suit, and rank.
    /// </summary>
    /// <param name="x">The x-coordinate of the cell.</param>
    /// <param name="y">The y-coordinate of the cell.</param>
    /// <param name="suit">The suit of the cell.</param>
    /// <param name="rank">The rank of the cell.</param>
    /// <returns>The created cell as a GameObject.</returns>
    private GameObject CreateCell(int x, int y, string suit, int rank)
    {
        var cellObject = Instantiate(CellPrefab, GetCellPosition(x, y), transform.rotation);
        cellObject.transform.parent = transform;
        cellObject.tag = transform.tag;

        var cell = cellObject.AddComponent<Cell>();
        cell.Rank = rank;
        cell.Suit = suit;

        var material = Resources.Load<Material>($"Materials/Grids/{rank}/{rank}_{suit}");
        var renderer = cellObject.GetComponent<Renderer>();
        if (renderer != null) renderer.material = material;

        cell.Content = material;
        cellObject.name = $"{cell.Rank} of {cell.Suit}";

        return cellObject;
    }

    /// <summary>
    /// Calculates the position of a cell in the grid based on its coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the cell.</param>
    /// <param name="y">The y-coordinate of the cell.</param>
    /// <returns>The position of the cell as a Vector3.</returns>
    private Vector3 GetCellPosition(int x, int y)
    {
        var relativePosition = new Vector3(x * CellPrefab.transform.localScale.x,
            y * CellPrefab.transform.localScale.y, 0);
        return transform.TransformDirection(relativePosition) + transform.position;
    }
}