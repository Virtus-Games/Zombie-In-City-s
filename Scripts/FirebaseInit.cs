using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FirebaseInit : MonoBehaviour
{
     void Start()
     {
          FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
          {
               FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
          });
     }
}
