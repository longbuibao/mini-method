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
    public class DllInspectorViewModel: BaseViewModel
    {
        private readonly DllInspectorModel _dllInspectorModel;
        private string _dllPath;
        private ObservableCollection<string> _classNames;
        private ObservableCollection<string> _methodNames;
        private string _selectedMethod;
        private List<MethodParameter> _methodParameters;
        private object? _result = null;

        public List<MethodParameter> MethodParameters
        {
            get => _methodParameters;
            set => SetProperty(ref _methodParameters, value);
        }

        public object? Result
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

        public ICommand ExecuteMethodCommand { get; }

        public ICommand GetMethodParamsCommand { get; }

        public DllInspectorViewModel()
        {
            _dllInspectorModel = new DllInspectorModel();
            _classNames = new ObservableCollection<string>();
            _methodNames = new ObservableCollection<string>();
            LoadDllCommand = new RelayCommand(LoadDll, CanLoadDll);
            ExecuteMethodCommand = new RelayCommand(ExecuteMethod, CanExecuteMethod);
            GetMethodParamsCommand = new RelayCommand(GetMethodParams, (object _) => true);
        }

        private void GetMethodParams(object parameter)
        {
            var methodParams = _dllInspectorModel.GetMethodParams(SelectedMethod);
            MethodParameters = methodParams.Select(x => new MethodParameter()
            {
                Name = x.Name,
                Type = x.ParameterType,
                Value = null,
            }).ToList();
        }

        private bool CanLoadDll(object parameter)
        {
            return !string.IsNullOrEmpty(DllPath);
        }

        private void ExecuteMethod(object parameter)
        {
            Result = _dllInspectorModel.RunMethod(SelectedMethod, MethodParameters);
        }

        private bool CanExecuteMethod(object parameter)
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
