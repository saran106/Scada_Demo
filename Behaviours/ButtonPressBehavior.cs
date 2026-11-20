
using System;
using System.IO;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Scada_Demo.Behaviors
{
    public static class ButtonPressBehavior
    {
        private static SoundPlayer? _buzzerPlayer;

        public static readonly DependencyProperty PressCommandProperty =
            DependencyProperty.RegisterAttached(
                "PressCommand",
                typeof(ICommand),
                typeof(ButtonPressBehavior),
                new PropertyMetadata(null, OnCommandChanged));

        public static readonly DependencyProperty ReleaseCommandProperty =
            DependencyProperty.RegisterAttached(
                "ReleaseCommand",
                typeof(ICommand),
                typeof(ButtonPressBehavior),
                new PropertyMetadata(null, OnCommandChanged));

        private static readonly DependencyProperty IsHookedProperty =
            DependencyProperty.RegisterAttached(
                "IsHooked",
                typeof(bool),
                typeof(ButtonPressBehavior),
                new PropertyMetadata(false));

        private static readonly DependencyProperty IsPressedProperty =
            DependencyProperty.RegisterAttached(
                "IsPressed",
                typeof(bool),
                typeof(ButtonPressBehavior),
                new PropertyMetadata(false));

        public static void SetPressCommand(UIElement element, ICommand value)
        {
            element.SetValue(PressCommandProperty, value);
        }

        public static ICommand GetPressCommand(UIElement element)
        {
            return (ICommand)element.GetValue(PressCommandProperty);
        }

        public static void SetReleaseCommand(UIElement element, ICommand value)
        {
            element.SetValue(ReleaseCommandProperty, value);
        }

        public static ICommand GetReleaseCommand(UIElement element)
        {
            return (ICommand)element.GetValue(ReleaseCommandProperty);
        }

        private static void OnCommandChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is not Button button)
                return;

            if ((bool)button.GetValue(IsHookedProperty))
                return;

            button.SetValue(IsHookedProperty, true);

            button.PreviewMouseLeftButtonDown += Button_MouseDown;
            button.PreviewMouseLeftButtonUp += Button_MouseUp;
            button.LostMouseCapture += Button_LostMouseCapture;
            button.Unloaded += Button_Unloaded;
        }

        // BUTTON PRESS
        private static void Button_MouseDown(
            object sender,
            MouseButtonEventArgs e)
        {
            if (sender is not Button button)
                return;

            if ((bool)button.GetValue(IsPressedProperty))
                return;

            button.SetValue(IsPressedProperty, true);

            // START BUZZER
            StartBuzzer(button);

            // EXECUTE PRESS COMMAND
            ExecuteCommand(GetPressCommand(button));
        }

        // BUTTON RELEASE
        private static void Button_MouseUp(
            object sender,
            MouseButtonEventArgs e)
        {
            if (sender is Button button)
                ReleaseButton(button);
        }

        // MOUSE CAPTURE LOST
        private static void Button_LostMouseCapture(
            object sender,
            MouseEventArgs e)
        {
            if (sender is Button button)
                ReleaseButton(button);
        }

        // BUTTON UNLOADED
        private static void Button_Unloaded(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is Button button)
                ReleaseButton(button);
        }

        // START BUZZER
        private static void StartBuzzer(Button button)
        {
            try
            {
                StopBuzzer(button);

                string soundPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Sounds",
                    "buzzer.wav");

                Debug.WriteLine("BUZZER PATH: " + soundPath);

                if (!File.Exists(soundPath))
                {
                    Debug.WriteLine("BUZZER FILE NOT FOUND!");
                    return;
                }

                _buzzerPlayer ??= new SoundPlayer(soundPath);

                _buzzerPlayer.Load();
                _buzzerPlayer.PlayLooping();

                Debug.WriteLine("BUZZER STARTED");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("BUZZER ERROR: " + ex.Message);
            }
        }

        // STOP BUZZER
        private static void StopBuzzer(Button button)
        {
            try
            {
                _buzzerPlayer?.Stop();

                Debug.WriteLine("BUZZER STOPPED");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("BUZZER STOP ERROR: " + ex.Message);
            }
        }

        // RELEASE BUTTON
        private static void ReleaseButton(Button button)
        {
            if (!(bool)button.GetValue(IsPressedProperty))
                return;

            button.SetValue(IsPressedProperty, false);

            // STOP BUZZER
            StopBuzzer(button);

            // EXECUTE RELEASE COMMAND
            ExecuteCommand(GetReleaseCommand(button));
        }

        // COMMAND EXECUTION
        private static void ExecuteCommand(ICommand command)
        {
            if (command != null && command.CanExecute(null))
            {
                command.Execute(null);
            }
        }
    }
}
