using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    private Material originalMaterial;      // ���� ���͸���
    private Material dissolveMaterial;      // Dissolve ȿ���� ����� ���͸���
    public Shader dissolveShader;           // Dissolve ȿ���� ���� ���̴�
    public Texture noiseTexture;            // Dissolve ȿ���� ���� ������ �ؽ�ó
    public float dissolveSpeed = 0.5f;      // ������Ʈ�� �Ҹ�Ǵ� �ӵ� ���� ����
    public float delayTime = 1.5f;          // ������ Ÿ�� ���� �� �Ҹ� ����

    private Color initialEdgeColor = new Color(0.1f, 0.1f, 0.1f, 1f); // �ʱ� ����
    private Color finalEdgeColor = new Color(1f, 1f, 1f, 1f);

    private void Start()
    {
        // ���� ���͸��� ����
        originalMaterial = GetComponent<Renderer>().material;

        // Dissolve ���͸��� ����
        dissolveMaterial = new Material(dissolveShader);

        // Dissolve ���͸��� ����
        dissolveMaterial = new Material(dissolveShader)
        {
            mainTexture = originalMaterial.mainTexture
        };
        dissolveMaterial.SetColor("_Color", originalMaterial.color);
        dissolveMaterial.SetTexture("_NoiseTex", noiseTexture);
        dissolveMaterial.SetColor("_EdgeColor", initialEdgeColor);

        // Dissolve ȿ�� ����
        GetComponent<Renderer>().material = dissolveMaterial;
        StartCoroutine(DissolveEffect());
    }

    /// <summary>
    /// �Ҹ���۽� Dissolve Shader�� �� ���͸��� ����.
    /// </summary>
    public void StartDestruction()
    {
        GetComponent<Renderer>().material = dissolveMaterial;
        StartCoroutine(DissolveEffect());
    }

    /// <summary>
    /// ���������� DissolveAmount ���� �������� ������Ʈ�� ���������� �Ҹ��Ű�� ȿ���� ����
    /// </summary>
    private IEnumerator DissolveEffect()
    {
        yield return new WaitForSeconds(delayTime);

        // EdgeColor ��ȭ ����
        float transitionTime = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;

            // EdgeColor�� ���������� ����
            Color currentEdgeColor = Color.Lerp(initialEdgeColor, finalEdgeColor, elapsedTime / transitionTime);
            dissolveMaterial.SetColor("_EdgeColor", currentEdgeColor);

            yield return null;
        }

        // �Ҹ� ����
        float dissolveAmount = 0f;
        while (dissolveAmount < 0.3f)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }

        // �Ҹ� �Ϸ� �� ������Ʈ ����.
        Destroy(gameObject);
    }
}
