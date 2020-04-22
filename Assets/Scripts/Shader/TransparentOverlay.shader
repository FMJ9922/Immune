Shader "Custom/TransparentOverlay"
{
	Properties
	{
		_MainTex("MainTex(RGBA)", 2D) = "white" {}
		_Color("Color",Color) = (1,1,1,1)
	}

		SubShader
	{
		LOD 100
		Cull Off Lighting Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Tags
		{
			"Queue" = "Overlay"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			SetTexture[_MainTex*_Color]
		}
	}
}
