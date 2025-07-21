using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using CountDownWidget;
using Java.Lang;

namespace CountDownWidget;

[BroadcastReceiver(Label = "Countdown Widget", Exported = true)]
[IntentFilter(new[]
{
    "android.appwidget.action.APPWIDGET_UPDATE",
    "android.intent.action.BOOT_COMPLETED",
    "android.intent.action.MY_PACKAGE_REPLACED"
})]
[MetaData("android.appwidget.provider", Resource = "@xml/countdown_widget_info")]
public class CountdownWidgetProvider : AppWidgetProvider
{
    public override void OnUpdate(Context? context, AppWidgetManager? appWidgetManager, int[]? appWidgetIds)
    {
        if (context == null)
            return;
        
        if (appWidgetIds != null)
            foreach (var widgetId in appWidgetIds)
            {
                var views = MainPage.BuildRemoteViews(context);
                appWidgetManager?.UpdateAppWidget(widgetId, views);
            }

        ScheduleNextUpdate(context!);
    }


    public override void OnReceive(Context? context, Intent? intent)
    {
        base.OnReceive(context, intent);


        if (context == null || intent == null)
            return;
        
        var action = intent.Action;
        if (string.IsNullOrWhiteSpace(action))
            return;

        if (action == AppWidgetManager.ActionAppwidgetUpdate ||
            action == Intent.ActionBootCompleted ||
            action == Intent.ActionMyPackageReplaced)
        {
            var mgr = AppWidgetManager.GetInstance(context);

            var ids = mgr?.GetAppWidgetIds(new ComponentName(context, Class.FromType(typeof(CountdownWidgetProvider))));
            if (ids == null)
                return;
            
            // Force a widget update
            foreach (var widgetId in ids)
            {
                var views = MainPage.BuildRemoteViews(context);
                mgr?.UpdateAppWidget(widgetId,views);
            }

            // And schedule the next update
            ScheduleNextUpdate(context);
        }
    }

    void ScheduleNextUpdate(Context context)
    {
        // Figure out when to fire: every 30 minutes if >48h left, otherwise every minute
        var preferences = Preferences.Default;
        var target = preferences.Get("targetDate", DateTime.Today).Date;
        var now = DateTime.Now;
        var diff = target - now;

        long interval;
        if (diff.TotalHours > 48)
            interval = 30 * 60 * 1000; // 30 minutes
        else
            interval = 60 * 1000;      // 1 minute

        var intent = new Intent(context, Class.FromType(typeof(CountdownWidgetProvider)));
        intent.SetAction("android.appwidget.action.APPWIDGET_UPDATE");
        var pi = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

        if (pi == null)
            throw new NullPointerException("PendingIntent is null");
        
        var am = context.GetSystemService(Context.AlarmService) as AlarmManager;
        
        if(am == null)
            throw new NullPointerException("AlarmManager is null");
        
        var triggerAt = JavaSystem.CurrentTimeMillis() + interval;
        am.Set(AlarmType.Rtc, triggerAt, pi);
    }
}