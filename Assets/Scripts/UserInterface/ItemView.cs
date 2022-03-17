
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using static DataItem;
using System;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [field: SerializeField]
    private Outline Outline { get; set; }
    [field: SerializeField]
    private CanvasGroup CanvasGroup { get; set; }
    [field: SerializeField]
    private float Speed { get; set; }
    [field: SerializeField]
    private bool Isloading { get; set; }
    [field: SerializeField]
    private RectTransform LoadingImage { get; set; }
    [field: SerializeField]
    private Image CategoryImage { get; set; }
    [field: SerializeField]
    private TextMeshProUGUI NumberText { get; set; }
    [field: SerializeField]
    private TextMeshProUGUI DescriptionText { get; set; }
    [field: SerializeField]
    private CategoryImageDictionary CategoryImagePair { get; set; }

    private void Update()
    {
        if(Isloading == true)
        {
            LoadingImage.transform.Rotate(new Vector3(0, 0, Speed * Time.deltaTime));
        }
    }

    public void SetLoadingMode(bool isloadingMode)
    {
        Isloading = isloadingMode;
        LoadingImage.gameObject.SetActive(isloadingMode);
        CategoryImage.gameObject.SetActive(!isloadingMode);
        NumberText.gameObject.SetActive(!isloadingMode);
        DescriptionText.gameObject.SetActive(!isloadingMode);
    }

    public void BindData(DataItem dataItem, int index)
    {
        CanvasGroup.alpha = 1f;
        CategoryImage.sprite = CategoryImagePair[dataItem.Category];
        NumberText.text = index.ToString();
        DescriptionText.text = dataItem.Description;

        if(dataItem.Special)
        {
            Outline.enabled = true;
        }
        else
        {
            Outline.enabled = false;
        }
    }

    public void Hide()
    {
        CanvasGroup.alpha = 0f;
    }
}


[Serializable]
public class CategoryImageDictionary : SerializableDictionaryBase<CategoryType, Sprite>
{

}