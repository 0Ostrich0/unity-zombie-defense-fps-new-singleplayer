using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FundSystem : MonoBehaviour {
	[SerializeField] private int fund = 0;

	Text fundText;

	void Start() {
		fundText = GameObject.Find("UI/InGameUI/PlayerUI/CharacterStatus/FundText").GetComponent<Text>();
		UpdateUI();
	}

	void UpdateUI() {
		fundText.text = "Fund: " + fund.ToString() + " $";
	}

	public int GetFund() {
		return fund;
	}

	public void AddFund(int amount) {
		fund += amount;
		UpdateUI();
	}

	public void TakeFund(int amount) {
		fund -= amount;
		UpdateUI();
	}
}
