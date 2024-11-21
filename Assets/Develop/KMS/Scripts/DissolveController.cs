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

    private void Start()
    {
        // ���� ���͸��� ����
        originalMaterial = GetComponent<Renderer>().material;

        // Dissolve ���͸��� ����
        dissolveMaterial = new Material(dissolveShader);

        // ���� ���͸����� �ؽ�ó�� ���� ����
        dissolveMaterial.mainTexture = originalMaterial.mainTexture;
        dissolveMaterial.SetColor("_Color", originalMaterial.color);
        dissolveMaterial.SetTexture("_NoiseTex", noiseTexture);

        // �Ҹ����
        StartDestruction();
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

        float dissolveAmount = 0;

        // ������ �Ҹ�.
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
