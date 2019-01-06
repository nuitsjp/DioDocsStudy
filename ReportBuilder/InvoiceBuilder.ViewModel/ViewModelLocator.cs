namespace InvoiceBuilder.ViewModel
{
    public static class ViewModelLocator
    {
        public static IViewModelProvider ViewModelProvider { private get; set; }

        public static MainWindowViewModel MainWindowViewModel => ViewModelProvider?.Resolve<MainWindowViewModel>();
    }
}
