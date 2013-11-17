﻿using UnityEngine;
using System.Collections;

public class PhotoManager : MonoBehaviour {
	private static PhotoManager instance;
	public static PhotoManager Instance{
		get{
			return instance;
		}
	}


	private ImageByID[] imageCache = new ImageByID[10];
	private int idx = 0;

	void Awake(){
		instance = this;
		for(int i=0; i<imageCache.Length; i++){
			imageCache[i] = new ImageByID();
		}
	}


	public void LoadImage(int id, string url){


		ImageByID image = imageCache[idx];
		if(image.texture != null){
			Destroy(image.texture);
		}
		image.id = id;
		image.url = url;
		StartCoroutine(LoadImage(image));


		idx = (idx +1)%imageCache.Length;
	}

	IEnumerator LoadImage(ImageByID image){
		WWW www = new WWW(image.url);
        yield return www;
        if(www.error != null){
			Debug.LogError("ERROR ON PHOTO LOAD: "+ www.error);
		}
		else{

			//check to see if we've overwritten this load, before it happens
			if(www.url == image.url){
				Texture2D tex = www.texture;
				if(tex != null){
					image.texture = tex;
				}
				else{
					Debug.LogError("www.texture is null");
				}
			}
			else{
				Destroy(www.texture);
			}
		}

		yield return 0;
	}

}


[System.Serializable]
public class ImageByID{
	public int id;
	public string url;
	public Texture2D texture;
}
