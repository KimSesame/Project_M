Shader "Custom/DissolveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0
        _EdgeColor ("Edge Color", Color) = (0.1, 0.1, 0.1, 1)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade // �׸��� �ɼ� �߰�

        sampler2D  _MainTex;
        sampler2D  _NoiseTex;
        float _DissolveAmount;
        fixed4 _EdgeColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NoiseTex;
        };

        void surf (Input IN, inout SurfaceOutput  o)
        {
             // �⺻ �ؽ�ó ����
            fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = mainColor.rgb;

            // ������ �ؽ�ó ���� ���� Dissolve ȿ�� ����
            float noiseValue = tex2D(_NoiseTex, IN.uv_NoiseTex).r;
            float dissolve = smoothstep(_DissolveAmount - 0.1, _DissolveAmount + 0.1, noiseValue);

            // �����ڸ� ���� ȿ�� �߰�
            float edgeEffect = smoothstep(_DissolveAmount, _DissolveAmount + 0.05, noiseValue);
            o.Emission = edgeEffect * _EdgeColor.rgb;

            // Alpha �� ���� (�Ҹ� ������ ���� Alpha ����)
            o.Alpha = dissolve;
            clip(dissolve - 0.1); // �׸��ڵ� �Ҹ�� �Բ� Ŭ���εǵ��� ����
        }
        ENDCG
    }
    FallBack "Diffuse"
}
