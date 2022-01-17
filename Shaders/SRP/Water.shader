Shader "Custom/Water"
{
	Properties
	{
		// color of the water
		_Color("Color", Color) = (1, 1, 1, 1)
		// color of the edge effect
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		// width of the edge effect
		_DepthFactor("Depth Factor", float) = 1.0
		//Bla bla indentation
		_DepthRampTex("Depth Ramp", 2D) = "white" {}
		//MainTex
		_MainTex("Main Texture", 2D) = "white" {}

	}
		SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}

		// Grab the screen behind the object into _BackgroundTexture
		GrabPass
		{
			"_BackgroundTexture"
		}

		Pass
		{
		Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			// required to use ComputeScreenPos()
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			 // Unity built-in - NOT required in Properties
			sampler2D _CameraDepthTexture;
			sampler2D _DepthRampTex;
			sampler2D _MainTex;
			float4 _Color;
			float4 _EdgeColor;
			float  _DepthFactor;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texCoord : TEXCOORD1;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 texCoord : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
			};

			vertexOutput vert(vertexInput input)
			  {
				vertexOutput output;

				// convert obj-space position to camera clip space
				output.pos = UnityObjectToClipPos(input.vertex);

				// compute depth (screenPos is a float4)
				output.screenPos = ComputeScreenPos(output.pos);

				output.texCoord = input.texCoord;

				return output;
			  }

			float4 frag(vertexOutput input) : COLOR
			{
				// apply depth texture
				float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenPos);
				float depth = LinearEyeDepth(depthSample).r;

				// create foamline
				float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));
				float4 foamRamp = float4(tex2D(_DepthRampTex, float2(foamLine, 0.5)).rgb, 1.0);

				// sample main texture
				float4 albedo = tex2D(_MainTex, input.texCoord.xy);

				float4 col = _Color * foamRamp + foamLine * _EdgeColor * albedo;
				return col;
			}

				ENDCG
	}
	}
}