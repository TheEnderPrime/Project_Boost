using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	float time;
	
	void Start()
	{
		TextMeshPro mText = gameObject.GetComponent<TextMeshPro>();
		time = Time.deltaTime;
	}

	// Update is called once per frame
	void Update () {
		timerText.text = "Time: " + time;
	}
}
