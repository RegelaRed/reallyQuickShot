using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private string _message;

    public void call()
    {
        Debug.Log(_message);
    }
}
