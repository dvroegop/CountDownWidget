using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using CountDownWidget;
using Java.Lang;

namespace CountDownWidget;

[BroadcastReceiver(Label = "Countdown Widget", Exported = true)]
[IntentFilter(new[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
[MetaData("android.appwidget.provider", Resource = "@xml/countdown_widget_info")]
public class CountdownWidgetProvider : AppWidgetProvider
{
    public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
    {
        foreach (var widgetId in appWidgetIds)
        {
            var views = MainPage.BuildRemoteViews(context);
            appWidgetManager.UpdateAppWidget(widgetId, views);
        }

        ScheduleNextUpdate(context);
    }

    void ScheduleNextUpdate(Context context)
    {
        // Figure out when to fire: once a day if >48h left, otherwise every minute
        var prefs = Preferences.Default;
        var target = prefs.Get("targetDate", DateTime.Today).Date;
        var now = DateTime.Now;
        var diff = target - now;

        long interval;
        if (diff.TotalHours > 48)
            interval = AlarmManager.IntervalDay; // daily
        else
            interval = 60 * 1000;               // 1 minute

        var intent = new Intent(context, Class.FromType(typeof(CountdownWidgetProvider)));
        intent.SetAction("android.appwidget.action.APPWIDGET_UPDATE");
        var pi = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);

        var am = context.GetSystemService(Context.AlarmService) as AlarmManager;
        var triggerAt = JavaSystem.CurrentTimeMillis() + interval;
        am.Set(AlarmType.Rtc, triggerAt, pi);
    }
}