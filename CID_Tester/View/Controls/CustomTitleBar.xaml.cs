using System.Windows;
using System.Windows.Controls;

namespace CID_Tester.View.Controls;

public partial class CustomTitleBar : UserControl
{
    public CustomTitleBar()
    {
        InitializeComponent();
    }

    public static readonly RoutedEvent MinimizeClickEvent = EventManager.RegisterRoutedEvent
               ("MinimizeClick", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(CustomTitleBar));

    public event RoutedEventHandler MinimizeClick
    {
        add { this.AddHandler(MinimizeClickEvent, value); }
        remove { this.RemoveHandler(MinimizeClickEvent, value); }
    }

    public static readonly RoutedEvent MaximizeClickEvent = EventManager.RegisterRoutedEvent
               ("MaximizeClick", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(CustomTitleBar));

    public event RoutedEventHandler MaximizeClick
    {
        add { this.AddHandler(MaximizeClickEvent, value); }
        remove { this.RemoveHandler(MaximizeClickEvent, value); }
    }

    public static readonly RoutedEvent CloseClickEvent = EventManager.RegisterRoutedEvent
               ("CloseClick", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(CustomTitleBar));

    public event RoutedEventHandler CloseClick
    {
        add { this.AddHandler(CloseClickEvent, value); }
        remove { this.RemoveHandler(CloseClickEvent, value); }
    }

    private void btnMin_Click(object sender, RoutedEventArgs e) => this.RaiseEvent(new RoutedEventArgs(MinimizeClickEvent));
    private void btnMax_Click(object sender, RoutedEventArgs e) => this.RaiseEvent(new RoutedEventArgs(MaximizeClickEvent));
    private void btnClose_Click(object sender, RoutedEventArgs e) => this.RaiseEvent(new RoutedEventArgs(CloseClickEvent));

}
