using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class MethodParameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
    }

    public class DllInspectorViewModel: BaseViewModel
    {
        private readonly DllInspectorModel _dllInspectorModel;
        private string _dllPath;
        private ObservableCollection<string> _classNames;
        private ObservableCollection<string> _methodNames;
        private string _selectedMethod;
        private List<MethodParameter> _methodParameters;
        private string _result;

        public List<MethodParameter> MethodParameters
        {
            get => _methodParameters;
            set => SetProperty(ref _methodParameters, value);
        }

        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public string SelectedMethod
        {
            get => _selectedMethod;
            set => SetProperty(ref _selectedMethod, value);
        }

        public string DllPath
        {
            get => _dllPath;
            set => SetProperty(ref _dllPath, value);
        }

        public ObservableCollection<string> ClassNames
        {
            get => _classNames;
            set => SetProperty(ref _classNames, value);
        }

        public ObservableCollection<string> MethodNames
        {
            get => _methodNames;
            set => SetProperty(ref _methodNames, value);
        }

        public ICommand LoadDllCommand { get; }

        public ICommand LoadMethodCommand { get; }

        public DllInspectorViewModel()
        {
            _dllInspectorModel = new DllInspectorModel();
            _classNames = new ObservableCollection<string>();
            _methodNames = new ObservableCollection<string>();
            LoadDllCommand = new RelayCommand(LoadDll, CanLoadDll);
            LoadMethodCommand = new RelayCommand(LoadMethod, CanLoadMethod);
        }

        private bool CanLoadDll(object parameter)
        {
            return !string.IsNullOrEmpty(DllPath);
        }

        private void LoadMethod(object parameter)
        {
            if(parameter is string param)
                _dllInspectorModel.RunMethod(param);
        }

        private bool CanLoadMethod(object parameter)
        {
            return true;
        }

        private void LoadDll(object parameter)
        {
            bool isLoaded = _dllInspectorModel.LoadDll(DllPath);

            if (isLoaded)
            {
                ClassNames.Clear();
                MethodNames.Clear();

                foreach (var className in _dllInspectorModel.ClassNames)
                    ClassNames.Add(className);

                foreach (var methodName in _dllInspectorModel.MethodNames)
                    MethodNames.Add(methodName);
            }
        }
    }
}
