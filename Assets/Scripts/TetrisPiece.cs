using System.Collections.Generic;
using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    public static class TetrisShapes
    {
        public static int[,] I = new int[,]
        {
            {1},
            {1},
            {1},
            {1},
            {1},
            {1},
            {1},
            {1},
            {1},
        };

        public static int[,] O = new int[,]
        {
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1},
        };

        public static int[,] S = new int[,]
        {
            {0, 0, 1, 1, 1, 1},
            {0, 0, 1, 1, 1, 1},
            {1, 1, 1, 1, 0, 0},
            {1, 1, 1, 1, 0, 0},
        };

        public static int[,] Z = new int[,]
        {
            {1, 1, 1, 1, 0, 0},
            {1, 1, 1, 1, 0, 0},
            {0, 0, 1, 1, 1, 1},
            {0, 0, 1, 1, 1, 1},
        };

        public static int[,] L = new int[,]
        {
            {1, 0, 0, 0},
            {1, 0, 0, 0},
            {1, 0, 0, 0},
            {1, 0, 0, 0},
            {1, 0, 0, 0},
            {1, 0, 0, 0},
            {1, 0, 0, 0},
            {1, 0, 0, 0},
            {1, 1, 1, 1},
        };

        public static int[,] T = new int[,]
        {
            {1, 1, 1, 1, 1},
            {0, 0, 1, 0, 0},
            {0, 0, 1, 0, 0},
            {0, 0, 1, 0, 0},
            {0, 0, 1, 0, 0},
            {0, 0, 1, 0, 0},
        };
    }

}
