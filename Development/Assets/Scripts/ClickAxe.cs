using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickAxe : MonoBehaviour
{
    public Button AxeImage;
    public static bool clicked;

    void Start()
    {
        Button btn = AxeImage.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        clicked = true;
        Debug.Log("You have clicked the button!");
    }
}