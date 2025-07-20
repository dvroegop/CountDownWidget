using Android;
using Android.Content;
using Android.Widget;
using Android.Appwidget;
using Resource = Android.Resource;

namespace CountDownWidget;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
        
        this.TargetDatePicker.Date = Preferences.Get("targetDate", DateTime.Today.AddDays(1));
		
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        Preferences.Set("targetDate", TargetDatePicker.Date);

        // Force a Widget update
        var ctx = Android.App.Application.Context;
        var mgr = AppWidgetManager.GetInstance(ctx);
        var ids = mgr.GetAppWidgetIds(new ComponentName(ctx, Java.Lang.Class.FromType(typeof(CountDownWidget.CountdownWidgetProvider))));

        mgr.UpdateAppWidget(ids, BuildRemoteViews(ctx));

        DisplayAlert("Saved!", $"Counting down to {TargetDatePicker.Date:d}", "Sweet!");
    }

    internal static Android.Widget.RemoteViews BuildRemoteViews(Context ctx)
    {
        var prefs = Preferences.Default;
        var target = prefs.Get("targetDate", DateTime.Today.AddDays(1));
        var now = DateTime.Now;

        var diff = target - now.Date;

        string text;
        if (diff.TotalDays > 2)
            text = $"{(int)diff.TotalDays} days left.";
        else
        {
            var minutes = (int)(target - now).TotalMinutes;
            text = $"{minutes} minutes left.";
        }

        var rv = new RemoteViews(ctx.PackageName, Android.Resource.Layout.widget_countdown);
        rv.SetTextViewText(Android.Resource.Id.CountDown, text);

        return rv;
    }
}
