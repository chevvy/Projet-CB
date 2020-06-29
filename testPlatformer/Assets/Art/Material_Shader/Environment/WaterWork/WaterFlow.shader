// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterFlow"
{
	Properties
	{
		_Base_Water("Base_Water", Color) = (0.2754015,0.3301887,0.2227216,0)
		_Water_Tip("Water_Tip", Color) = (0.5301709,0.6981132,0.6330701,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Base_Water;
		uniform float4 _Water_Tip;


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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 panner1 = ( _Time.y * float2( 0,-0.05 ) + v.texcoord.xy);
			float simplePerlin2D4 = snoise( panner1*7.0 );
			simplePerlin2D4 = simplePerlin2D4*0.5 + 0.5;
			float simplePerlin2D8 = snoise( panner1*15.0 );
			simplePerlin2D8 = simplePerlin2D8*0.5 + 0.5;
			float2 panner17 = ( _Time.y * float2( 0,-0.1 ) + v.texcoord.xy);
			float simplePerlin2D16 = snoise( panner17*20.0 );
			simplePerlin2D16 = simplePerlin2D16*0.5 + 0.5;
			float simplePerlin2D21 = snoise( panner17*25.0 );
			simplePerlin2D21 = simplePerlin2D21*0.5 + 0.5;
			float temp_output_15_0 = ( ( simplePerlin2D4 * simplePerlin2D8 ) * simplePerlin2D16 * simplePerlin2D21 );
			float3 temp_cast_0 = ((0.0 + (temp_output_15_0 - 0.0) * (0.7 - 0.0) / (1.0 - 0.0))).xxx;
			v.vertex.xyz += temp_cast_0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner1 = ( _Time.y * float2( 0,-0.05 ) + i.uv_texcoord);
			float simplePerlin2D4 = snoise( panner1*7.0 );
			simplePerlin2D4 = simplePerlin2D4*0.5 + 0.5;
			float simplePerlin2D8 = snoise( panner1*15.0 );
			simplePerlin2D8 = simplePerlin2D8*0.5 + 0.5;
			float2 panner17 = ( _Time.y * float2( 0,-0.1 ) + i.uv_texcoord);
			float simplePerlin2D16 = snoise( panner17*20.0 );
			simplePerlin2D16 = simplePerlin2D16*0.5 + 0.5;
			float simplePerlin2D21 = snoise( panner17*25.0 );
			simplePerlin2D21 = simplePerlin2D21*0.5 + 0.5;
			float temp_output_15_0 = ( ( simplePerlin2D4 * simplePerlin2D8 ) * simplePerlin2D16 * simplePerlin2D21 );
			float layeredBlendVar26 = temp_output_15_0;
			float4 layeredBlend26 = ( lerp( _Base_Water,_Water_Tip , layeredBlendVar26 ) );
			o.Albedo = layeredBlend26.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
0;0;1920;1019;711.5264;532.2238;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1367.857,-4.223585;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;12;-1302.817,157.4964;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;0,-0.05;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;11;-1323.817,347.4964;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;1;-1052.636,37.14592;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-1370.005,477.0731;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;20;-1325.965,828.793;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;19;-1303.92,638.7931;Inherit;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;False;0;0,-0.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;17;-1053.739,518.4426;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;4;-745.4799,-179.6262;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;7;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;8;-745.8571,88.77641;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-422.9051,-57.22359;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;16;-748.0054,571.0731;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;21;-746.6033,803.0021;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;25;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-123.5264,-365.2238;Inherit;False;Property;_Base_Water;Base_Water;0;0;Create;True;0;0;False;0;0.2754015,0.3301887,0.2227216,0;0.2754015,0.3301887,0.2227216,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-116.904,127.3073;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-135.5264,-169.2238;Inherit;False;Property;_Water_Tip;Water_Tip;1;0;Create;True;0;0;False;0;0.5301709,0.6981132,0.6330701,0;0.5301709,0.6981132,0.6330701,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;14;235.3713,191.5141;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;26;150.4736,-317.2238;Inherit;True;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;550.696,-51.93692;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;WaterFlow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;9;0
WireConnection;1;2;12;0
WireConnection;1;1;11;0
WireConnection;17;0;18;0
WireConnection;17;2;19;0
WireConnection;17;1;20;0
WireConnection;4;0;1;0
WireConnection;8;0;1;0
WireConnection;7;0;4;0
WireConnection;7;1;8;0
WireConnection;16;0;17;0
WireConnection;21;0;17;0
WireConnection;15;0;7;0
WireConnection;15;1;16;0
WireConnection;15;2;21;0
WireConnection;14;0;15;0
WireConnection;26;0;15;0
WireConnection;26;1;22;0
WireConnection;26;2;23;0
WireConnection;0;0;26;0
WireConnection;0;11;14;0
ASEEND*/
//CHKSM=7EC34FC0BFA8E34D635545FA6E285A95784D2BF6