using UnityEngine;

[ExecuteInEditMode]
public class CameraShader : MonoBehaviour
{
	public Shader Shader;
	private Material _material;

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (Shader == null) return;
		if (_material == null || _material.shader != Shader)
		{
			_material = new Material(Shader);
		}

		Graphics.Blit(src, dest, _material);
	}
}
