// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterFall"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Base_Water("Base_Water", Color) = (0.2754015,0.3301887,0.2227216,0)
		_Water_Tip("Water_Tip", Color) = (0.5301709,0.6981132,0.6330701,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Base_Water;
		uniform float4 _Water_Tip;
		uniform float _Cutoff = 0.5;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1);
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1);
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime20 = _Time.y * 1.1;
			float2 uv_TexCoord9 = v.texcoord.xy * float2( 3,0.2 );
			float2 panner1 = ( mulTime20 * float2( 0,1 ) + uv_TexCoord9);
			float simplePerlin2D4 = snoise( panner1 );
			simplePerlin2D4 = simplePerlin2D4*0.5 + 0.5;
			float simplePerlin2D8 = snoise( panner1*3.0 );
			simplePerlin2D8 = simplePerlin2D8*0.5 + 0.5;
			float mulTime38 = _Time.y * 0.9;
			float2 uv_TexCoord18 = v.texcoord.xy * float2( 3,0.2 );
			float2 panner17 = ( mulTime38 * float2( 0,1 ) + uv_TexCoord18);
			float simplePerlin2D16 = snoise( panner17*5.0 );
			simplePerlin2D16 = simplePerlin2D16*0.5 + 0.5;
			float simplePerlin2D21 = snoise( panner17*7.0 );
			simplePerlin2D21 = simplePerlin2D21*0.5 + 0.5;
			float temp_output_15_0 = ( ( simplePerlin2D4 * simplePerlin2D8 ) * simplePerlin2D16 * simplePerlin2D21 );
			float3 temp_cast_0 = ((0.0 + (temp_output_15_0 - 0.0) * (0.5 - 0.0) / (1.0 - 0.0))).xxx;
			v.vertex.xyz += temp_cast_0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime20 = _Time.y * 1.1;
			float2 uv_TexCoord9 = i.uv_texcoord * float2( 3,0.2 );
			float2 panner1 = ( mulTime20 * float2( 0,1 ) + uv_TexCoord9);
			float simplePerlin2D4 = snoise( panner1 );
			simplePerlin2D4 = simplePerlin2D4*0.5 + 0.5;
			float simplePerlin2D8 = snoise( panner1*3.0 );
			simplePerlin2D8 = simplePerlin2D8*0.5 + 0.5;
			float mulTime38 = _Time.y * 0.9;
			float2 uv_TexCoord18 = i.uv_texcoord * float2( 3,0.2 );
			float2 panner17 = ( mulTime38 * float2( 0,1 ) + uv_TexCoord18);
			float simplePerlin2D16 = snoise( panner17*5.0 );
			simplePerlin2D16 = simplePerlin2D16*0.5 + 0.5;
			float simplePerlin2D21 = snoise( panner17*7.0 );
			simplePerlin2D21 = simplePerlin2D21*0.5 + 0.5;
			float temp_output_15_0 = ( ( simplePerlin2D4 * simplePerlin2D8 ) * simplePerlin2D16 * simplePerlin2D21 );
			float layeredBlendVar26 = temp_output_15_0;
			float4 layeredBlend26 = ( lerp( _Base_Water,_Water_Tip , layeredBlendVar26 ) );
			o.Albedo = layeredBlend26.rgb;
			Gradient gradient34 = NewGradient( 0, 4, 2, float4( 0, 0, 0, 0.3029374 ), float4( 1, 1, 1, 0.4264744 ), float4( 1, 1, 1, 0.5617609 ), float4( 0, 0, 0, 0.6970626 ), 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float4 temp_cast_1 = (temp_output_15_0).xxxx;
			float4 blendOpSrc30 = SampleGradient( gradient34, i.uv_texcoord.x );
			float4 blendOpDest30 = temp_cast_1;
			float grayscale37 = Luminance(( saturate( ( blendOpSrc30 * blendOpDest30 ) )).rgb);
			float temp_output_36_0 = (-0.3 + (grayscale37 - 0.0) * (100.0 - -0.3) / (1.0 - 0.0));
			o.Alpha = temp_output_36_0;
			clip( temp_output_36_0 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
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
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
29;48;1093;895;2250.759;458.8169;1.889259;True;True
Node;AmplifyShaderEditor.Vector2Node;12;-1302.817,157.4964;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;20;-1654.952,112.0317;Inherit;False;1;0;FLOAT;1.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1369.332,-4.223585;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,0.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-1371.605,477.0731;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,0.2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;19;-1303.92,638.7931;Inherit;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;False;0;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;1;-1052.636,64.62427;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;38;-1627.011,564.7471;Inherit;False;1;0;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;4;-745.4799,-179.6262;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;8;-745.8571,88.77641;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;17;-1053.739,518.4426;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;21;-746.6033,802.0021;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;7;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;16;-749.0054,572.0731;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-422.9051,-57.22359;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;34;262.123,-789.4429;Inherit;False;0;4;2;0,0,0,0.3029374;1,1,1,0.4264744;1,1,1,0.5617609;0,0,0,0.6970626;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;264.0551,-600.9471;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientSampleNode;32;545.6823,-734.4429;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-116.904,127.3073;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;30;882.3816,-541.1866;Inherit;True;Multiply;True;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;22;-123.5264,-365.2238;Inherit;False;Property;_Base_Water;Base_Water;1;0;Create;True;0;0;False;0;0.2754015,0.3301887,0.2227216,0;0.2754015,0.3301887,0.2227216,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-135.5264,-169.2238;Inherit;False;Property;_Water_Tip;Water_Tip;2;0;Create;True;0;0;False;0;0.5301709,0.6981132,0.6330701,0;0.5301709,0.6981132,0.6330701,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;37;1191.268,-523.3088;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;14;235.3713,191.5141;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;27;266.6936,-710.2997;Inherit;False;0;4;2;0,0,0,0.2500038;1,1,1,0.4;1,1,1,0.6176547;0,0,0,0.7411765;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.TFHCRemapNode;36;1149.527,-80.18855;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.3;False;4;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;26;150.4736,-317.2238;Inherit;True;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1618.354,-132.7221;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;WaterFall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;9;0
WireConnection;1;2;12;0
WireConnection;1;1;20;0
WireConnection;4;0;1;0
WireConnection;8;0;1;0
WireConnection;17;0;18;0
WireConnection;17;2;19;0
WireConnection;17;1;38;0
WireConnection;21;0;17;0
WireConnection;16;0;17;0
WireConnection;7;0;4;0
WireConnection;7;1;8;0
WireConnection;32;0;34;0
WireConnection;32;1;29;1
WireConnection;15;0;7;0
WireConnection;15;1;16;0
WireConnection;15;2;21;0
WireConnection;30;0;32;0
WireConnection;30;1;15;0
WireConnection;37;0;30;0
WireConnection;14;0;15;0
WireConnection;36;0;37;0
WireConnection;26;0;15;0
WireConnection;26;1;22;0
WireConnection;26;2;23;0
WireConnection;0;0;26;0
WireConnection;0;9;36;0
WireConnection;0;10;36;0
WireConnection;0;11;14;0
ASEEND*/
//CHKSM=07A145461853CDA36893F759AD9AD35A5BBE066C