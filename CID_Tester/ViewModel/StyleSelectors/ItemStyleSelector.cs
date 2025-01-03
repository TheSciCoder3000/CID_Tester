using AvalonDock.Controls;
using System.Windows;
using System.Windows.Controls;

namespace CID_Tester.ViewModel.StyleSelectors;

public class ItemStyleSelector : StyleSelector
{
    public Style DocumentStyle { get; set; }
    public Style AnchorableStyle { get; set; }

    public override Style SelectStyle(object item, DependencyObject container)
    {

        if (container is LayoutAnchorableItem)
            return AnchorableStyle;

        if (container is LayoutDocumentItem)
            return DocumentStyle;

        return base.SelectStyle(item, container);
    }
}
