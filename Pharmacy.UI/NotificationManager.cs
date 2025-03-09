using System;
using System.Threading;
using System.Windows.Forms;

public class NotificationManager
{
    private Label label;
    private string[] notifications;
    private int currentIndex;
    private bool isRunning;
    private Thread notificationThread;

    public NotificationManager(Label label, string[] notifications)
    {
        this.label = label;
        this.notifications = notifications;
        this.currentIndex = 0;
        this.isRunning = false;
    }

    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            notificationThread = new Thread(RunNotifications);
            notificationThread.Start();
        }
    }

    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            notificationThread.Join();
        }
    }

    private void RunNotifications()
    {
        while (isRunning)
        {
            string notification = notifications[currentIndex];
            currentIndex = (currentIndex + 1) % notifications.Length;

            // Clear the label text
            label.Invoke((MethodInvoker)delegate { label.Text = ""; });

            // Blink and continuously type the notification
            for (int i = 0; i < notification.Length; i++)
            {
                if (!isRunning)
                    return;

                char currentChar = notification[i];

                label.Invoke((MethodInvoker)delegate
                {
                    label.Text += currentChar;
                });

                Thread.Sleep(200); // Delay between typing each character
            }

            Thread.Sleep(1000); // Delay before clearing the label for the next notification
        }
    }
}
