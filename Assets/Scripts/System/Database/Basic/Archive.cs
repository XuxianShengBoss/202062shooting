using UnityEngine;
using System.IO;

namespace gungame
{

    public class Archive
    {
        

        public static byte[] Read(string name)
        {
			name = Application.persistentDataPath + "/" + name;
//			Debug.Log ("LOAD: "+name);
			if (File.Exists (name)) {
				FileStream fs = new FileStream (name, FileMode.Open, FileAccess.Read);
				int length = (int)fs.Length;
				byte[] data = new byte[length];
				fs.Read (data, 0, length);
				fs.Close ();
				return data;
			} else {
				return null;
			}
        }
        
        public static void Save(string name, byte[] data)
        {
			name = Application.persistentDataPath + "/" + name;
//			Debug.Log ("SAVE: " + name);
			FileStream fs = new FileStream(name, FileMode.OpenOrCreate, FileAccess.Write);
			fs.Write (data, 0, data.Length);
			fs.Close ();
        }
    }
}
