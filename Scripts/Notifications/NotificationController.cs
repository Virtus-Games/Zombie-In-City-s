using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using System;

public class NotificationController : MonoBehaviour
{
    public AndroidNotificationChannel defaultNotificationCenter;

    private int identifier;
    void Start()
    {
        defaultNotificationCenter = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default Channel",
            Description = "For generic notifications",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationCenter);

        DateTime delivery_time = new DateTime(2022, 5, 28, 9, 0, 0);
        notificationPerThirtySixHours(delivery_time);
    }

    void notificationPerThirtySixHours(DateTime delivery_time)
    {

        if (delivery_time < DateTime.Now)
        {
            delivery_time = delivery_time.AddHours(36);     // if default delivery time is past this logic adds 36 hours until delivery time is become future
            notificationPerThirtySixHours(delivery_time);
        }

        AndroidNotification notification = new AndroidNotification()
        {
            Title = "City needs your help!",
            Text = "Come and vanish the Zombies!!!",
            SmallIcon = "icon_0",
            LargeIcon = "default",
            FireTime = System.DateTime.Now.AddHours(36),
        };

        // delivery_time = delivery_time.AddHours(36); // when notification sent the next notification time can be scheduled like this.

        identifier = AndroidNotificationCenter.SendNotification(notification, "default_channel");

        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler = delegate (AndroidNotificationIntentData data)
        {
            var msg = "Bildirim Alındı: " + data.Id + "\n";
            msg += "\nBildirim Alındı: ";
            msg += "\n .Başlık: " + data.Notification.Title;
            msg += "\n .Gövde: " + data.Notification.Text;
            msg += "\n .Kanal: " + data.Channel;
            // delivery_time = delivery_time.AddHours(36); //! not worked!
        };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

        if (notificationIntentData != null)
        {
            Debug.Log("Uygulama Bildirim ile açıldı");
        }
    }
}
