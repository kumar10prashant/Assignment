using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Video;

public class TimeLineManager : MonoBehaviour
{

    //Year Wise Info Data is a scriptble object which help you to create dynamic year info data
    [SerializeField] YearWiseInfo[] yearWiseInfoData;
    [SerializeField] Transform ContentParent;


    public GameObject Knot;
    public Transform Parent;
    public Transform TimeLineParent;
    [SerializeField] Button videoPlayBackBtn;
    [SerializeField] VideoPlayer VideoPlayer;
    [SerializeField] VideoClip idleVideoClip;
    public RectTransform rootRect;

    //Storing Last selected year knot imge so we can change color when we switch to antoher year in timeline
    Image LastSelectedImage;

    void Start()
    {
        StartCoroutine(Generate());
        VideoPlayer.loopPointReached += ResetVideoPlayer;
    
    }

    
    //This method generate time line 
    IEnumerator Generate()
    {
        int dotCounter = 0;
        for (int i = 0; i < yearWiseInfoData.Length; i++)
        {
            Transform dot = TimeLineParent.GetChild(dotCounter);
            dot.gameObject.SetActive(true);
            dot.GetComponent<Image>().enabled = true;
            dot.GetChild(0).gameObject.SetActive(true);
            dot.GetChild(0).GetComponent<TMP_Text>().text = yearWiseInfoData[i].Year.ToString();
            int index = i;
            dot.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(TimeLineButtonCallBack(dot, index));
            });
           
            dotCounter++;
           

            if (yearWiseInfoData.Length - 1 > i)
            {
                int EmptyDot = yearWiseInfoData[i + 1].Year - yearWiseInfoData[i].Year;
                for (int k = 0; k < EmptyDot - 1; k++)
                {
                    TimeLineParent.GetChild(dotCounter).gameObject.SetActive(true);
                    dotCounter++;
                    yield return null;
                }

            }
            yield return null;
        }
    }


    //This method trigger in event when you select any year in time line
    IEnumerator TimeLineButtonCallBack(Transform dot,int index)
    {
        videoPlayBackBtn.gameObject.SetActive (true);
        videoPlayBackBtn.onClick.RemoveAllListeners();
        yield return null;
        videoPlayBackBtn.onClick.AddListener(() =>
        {
            if (yearWiseInfoData[index].clip != null)
            {
                VideoPlayer.clip = yearWiseInfoData[index].clip;
                VideoPlayer.Play();
                
            }
        });
        if (LastSelectedImage != null)
        {
            LastSelectedImage.color = Color.white;
        }
        dot.GetComponent<Image>().color = Color.red;
        LastSelectedImage = dot.GetComponent<Image>();
        
        foreach (var contentChild in ContentParent.GetComponentsInChildren<TextMeshProUGUI>())
        {
            contentChild.transform.parent.gameObject.SetActive(false);
            yield return null;
        }

        for (int j = 0; j < yearWiseInfoData[index].creator.Length; j++)
        {
            Transform contentParent = ContentParent.transform.GetChild(j);
            contentParent.gameObject.SetActive(true);

            contentParent.GetChild(0).GetComponent<TMP_Text>().text = yearWiseInfoData[index].creator[j].Header;
            contentParent.GetChild(1).GetComponent<TMP_Text>().text = yearWiseInfoData[index].creator[j].Description;
           

        }
      
        //I am using this to refresh the ui layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(rootRect);

        rootRect.GetComponent<VerticalLayoutGroup>().enabled = false;
        yield return new WaitForEndOfFrame();
        rootRect.GetComponent<VerticalLayoutGroup>().enabled = true;

        yield return null;
    }
  

    public void ResetVideoPlayer(UnityEngine.Video.VideoPlayer vp)
    {
            VideoPlayer.clip = idleVideoClip;
    }
}
