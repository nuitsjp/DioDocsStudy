namespace InvoiceBuilder.ViewModel
{
    public interface IViewModelProvider
    {
        T Resolve<T>() where T : class;
    }
}