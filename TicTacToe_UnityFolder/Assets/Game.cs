/*
 * Kacper Bazan
 * Tic Tac Toe Implementation
 * Game.cs
 * 11/20/2020
 */

using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    /*Mouse Variables*/
    public GameObject mouseCursor;
    public GameObject hoverItem;
    [HideInInspector]
    public MouseCursor mouseCursorScript;
    public Text winText;

    /*Array Variables*/
    public string[,] boardState = new string[3, 3]; //string representation of the board.
    public GameObject[,] boardObjects = new GameObject[3, 3]; //instantiated x's and o's
    public GameObject[,] buttons = new GameObject[3, 3]; //clickable buttons
    private KeyCode[] resetKeys = new KeyCode[] { KeyCode.R };
    private int[] bestMove = new int[2];

    /*Player Token Game Objects*/
    public GameObject xGameObject;
    public GameObject oGameObject;

    /*Game Variables*/
    private bool xMove = true; //Since x starts, nthe initial xMove must be true.
    private bool playerTurn = true; //player will always start.n
    private int winner = 0; //board state initially isn't win or lose.


    void Start()
    {
        Cursor.visible = true;
        mouseCursorScript = FindObjectOfType<MouseCursor>();


        //Initializes boxes array with the 9 game objects and enables their colliders.
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int index = (i * 3) + j;
                buttons[i, j] = GameObject.Find("Space" + index.ToString());
                buttons[i, j].GetComponent<BoxCollider>().enabled = true;
                boardState[i, j] = null;
            }
        }
    }

    private void ResetSpaces()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                buttons[i, j].GetComponent<BoxCollider>().enabled = true; //turn on buttons
                mouseCursor.transform.position = new Vector3(-100f, -100f, 0f);
                //buttons[i, j].GetComponent<MeshRenderer>().enabled = true;
                boardState[i, j] = null; //set each string to null
                if (boardObjects[i, j] != null)
                {
                    Destroy(boardObjects[i, j].gameObject); //destroy instatiated objects
                }
            }
        }
        xMove = true; //Since x starts, the xMove must be true.
        playerTurn = true; //player will always start.
        winner = 0; //board state initially isn't win or lose.
    }

    void DisableSpaces()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                buttons[i, j].gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    void PlayerMove()
    {
        if (Input.GetMouseButtonDown(0) && hoverItem != null)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (buttons[i, j] == hoverItem)
                    {
                        XMove(xMove, i, j);
                    }
                }
            }
            hoverItem = null;
            mouseCursorScript.hoverObject = null;
            playerTurn = !playerTurn;
        }
    }

    void AIMove()
    {
        if(CheckWin(boardState)!= 0 || CheckTie(boardState))
        {
            return;
        }
        else
        {
            MiniMax(boardState, 9, xMove,9);
            XMove(xMove, bestMove[0], bestMove[1]);
            playerTurn = !playerTurn;
        }
    }

    int MiniMax(string[,] position, int depth, bool maximizingPlayer, int startingDepth)
    {
        if (depth == 0 || CheckWin(position) != 0 || CheckTie(position))
        {
            return CheckWin(position);
        }
        
        if(maximizingPlayer)
        {
            int maxEval = -1000;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(position[i,j] == null)
                    {
                        position[i,j] = "X";
                        int eval = MiniMax(position, depth-1, false, startingDepth);
                        position[i, j] = null;

                        if (eval > maxEval)
                        {
                            maxEval = eval;
                            if (depth == startingDepth)
                            {
                                bestMove[0] = i;
                                bestMove[1] = j;
                            }


                        }
                    }
                }
            }
            return maxEval;
        }

        else
        {
            int minEval = 1000;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (position[i, j] == null)
                    {
                        position[i,j] = "O";
                        int eval = MiniMax(position, depth - 1, true, startingDepth);
                        position[i, j] = null;

                        if (eval < minEval)
                        {
                            minEval = eval;
                            if (depth == startingDepth)
                            {
                                bestMove[0] = i;
                                bestMove[1] = j;
                            }
                        }
                        
                    }
                }
            }
            return minEval;
        }
    }

    void XMove(bool areYouX, int row, int col)
    {
        if (areYouX)
        {
            boardState[row, col] = "X";
            boardObjects[row, col] = Instantiate(xGameObject, buttons[row, col].transform.position, Quaternion.identity);
        }
        else
        {
            boardState[row, col] = "O";
            boardObjects[row, col] = Instantiate(oGameObject, buttons[row, col].transform.position, Quaternion.identity);
        }
        buttons[row, col].gameObject.GetComponent<BoxCollider>().enabled = false;
        xMove = !xMove;
    }

    int CheckWin(string[,] stringVal)
    {
        for (int i = 0; i < 3; i++)
        {
            if (((stringVal[i, 0] == stringVal[i, 1]) && (stringVal[i, 1] == stringVal[i, 2])) && (stringVal[i, 0] != null))
            {
                if (stringVal[i, 0] == "X")
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            if (((stringVal[0, i] == stringVal[1, i]) && (stringVal[1, i] == stringVal[2, i])) && (stringVal[0, i] != null))
            {
                if (stringVal[0, i] == "X")
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            if (((stringVal[0, 0] == stringVal[1, 1]) && (stringVal[1, 1] == stringVal[2, 2])) && (stringVal[0, 0] != null))
            {
                if (stringVal[0, 0] == "X")
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            if (((stringVal[0, 2] == stringVal[1, 1]) && (stringVal[1, 1] == stringVal[2, 0])) && (stringVal[0, 2] != null))
            {
                if (stringVal[0, 2] == "X")
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
        return 0;


    }
    bool CheckTie(string[,] stringVal)
    {
        if (CheckWin(stringVal) == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (stringVal[i, j] == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }
    void Update()
    {
        //checks each reset key and then performs resetspaces().
        foreach (KeyCode key in resetKeys)
        {
            if (Input.GetKeyDown(key))
            {
                ResetSpaces();
            }
        }

        //transforms mouse
        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        mouseCursor.transform.position = new Vector3(x, y, 0);
        hoverItem = mouseCursorScript.hoverObject;
        winner = CheckWin(boardState);

        if (winner == 0 && !CheckTie(boardState))
        {
            winText.text = "";
            if (playerTurn)
            {
                PlayerMove();
            }
            else
            {
                AIMove();
            }
        }
        else
        {
            DisableSpaces();
            switch(winner)
            {
                case -1:
                    winText.text = "O has won!";
                    break;
                case 0:
                    winText.text = "It is a tie!";
                    break;
                case 1:
                    winText.text = "X has won!";
                    break;

            }
        }

    }
}
