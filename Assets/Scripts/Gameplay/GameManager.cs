using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI moneyUI = default;

	public float neededScore;
	[SerializeField] public int score = 0;

	[HideInInspector] public List<Action> onDayEnd = new List<Action>();

	public static GameManager singleton;
    private void Awake()
    {
	    if (singleton)
		    Destroy(singleton.gameObject);
        singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
		score = 0;
		moneyUI.text = score.ToString();

		GetComponent<Timer>().StartDay();
    }

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

    public void EndDay()
	{
		Debug.Log("Day ended!");
		foreach (var action in onDayEnd)
		{
			action();
		}
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
	}

	public void ChangeScore(int change)
	{
		score += change;
		moneyUI.text = score.ToString();
	}

	public void RunCoroutine(IEnumerator iEnumerator)
	{
		StartCoroutine(iEnumerator);
	}
}
