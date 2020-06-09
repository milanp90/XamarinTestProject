using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace WorkOrders.Helpers
{
    public class CloneHelper
    {
        public static T DeepClone<T>(T sourceObj)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("Class must be marked [Serializable]");
            }
            if (ReferenceEquals(sourceObj, null))
            {
                return default(T);
            }
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, sourceObj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
