using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SlideController : MonoBehaviour
{

	public List<Texture> Slides = new List<Texture> ();
	GoogleDrive Drive = new GoogleDrive ();


	private int mSlideIndex = 0;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (ListPresentationsCoroutine ());


		if (Slides != null && Slides.Count > 0) {
			renderer.material.mainTexture = Slides [0];
		}
	}

	IEnumerator ListPresentationsCoroutine ()
	{
		Drive.ClientID = "327028157545-te6a8nm4josmi80t08i3mvkn3snmnt9m.apps.googleusercontent.com";
		Drive.ClientSecret = "XJWfBPzJinRvj1JLytI-eyWK";
		
		// Request authorization.
		var authorization = Drive.Authorize ();
		yield return StartCoroutine (authorization);
		
		if (authorization.Current is Exception) {
			Debug.LogWarning (authorization.Current as Exception);
			yield break;
		}
		
		// Authorization succeeded.
		Debug.Log ("User Account: " + Drive.UserAccount);

		var listFiles = Drive.ListFilesByQueary ("mimeType = 'application/vnd.google-apps.presentation'");
		//var listFiles = Drive.ListAllFiles ();
		yield return StartCoroutine (listFiles);
		var files = GoogleDrive.GetResult<List<GoogleDrive.File>> (listFiles);
		
		if (files != null && files.Count > 0) {
			foreach (GoogleDrive.File file in files) {
				//Debug.Log (file.Title);
			}
			GoogleDrive.File file0 = files [0];
			var pdfExportLink = file0.ExportLinks ["application/pdf"];

			var download = Drive.DownloadFile ((string)pdfExportLink);
			yield return StartCoroutine (download);
			var data = GoogleDrive.GetResult<byte[]> (download);
			if (data != null) {
				Debug.Log ("Happy");
				Stream pdfStream = new MemoryStream (data);
				//PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open (pdfStream, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
			    
				//	Debug.Log (pdfDoc.Info.Title);
			}

		} else {
			Debug.Log ("files: " + files.Count);
		}


	}


	private void RenderPNGs (byte[] data)
	{
#if !UNITY_EDITOR && UNITY_Android
		using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			
			using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
				
				AndroidJavaClass cls_MainActivity = new AndroidJavaClass ("com.me.callActivity.MainActivity");
				
				cls_MainActivity.CallStatic ("Init", obj_Activity);
				
				cls_MainActivity.CallStatic ("renderToBitmap", data, Application.persistentDataPath);
				
			}	
		}
#endif
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
