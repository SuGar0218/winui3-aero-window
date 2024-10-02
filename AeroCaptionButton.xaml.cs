using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AeroLike;

public sealed partial class AeroCaptionButton : UserControl {
    public AeroCaptionButton() {
        InitializeComponent();
    }

    public event RoutedEventHandler Click;

    public string ButtonImage {
        get => (string)GetValue(ButtonImageProperty);
        set => SetValue(ButtonImageProperty, value);
    }

    public string UnfocusedImage {
        get => (string)GetValue(UnfocusedImageProperty);
        set => SetValue(UnfocusedImageProperty, value);
    }

    public string HoverImage {
        get => (string)GetValue(HoverImageProperty);
        set => SetValue(HoverImageProperty, value);
    }

    public string PressedImage {
        get => (string)GetValue(PressedImageProperty);
        set => SetValue(PressedImageProperty, value);
    }

    public string ContentImage {
        get => (string)GetValue(ContentImageProperty);
        set => SetValue(ContentImageProperty, value);
    }

    public string LuminosityImage {
        get => (string)GetValue(LuminosityImageProperty);
        set => SetValue(LuminosityImageProperty, value);
    }

    public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
        nameof(Click),
        typeof(RoutedEventHandler),
        typeof(AeroCaptionButton),
        new PropertyMetadata(default(RoutedEventHandler))
    );

    public static readonly DependencyProperty ButtonImageProperty = DependencyProperty.Register(
        nameof(ButtonImage),
        typeof(string),
        typeof(AeroCaptionButton),
        new PropertyMetadata(default(string),
            static (DependencyObject obj, DependencyPropertyChangedEventArgs e) => {
                var self = obj as AeroCaptionButton;
                self.origin.Source = new BitmapImage(new Uri(self.BaseUri, e.NewValue as string));
            }
        )
    );

    public static readonly DependencyProperty UnfocusedImageProperty = DependencyProperty.Register(
        nameof(UnfocusedImage),
        typeof(string),
        typeof(AeroCaptionButton),
        new PropertyMetadata(default(string),
            static (DependencyObject obj, DependencyPropertyChangedEventArgs e) => {
                var self = obj as AeroCaptionButton;
                self.unfocused.Source = new BitmapImage(new Uri(self.BaseUri, e.NewValue as string));
            }
        )
    );

    public static readonly DependencyProperty HoverImageProperty = DependencyProperty.Register(
        nameof(HoverImage),
        typeof(string),
        typeof(AeroCaptionButton),
        new PropertyMetadata(default(string),
            static (DependencyObject obj, DependencyPropertyChangedEventArgs e) => {
                var self = obj as AeroCaptionButton;
                self.hover.Source = new BitmapImage(new Uri(self.BaseUri, e.NewValue as string));
            }
        )
    );

    public static readonly DependencyProperty PressedImageProperty = DependencyProperty.Register(
        nameof(PressedImage),
        typeof(string),
        typeof(AeroCaptionButton),
        new PropertyMetadata(default(string),
            static (DependencyObject obj, DependencyPropertyChangedEventArgs e) => {
                var self = obj as AeroCaptionButton;
                self.pressed.Source = new BitmapImage(new Uri(self.BaseUri, e.NewValue as string));
            }
        )
    );

    public static readonly DependencyProperty ContentImageProperty = DependencyProperty.Register(
        nameof(ContentImage),
        typeof(string),
        typeof(AeroCaptionButton),
        new PropertyMetadata(default(string),
            static (DependencyObject obj, DependencyPropertyChangedEventArgs e) => {
                var self = obj as AeroCaptionButton;
                self.content.Source = new BitmapImage(new Uri(self.BaseUri, e.NewValue as string));
            }
        )
    );

    public static readonly DependencyProperty LuminosityImageProperty = DependencyProperty.Register(
        nameof(LuminosityImage),
        typeof(string),
        typeof(AeroCaptionButton),
        new PropertyMetadata(default(string),
            static (DependencyObject obj, DependencyPropertyChangedEventArgs e) => {
                var self = obj as AeroCaptionButton;
                self.luminosity.Source = new BitmapImage(new Uri(self.BaseUri, e.NewValue as string));
            }
        )
    );

    public void OnPointerEntered(object sender, PointerRoutedEventArgs e) {
        hover.Visibility = Visibility.Visible;
        luminosity.Visibility = Visibility.Visible;
    }

    public void OnPointerPressed(object sender, PointerRoutedEventArgs e) {
        pressed.Visibility = Visibility.Visible;
    }

    public void OnPointerReleased(object sender, PointerRoutedEventArgs e) {
        pressed.Visibility = Visibility.Collapsed;
        Click?.Invoke(this, e);
    }

    public void OnPointerExited(object sender, PointerRoutedEventArgs e) {
        hover.Visibility = Visibility.Collapsed;
        pressed.Visibility = Visibility.Collapsed;
        luminosity.Visibility = Visibility.Collapsed;
    }

    public void OnPointerCanceled(object sender, PointerRoutedEventArgs e) {
        hover.Visibility = Visibility.Collapsed;
        pressed.Visibility = Visibility.Collapsed;
    }

    public void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e) {
        hover.Visibility = Visibility.Collapsed;
        pressed.Visibility = Visibility.Collapsed;
    }

    public void LoseFocus() {
        origin.Visibility = Visibility.Collapsed;
        unfocused.Visibility = Visibility.Visible;
    }

    public void GetFocus() {
        unfocused.Visibility = Visibility.Collapsed;
        origin.Visibility = Visibility.Visible;
    }
}
