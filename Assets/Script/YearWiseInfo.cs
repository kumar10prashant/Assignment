using UnityEngine;
using UnityEngine.Video;


[CreateAssetMenu(menuName ="CreateNewYearData")]
public class YearWiseInfo : ScriptableObject
{
    public int Year;
    public VideoClip clip;

    [SerializeField]public DataInfoCreator[] creator;



    [System.Serializable]
    public class DataInfoCreator
    {
        public string Header;
        public string Description;
      
    }
}
