Shader "Unlit/DissolveHLSL"
{
     Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}      // �⺻ �ؽ�ó
        _NoiseTex ("Noise Texture", 2D) = "white" {}   // ������ �ؽ�ó
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0.5 // Dissolve �Ӱ谪
        _EdgeColor ("Edge Color", Color) = (1, 0.5, 0, 1)     // �����ڸ� ����
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha // ���� ���� ����
        ZWrite Off                      // Z ���� ��� ���� (������ ��ü ó��)

        Pass
        {
            HLSLPROGRAM
            #include "UnityCG.cginc" // Unity ���̴� ���̺귯�� ����
            #pragma vertex vert
            #pragma fragment frag

            // �ؽ�ó�� �Ķ����
            sampler2D _MainTex;      // �⺻ �ؽ�ó
            sampler2D _NoiseTex;     // ������ �ؽ�ó
            float _DissolveAmount;   // Dissolve �Ӱ谪
            float4 _EdgeColor;       // �����ڸ� ����

            // ���� ���̴� �Է�
            struct appdata
            {
                float4 vertex : POSITION;  // ���� ��ġ
                float2 uv : TEXCOORD0;     // UV ��ǥ
            };

            // ���� ���̴� ���
            struct v2f
            {
                float4 pos : SV_POSITION;  // ȭ�� ��ǥ
                float2 uv_MainTex : TEXCOORD0;  // �⺻ �ؽ�ó UV
                float2 uv_NoiseTex : TEXCOORD1; // ������ �ؽ�ó UV
            };

            // ���� ���̴�
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);  // ���� �� Ŭ�� ��ǥ ��ȯ
                o.uv_MainTex = v.uv;                    // UV ����
                o.uv_NoiseTex = v.uv;                   // ���� UV ���� (�ʿ� �� ���� ����)
                return o;
            }

            // �ȼ� ���̴�
            float4 frag(v2f i) : SV_Target
            {
                // �⺻ �ؽ�ó ���ø�
                float4 mainColor = tex2D(_MainTex, i.uv_MainTex);

                // ������ �ؽ�ó ���ø�
                float noiseValue = tex2D(_NoiseTex, i.uv_NoiseTex).r;

                // Dissolve �� ���
                float dissolve = smoothstep(_DissolveAmount - 0.1, _DissolveAmount + 0.1, noiseValue);

                // �����ڸ� ȿ�� ���
                float edgeEffect = smoothstep(_DissolveAmount, _DissolveAmount + 0.05, noiseValue);
                float3 edgeColor = edgeEffect * _EdgeColor.rgb;

                // ���� �� Ŭ����
                float alpha = dissolve;
                if (alpha  < 0.1) discard; // Ŭ����

                // ���� ����
                float4 finalColor = float4(mainColor.rgb + edgeColor, dissolve);
                return finalColor;
            }
            ENDHLSL
        }
    }
}
