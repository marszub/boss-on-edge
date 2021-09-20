using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            Play();
    }

    public void Play()
    {
        SceneManager.LoadScene("Lvl1", LoadSceneMode.Single);
    }

    public void MouseEnter()
    {
        GetComponent<AudioSource>().Play();
    }
}
