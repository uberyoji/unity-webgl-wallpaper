using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStreamer : MonoBehaviour
{
    private SwipeModule SM = new SwipeModule();

    public string[] Scenes;
    private int Index = 0;
    private string LastScene = "";
    private string CurrentScene = "";

    // Start is called before the first frame update
    void Start()
    {
        SM.OnSwipe += OnSwipe;

        CurrentScene = Scenes[Index];

        SceneManager.LoadScene(CurrentScene, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        SM.Update();
    }

    void OnSwipe(SwipeModule.Direction Dir)
    {
//        Debug.Log("Swipe detected: " + Dir);

        switch (Dir)
        {
            case SwipeModule.Direction.Up:
                {
                    LastScene = Scenes[Index];
                    SceneManager.UnloadSceneAsync(LastScene);
                    Index = ++Index % Scenes.Length;
                    CurrentScene = Scenes[Index];
                    SceneManager.LoadScene(CurrentScene, LoadSceneMode.Additive);
                }
                break;
            case SwipeModule.Direction.Down:
                {
                    LastScene = Scenes[Index];
                    SceneManager.UnloadSceneAsync(LastScene);
                    Index = (Index + Scenes.Length - 1) % Scenes.Length;
                    CurrentScene = Scenes[Index];
                    SceneManager.LoadScene(CurrentScene, LoadSceneMode.Additive);
                }
                break;
        }
    }
}
