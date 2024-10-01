using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;
using WinRT;

namespace AeroLike;

public sealed partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        Title = "Aero-like Glass";
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(draggable);
        AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;

        backdropTarget = this.As<ICompositionSupportsSystemBackdrop>();
        backdropController = new DesktopAcrylicController {
            LuminosityOpacity = 0.0f,
            TintOpacity = 0.0f,
        };
        
        backdropController.AddSystemBackdropTarget(backdropTarget);
        backdropController.SetSystemBackdropConfiguration(backdropConfig);

        presenter = AppWindow.Presenter as OverlappedPresenter;
        //hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

        lastPosition = AppWindow.Position;
        AppWindow.Changed += delegate (AppWindow sender, AppWindowChangedEventArgs args) {
            if (args.DidPositionChange) {
                double rasterizationScale = aeroSurfaceReflection.XamlRoot.RasterizationScale;
                transformMatrix = new Matrix {
                    M11 = transformMatrix.M11, M12 = 0.0,
                    M21 = 0.0, M22 = transformMatrix.M22,
                    OffsetX = transformMatrix.OffsetX - (AppWindow.Position.X - lastPosition.X) / rasterizationScale,
                    OffsetY = transformMatrix.OffsetY - (AppWindow.Position.Y - lastPosition.Y) / rasterizationScale,
                };
                (aeroSurfaceReflection.RenderTransform as MatrixTransform).Matrix = transformMatrix;
                lastPosition = AppWindow.Position;
            }
            if (args.DidSizeChange) {
                if (presenter?.State == OverlappedPresenterState.Maximized) {
                    maximizeButtonGrid.Visibility = Visibility.Collapsed;
                    restoreButtonGrid.Visibility = Visibility.Visible;
                } else if (presenter?.State == OverlappedPresenterState.Restored) {
                    restoreButtonGrid.Visibility = Visibility.Collapsed;
                    maximizeButtonGrid.Visibility = Visibility.Visible;
                }
            }
        };
    }

    private PointInt32 lastPosition;

    //private readonly IntPtr hwnd;
    private readonly OverlappedPresenter presenter;

    private readonly ISystemBackdropControllerWithTargets backdropController;
    private readonly ICompositionSupportsSystemBackdrop backdropTarget;
    private static readonly SystemBackdropConfiguration backdropConfig = new() { IsInputActive = true };

    private const int aeroSurfaceReflectionWidth = 832;
    private const int aeroSurfaceReflectionHeight = 628;

    private Matrix transformMatrix = new Matrix {
        M11 = (float) ScreenResolution.Width / aeroSurfaceReflectionWidth, M12 = 0.0,
        M21 = 0.0, M22 = (float)ScreenResolution.Height / aeroSurfaceReflectionHeight,
        OffsetX = 0, OffsetY = 0
    };

    private void Dispose() {
        backdropController.Dispose();
    }

    private void FocusChanged(object _, WindowActivatedEventArgs args) {
        if (args.WindowActivationState == WindowActivationState.Deactivated) { // lost focus
            accentColor.Visibility = Visibility.Collapsed;
            //aeroReflection.Visibility = Visibility.Collapsed;
            aeroCornerReflection.Opacity = 0.3;

            minimizeButton.Visibility = Visibility.Collapsed;
            minimizeButtonFocusLost.Visibility = Visibility.Visible;

            maximizeButton.Visibility = Visibility.Collapsed;
            maximizeButtonFocusLost.Visibility = Visibility.Visible;

            closeButton.Visibility = Visibility.Collapsed;
            closeButtonFocusLost.Visibility = Visibility.Visible;
        } else { // engage focus
            accentColor.Visibility = Visibility.Visible;
            //aeroReflection.Visibility = Visibility.Visible;
            aeroCornerReflection.Opacity = 0.5;

            minimizeButton.Visibility = Visibility.Visible;
            minimizeButtonFocusLost.Visibility = Visibility.Collapsed;

            maximizeButton.Visibility = Visibility.Visible;
            maximizeButtonFocusLost.Visibility = Visibility.Collapsed;

            closeButton.Visibility = Visibility.Visible;
            closeButtonFocusLost.Visibility = Visibility.Collapsed;
        }
    }

    private void CloseButtonPointerEntered(object sender, PointerRoutedEventArgs e) {
        closeButtonHover.Visibility = Visibility.Visible;
        closeButtonLight.Visibility = Visibility.Visible;
    }

    private void CloseButtonPointerExited(object sender, PointerRoutedEventArgs e) {
        closeButtonHover.Visibility = Visibility.Collapsed;
        closeButtonLight.Visibility = Visibility.Collapsed;
        closeButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void CloseButtonPointerPressed(object sender, PointerRoutedEventArgs e) {
        closeButtonPressed.Visibility = Visibility.Visible;
    }

    private void ClosePointerReleased(object sender, PointerRoutedEventArgs e) {
        closeButtonPressed.Visibility = Visibility.Collapsed;
        Close();
    }

    private void ClosePointerCanceled(object sender, PointerRoutedEventArgs e) {
        closeButtonHover.Visibility = Visibility.Collapsed;
        closeButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void ClosePointerCaptureLost(object sender, PointerRoutedEventArgs e) {
        closeButtonHover.Visibility = Visibility.Collapsed;
        closeButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void MaximizePointerEntered(object sender, PointerRoutedEventArgs e) {
        maximizeButtonHover.Visibility = Visibility.Visible;
        maximizeButtonLight.Visibility = Visibility.Visible;
    }

    private void MaximizePointerExited(object sender, PointerRoutedEventArgs e) {
        maximizeButtonHover.Visibility = Visibility.Collapsed;
        maximizeButtonLight.Visibility = Visibility.Collapsed;
        maximizeButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void MaximizePointerPressed(object sender, PointerRoutedEventArgs e) {
        maximizeButtonPressed.Visibility = Visibility.Visible;
    }

    private void MaximizePointerReleased(object sender, PointerRoutedEventArgs e) {
        maximizeButtonPressed.Visibility = Visibility.Collapsed;
        presenter.Maximize();
    }

    private void MaximizePointerCanceled(object sender, PointerRoutedEventArgs e) {
        maximizeButtonHover.Visibility = Visibility.Collapsed;
        maximizeButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void MaximizePointerCaptureLost(object sender, PointerRoutedEventArgs e) {
        maximizeButtonHover.Visibility = Visibility.Collapsed;
        maximizeButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void RestorePointerEntered(object sender, PointerRoutedEventArgs e) {
        restoreButtonHover.Visibility = Visibility.Visible;
        restoreButtonLight.Visibility = Visibility.Visible;
    }

    private void RestorePointerExited(object sender, PointerRoutedEventArgs e) {
        restoreButtonHover.Visibility = Visibility.Collapsed;
        restoreButtonLight.Visibility = Visibility.Collapsed;
        restoreButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void RestorePointerPressed(object sender, PointerRoutedEventArgs e) {
        restoreButtonPressed.Visibility = Visibility.Visible;
    }

    private void RestorePointerReleased(object sender, PointerRoutedEventArgs e) {
        restoreButtonPressed.Visibility = Visibility.Collapsed;
        presenter.Restore();
    }

    private void RestorePointerCanceled(object sender, PointerRoutedEventArgs e) {
        restoreButtonHover.Visibility = Visibility.Collapsed;
        restoreButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void RestorePointerCaptureLost(object sender, PointerRoutedEventArgs e) {
        restoreButtonHover.Visibility = Visibility.Collapsed;
        restoreButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void MinimizePointerEntered(object sender, PointerRoutedEventArgs e) {
        minimizeButtonHover.Visibility = Visibility.Visible;
        minimizeButtonLight.Visibility = Visibility.Visible;
    }

    private void MinimizePointerExited(object sender, PointerRoutedEventArgs e) {
        minimizeButtonHover.Visibility = Visibility.Collapsed;
        minimizeButtonLight.Visibility = Visibility.Collapsed;
        minimizeButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void MinimizePointerPressed(object sender, PointerRoutedEventArgs e) {
        minimizeButtonPressed.Visibility = Visibility.Visible;
    }

    private void MinimizePointerReleased(object sender, PointerRoutedEventArgs e) {
        minimizeButtonPressed.Visibility = Visibility.Collapsed;
        presenter.Minimize();
    }

    private void MinimizePointerCanceled(object sender, PointerRoutedEventArgs e) {
        minimizeButtonHover.Visibility = Visibility.Collapsed;
        minimizeButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void MinimizePointerCaptureLost(object sender, PointerRoutedEventArgs e) {
        minimizeButtonHover.Visibility = Visibility.Collapsed;
        minimizeButtonPressed.Visibility = Visibility.Collapsed;
    }

    private void AeroSurfaceReflectionLoaded(object sender, RoutedEventArgs e) {
        double rasterizationScale = aeroSurfaceReflection.XamlRoot.RasterizationScale;
        transformMatrix = new Matrix {
            M11 = transformMatrix.M11, M12 = 0.0,
            M21 = 0.0, M22 = transformMatrix.M22,
            OffsetX = -AppWindow.Position.X / rasterizationScale,
            OffsetY = -AppWindow.Position.Y / rasterizationScale,
        };
        (aeroSurfaceReflection.RenderTransform as MatrixTransform).Matrix = transformMatrix;
    }
}
