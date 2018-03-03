using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class Fade : MonoBehaviour {

	public Shader shader;
	public Texture ruleTex;

	float scale;
	Material material;
	List<FadeData> que = new List<FadeData>();
	float time;

	// Use this for initialization
	void Awake () {
		material = new Material(shader);
	}
	
	// Update is called once per frame
	void Update () {
		if (que.Count > 0) {
			var per = time / que[0].time;
			scale = Mathf.Lerp(que[0].start, que[0].end, per);
			time += Time.deltaTime;
			if (per >= 1.0f) {
				time = 0;
				if (que[0].callback != null) que[0].callback();
				que.RemoveAt(0);
			}
		}
	}

	Fade Fadeing(float _time, float _start, float _end, System.Action _callback) {
		FadeData data = new FadeData {
			time = _time,
			callback = _callback,
			start = _start,
			end = _end
		};
		que.Add(data);
		return this;
	}

	public Fade FadeIn(float time, System.Action callback = null) {
		return Fadeing(time, 1, 0, callback);
	}
	public Fade FadeOut(float time, System.Action callback = null) {
		return Fadeing(time, 0, 1, callback);
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		material.SetFloat("_Scale", scale);
		material.SetTexture("_RuleTex", ruleTex);
		Graphics.Blit(source, destination, material);
	}

	struct FadeData {
		public float time;
		public System.Action callback;
		public float start;
		public float end;
	}

	public void FadeGameObject(GameObject objectToFade, bool fadeIn, float duration){
		StartCoroutine(CoFadeGameObject(objectToFade, fadeIn, duration));
	}

	IEnumerator CoFadeGameObject(GameObject objectToFade, bool fadeIn, float duration)
	{
		float counter = 0f;

		//Set Values depending on if fadeIn or fadeOut
		float a, b;
		if (fadeIn)
		{
			a = 0;
			b = 1;
		}
		else
		{
			a = 1;
			b = 0;
		}

		int mode = 0;
		Color currentColor = Color.clear;

		SpriteRenderer tempSPRenderer = objectToFade.GetComponent<SpriteRenderer>();
		Image tempImage = objectToFade.GetComponent<Image>();
		RawImage tempRawImage = objectToFade.GetComponent<RawImage>();
		MeshRenderer tempRenderer = objectToFade.GetComponent<MeshRenderer>();
		Text tempText = objectToFade.GetComponent<Text>();

		//Check if this is a Sprite
		if (tempSPRenderer != null)
		{
			currentColor = tempSPRenderer.color;
			mode = 0;
		}
		//Check if Image
		else if (tempImage != null)
		{
			currentColor = tempImage.color;
			mode = 1;
		}
		//Check if RawImage
		else if (tempRawImage != null)
		{
			currentColor = tempRawImage.color;
			mode = 2;
		}
		//Check if Text 
		else if (tempText != null)
		{
			currentColor = tempText.color;
			mode = 3;
		}

		//Check if 3D Object
		else if (tempRenderer != null)
		{
			currentColor = tempRenderer.material.color;
			mode = 4;

			//ENABLE FADE Mode on the material if not done already
			tempRenderer.material.SetFloat("_Mode", 2);
			tempRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			tempRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			tempRenderer.material.SetInt("_ZWrite", 0);
			tempRenderer.material.DisableKeyword("_ALPHATEST_ON");
			tempRenderer.material.EnableKeyword("_ALPHABLEND_ON");
			tempRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			tempRenderer.material.renderQueue = 3000;
		}
		else
		{
			yield break;
		}

		while (counter < duration)
		{
			counter += Time.unscaledDeltaTime;
			float alpha = Mathf.Lerp(a, b, counter / duration);

			switch (mode)
			{
				case 0:
					tempSPRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
					break;
				case 1:
					tempImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
					break;
				case 2:
					tempRawImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
					break;
				case 3:
					tempText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
					break;
				case 4:
					tempRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
					break;
			}
			yield return null;
		}
	}
}
