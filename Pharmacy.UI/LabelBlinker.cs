using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class LabelBlinker
{
    public static IPagedList<LowStockNotiVM> LowStockNotis;
    public static List<LowStockNotiVM> LowStockNotisList;
    public static IPagedList<ExpiryNotiVM> ExpiryNotis;
    public static List<ExpiryNotiVM> ExpiryNotisList;
    public static UnitOfWork unitOfWork;
    private static async Task<string[]> GenerateExpNotificationAsync()
    {
        int i = 0;
        int ExpiryPeriodCount = SharedVariables.AdminPharmacySetting.ExpiryPeriod;


        DateTime ExpNotiPeriod = DateTime.Now;
        if (SharedVariables.AdminPharmacySetting.ExpiryPeriodUnit == (int)SharedVariables.ExpiryPeriodUnit.Days)
        {
            ExpNotiPeriod = DateTime.Now.AddDays(ExpiryPeriodCount);
        }
        else
        {
            ExpNotiPeriod = DateTime.Now.AddMonths(ExpiryPeriodCount);
        }

        using (unitOfWork = new UnitOfWork())
        {
            ExpiryNotisList = await Task.Run(() => unitOfWork.ItemRspository.GetExpiryNotisList(ExpNotiPeriod));
            LowStockNotisList = unitOfWork.ItemRspository.GetLowStockNotisList() ;

        }
        double expiryDate = await ProductActivation.GetExpiryDateAsync();
        string[] systemexpirynotic = { "Your System will Expire in " + expiryDate + " days." };
        string[] notifications = new string[ExpiryNotisList.Count];
        string[] LowStockNotif = new string[LowStockNotisList.Count];

        for (int j = 0; j < ExpiryNotisList.Count; j++)
        {
            if (expiryDate < 10)
            {
                notifications[j] = "Batch No# '" + ExpiryNotisList[j].BatchNo + "' of Item '" + ExpiryNotisList[j].ItemName + "' will Expire on '" + ExpiryNotisList[j].BatchExp.ToShortDateString() + "'.";
            }
        }
        for (int j = 0; j < LowStockNotisList.Count; j++)
        {
            if (LowStockNotisList[j].ReorderingLevel > LowStockNotisList[j].AvailableStock && !(expiryDate < 10))
            {
                LowStockNotif[j] = "Item '" + LowStockNotisList[j].ItemName + "' is going to out of stock soon. Remaining Stock is:'"+ (LowStockNotisList[j].AvailableStock > 1 ? LowStockNotisList[j].AvailableStock.ToString() +"' Units." : LowStockNotisList[j].AvailableStock.ToString() + "' Unit.");
            }
        }
        string[] mergedArray = LabelBlinker.MergeArrays(notifications, LowStockNotif);
        mergedArray = LabelBlinker.MergeArrays(mergedArray, systemexpirynotic);
        return mergedArray;
    }

    public static void SetGradientBackground(Label label, Color startColor, Color endColor)
    {
        if (label.Width == 0 || label.Height == 0)
        {
            return; // Skip setting the gradient background if the label has zero width or height
        }

        LinearGradientBrush brush = new LinearGradientBrush(label.ClientRectangle, startColor, endColor, LinearGradientMode.Vertical);
        Bitmap bitmap = new Bitmap(label.ClientRectangle.Width, label.ClientRectangle.Height);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.FillRectangle(brush, label.ClientRectangle);
        }

        label.BackColor = Color.Transparent;
        label.BackgroundImage = bitmap;
    }


    public static async void TypeStringsAsync(Label label)
    {

        label.Text = "";
        Thread typeThread = new Thread(async () =>
        {
            while (true)
            {
                if(SharedVariables.LoggedInUser == null)
                {
                    continue;
                }
                string[] notifications = await GenerateExpNotificationAsync();
                
                foreach (string str in notifications)
                {
                    if(str == string.Empty)
                    {
                        continue;
                    }
                    label.Invoke(new MethodInvoker(async delegate
                    {
                        //await FadeOut(label);
                        label.Text = str;
                        /*Color startColor = Color.FromArgb(36, 54, 82); // Yellow
                        Color endColor = Color.FromArgb(104, 136, 168); // Cyan

                        SetGradientBackground(label, startColor, endColor);*/
                        //await FadeIn(label);
                    }));

                    

                    Thread.Sleep(2500); // Delay after typing each string (in milliseconds)

                    // Clear the label
                    label.Invoke(new MethodInvoker(delegate
                    {
                        label.Text = string.Empty;
                    }));
                }
            }
        });

        typeThread.IsBackground = true;
        typeThread.Start();
    }

    public static string[] MergeArrays(string[] array1, string[] array2)
    {
        List<string> mergedList = new List<string>();

        if (array1 != null)
        {
            mergedList.AddRange(array1.Where(x => !string.IsNullOrEmpty(x)));
        }

        if (array2 != null)
        {
            mergedList.AddRange(array2.Where(x => !string.IsNullOrEmpty(x)));
        }

        return mergedList.ToArray();
    }

}
