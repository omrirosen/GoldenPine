﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	
	[SerializeField] int thisIndex;
    private void Awake()
    {
		menuButtonController = GetComponentInParent<MenuButtonController>();
		animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
	{
		if (menuButtonController.index == thisIndex)
		{
			animator.SetBool("selected", true);
			if (Input.GetAxis("Submit") == 1)
			{
				animator.SetBool("pressed", true);
			}
			else if (animator.GetBool("pressed"))
			{
				animator.SetBool("pressed", false);
				
			}
		}
		else
		{
			animator.SetBool("selected", false);
		}
	}
}
