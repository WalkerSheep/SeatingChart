
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    private TextMeshProUGUI textBox;

    public float waveSpeed = 5;
    public float waveSpeedMultiplier = 1;
    public float waveHeight = 5;
    public float waveHeightMultiplier = 1;
    public float waveSpacing = 0.5f;
    public float waveSpacingMultiplier = 1;

    public float lerpSpeed = 10f;

    private float currentSpeedMultiplier;
    private float currentHeightMultiplier;
    private float currentSpacingMultiplier;

    private float waveTime = 0;

    void Awake()
    {
        textBox = GetComponent<TextMeshProUGUI>();

        currentSpeedMultiplier = waveSpeedMultiplier;
        currentHeightMultiplier = waveHeightMultiplier;
        currentSpacingMultiplier = waveSpacingMultiplier;
    }

    void Update()
    {
        currentSpeedMultiplier = Mathf.Lerp(currentSpeedMultiplier, waveSpeedMultiplier, lerpSpeed * Time.deltaTime);
        currentHeightMultiplier = Mathf.Lerp(currentHeightMultiplier, waveHeightMultiplier, lerpSpeed * Time.deltaTime);
        currentSpacingMultiplier = Mathf.Lerp(currentSpacingMultiplier, waveSpacingMultiplier, lerpSpeed * Time.deltaTime);

        float speed = waveSpeed * currentSpeedMultiplier;
        float height = waveHeight * currentHeightMultiplier;
        float spacing = waveSpacing * currentSpacingMultiplier;

        waveTime += Time.deltaTime * speed;

        textBox.ForceMeshUpdate();

        TMP_TextInfo textInfo = textBox.textInfo;

        for(int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo character = textInfo.characterInfo[i];

            if(character.isVisible)
            {
                int vertexIndex = character.vertexIndex;
                int materialIndex = character.materialReferenceIndex;

                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                float wave = Mathf.Sin(waveTime + i * spacing) * height;

                for(int j = 0; j < 4; j++)
                {
                    vertices[vertexIndex + j].y += wave;
                }
            }
        }

        textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
