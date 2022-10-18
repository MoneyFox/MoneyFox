namespace MoneyFox.Ui.Controls;

using System.Collections;

public partial class CustomPicker : ContentView
{
    public CustomPicker()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(CustomPicker), string.Empty);
    public static readonly BindableProperty PickerItemsSourceProperty = BindableProperty.Create(nameof(PickerItemsSource), typeof(IList), typeof(CustomPicker));
    public static readonly BindableProperty PickerSelectedItemProperty = BindableProperty.Create(nameof(PickerSelectedItem), typeof(object), typeof(CustomPicker));
    public static readonly BindableProperty PickerIsVisibleProperty = BindableProperty.Create(nameof(PickerIsVisible), typeof(bool), typeof(CustomPicker));

    public string PickerTitle
    {
        get => (string)GetValue(CustomPicker.PickerTitleProperty);
        set => SetValue(CustomPicker.PickerTitleProperty, value);
    }
    public IList PickerItemsSource
    {
        get => (IList)GetValue(CustomPicker.PickerItemsSourceProperty);
        set => SetValue(CustomPicker.PickerItemsSourceProperty, value);
    }

    public object PickerSelectedItem
    {
        get => GetValue(CustomPicker.PickerSelectedItemProperty);
        set => SetValue(CustomPicker.PickerSelectedItemProperty, value);
    }

    public bool PickerIsVisible
    {
        get => (bool)GetValue(CustomPicker.PickerIsVisibleProperty);
        set => SetValue(CustomPicker.PickerIsVisibleProperty, value);
    }


    BindingBase itemDisplayBinding;
    public BindingBase PickerItemDisplayBinding
    {
        get { return itemDisplayBinding; }
        set
        {
            if (itemDisplayBinding == value)
                return;

            OnPropertyChanging();
            itemDisplayBinding = value;
        }
    }
}
