using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] MainMenuSoundManager soundManager;
	[SerializeField] int thisIndex;
    private void Awake()
    {
		menuButtonController = GetComponentInParent<MenuButtonController>();
		animator = GetComponent<Animator>();
		soundManager = GetComponentInParent<MainMenuSoundManager>();

	}
    
    
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

	public void PlayPressedSound()
    {
		soundManager.PlayOneSound("Pressed");
    }

	public void PlaySelectedSound()
    {
		soundManager.PlayOneSound("Selected");
	}
}
