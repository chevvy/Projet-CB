// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterPool_Flow"
{
	Properties
	{
		_Base_Water("Base_Water", Color) = (0.616367,0.7613711,0.764151,1)
		_Color1("Color 1", Color) = (0.2754015,0.3301887,0.2227216,0)
		_Water_Tip("Water_Tip", Color) = (0.7817729,0.9056604,0.8566816,0)
		_Color0("Color 0", Color) = (0.5301709,0.6981132,0.6330701,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
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

		uniform sampler2D _TextureSample0;
		uniform float4 _Color1;
		uniform float4 _Color0;
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
			float2 uv_TexCoord88 = v.texcoord.xy * float2( 1.8,1.8 ) + float2( -0.4,-0.4 );
			float4 tex2DNode86 = tex2Dlod( _TextureSample0, float4( uv_TexCoord88, 0, 0.0) );
			float2 uv_TexCoord70 = v.texcoord.xy * float2( 5,5 ) + float2( -2.5,-2.5 );
			float2 temp_output_1_0_g26 = uv_TexCoord70;
			float2 temp_output_11_0_g26 = ( temp_output_1_0_g26 - float2( 0,0 ) );
			float2 break18_g26 = temp_output_11_0_g26;
			float2 appendResult19_g26 = (float2(break18_g26.y , -break18_g26.x));
			float dotResult12_g26 = dot( temp_output_11_0_g26 , temp_output_11_0_g26 );
			float2 temp_output_72_0 = ( temp_output_1_0_g26 + ( appendResult19_g26 * ( dotResult12_g26 * float2( 0.05,0.05 ) ) ) + float2( 0,0 ) );
			float mulTime73 = _Time.y * -0.1;
			float cos76 = cos( mulTime73 );
			float sin76 = sin( mulTime73 );
			float2 rotator76 = mul( temp_output_72_0 - float2( 0,0 ) , float2x2( cos76 , -sin76 , sin76 , cos76 )) + float2( 0,0 );
			float simplePerlin2D78 = snoise( rotator76*5.0 );
			simplePerlin2D78 = simplePerlin2D78*0.5 + 0.5;
			float mulTime74 = _Time.y * -0.25;
			float cos75 = cos( mulTime74 );
			float sin75 = sin( mulTime74 );
			float2 rotator75 = mul( temp_output_72_0 - float2( 0,0 ) , float2x2( cos75 , -sin75 , sin75 , cos75 )) + float2( 0,0 );
			float simplePerlin2D77 = snoise( rotator75*8.0 );
			simplePerlin2D77 = simplePerlin2D77*0.5 + 0.5;
			float temp_output_79_0 = ( simplePerlin2D78 * simplePerlin2D77 );
			float2 uv_TexCoord32 = v.texcoord.xy * float2( 5,5 ) + float2( -2.5,-2.5 );
			float2 temp_output_1_0_g21 = uv_TexCoord32;
			float2 temp_output_11_0_g21 = ( temp_output_1_0_g21 - float2( 0,0 ) );
			float2 break18_g21 = temp_output_11_0_g21;
			float2 appendResult19_g21 = (float2(break18_g21.y , -break18_g21.x));
			float dotResult12_g21 = dot( temp_output_11_0_g21 , temp_output_11_0_g21 );
			float2 temp_output_38_0 = ( temp_output_1_0_g21 + ( appendResult19_g21 * ( dotResult12_g21 * float2( -0.1,-0.1 ) ) ) + float2( 0,0 ) );
			float mulTime63 = _Time.y * 0.1;
			float cos25 = cos( mulTime63 );
			float sin25 = sin( mulTime63 );
			float2 rotator25 = mul( temp_output_38_0 - float2( 0,0 ) , float2x2( cos25 , -sin25 , sin25 , cos25 )) + float2( 0,0 );
			float simplePerlin2D31 = snoise( rotator25*5.0 );
			simplePerlin2D31 = simplePerlin2D31*0.5 + 0.5;
			float mulTime62 = _Time.y * 0.2;
			float cos61 = cos( mulTime62 );
			float sin61 = sin( mulTime62 );
			float2 rotator61 = mul( temp_output_38_0 - float2( 0,0 ) , float2x2( cos61 , -sin61 , sin61 , cos61 )) + float2( 0,0 );
			float simplePerlin2D60 = snoise( rotator61*8.0 );
			simplePerlin2D60 = simplePerlin2D60*0.5 + 0.5;
			float mulTime95 = _Time.y * 0.4;
			float cos94 = cos( mulTime95 );
			float sin94 = sin( mulTime95 );
			float2 rotator94 = mul( temp_output_38_0 - float2( 0,0 ) , float2x2( cos94 , -sin94 , sin94 , cos94 )) + float2( 0,0 );
			float simplePerlin2D93 = snoise( rotator94*14.0 );
			simplePerlin2D93 = simplePerlin2D93*0.5 + 0.5;
			float temp_output_59_0 = ( simplePerlin2D31 * simplePerlin2D60 * simplePerlin2D93 );
			float layeredBlendVar67 = tex2DNode86.a;
			float layeredBlend67 = ( lerp( (0.0 + (temp_output_79_0 - 0.0) * (0.06 - 0.0) / (1.0 - 0.0)),(0.0 + (temp_output_59_0 - 0.0) * (0.1 - 0.0) / (1.0 - 0.0)) , layeredBlendVar67 ) );
			float3 temp_cast_0 = (layeredBlend67).xxx;
			v.vertex.xyz += temp_cast_0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord88 = i.uv_texcoord * float2( 1.8,1.8 ) + float2( -0.4,-0.4 );
			float4 tex2DNode86 = tex2D( _TextureSample0, uv_TexCoord88 );
			float2 uv_TexCoord70 = i.uv_texcoord * float2( 5,5 ) + float2( -2.5,-2.5 );
			float2 temp_output_1_0_g26 = uv_TexCoord70;
			float2 temp_output_11_0_g26 = ( temp_output_1_0_g26 - float2( 0,0 ) );
			float2 break18_g26 = temp_output_11_0_g26;
			float2 appendResult19_g26 = (float2(break18_g26.y , -break18_g26.x));
			float dotResult12_g26 = dot( temp_output_11_0_g26 , temp_output_11_0_g26 );
			float2 temp_output_72_0 = ( temp_output_1_0_g26 + ( appendResult19_g26 * ( dotResult12_g26 * float2( 0.05,0.05 ) ) ) + float2( 0,0 ) );
			float mulTime73 = _Time.y * -0.1;
			float cos76 = cos( mulTime73 );
			float sin76 = sin( mulTime73 );
			float2 rotator76 = mul( temp_output_72_0 - float2( 0,0 ) , float2x2( cos76 , -sin76 , sin76 , cos76 )) + float2( 0,0 );
			float simplePerlin2D78 = snoise( rotator76*5.0 );
			simplePerlin2D78 = simplePerlin2D78*0.5 + 0.5;
			float mulTime74 = _Time.y * -0.25;
			float cos75 = cos( mulTime74 );
			float sin75 = sin( mulTime74 );
			float2 rotator75 = mul( temp_output_72_0 - float2( 0,0 ) , float2x2( cos75 , -sin75 , sin75 , cos75 )) + float2( 0,0 );
			float simplePerlin2D77 = snoise( rotator75*8.0 );
			simplePerlin2D77 = simplePerlin2D77*0.5 + 0.5;
			float temp_output_79_0 = ( simplePerlin2D78 * simplePerlin2D77 );
			float layeredBlendVar91 = temp_output_79_0;
			float4 layeredBlend91 = ( lerp( _Color1,_Color0 , layeredBlendVar91 ) );
			float2 uv_TexCoord32 = i.uv_texcoord * float2( 5,5 ) + float2( -2.5,-2.5 );
			float2 temp_output_1_0_g21 = uv_TexCoord32;
			float2 temp_output_11_0_g21 = ( temp_output_1_0_g21 - float2( 0,0 ) );
			float2 break18_g21 = temp_output_11_0_g21;
			float2 appendResult19_g21 = (float2(break18_g21.y , -break18_g21.x));
			float dotResult12_g21 = dot( temp_output_11_0_g21 , temp_output_11_0_g21 );
			float2 temp_output_38_0 = ( temp_output_1_0_g21 + ( appendResult19_g21 * ( dotResult12_g21 * float2( -0.1,-0.1 ) ) ) + float2( 0,0 ) );
			float mulTime63 = _Time.y * 0.1;
			float cos25 = cos( mulTime63 );
			float sin25 = sin( mulTime63 );
			float2 rotator25 = mul( temp_output_38_0 - float2( 0,0 ) , float2x2( cos25 , -sin25 , sin25 , cos25 )) + float2( 0,0 );
			float simplePerlin2D31 = snoise( rotator25*5.0 );
			simplePerlin2D31 = simplePerlin2D31*0.5 + 0.5;
			float mulTime62 = _Time.y * 0.2;
			float cos61 = cos( mulTime62 );
			float sin61 = sin( mulTime62 );
			float2 rotator61 = mul( temp_output_38_0 - float2( 0,0 ) , float2x2( cos61 , -sin61 , sin61 , cos61 )) + float2( 0,0 );
			float simplePerlin2D60 = snoise( rotator61*8.0 );
			simplePerlin2D60 = simplePerlin2D60*0.5 + 0.5;
			float mulTime95 = _Time.y * 0.4;
			float cos94 = cos( mulTime95 );
			float sin94 = sin( mulTime95 );
			float2 rotator94 = mul( temp_output_38_0 - float2( 0,0 ) , float2x2( cos94 , -sin94 , sin94 , cos94 )) + float2( 0,0 );
			float simplePerlin2D93 = snoise( rotator94*14.0 );
			simplePerlin2D93 = simplePerlin2D93*0.5 + 0.5;
			float temp_output_59_0 = ( simplePerlin2D31 * simplePerlin2D60 * simplePerlin2D93 );
			float layeredBlendVar65 = (0.0 + (temp_output_59_0 - 0.0) * (1.0 - 0.0) / (0.52 - 0.0));
			float4 layeredBlend65 = ( lerp( _Base_Water,_Water_Tip , layeredBlendVar65 ) );
			float layeredBlendVar92 = tex2DNode86.a;
			float4 layeredBlend92 = ( lerp( layeredBlend91,layeredBlend65 , layeredBlendVar92 ) );
			o.Albedo = layeredBlend92.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
0;0;1920;1019;107.7013;163.3105;1;True;True
Node;AmplifyShaderEditor.Vector2Node;40;-1269.348,743.0174;Inherit;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;37;-1275.394,477.1211;Inherit;False;Constant;_Vector2;Vector 2;0;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;39;-1273.605,609.2109;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;-0.1,-0.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-1775.355,400.181;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;-2.5,-2.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;63;-854.4973,727.1727;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-1212.696,1501.231;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;-2.5,-2.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;62;-855.4421,983.0052;Inherit;False;1;0;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;68;-669.0291,1871.969;Inherit;False;Constant;_Vector3;Vector 3;0;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;69;-705.2864,1737.162;Inherit;False;Constant;_Vector4;Vector 4;0;0;Create;True;0;0;False;0;0.05,0.05;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;71;-702.0755,1597.072;Inherit;False;Constant;_Vector5;Vector 5;0;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;38;-1001.839,389.5596;Inherit;True;Radial Shear;-1;;21;c6dc9fc7fa9b08c4d95138f2ae88b526;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;95;-872.3585,1169.097;Inherit;False;1;0;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;74;-309.5099,2101.642;Inherit;False;1;0;FLOAT;-0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;73;-310.1795,1843.824;Inherit;False;1;0;FLOAT;-0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;94;-507.9915,1037.995;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;25;-570.4526,394.017;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;61;-560.8218,660.9875;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;72;-454.5213,1516.211;Inherit;True;Radial Shear;-1;;26;c6dc9fc7fa9b08c4d95138f2ae88b526;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;60;-116.3244,651.4232;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;75;-17.77794,1778.939;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;93;-84.76183,974.0327;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;14;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;31;-131.261,385.7104;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;76;-26.13477,1511.968;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;78;413.0569,1503.661;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;533.677,649.6073;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;77;426.7197,1769.374;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;8;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;90;750.1859,2279.03;Inherit;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.5301709,0.6981132,0.6330701,0;0.5301709,0.6981132,0.6330701,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;66;504.5673,160.3859;Inherit;False;Property;_Water_Tip;Water_Tip;2;0;Create;True;0;0;False;0;0.7817729,0.9056604,0.8566816,0;0.5301709,0.6981132,0.6330701,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;89;750.1859,2059.03;Inherit;False;Property;_Color1;Color 1;1;0;Create;True;0;0;False;0;0.2754015,0.3301887,0.2227216,0;0.2754015,0.3301887,0.2227216,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;64;504.5673,-59.61413;Inherit;False;Property;_Base_Water;Base_Water;0;0;Create;True;0;0;False;0;0.616367,0.7613711,0.764151,1;0.2754015,0.3301887,0.2227216,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;97;753.3062,388.7983;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.52;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;739.5877,1656.604;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;88;902.9136,1235.901;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.8,1.8;False;1;FLOAT2;-0.4,-0.4;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LayeredBlendNode;65;910.5681,131.7294;Inherit;True;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LayeredBlendNode;91;1156.187,2250.374;Inherit;True;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;58;1057.303,556.1572;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;86;1183.413,1175.4;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;-1;3d74b66c00fbb7245966a6be8c5ebd2f;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;81;1126.143,1522.084;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.06;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;67;1678.454,1007.488;Inherit;True;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;92;1637.83,309.0027;Inherit;True;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;34;-482.1938,917.3078;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;80;62.12405,2035.26;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2178.546,186.9731;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;WaterPool_Flow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;1;32;0
WireConnection;38;2;37;0
WireConnection;38;3;39;0
WireConnection;38;4;40;0
WireConnection;94;0;38;0
WireConnection;94;2;95;0
WireConnection;25;0;38;0
WireConnection;25;2;63;0
WireConnection;61;0;38;0
WireConnection;61;2;62;0
WireConnection;72;1;70;0
WireConnection;72;2;71;0
WireConnection;72;3;69;0
WireConnection;72;4;68;0
WireConnection;60;0;61;0
WireConnection;75;0;72;0
WireConnection;75;2;74;0
WireConnection;93;0;94;0
WireConnection;31;0;25;0
WireConnection;76;0;72;0
WireConnection;76;2;73;0
WireConnection;78;0;76;0
WireConnection;59;0;31;0
WireConnection;59;1;60;0
WireConnection;59;2;93;0
WireConnection;77;0;75;0
WireConnection;97;0;59;0
WireConnection;79;0;78;0
WireConnection;79;1;77;0
WireConnection;65;0;97;0
WireConnection;65;1;64;0
WireConnection;65;2;66;0
WireConnection;91;0;79;0
WireConnection;91;1;89;0
WireConnection;91;2;90;0
WireConnection;58;0;59;0
WireConnection;86;1;88;0
WireConnection;81;0;79;0
WireConnection;67;0;86;4
WireConnection;67;1;81;0
WireConnection;67;2;58;0
WireConnection;92;0;86;4
WireConnection;92;1;91;0
WireConnection;92;2;65;0
WireConnection;0;0;92;0
WireConnection;0;11;67;0
ASEEND*/
//CHKSM=DC5AE21158874A06F829E7512E72014C62FF3136