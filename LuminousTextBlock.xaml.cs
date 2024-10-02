using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AeroLike;

public sealed partial class LuminousTextBlock : UserControl {
    public LuminousTextBlock() {
        InitializeComponent();
    }

    public string Text {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(LuminousTextBlock),
        new PropertyMetadata(default(string),
            static (DependencyObject obj, DependencyPropertyChangedEventArgs args) => {
                (obj as LuminousTextBlock).textBlock.Text = args.NewValue as string;
            }
        )
    );

    public new object Content {
        get => (object)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly new DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content),
        typeof(object),
        typeof(LuminousTextBlock),
        new PropertyMetadata(default(object),
            static (DependencyObject obj, DependencyPropertyChangedEventArgs args) => {
                (obj as LuminousTextBlock).Text = args.NewValue as string;
            }
        )
    );
}
