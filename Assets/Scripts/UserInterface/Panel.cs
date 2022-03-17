using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [field: SerializeField]
    private Button ButtonPrevious { get; set; }
    [field: SerializeField]
    private Button ButtonNext { get; set; }
    [field: SerializeField]
    private RectTransform Content { get; set; }
    [field: SerializeField]
    private GameObject ViewPrefab { get; set; }
    [field: SerializeField]
    private int NumberOfViews { get; set; }

    private List<ItemView> ItemViewCollection { get; set; }
    private IDataServer DataServer { get; set; }
    private List<DataItem> DataItems { get; set; }
    private int LoadedDataCounter { get; set; }
    private int TotalDataCounter { get; set; }

    private async void Start()
    {
        CreateViews();
        SetLoadingMode(true);

        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;
        DataServer = new DataServerMock();

        TotalDataCounter = await DataServer.DataAvailable(token);
        DataItems = (List<DataItem>) await DataServer.RequestData(0, TotalDataCounter, token);
        source.Dispose();
        LoadedDataCounter = NumberOfViews;

        SetLoadingMode(false);
        BindItemView();
    }

    private void CreateViews()
    {
        ItemViewCollection = new List<ItemView>();

        for (int i=0; i<NumberOfViews; i++)
        {
            ItemView itemView = Instantiate(ViewPrefab, Content).GetComponent<ItemView>();
            ItemViewCollection.Add(itemView);
        }
    }

    private void SetLoadingMode(bool isloadingMode)
    {
        for (int i = 0; i < ItemViewCollection.Count; i++)
        {
            ItemViewCollection[i].SetLoadingMode(isloadingMode);
        }

        if (isloadingMode == true)
        {
            ButtonPrevious.interactable = false;
            ButtonNext.interactable = false;
        }
        else
        {
            ButtonPrevious.interactable = true;
            ButtonNext.interactable = true;
        }
    }

    private void BindItemView()
    {
        List<DataItem> DataToDisplay;

        if (LoadedDataCounter > TotalDataCounter)
        {
            DataToDisplay = DataItems.GetRange(LoadedDataCounter - NumberOfViews, TotalDataCounter - (LoadedDataCounter - NumberOfViews));
        }
        else
        {
            DataToDisplay = DataItems.GetRange(LoadedDataCounter - NumberOfViews, NumberOfViews);
        }


        for (int i = 0; i < ItemViewCollection.Count; i++)
        {
            if (i < DataToDisplay.Count)
            {
                ItemViewCollection[i].BindData(DataToDisplay[i], (LoadedDataCounter - NumberOfViews) + i + 1);
            }
            else
            {
                ItemViewCollection[i].Hide();
            }
        }

        SetInteractableButtons();
    }

    private void SetInteractableButtons()
    {
        if(LoadedDataCounter >= TotalDataCounter)
        {
            ButtonNext.interactable = false;
        }
        else
        {
            ButtonNext.interactable = true;
        }

        if(LoadedDataCounter <= NumberOfViews)
        {
            ButtonPrevious.interactable = false;
        }
        else
        {
            ButtonPrevious.interactable = true;
        }
    }

    public void OnClickButtonPrevious()
    {
        LoadedDataCounter -= NumberOfViews;
        BindItemView();
    }

    public void OnClickButtonNext()
    {
        LoadedDataCounter += NumberOfViews;
        BindItemView();
    }
}
