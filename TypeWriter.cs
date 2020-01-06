using System.Collections;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    public static TypeWriter Instance = null;

    [SerializeField]
    private float typeSpeed = 0.05f;
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
    }
    
    //Call this function to write the text into a given (TextMeshPro UI) textfield
    public void Type(TMPro.TextMeshProUGUI textField, string wordToType)
    {
        StartCoroutine(TypeToText(textField, wordToType));
    }

    IEnumerator TypeToText(TMPro.TextMeshProUGUI textField, string wordToType)
    {
        textField.text = "";

        foreach(char letter in wordToType.ToCharArray())
        {
            textField.text += letter.ToString();
            yield return new WaitForSeconds(typeSpeed);
        }
        textField.text = wordToType;
    }
}
