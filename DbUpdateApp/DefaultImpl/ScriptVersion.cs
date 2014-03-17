using System;
using System.Text.RegularExpressions;

namespace DbUpdateApp
{
    public class ScriptVersion:IComparable
    {
        public static int[] NewVersionNumber(int[] old)
        {
            var nv = new int[old.Length];
            Array.Copy(old, nv, old.Length);
            nv[nv.Length - 1]++;
            return nv;
        }
   public ScriptVersion(string file)
        {
            this.Name = file;
            CreateVersion();
        }

        private void CreateVersion()
        {
            const string pattern = @"(\d+)?(?:\.)?(\d+)?(?:\.)?(\d+)?(?:\.)?(\d+)?(?:\.)?";
            var f = Regex.Match(Name, pattern);
            for (int z = 1; z < f.Groups.Count; z++)
            {
                if (f.Groups[z].Success)
                    _version[z - 1] = int.Parse(f.Groups[z].Value);
                else break;
            }
        }
        public string Name { get; set; }
        private readonly int[] _version= new int[4];
        public int[] Version { get { return _version; } }

        public int CompareTo(object obj)
        {
            var v = (ScriptVersion) obj;
            int l = _version.Length;
            int results = 0;
            int i=0;

            do
            {
                if (Version[i] > v.Version[i])
                {
                    results = 1;
                }
                else if (Version[i] < v.Version[i])
                {
                    results = -1;
                }
                i++;
            } while (i<l && results==0);

            return results;
        }


    }
}