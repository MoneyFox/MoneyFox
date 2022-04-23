namespace MoneyFox.Views.Dialogs
{

    using System;
    using System.Threading.Tasks;
    using Xamarin.CommunityToolkit.Extensions;
    using Xamarin.Forms;

    public partial class ConfirmMessageDialog
    {
        public static readonly BindableProperty PopupTitleProperty = BindableProperty.Create(
            propertyName: nameof(PopupTitle),
            returnType: typeof(string),
            declaringType: typeof(ConfirmMessageDialog));

        public static readonly BindableProperty PopupMessageProperty = BindableProperty.Create(
            propertyName: nameof(PopupMessage),
            returnType: typeof(string),
            declaringType: typeof(ConfirmMessageDialog));

        public static readonly BindableProperty PositiveTextProperty = BindableProperty.Create(
            propertyName: nameof(PositiveText),
            returnType: typeof(string),
            declaringType: typeof(ConfirmMessageDialog));

        public static readonly BindableProperty NegativeTextProperty = BindableProperty.Create(
            propertyName: nameof(NegativeText),
            returnType: typeof(string),
            declaringType: typeof(ConfirmMessageDialog));

        public ConfirmMessageDialog(string title, string message, string positiveText = "", string negativeText = "")
        {
            InitializeComponent();
            PopupTitle = title;
            PopupMessage = message;
            PositiveText = positiveText;
            NegativeText = negativeText;
        }

        public string PopupTitle
        {
            get => (string)GetValue(PopupTitleProperty);
            set => SetValue(property: PopupTitleProperty, value: value);
        }

        public string PopupMessage
        {
            get => (string)GetValue(PopupMessageProperty);
            set => SetValue(property: PopupMessageProperty, value: value);
        }

        public string PositiveText
        {
            get => (string)GetValue(PositiveTextProperty);
            set => SetValue(property: PositiveTextProperty, value: value);
        }

        public string NegativeText
        {
            get => (string)GetValue(NegativeTextProperty);
            set => SetValue(property: NegativeTextProperty, value: value);
        }

        public async Task<bool> ShowAsync()
        {
            var result = await Application.Current.MainPage.Navigation.ShowPopupAsync(this);

            return (bool)result;
        }

        private void PositiveHandlerClicked(object sender, EventArgs e)
        {
            Dismiss(true);
        }

        private void NegativeHandlerClicked(object sender, EventArgs e)
        {
            Dismiss(false);
        }
    }

}
