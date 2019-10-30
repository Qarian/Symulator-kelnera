using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI moneyUI = default;

	private int score = 0;

	public static GameManager singleton;
    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
		score = 0;
		moneyUI.text = score.ToString();
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	public void ChangeScore(int change)
	{
		score += change;
		moneyUI.text = score.ToString();
	}

}
