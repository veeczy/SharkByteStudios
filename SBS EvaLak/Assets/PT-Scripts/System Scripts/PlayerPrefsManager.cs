//using Mono.Cecil.Cil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    [Header("Floats")]
    public float gameVolume;

    //[Header("Strings")]


    void Start() // every time a scene starts, set the data to what it is stored
    {
        /*if (!PlayerPrefs.HasKey("Game_Volume")) //do this for every ppref variable
        {
            PlayerPrefs.SetFloat("Game_Volume", gameVolume);
            PlayerPrefs.Save();
        }*/
        gameVolume = PlayerPrefs.GetFloat("Game_Volume"); //grab it


        PlayerPrefs.SetFloat("Game_Volume", gameVolume);
        
        
        
    }

    void Update() //check is ppref is updated 
    {
        //gameVolume = PlayerPrefs.GetFloat("Game_Volume");

    }


    void SavePrefs()
    {
        var variable = "TEST"; //temp until i figure out how to anonymously pass a parameter to a method in c# ... we might have to make it a class instead to do it this way?
        

        var variableType = variable.GetType();
        string variableTypeName = "";

        if (variableType == typeof(float)) { variableTypeName = "float"; }
        if (variableType == typeof(string)) { variableTypeName = "string"; }
        if (variableType == typeof(int)) { variableTypeName = "int"; }

        /*switch(variableTypeName)
        {
            case "float":
                SaveFloat(variable);
                break;
            case "int":
                SaveInt(variable);
                break;
            case "string":
                SaveString(variable);
                break;
            default:
                Console.WriteLine("ERROR Type Exception while saving " + nameof(variable) + ". Is this a float/int/string?"); break;
        }*/
    }

    void SaveFloat(float variable)
    {
        string variableName = nameof(variable); //get name of variable you are saving

        if(!PlayerPrefs.HasKey(variableName)) // check if the variable has been initialized, if it is not a key it must be initialized
        {
            PlayerPrefs.SetFloat(variableName, 1.0f);
        }
        if(PlayerPrefs.HasKey(variableName)) // if variable has a key set already, overwrite it with the new save data
        {
            PlayerPrefs.SetFloat(variableName, variable);
        }
        PlayerPrefs.Save();
    }
    void SaveInt(int variable)
    {
        string variableName = nameof(variable); //get name of variable you are saving

        if (!PlayerPrefs.HasKey(variableName)) // check if the variable has been initialized, if it is not a key it must be initialized
        {
            PlayerPrefs.SetInt(variableName, 0);
        }
        if (PlayerPrefs.HasKey(variableName)) // if variable has a key set already, overwrite it with the new save data
        {
            PlayerPrefs.SetInt(variableName, variable);
        }
        PlayerPrefs.Save();
    }

    void SaveString(string variable)
    {
        string variableName = nameof(variable); //get name of variable you are saving

        if (!PlayerPrefs.HasKey(variableName)) // check if the variable has been initialized, if it is not a key it must be initialized
        {
            PlayerPrefs.SetString(variableName, "");
        }
        if (PlayerPrefs.HasKey(variableName)) // if variable has a key set already, overwrite it with the new save data
        {
            PlayerPrefs.SetString(variableName, variable);
        }
        PlayerPrefs.Save();
    }


}
