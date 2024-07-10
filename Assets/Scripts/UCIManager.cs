using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class UCIManager : MonoBehaviour
{

    public string path;

    bool tomove = true;

    private Process uciProcess;
    private StreamWriter uciWriter;
    private StreamReader uciReader;

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
    }

    private void ReadOutput()
    {
        while (!uciProcess.HasExited)
        {
            string output = uciReader.ReadLine();
            if (!string.IsNullOrEmpty(output))
            {
                if (output.Split()[0] == "bestmove")
                {
                    print(output);
                    string move = output.Split()[1];
                    if (move == "e1g1")
                    {
                        Vector2 coords = GameManager.instance.SquareToCoord('e', 1);
                        Collider2D[] colls = Physics2D.OverlapCircleAll(coords, 0.1f);
                        GameObject piece = colls[0].gameObject;
                        if (piece.name == "white_king")
                        {
                            Vector2 rookCoord = GameManager.instance.SquareToCoord('h', 1);
                            Collider2D[] rookColl = Physics2D.OverlapCircleAll(rookCoord, 0.1f);
                            Vector2 rookToCoord = GameManager.instance.SquareToCoord('f', 1);
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 1);
                            GameObject rook = rookColl[0].gameObject;
                            rook.transform.position = rookToCoord;
                            piece.transform.position = toMoveToCoord;
                        }
                        else
                        {
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 1);
                            Collider2D[] collsToRem = Physics2D.OverlapCircleAll(toMoveToCoord, 0.1f);
                            if (collsToRem.Length > 0)
                            {
                                Destroy(collsToRem[0].gameObject);
                            }
                            piece.transform.position = toMoveToCoord;
                        }
                    }
                    else if (move == "e1c1")
                    {
                        Vector2 coords = GameManager.instance.SquareToCoord('e', 1);
                        Collider2D[] colls = Physics2D.OverlapCircleAll(coords, 0.1f);
                        GameObject piece = colls[0].gameObject;
                        if (piece.name == "white_king")
                        {
                            Vector2 rookCoord = GameManager.instance.SquareToCoord('a', 1);
                            Collider2D[] rookColl = Physics2D.OverlapCircleAll(rookCoord, 0.1f);
                            Vector2 rookToCoord = GameManager.instance.SquareToCoord('d', 1);
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 1);
                            GameObject rook = rookColl[0].gameObject;
                            rook.transform.position = rookToCoord;
                            piece.transform.position = toMoveToCoord;
                        }
                        else
                        {
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 1);
                            Collider2D[] collsToRem = Physics2D.OverlapCircleAll(toMoveToCoord, 0.1f);
                            if (collsToRem.Length > 0)
                            {
                                Destroy(collsToRem[0].gameObject);
                            }
                            piece.transform.position = toMoveToCoord;
                        }
                    }
                    if (move == "e8g8")
                    {
                        Vector2 coords = GameManager.instance.SquareToCoord('e', 8);
                        Collider2D[] colls = Physics2D.OverlapCircleAll(coords, 0.1f);
                        GameObject piece = colls[0].gameObject;
                        if (piece.name == "black_king")
                        {
                            Vector2 rookCoord = GameManager.instance.SquareToCoord('h', 8);
                            Collider2D[] rookColl = Physics2D.OverlapCircleAll(rookCoord, 0.1f);
                            Vector2 rookToCoord = GameManager.instance.SquareToCoord('f', 8);
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 8);
                            GameObject rook = rookColl[0].gameObject;
                            rook.transform.position = rookToCoord;
                            piece.transform.position = toMoveToCoord;
                        }
                        else
                        {
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('g', 8);
                            Collider2D[] collsToRem = Physics2D.OverlapCircleAll(toMoveToCoord, 0.1f);
                            if (collsToRem.Length > 0)
                            {
                                Destroy(collsToRem[0].gameObject);
                            }
                            piece.transform.position = toMoveToCoord;
                        }
                    }
                    else if (move == "e8c8")
                    {
                        Vector2 coords = GameManager.instance.SquareToCoord('e', 8);
                        Collider2D[] colls = Physics2D.OverlapCircleAll(coords, 0.1f);
                        GameObject piece = colls[0].gameObject;
                        if (piece.name == "black_king")
                        {
                            Vector2 rookCoord = GameManager.instance.SquareToCoord('a', 8);
                            Collider2D[] rookColl = Physics2D.OverlapCircleAll(rookCoord, 0.1f);
                            Vector2 rookToCoord = GameManager.instance.SquareToCoord('d', 8);
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 8);
                            GameObject rook = rookColl[0].gameObject;
                            rook.transform.position = rookToCoord;
                            piece.transform.position = toMoveToCoord;
                        }
                        else
                        {
                            Vector2 toMoveToCoord = GameManager.instance.SquareToCoord('c', 8);
                            Collider2D[] collsToRem = Physics2D.OverlapCircleAll(toMoveToCoord, 0.1f);
                            if (collsToRem.Length > 0)
                            {
                                Destroy(collsToRem[0].gameObject);
                            }
                            piece.transform.position = toMoveToCoord;
                        }
                    }
                    else
                    {
                        Vector2 coords = GameManager.instance.SquareToCoord(move[0], move[1] - 48);
                        Collider2D[] colls = Physics2D.OverlapCircleAll(coords, 0.1f);
                        GameObject piece = colls[0].gameObject;
                        Vector2 toMoveToCoord = GameManager.instance.SquareToCoord(move[2], move[3] - 48);
                        Collider2D[] collsToRem = Physics2D.OverlapCircleAll(toMoveToCoord, 0.1f);
                        if (collsToRem.Length > 0)
                        {
                            Destroy(collsToRem[0].gameObject);
                        }
                        piece.transform.position = toMoveToCoord;
                    }
                    GameManager.instance.moveList += " " + output.Split()[1];
                    tomove = true;
                    return;
                }
            }
        }
    }

    public void SendCommand(string command)
    {
        if (uciWriter != null && !uciProcess.HasExited)
        {
            //print("Sending command: " + command);
            uciWriter.WriteLine(command);
            uciWriter.Flush();
        }
    }

    void Start()
    {
        SendCommand("uci");
        SendCommand("ucinewgame");
        SendCommand("isready");
        SendCommand("go depth 12");
        ReadOutput();
    }

    public IEnumerator Move()
    {
        yield return new WaitForSeconds(1);
        SendCommand("position startpos moves" + GameManager.instance.moveList);
        SendCommand("go depth 12");
        ReadOutput();
    }

    void Update()
    {
        if (tomove)
        {
            tomove = false;
            StartCoroutine(Move());
        }
    }

    void OnApplicationQuit()
    {
        uciProcess.Kill();
    }
}
