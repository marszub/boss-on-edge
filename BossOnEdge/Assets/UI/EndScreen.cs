using Assets.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    private Animator animator;

    private bool dead;

    private void OnEnable()
    {
        PlayerBehaviour.Die += OnDead;
    }

    void Start()
    {
        dead = false;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (dead && Input.GetButtonDown("Jump"))
            TryAgain();
    }

    private void OnDisable()
    {
        PlayerBehaviour.Die -= OnDead;
    }

    private void OnDead()
    {
        dead = true;
        animator.SetTrigger("FadeIn");

    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
