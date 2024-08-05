using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public Vector2 a1 = new Vector2(-4.875f, -3.375f);
    public float spacing = 0.75f;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] colls = Physics2D.OverlapCircleAll(new Vector2(mousePos.x, mousePos.y), 0.01f);
            if (colls.Length > 0)
            {
                colls[0].GetComponent<Piece>().OnClicked();
            }
        }
    }

    // Update is called once per frame
    public Vector2 SquareToCoord(char file, int rank)
    {
        Vector2 res = a1;
        int _file = file - 'a';
        res.x += _file * spacing;
        res.y += (rank-1) * spacing;
        return res;
    }

    public string CoordToSquare(Vector2 coords)
    {
        Vector2 _coords = coords;
        _coords.x -= a1.x;
        _coords.x *= 1 / spacing;
        _coords.x = Mathf.FloorToInt(_coords.x) + 1;
        string file = ((char)(int)(_coords.x + 64)).ToString();
        _coords.y -= a1.y;
        _coords.y *= 1 / spacing;
        _coords.y = Mathf.FloorToInt(_coords.y) + 2;
        string rank = ((int)_coords.y).ToString();
        return file + rank;
    }
}
