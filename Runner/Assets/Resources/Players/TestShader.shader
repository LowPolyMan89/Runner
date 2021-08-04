
Shader "PlayerOutlineUnlitShader"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_LightColor("LightColor", Color) = (0,0,0,0)
		_ContureLightColor("ContureLightColor", Color) = (0,0,0,0)
		_ShadowIntensity("ShadowIntensity", Range( 0 , 1)) = 0
		_ShadowSmooth("ShadowSmooth", Range( 0 , 10)) = 1
		_CounterSmooth("CounterSmooth", Range( 0 , 10)) = 1
		_Width("Width", Range( 0 , 1)) = 0
		_OutlineColor("OutlineColor", Color) = (0,0,0,0)
		_MainLitgthDirection("MainLitgthDirection", Vector) = (0,0,0,0)
		_ContureLitgthDirection("ContureLitgthDirection", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		ZWrite Off
		ZTest LEqual
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		
		struct Input
		{
			half filler;
		};
		uniform half4 _OutlineColor;
		uniform half _Width;
		
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float outlineVar = _Width;
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _OutlineColor.rgb;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		ZTest LEqual
		CGPROGRAM
		#pragma target 3.0
		#pragma only_renderers d3d9 d3d11_9x d3d11 glcore gles gles3 
		#pragma surface surf Unlit keepalpha noshadow exclude_path:deferred noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			half3 worldNormal;
		};

		uniform sampler2D _Texture0;
		uniform half4 _Texture0_ST;
		uniform half4 _LightColor;
		uniform half _ShadowSmooth;
		uniform half3 _MainLitgthDirection;
		uniform half _ShadowIntensity;
		uniform half _CounterSmooth;
		uniform half3 _ContureLitgthDirection;
		uniform half4 _ContureLightColor;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			half3 ase_worldNormal = i.worldNormal;
			half3 ase_normWorldNormal = normalize( ase_worldNormal );
			half dotResult20 = dot( _MainLitgthDirection , ase_normWorldNormal );
			half smoothstepResult60 = smoothstep( 0.0 , _ShadowSmooth , ( ( dotResult20 + 1.0 ) / 2.0 ));
			half clampResult45 = clamp( smoothstepResult60 , _ShadowIntensity , 1.0 );
			half4 temp_output_28_0 = ( _LightColor * clampResult45 );
			half4 temp_cast_0 = (_CounterSmooth).xxxx;
			half dotResult64 = dot( _ContureLitgthDirection , ase_normWorldNormal );
			half4 smoothstepResult83 = smoothstep( float4( 0,0,0,0 ) , temp_cast_0 , ( ( ( ( dotResult64 + 1.0 ) / 2.0 ) * _ContureLightColor ) - temp_output_28_0 ));
			half4 blendOpSrc82 = ( tex2D( _Texture0, uv_Texture0 ) * temp_output_28_0 );
			half4 blendOpDest82 = smoothstepResult83;
			o.Emission = ( saturate( 	max( blendOpSrc82, blendOpDest82 ) )).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
}
