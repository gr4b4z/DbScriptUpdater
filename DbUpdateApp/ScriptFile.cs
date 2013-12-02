using System;
using System.Text.RegularExpressions;

namespace DbUpdateApp
{
    public class ScriptFile:IComparable
    {
        public ScriptFile(string file)
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
                    _arr[z - 1] = int.Parse(f.Groups[z].Value);
                else break;
            }
        }
        public string Name { get; set; }
        public string Content { get; set; }
        private readonly int[] _arr= new int[4];
        private int[] Version { get { return _arr; } }

        public int CompareTo(object obj)
        {
            var v = (ScriptFile) obj;
            int l = _arr.Length;
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