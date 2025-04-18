using UnityEngine;
using System.Collections.Generic;

public class MazeGeneratorDFS : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public int separation = 5;

    public GameObject wallPrefab;
    public GameObject floorPrefab;

    public Vector2Int start;
    public Vector2Int goal;

    private bool[,] mazeGrid;
    private System.Random rng = new System.Random();

    [ContextMenu("Generar Laberinto")]
    public void GenerateMazeInEditor()
    {
        // Limpiar laberinto anterior
        foreach (Transform child in transform)
            DestroyImmediate(child.gameObject);

        int gridWidth = width * 2 + 1;
        int gridHeight = height * 2 + 1;

        mazeGrid = new bool[gridWidth, gridHeight];

        // Llenar todo de paredes
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                mazeGrid[x, y] = false;

        Vector2Int[] directions = {
            new Vector2Int(0, 2),     // arriba
            new Vector2Int(0, -2),    // abajo
            new Vector2Int(2, 0),     // derecha
            new Vector2Int(-2, 0)     // izquierda
        };

        // DFS para generar caminos
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int current = new Vector2Int(1, 1);
        mazeGrid[current.x, current.y] = true;
        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Pop();
            List<Vector2Int> neighbors = new List<Vector2Int>();

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;
                if (neighbor.x > 0 && neighbor.x < gridWidth - 1 &&
                    neighbor.y > 0 && neighbor.y < gridHeight - 1 &&
                    !mazeGrid[neighbor.x, neighbor.y])
                {
                    neighbors.Add(neighbor);
                }
            }

            if (neighbors.Count > 0)
            {
                stack.Push(current);

                Vector2Int chosen = neighbors[rng.Next(neighbors.Count)];
                Vector2Int wall = current + (chosen - current) / 2;

                mazeGrid[wall.x, wall.y] = true;
                mazeGrid[chosen.x, chosen.y] = true;
                stack.Push(chosen);
            }
        }

        // Posición aleatoria para entrada/salida (esquinas opuestas)
        start = new Vector2Int(1, 1);
        goal = new Vector2Int(gridWidth - 2, gridHeight - 2);

        // Instanciar objetos
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 pos = new Vector3(x * separation, 0, y * separation);
                if (mazeGrid[x, y])
                {
                    GameObject tile = Instantiate(floorPrefab, pos, Quaternion.identity, transform);

                    // Colorea entrada y salida
                    if (x == start.x && y == start.y)
                        tile.GetComponent<Renderer>().material.color = Color.green;
                    else if (x == goal.x && y == goal.y)
                        tile.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    Instantiate(wallPrefab, pos + new Vector3(0, 1, 0), Quaternion.identity, transform);
                }
            }
        }

        Debug.Log($"Laberinto generado. Inicio: {start}, Objetivo: {goal}");
    }
}
