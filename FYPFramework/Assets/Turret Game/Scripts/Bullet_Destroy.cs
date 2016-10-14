using UnityEngine;
using System.Collections;

public class Bullet_Destroy : MonoBehaviour {
	private Animator animator;
	private GameObject Bullet_Effect;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();

		Bullet_Effect = GameObject.Find ("Bullet_Effect");
	}
	
	// Update is called once per frame
	void Update () {
		if (animator.GetBool ("Destroy")) {
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Destroy") && animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1 && !animator.IsInTransition(0)) {
				animator.SetBool ("Destroy", false);
				gameObject.tag = "Bullet_Effect_Stop";
				gameObject.transform.position = Bullet_Effect.transform.position;
				gameObject.transform.SetParent (Bullet_Effect.transform);
			}
		}
	}
}
