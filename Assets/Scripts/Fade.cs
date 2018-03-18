using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

	//Fade GameObject
	public void FadeGameObject(GameObject objectToFade, float duration, float initialValue, float finalValue){
		StartCoroutine(JustFadeGameObject(objectToFade, duration, initialValue, finalValue));
	}

	IEnumerator JustFadeGameObject(GameObject objectToFade, float duration, float initialValue, float finalValue)
	{
		float counter = 0f;
		int mode = 0;
		Color currentColor = Color.clear;

		SpriteRenderer tempSPRenderer = objectToFade.GetComponent<SpriteRenderer>();
		Image tempImage = objectToFade.GetComponent<Image>();
		RawImage tempRawImage = objectToFade.GetComponent<RawImage>();
		MeshRenderer tempRenderer = objectToFade.GetComponent<MeshRenderer>();
		Text tempText = objectToFade.GetComponent<Text>();
		Tilemap tempTile = objectToFade.GetComponent<Tilemap>();

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

		//Check if Tilemap
		else if(tempTile){
			currentColor = tempTile.color;
			mode = 5;
		}
		else
		{
			yield break;
		}

		while (counter < duration)
		{
			counter += Time.unscaledDeltaTime;
			float alpha = Mathf.Lerp(initialValue, finalValue, counter / duration);

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
				case 5:
					tempTile.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
					break;
			}
			yield return null;
		}
	}
}