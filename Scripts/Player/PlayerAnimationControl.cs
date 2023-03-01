using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerNameSpace;
using System;

public enum MovementState
{
    idle,
    walk,
    shoot,
    walking_shoot
}
public class PlayerAnimationControl : MonoBehaviour
{

    private Animator m_animator;
    public static MovementState movementState;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        SetMovementState(MovementState.idle);
    }
    private void Start()
    {
        StartCoroutine(PlayAnimations());

    }

    public void SetMovementState(MovementState state)
    {
        movementState = state;
    }

    public void SetAnimSpeed(string parameter, int value)
    {
        m_animator.SetFloat(parameter, value);
    }
    IEnumerator PlayAnimations()
    {
        while (true)
        {
            switch (movementState)
            {
                case MovementState.idle:
                    SetAnim("idle");
                    break;

                case MovementState.walk:
                    SetAnim("walk");
                    break;

                case MovementState.shoot:
                    SetAnim("shoot");
                    break;

                case MovementState.walking_shoot:

                    SetAnim("walking shoot");
                    break;


            }

            yield return null;
        }
    }

    internal void Idle()
    {
        SetAnim("idle");
    }

    private void SetAnim(string name)
    {
        switch (name)
        {
            case "idle":

                m_animator.SetBool("idle", true);
                m_animator.SetBool("walk", false);
                m_animator.SetBool("walking shoot", false);
                m_animator.SetBool("shoot", false);
                break;

            case "walk":

                m_animator.SetBool("idle", false);
                m_animator.SetBool("walk", true);
                m_animator.SetBool("walking shoot", false);
                m_animator.SetBool("shoot", false);

                break;

            case "walking shoot":
                m_animator.SetBool("walking shoot", true);
                m_animator.SetBool("shoot", false);
                m_animator.SetBool("idle", false);
                m_animator.SetBool("walk", false);



                break;

            case "shoot":
                m_animator.SetBool("walking shoot", false);
                m_animator.SetBool("shoot", true);
                m_animator.SetBool("idle", false);
                m_animator.SetBool("walk", false);

                break;

        }
    }






}
