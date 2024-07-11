using System;
using System.Linq;
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
    public TextMeshProUGUI pvText;

    public float whiteTime = 180000;
    public float blackTime = 180000;

    public GameObject[] pieceTable = new GameObject[64];
    //public int deathLocation;
    public bool death;
    GameObject diePiece;

    public Vector2 moveLocation;
    public int movePiece;
    public string moveList = "";

    public string castle = "-";

    public string pv;
    public bool pvChange;

    public GameObject[] promoteTo = new GameObject[8];

    public int promoteLocation = -1;
    public char promoteType;

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
                //print(output);
                string[] z = output.Split();
                if (z[0] == "info")
                {
                    pvChange = true;
                    pv = output;
                }
                else if (z[0] == "bestmove")
                {
                    moveGoing = false;
                    string move = z[1];
                    if (move.Length == 5)
                    {
                        int rank = move[0] - 'a';
                        int file = move[1] - '1';
                        int drank = move[2] - 'a';
                        int dfile = move[3] - '1';
                        GameObject piece = pieceTable[file * 8 + rank];
                        Vector2 toMoveToCoord = GameManager.instance.SquareToCoord(move[2], move[3] - 48);
                        death = true;
                        diePiece = pieceTable[dfile * 8 + drank];
                        moveLocation = toMoveToCoord;
                        movePiece = dfile * 8 + drank;
                        pieceTable[dfile * 8 + drank] = piece;
                        pieceTable[file * 8 + rank] = null;
                        moveList += " " + move;
                        promoteLocation = dfile * 8 + drank;
                        promoteType = move[-1];
                    }
                    else if (move == "e1g1")
                    {
                        GameObject piece = pieceTable[4];
                        if (piece.name == "white_king")
                        {
                            castle = move;
                            moveList += " " + move;
                        }
                    }
                    else if (move == "e1c1")
                    {
                        GameObject piece = pieceTable[4];
                        if (piece.name == "white_king")
                        {
                            castle = move;
                            moveList += " " + move;
                        }
                    }
                    else if (move == "e8g8")
                    {
                        GameObject piece = pieceTable[60];
                        if (piece.name == "black_king")
                        {
                            castle = move;
                            moveList += " " + move;
                        }
                    }
                    else if (move == "e8c8")
                    {
                        GameObject piece = pieceTable[60];
                        if (piece.name == "black_king")
                        {
                            castle = move;
                            moveList += " " + move;
                        }
                    }
                    else
                    {
                        int rank = move[0] - 'a';
                        int file = move[1] - '1';
                        int drank = move[2] - 'a';
                        int dfile = move[3] - '1';
                        GameObject piece = pieceTable[file * 8 + rank];
                        Vector2 toMoveToCoord = GameManager.instance.SquareToCoord(move[2], move[3] - 48);
                        death = true;
                        if (rank == 5 && Mathf.Abs(file-dfile)==1)
                        {
                            if (moveList[-1] == int.Parse(move[3].ToString())-1 && moveList[-2] == move[0] && move[-3] == int.Parse(move[3].ToString())+1 && move[-4] == move[0])
                            {
                                diePiece = pieceTable[(dfile - 1) * 8 + rank];
                            }
                            else
                            {
                                diePiece = pieceTable[dfile * 8 + drank];
                            }
                        }
                        else if (rank == 4 && Mathf.Abs(file - dfile) == 1)
                        {
                            if (moveList[-1] == int.Parse(move[3].ToString()) + 1 && moveList[-2] == move[0] && move[-3] == int.Parse(move[3].ToString()) - 1 && move[-4] == move[0])
                            {
                                diePiece = pieceTable[(dfile + 1) * 8 + rank];
                            }
                            else
                            {
                                diePiece = pieceTable[dfile * 8 + drank];
                            }
                        }
                        else
                        {
                            diePiece = pieceTable[dfile * 8 + drank];
                        }
                        moveLocation = toMoveToCoord;
                        movePiece = dfile * 8 + drank;
                        pieceTable[dfile * 8 + drank] = piece;
                        pieceTable[file * 8 + rank] = null;
                        moveList += " " + move;
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
        if (pvChange)
        {
            string[] pvs = pv.Split();
            int startIndex = 17;
            int length = pvs.Length - 18;

            string[] _pv = pvs.Skip(startIndex).Take(length).ToArray();

            pvText.text = string.Join(" ", _pv);
        }
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
        if (castle != "-")
        {
            string move = castle;
            castle = "-";
            if (move == "e1c1")
            {
                Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 1);
                pieceTable[4].transform.position = toMoveToCoord; 
                pieceTable[2] = pieceTable[4];
                pieceTable[4] = null;
                pieceTable[3] = pieceTable[0];
                pieceTable[0] = null;
            }
            if (move == "e1g1")
            {
                Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 1);
                pieceTable[4].transform.position = toMoveToCoord;
                pieceTable[6] = pieceTable[4];
                pieceTable[4] = null;
                pieceTable[5] = pieceTable[7];
                pieceTable[7] = null;
            }
            if (move == "e8c8")
            {
                Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 8);
                pieceTable[60].transform.position = toMoveToCoord;
                pieceTable[58] = pieceTable[4];
                pieceTable[60] = null;
                pieceTable[59] = pieceTable[56];
                pieceTable[56] = null;
            }
            if (move == "e1c1")
            {
                Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 8);
                pieceTable[60].transform.position = toMoveToCoord;
                pieceTable[62] = pieceTable[60];
                pieceTable[60] = null;
                pieceTable[61] = pieceTable[62];
                pieceTable[63] = null;
            }
            tomove = true;
        }
        if (promoteLocation != -1)
        {
            GameObject p = new GameObject();
            GameObject r = p;
            if (moveSide == -1)
            {
                if (promoteType == 'n')
                {
                    p = Instantiate(promoteTo[0], pieceTable[promoteLocation].transform.position, Quaternion.identity);
                }
                else if (promoteType == 'b')
                {
                    p = Instantiate(promoteTo[1], pieceTable[promoteLocation].transform.position, Quaternion.identity);
                }
                else if (promoteType == 'r')
                {
                    p = Instantiate(promoteTo[2], pieceTable[promoteLocation].transform.position, Quaternion.identity);
                }
                else if (promoteType == 'q')
                {
                    p = Instantiate(promoteTo[3], pieceTable[promoteLocation].transform.position, Quaternion.identity);
                }
            }
            else if (moveSide == 1)
            {
                if (promoteType == 'n')
                {
                    p = Instantiate(promoteTo[4], pieceTable[promoteLocation].transform.position, Quaternion.identity);
                }
                else if (promoteType == 'b')
                {
                    p = Instantiate(promoteTo[5], pieceTable[promoteLocation].transform.position, Quaternion.identity);
                }
                else if (promoteType == 'r')
                {
                    p = Instantiate(promoteTo[6], pieceTable[promoteLocation].transform.position, Quaternion.identity);
                }
                else if (promoteType == 'q')
                {
                    p = Instantiate(promoteTo[7], pieceTable[promoteLocation].transform.position, Quaternion.identity);
                }
            }
            Destroy(pieceTable[promoteLocation]);
            Destroy(r);
            pieceTable[promoteLocation] = p;
        }
    }

    void OnApplicationQuit()
    {
        uciProcess.Kill();
    }
}
