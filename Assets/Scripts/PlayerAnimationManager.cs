using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private string currentAnimation;
    
    // Cache Components
    private Animator animator;
    
    //Animation States
    private const string DASH_MOVE = "Dash Move";
    private const string DASH_STOP = "Dash Stop";
    private const string FLIP_LEFT = "Flip Left 2";
    private const string FLIP_RIGHT = "Flip Right 2";
    private const string FLIP_SINGLE = "Flip Single";
    private const string IDLE_LEFT = "Idle Left";
    private const string IDLE_RIGHT = "Idle Right";
    private const string IDLE_TO_RUN = "Idle To Run";
    private const string FALL_DOWN = "Fall Down"; //Jump Down
    private const string JUMP_PEAK = "Jump Peak";
    private const string JUMP_SIDE_LAND = "Jump Side Land";
    private const string JUMP_STRAIGHT_LAND = "Jump Straight Land";
    private const string JUMP_STRAIGHT_UP = "Jump Straight UP";
    private const string RUN_TO_IDLE = "Run To Idle";
    private const string RUN = "Run";
    private const string SIDE_JUMP = "Side Jump";
    private const string WALL_CONTACT = "Wall Contact";
    private const string WALL_SLIDE = "Wall Slide";
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
        
    }
}
