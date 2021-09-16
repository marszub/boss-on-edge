using Assets.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    private Animator animator;

    private void OnEnable()
    {
        PlayerBehaviour.Die += OnDead;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void nDisable()
    {
        PlayerBehaviour.Die -= OnDead;
    }

    private void OnDead()
    {
        animator.SetTrigger("FadeIn");
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
