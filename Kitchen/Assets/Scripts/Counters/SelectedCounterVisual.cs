using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounterClass baseCounterClass;
    [SerializeField] private GameObject[] visualGameObjectArray;
    
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_onSelectedCounterChanged;
    }

    private void Player_onSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounterClass)
        {
            Show();   
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
        
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
