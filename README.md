Unity Version I used 6000.0.43f1

# 📅 Real-Time Scrollable Timeline Generation in Unity

This guide explains how to dynamically generate a scrollable timeline in Unity using pre-created UI knots and ScriptableObjects. This system allows you to visualize chronological data such as historical events, video archives, or product evolution.

---

## 🔄 Timeline Generation Logic

The code snippet below dynamically activates timeline dots ("knots") based on year-wise data and handles gaps between non-consecutive years by enabling intermediary dots without labels or images.

```csharp
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
```

### 🧠 Explanation:

* The loop iterates through the `yearWiseInfoData` array.
* Each relevant timeline dot is activated and labeled with the corresponding year.
* Buttons are assigned listeners that trigger content and video playback.
* If there's a year gap, intermediate dots are activated (without labels/images) to maintain consistent spacing.

---

## 🎬 Year Selection Callback

This coroutine runs when a timeline dot is clicked. It shows detailed content and plays an associated video clip.

```csharp
IEnumerator TimeLineButtonCallBack(Transform dot, int index)
{
    videoPlayBackBtn.gameObject.SetActive(true);
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

    LayoutRebuilder.ForceRebuildLayoutImmediate(rootRect);

    rootRect.GetComponent<VerticalLayoutGroup>().enabled = false;
    yield return new WaitForEndOfFrame();
    rootRect.GetComponent<VerticalLayoutGroup>().enabled = true;

    yield return null;
}
```

### ✅ Function Highlights:

* Video clip playback for the selected year.
* UI content refresh with headers and descriptions.
* Visual highlighting of the selected timeline dot.

---

## 🧾 ScriptableObject: YearWiseInfo

A `ScriptableObject` is used to store the timeline's year-based data. This decouples data from the logic and simplifies updates via the Unity Inspector.

```csharp
[CreateAssetMenu(menuName = "CreateNewYearData")]
public class YearWiseInfo : ScriptableObject
{
    public int Year;
    public VideoClip clip;

    [SerializeField]
    public DataInfoCreator[] creator;

    [System.Serializable]
    public class DataInfoCreator
    {
        public string Header;
        public string Description;
    }
}
```

### 📌 Usage:

1. Create instances of `YearWiseInfo` for each year.
2. Assign a video clip and content entries.
3. Load these into a runtime array to drive the timeline generation.

---

## 📦 Requirements

* Unity Editor
* TextMeshPro
* Unity UI system (Canvas, Layout Groups, etc.)
* VideoPlayer component

---

## 📌 Summary

* Efficient real-time timeline generation
* Easy data configuration using ScriptableObjects
* Smooth coroutine-based layout updates
* Ideal for interactive timelines, historical data viewers, and educational tools
