using BCC.Core.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZXing.Mobile;

namespace BCC.Core.ViewModels
{
    public class ScanViewModel : MvxViewModel
    {
        private ObservableCollection<string> barcodes = new ObservableCollection<string>();        public ObservableCollection<string> Barcodes
        {
            get { return barcodes; }
            set { SetProperty(ref barcodes, value); }
        }        public ICommand GenerateQRCodeCommand { get; private set; }
        public ICommand ScanContinuouslyCommand { get; private set; }
        public IMobileBarcodeScanner scanner;

        public ScanViewModel()
        {
            GenerateQRCodeCommand = new MvxCommand<string>(selectedBarcode =>
            {
                ShowViewModel<BarcodeViewModel>(new { param = selectedBarcode });
            });
            ScanContinuouslyCommand = new MvxCommand(ScanContinuously);
        }

        public void ScanContinuously()
        {
            var options = new MobileBarcodeScanningOptions();
            options.UseNativeScanning = true;
            scanner.ScanContinuously(OnResult);
        }

        public void OnResult(ZXing.Result result)
        {
            var barcode = result.Text;
            Barcodes.Add(barcode);
            Mvx.Resolve<IToast>().Show(string.Format("Bar code = {0} added to list", barcode));
        }
    }
}
