using Assets.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private bool won;

    private void OnEnable()
    {
        GargulBehaviour.Win += OnWin;
    }

    void Start()
    {
        won = false;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (won && Input.GetButtonDown("Jump"))
            Menu();
    }

    private void OnDisable()
    {
        GargulBehaviour.Win -= OnWin;
    }

    private void OnWin()
    {
        won = true;
        animator.SetTrigger("FadeIn");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
