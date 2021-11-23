using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton Pattern
    // Static singleton instance
    private static GameManager instance;

    // Static singleton property
    public static GameManager Instance
    {
        // ajout ET création du composant à un GameObject nommé "SingletonHolder" 
        //get { return instance ?? (instance = new GameObject("SingletonHolder").AddComponent<GameManager>()); }

        //on dit que ce composant est l'instance
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if(instance == null)
                {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null) Destroy(this);

        DontDestroyOnLoad(gameObject);//le GameObject qui porte ce script ne sera pas détruit
    }
    #endregion Singleton Pattern

    public bool gamePlaying = false;
    public bool gamePaused = false;
    public bool isInCutscene = false;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        gamePlaying = true;
    }
}
