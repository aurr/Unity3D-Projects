using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGui2 : MonoBehaviour
{
    private int[,] board = new int[3, 3];
    private int turn = 1;
    void Start()
    {
        Reset();
    }
    void Reset()
    {
        turn = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = 0;
            }
        }
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(300, 210, 50, 30), "Reset"))
        {
            Reset();
        }
        
        int State = isWin();
        if (State == 2)
        {
            GUI.Label(new Rect(405, 20, 70, 70), "X Win");
        }
        else if (State == 1)
        {
            GUI.Label(new Rect(405, 20, 70, 70), "O Win");
        }
        else if (State == 0)
        {
            GUI.Label(new Rect(405, 20, 70, 70), "Tied");
        }
   
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == 1)
                {
                    GUI.Button(new Rect(300 + 50 * i, 50 + 50 * j, 50, 50), "O");
                }
                else if (board[i, j] == 2)
                {
                    GUI.Button(new Rect(300 + 50 * i, 50 + 50 * j, 50, 50), "X");
                }       

                if (GUI.Button(new Rect(300 + 50 * i, 50 + 50 * j, 50, 50), ""))
                {
                    if (State == 3)
                    {
                        if (turn == 1)
                        {
                            board[i, j] = 1;
                        }
                        else if (turn == -1)
                        {
                            board[i, j] = 2;
                        }
                        turn = -turn;
                    }
                }
            }
        }
    }
    int isWin()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2] && board[i, 0] != 0)
            {
                return board[i, 0]; 
            }
        }
        
        for (int j = 0; j < 3; j++)
        {
            if (board[0, j] == board[1, j] && board[0, j] == board[2, j] && board[0, j] != 0)
            {
                return board[0, j]; 
            }
        }
        
        if (board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2] && board[0, 0] != 0) return board[0, 0];

        if (board[0, 2] == board[1, 1] && board[0, 2] == board[2, 0] && board[0, 2] != 0) return board[0, 2];
        
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] != 0)
                {
                    count++;
                }
            }
        }
        if (count == 9)
        {
            return 0;
        }
        return 3; 
    }

    // Update is called once per frame
    void Update()
    {

    }

}