// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TestShader"
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
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
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
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
		uniform half4 _OutlineColor;
		uniform half _Width;

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
		CGPROGRAM
		#pragma only_renderers d3d9 d3d11_9x d3d11 glcore gles gles3 
		#pragma surface surf Unlit keepalpha fullforwardshadows noshadow exclude_path:deferred noambient novertexlights nolightmap  nodynlightmap nodirlightmap nometa noforwardadd vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
-1920;0;1920;1019;4511.605;2515.178;5.252891;True;True
Node;AmplifyShaderEditor.Vector3Node;57;-1970.963,875.6999;Inherit;False;Property;_MainLitgthDirection;MainLitgthDirection;8;0;Create;True;0;0;0;False;0;False;0,0,0;15.1,6.5,-16.37;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;19;-1988.484,1215.046;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;37;-1420.466,1493.31;Inherit;False;Constant;_ShadowOffset;ShadowOffset;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;20;-1500.718,1037.782;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;68;-2048.973,175.796;Inherit;False;Property;_ContureLitgthDirection;ContureLitgthDirection;9;0;Create;True;0;0;0;False;0;False;0,0,0;-47.02,-9.4,81.06;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;63;-2058.916,499.9859;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1196.467,1205.31;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1143.098,1453.292;Inherit;False;Constant;_Contrast;Contrast;7;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;64;-1571.15,322.7221;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1536.367,561.5189;Inherit;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;34;-904.9688,1083.461;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-924.3208,1447.537;Inherit;False;Property;_ShadowSmooth;ShadowSmooth;4;0;Create;True;0;0;0;False;0;False;1;5.05;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;60;-497.2986,1061.536;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1322.653,565.4535;Inherit;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-53.97381,1471.528;Inherit;False;Constant;_ClampMax;ClampMax;6;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;67;-1289.634,335.6593;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-507.9425,1404.109;Inherit;False;Property;_ShadowIntensity;ShadowIntensity;3;0;Create;True;0;0;0;False;0;False;0;0.682;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;69;-937.5101,445.6965;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;71;-1009.406,722.0417;Inherit;False;Property;_ContureLightColor;ContureLightColor;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.06301175,0.1820998,0.2264151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;117.3549,818.6049;Inherit;False;Property;_LightColor;LightColor;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.9904841,1,0.9103774,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;45;73.85074,1079.505;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;484.4737,827.9496;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-596.2227,472.1547;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;223.8386,-889.9518;Inherit;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;0;False;0;False;None;67a3356f1619e86469471e4eb1d1eeec;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleSubtractOpNode;76;234.141,355.721;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-99.38815,85.68576;Inherit;False;Property;_CounterSmooth;CounterSmooth;5;0;Create;True;0;0;0;False;0;False;1;2.82;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;546.8386,-825.9518;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;1337.124,-430.6365;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;83;722.9915,-4.606221;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;1072.693,454.7897;Inherit;False;Property;_Width;Width;6;0;Create;True;0;0;0;False;0;False;0;0.022;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;1075.001,285.0038;Inherit;False;Property;_OutlineColor;OutlineColor;7;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;82;1590.655,-51.5509;Inherit;False;Lighten;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.OutlineNode;23;1457.546,385.9818;Inherit;False;0;True;None;2;3;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;59;2093.961,-52.49714;Half;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;TestShader;False;False;False;False;True;True;True;True;True;False;True;True;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;6;d3d9;d3d11_9x;d3d11;glcore;gles;gles3;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;57;0
WireConnection;20;1;19;0
WireConnection;33;0;20;0
WireConnection;33;1;37;0
WireConnection;64;0;68;0
WireConnection;64;1;63;0
WireConnection;34;0;33;0
WireConnection;34;1;38;0
WireConnection;60;0;34;0
WireConnection;60;2;61;0
WireConnection;67;0;64;0
WireConnection;67;1;65;0
WireConnection;69;0;67;0
WireConnection;69;1;66;0
WireConnection;45;0;60;0
WireConnection;45;1;46;0
WireConnection;45;2;58;0
WireConnection;28;0;29;0
WireConnection;28;1;45;0
WireConnection;70;0;69;0
WireConnection;70;1;71;0
WireConnection;76;0;70;0
WireConnection;76;1;28;0
WireConnection;1;0;2;0
WireConnection;26;0;1;0
WireConnection;26;1;28;0
WireConnection;83;0;76;0
WireConnection;83;2;84;0
WireConnection;82;0;26;0
WireConnection;82;1;83;0
WireConnection;23;0;24;0
WireConnection;23;1;25;0
WireConnection;59;2;82;0
WireConnection;59;11;23;0
ASEEND*/
//CHKSM=5AAF662CCFD094F32D601B136AEF250E640EEFDE