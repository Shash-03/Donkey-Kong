using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    
    private int lives;
    int level;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        NewGame();

    }

    private void NewGame()
    {
        
        lives = 1;
        LoadLevel(1);



    }
    private void LoadLevel(int index) {
        level = index;
        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.cullingMask = 0;
        }
        Invoke(nameof(LoadScene), 1f);
        



    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }
    

    public void LevelComplete()
    {
        //Display a blank screen with the next "YOU WIN" 
        int nextLevel = level + 1;
        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        }
        else
        {
            LoadLevel(1);
        }
        

    }

    public void LevelFailed()
    {
        lives--;
        if (lives == 0)
        {
            NewGame();
        }


    }
}
