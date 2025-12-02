using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StarsStringDecoder 
{

    public static bool[] DecodeString(string code)
    {
        char[] numbers = code.ToCharArray();
        bool[] flags = new bool[3];
        for (int i = 0; i < numbers.Length; i++)
        {
            if (numbers[i] == '1')
            {
                flags[i] = true;
            }
            else flags[i] = false;
        }

        return flags;
    }

    


}
