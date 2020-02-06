using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI moneyUI = default;

    [Space]
    [Tooltip("In game time in minutes")]
    [SerializeField] float dayDuration = 6;

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
        StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
		//TODO some UI indicator
		yield return new WaitForSeconds(dayDuration * 60);
		EndTime();
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	private void EndTime()
	{
		Debug.LogWarning("Time Ended!");
		CustomersManager.singleton.EndTime();
	}

	public void EndDay()
	{
		Debug.Log("Day ended!");
	}

	public void ChangeScore(int change)
	{
		score += change;
		moneyUI.text = score.ToString();
	}

}
