using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DllInspectorModel
    {
        public List<string> ClassNames { get; set; } = new List<string>();
        public List<string> MethodNames { get; set; } = new List<string>();

        private Assembly? _assembly;

        public void RunMethod(string methodName)
        {

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
