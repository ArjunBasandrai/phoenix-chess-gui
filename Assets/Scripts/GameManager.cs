using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Vector2 a1 = new Vector2(-4.875f, -3.375f);
    public static float spacing = 0.75f;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Vector2 test = SquareToCoord('E', 6);
        print(test.x);
        print(test.y);
        print(CoordToSquare(new Vector2(-3.849f, -1.594f)));
    }

    // Update is called once per frame
    Vector2 SquareToCoord(char file, int rank)
    {
        Vector2 res = a1;
        int _file = file - 'A';
        res.x += _file * spacing;
        res.y += (rank-1) * spacing;
        return res;
    }

    string CoordToSquare(Vector2 coords)
    {
        string file = "";
        string rank = "";

        Vector2 _coords = coords;
        _coords.x -= a1.x;
        _coords.x *= 1 / spacing;
        _coords.x = Mathf.FloorToInt(_coords.x) + 1;
        file = ((char)(int)(_coords.x + 64)).ToString();
        _coords.y -= a1.y;
        _coords.y *= 1 / spacing;
        _coords.y = Mathf.FloorToInt(_coords.y) + 1;
        rank = ((int)_coords.y).ToString();

        return file + rank;
    }
}
