using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillReward : MonoBehaviour {
	public int exp;
	public int fund;

	void Start() {
		SetReward(exp, fund);
	}

	public void SetReward(int newExp, int newFund) {
		exp = newExp;
		fund = newFund;
	}
}
