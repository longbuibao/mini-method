using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MethodParameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
    }

    public class DllInspectorModel
    {
        public List<string> ClassNames { get; set; } = new List<string>();
        public List<string> MethodNames { get; set; } = new List<string>();

        private Assembly? _assembly;

        public List<ParameterInfo> GetMethodParams(string fullName)
        {
            if (String.IsNullOrEmpty(fullName))
            {
                return new List<ParameterInfo>();
            }

            if (_assembly != null)
            {
                var strs = fullName.Split(".");
                var className = strs[1];
                var methodName = strs[2];
                var type = _assembly.GetTypes().FirstOrDefault(x => x.Name.Equals(className));
                if (type != null)
                {
                    var method = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(m => m.Name == methodName);
                    if (method != null)
                    {
                        return method.GetParameters().ToList();
                    }
                }
            }
            return new List<ParameterInfo>();
        }

        public object? RunMethod(string fullName, List<MethodParameter> methodParameters)
        {
            if (String.IsNullOrEmpty(fullName))
            {
                return new List<ParameterInfo>();
            }

            if (_assembly != null)
            {
                var strs = fullName.Split(".");
                var className = strs[1];
                var methodName = strs[2];
                var type = _assembly.GetTypes().FirstOrDefault(x => x.Name.Equals(className));
                if (type != null)
                {
                    object[] parameters = methodParameters.Select(p => Convert.ChangeType(p.Value, p.Type)).ToArray();
                    var method = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(m => m.Name == methodName);
                    var instance = Activator.CreateInstance(type);
                    if (instance != null && method != null)
                    {
                        return method.Invoke(instance, parameters);
                    }
                }
            }

            return null;
        }

        public bool LoadDll(string filePath)
        {
            try
            {
                _assembly = Assembly.LoadFrom(filePath);
                ClassNames.Clear();
                MethodNames.Clear();

                foreach (var type in _assembly.GetTypes())
                {
                    if (type.FullName == null) continue;
                    ClassNames.Add(type.FullName);
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                    {
                        MethodNames.Add($"{type.FullName}.{method.Name}.{method.ReturnParameter}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
