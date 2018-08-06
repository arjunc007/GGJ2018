using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour {

	public void Press()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
