using UnityEngine;
using System.Collections.Generic;

public class SlideController : MonoBehaviour
{

	public List<Texture> Slides = new List<Texture> ();

	private int mSlideIndex = 0;
	// Use this for initialization
	void Start ()
	{
		if (Slides != null && Slides.Count > 0) {
			renderer.material.mainTexture = Slides [0];
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			if (Slides.Count > mSlideIndex) {
				mSlideIndex++;
				renderer.material.mainTexture = Slides [mSlideIndex];

			}
		}
	}
}
