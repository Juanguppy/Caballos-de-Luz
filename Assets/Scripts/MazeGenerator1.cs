using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator1 : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public int separation = 50;

    public GameObject wallPrefab = null;
    public GameObject floorPrefab = null;

    private Cell[,] cells;
    private System.Random rng = new System.Random();

    // Variables para las coordenadas de inicio y objetivo
    public Vector2Int start = new Vector2Int(0, 0);
    public Vector2Int goal = new Vector2Int(9, 9);

    [ContextMenu("Generar Laberinto")]
    public void GenerateMazeInEditor()
    {
        // Limpiar si ya hay algo generado
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        cells = new Cell[width, height];

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int current = new Vector2Int(0, 0);

        // Inicializar celdas
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new Cell();
            }
        }

        // Marcar la celda inicial como visitada
        cells[current.x, current.y].visited = true;
        stack.Push(current);

        // Generación del laberinto
        while (stack.Count > 0)
        {
            current = stack.Pop();
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(current);

            if (neighbors.Count > 0)
            {
                stack.Push(current);
                var next = neighbors[rng.Next(neighbors.Count)];
                RemoveWallBetween(current, next);
                cells[next.x, next.y].visited = true;
                stack.Push(next);
            }
        }

        // Instanciar todos los objetos basados en celdas
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * separation, 0, y * separation);
                var cell = cells[x, y];

                Instantiate(floorPrefab, pos, Quaternion.identity, transform);

                if (cell.northWall)
                    Instantiate(wallPrefab, pos + new Vector3(0, 1, separation), Quaternion.identity, transform);  
                if (cell.southWall)
                    Instantiate(wallPrefab, pos + new Vector3(0, 1, -separation), Quaternion.identity, transform);  
                if (cell.eastWall)
                    Instantiate(wallPrefab, pos + new Vector3(separation, 1, 0), Quaternion.Euler(0, 90, 0), transform);  
                if (cell.westWall)
                    Instantiate(wallPrefab, pos + new Vector3(-separation, 1, 0), Quaternion.Euler(0, 90, 0), transform);
            }
        }

        // Asignar el punto de inicio y el objetivo (en este caso en las esquinas)
        start = new Vector2Int(0, 0);  // Inicio en la esquina superior izquierda
        goal = new Vector2Int(width - 1, height - 1);  // Objetivo en la esquina inferior derecha

        // Ahora puedes hacer cualquier cosa con las coordenadas de inicio y objetivo, por ejemplo:
        Debug.Log("Inicio del laberinto: " + start);
        Debug.Log("Objetivo del laberinto: " + goal);
    }

    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        var list = new List<Vector2Int>();

        Vector2Int[] directions = {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
        };

        foreach (var dir in directions)
        {
            var neighbor = cell + dir;
            if (neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < width && neighbor.y < height)
            {
                if (!cells[neighbor.x, neighbor.y].visited)
                    list.Add(neighbor);
            }
        }

        return list;
    }

    void RemoveWallBetween(Vector2Int a, Vector2Int b)
    {
        var diff = b - a;

        if (diff == Vector2Int.up)
        {
            cells[a.x, a.y].northWall = false;
            cells[b.x, b.y].southWall = false;
        }
        else if (diff == Vector2Int.down)
        {
            cells[a.x, a.y].southWall = false;
            cells[b.x, b.y].northWall = false;
        }
        else if (diff == Vector2Int.right)
        {
            cells[a.x, a.y].eastWall = false;
            cells[b.x, b.y].westWall = false;
        }
        else if (diff == Vector2Int.left)
        {
            cells[a.x, a.y].westWall = false;
            cells[b.x, b.y].eastWall = false;
        }
    }

    class Cell
    {
        public bool visited = false;
        public bool northWall = true, southWall = true, eastWall = true, westWall = true;
    }
}
