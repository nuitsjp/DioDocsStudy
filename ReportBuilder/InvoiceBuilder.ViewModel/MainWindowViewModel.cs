using System.Collections.ObjectModel;
using System.Reactive.Linq;
using InvoiceBuilder.UseCase;
using PropertyChanged;
using Reactive.Bindings;

namespace InvoiceBuilder.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {
        private readonly IBuildInvoice _buildInvoice;

        public ReactiveCommand SearchCommand { get; } = new ReactiveCommand();

        public ObservableCollection<SalesOrder> SalesOrders { get; } = new ObservableCollection<SalesOrder>();

        public ReactiveProperty<SalesOrder> SelectedSalesOrder { get; } = new ReactiveProperty<SalesOrder>();

        public ReactiveCommand SaveToPdfCommand { get; }

        public MainWindowViewModel(IBuildInvoice buildInvoice)
        {
            _buildInvoice = buildInvoice;

            SearchCommand.Subscribe(Search);

            SaveToPdfCommand = SelectedSalesOrder.Select(x => x != null).ToReactiveCommand();
            SaveToPdfCommand.Subscribe(SaveToPdf);
        }

        private void Search()
        {
            SalesOrders.Clear();
            foreach (var salesOrder in _buildInvoice.GetSalesOrders())
            {
                SalesOrders.Add(salesOrder);
            }
        }

        private void SaveToPdf()
        {
            _buildInvoice.Build(SelectedSalesOrder.Value);
        }
    }
}
