using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Site.Transforms
{
    public static class Transform
    {
        public static T TransformObject<T>(Object entidade) where T : class
        {
            var typeClass = typeof(T);
            var nameSpace = typeClass.Assembly.ToString();
            var nameClass = typeClass.FullName;
            var asm = Assembly.Load(nameSpace);
            var classeModelo = asm.GetType(nameClass);
            var objeto = Activator.CreateInstance(classeModelo);

            foreach (var item in entidade.GetType().GetProperties())
            {
                var typeObjeto = objeto.GetType().GetProperty(item.Name);
                var valorU = entidade.GetType().GetProperty(item.Name).GetValue(entidade);

                try
                {
                    typeObjeto.SetValue(objeto, valorU);
                }
                catch
                {
                }
            }

            return (T)objeto;
        }

        public static T JTransform<T>(Object entidade) where T : class
        {
            return (T) Convert<T>(entidade);
        }

        public static T JeTransform<T>(this Object entidade) where T : class
        {
            return (T) Convert<T>(entidade);
        }

        private static Object Convert<T>(Object entidade) where T : class
        {
            var jEntidade = JsonConvert.SerializeObject(entidade);
            var entidadeConvertida = JsonConvert.DeserializeObject<T>(jEntidade);
            return entidadeConvertida;
        }
    }
}
