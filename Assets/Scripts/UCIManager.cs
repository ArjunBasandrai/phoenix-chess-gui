using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class UCIManager : MonoBehaviour
{

    public string path;

    bool tomove = false;

    private Process uciProcess;
    private StreamWriter uciWriter;
    private StreamReader uciReader;

    public int moveSide = -1;
    public bool moveGoing = false;

    public TextMeshProUGUI whiteTimeUI;
    public TextMeshProUGUI blackTimeUI;

    public float whiteTime = 180000;
    public float blackTime = 180000;

    public GameObject[] pieceTable = new GameObject[64];
    //public int deathLocation;
    public bool death;
    GameObject diePiece;

    public Vector2 moveLocation;
    public int movePiece;
    public string moveList = "";


    void Awake()
    {


        uciProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        uciProcess.Start();

        uciWriter = uciProcess.StandardInput;
        uciReader = uciProcess.StandardOutput;

        Task.Run(() => ReadOutput());
    }

    private async Task ReadOutput()
    {
        while (!uciProcess.HasExited)
        {
            string output = await uciReader.ReadLineAsync();
            if (!string.IsNullOrEmpty(output))
            {
                print(output);
                string[] z = output.Split();
                if (z[0] == "bestmove")
                {
                    moveGoing = false;
                    string move = z[1];
                    if (move == "e1g1")
                    {
                        GameObject piece = pieceTable[4];
                        if (piece.name == "white_king")
                        {
                            GameObject rook = pieceTable[7];
                            Vector2 rookToCoord = GameManager.instance.SquareToCoord('f', 1);
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 1);
                            rook.transform.position = rookToCoord;
                            piece.transform.position = toMoveToCoord;
                            pieceTable[6] = piece;
                            pieceTable[4] = null;
                            pieceTable[5] = rook;
                            pieceTable[7] = null;
                        }
                        else
                        {
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 1);
                            GameObject die = pieceTable[6];
                            if (die)
                            {
                                Destroy(die);
                            }
                            piece.transform.position = toMoveToCoord;
                            pieceTable[6] = piece;
                            pieceTable[4] = null;
                        }
                    }
                    else if (move == "e1c1")
                    {
                        GameObject piece = pieceTable[4];
                        if (piece.name == "white_king")
                        {
                            Vector2 rookCoord = GameManager.instance.SquareToCoord('a', 1);
                            Vector2 rookToCoord = GameManager.instance.SquareToCoord('d', 1);
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 1);
                            GameObject rook = pieceTable[0];
                            rook.transform.position = rookToCoord;
                            piece.transform.position = toMoveToCoord;
                            pieceTable[2] = piece;
                            pieceTable[4] = null;
                            pieceTable[3] = rook;
                            pieceTable[0] = null;
                        }
                        else
                        {
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 1);
                            GameObject die = pieceTable[2];
                            if (die)
                            {
                                Destroy(die);
                            }
                            piece.transform.position = toMoveToCoord;
                            pieceTable[2] = piece;
                            pieceTable[4] = null;
                        }
                    }
                    if (move == "e8g8")
                    {
                        GameObject piece = pieceTable[60];
                        if (piece.name == "black_king")
                        {
                            GameObject rook = pieceTable[63];
                            Vector2 rookToCoord = GameManager.instance.SquareToCoord('f', 8);
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 8);
                            rook.transform.position = rookToCoord;
                            piece.transform.position = toMoveToCoord;
                            pieceTable[62] = piece;
                            pieceTable[60] = null;
                            pieceTable[61] = rook;
                            pieceTable[63] = null;
                        }
                        else
                        {
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 8);
                            GameObject die = pieceTable[62];
                            if (die)
                            {
                                Destroy(die);
                            }
                            piece.transform.position = toMoveToCoord;
                            pieceTable[62] = piece;
                            pieceTable[60] = null;
                        }
                    }
                    else if (move == "e8c8")
                    {
                        GameObject piece = pieceTable[60];
                        if (piece.name == "black_king")
                        {
                            Vector2 rookCoord = GameManager.instance.SquareToCoord('a', 8);
                            Vector2 rookToCoord = GameManager.instance.SquareToCoord('d', 8);
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 8);
                            GameObject rook = pieceTable[56];
                            rook.transform.position = rookToCoord;
                            piece.transform.position = toMoveToCoord;
                            pieceTable[58] = piece;
                            pieceTable[60] = null;
                            pieceTable[59] = rook;
                            pieceTable[56] = null;
                        }
                        else
                        {
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 8);
                            GameObject die = pieceTable[58];
                            if (die)
                            {
                                Destroy(die);
                            }
                            piece.transform.position = toMoveToCoord;
                            pieceTable[58] = piece;
                            pieceTable[60] = null;
                        }
                    }
                    else
                    {
                        Vector2 coords = GameManager.instance.SquareToCoord(move[0], move[1] - 48);
                        print(coords);
                        int rank = move[0] - 'a';
                        int file = move[1] - '1';
                        print(rank);
                        print(file);
                        GameObject piece = pieceTable[file * 8 + rank];
                        print(" ");
                        Vector2 toMoveToCoord = GameManager.instance.SquareToCoord(move[2], move[3] - 48);
                        int drank = move[2] - 'a';
                        int dfile = move[3] - '1';
                        print(drank);
                        death = true;
                        diePiece = pieceTable[dfile * 8 + drank];
                        print(dfile * 3);
                        moveLocation = toMoveToCoord;
                        movePiece = dfile * 8 + drank;
                        pieceTable[dfile * 8 + drank] = piece;
                        pieceTable[file * 8 + rank] = null;
                        moveList += " " + z[1];
                        tomove = true;
                    }
                }
            }
        }
    }

    void Move(string[] z)
    {

    }

    public void SendCommand(string command)
    {
        if (uciWriter != null && !uciProcess.HasExited)
        {
            uciWriter.WriteLine(command);
            uciWriter.Flush();
        }
    }

    void Start()
    {
        SendCommand("uci");
        SendCommand("ucinewgame");
        SendCommand("isready");
        SendCommand("go wtime " + whiteTime.ToString() + " btime " + blackTime.ToString());
        moveGoing = true;
        tomove = false;
    }

    public IEnumerator Move()
    {
        SendCommand("position startpos moves" + moveList);
        yield return new WaitForSeconds(0.006f);
        moveSide *= -1;
        moveGoing = true;
        tomove = false;
        SendCommand("go wtime " + whiteTime.ToString() + " btime " + blackTime.ToString());
    }

    void Update()
    {
        print("Herllo");
        print(tomove);
        if (tomove)
        {
            StartCoroutine(Move());
            tomove = false;
        }
        if (moveGoing)
        {
            if (moveSide == -1)
            {
                whiteTime -= Time.deltaTime * 1000;
                int minutes = Mathf.FloorToInt((whiteTime / 1000) / 60);
                int seconds = Mathf.FloorToInt((whiteTime / 1000) % 60);
                whiteTimeUI.text = minutes.ToString() + ":" + seconds.ToString();
            }
            if (moveSide == 1)
            {
                blackTime -= Time.deltaTime * 1000;
                int minutes = Mathf.FloorToInt((blackTime / 1000) / 60);
                int seconds = Mathf.FloorToInt((blackTime / 1000) % 60);
                blackTimeUI.text = minutes.ToString() + ":" + seconds.ToString();
            }
        }
        if (death)
        {
            death = false;
            Destroy(diePiece);
        }
        if (movePiece != -1)
        {
            pieceTable[movePiece].transform.position = moveLocation;
            movePiece = -1;
        }
    }

    void OnApplicationQuit()
    {
        uciProcess.Kill();
    }
}
