using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwesomeCodeFixerUI
{
    public class Divide_Input
    {
        public static void find_end(List<string> lines, List<string> com, int i, string end)
        {
            for (; i < lines.Count; i++)
            {
                if (Regex.Match(lines[i], end).Success)
                {
                    com[2] = i.ToString();
                    break;
                }
            }
        }

        /// <summary>
        /// Identify component types within the file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<List<string>> divide(string path)
        {
            List<string> lines = File.ReadAllLines(path).ToList();

            var beginings1 = from line in lines.Select((Value, Index) => new { Value, Index })
                             where Regex.Match(line.Value, "^<(.*) ", RegexOptions.Singleline).Success
                             select new List<string> {line.Index.ToString(),
                                                    line.Value.Substring(line.Value.IndexOf("<")+1,
                                                    line.Value.IndexOf(" ")-1)};


            var beginings2 = from line in lines.Select((Value, Index) => new { Value, Index })
                             where Regex.Match(line.Value, "^```(.+)", RegexOptions.Singleline).Success
                             select new List<string> { line.Index.ToString(), line.Value.Split("\n")[0].Remove(0,3) };


            var beginings3 = from line in lines.Select((Value, Index) => new { Value, Index })
                             where Regex.Match(line.Value, @"^(([^\$]+\$ )|(\$ ))", RegexOptions.Singleline).Success
                             select new List<string> { line.Index.ToString(), "InlineLatex", line.Index.ToString() };


            var beginings4 = from line in lines.Select((Value, Index) => new { Value, Index })
                             where Regex.Match(line.Value, @"\$\$", RegexOptions.Singleline).Success
                             select new List<string> { line.Index.ToString(), "BlockLatex" };

            var beginings41 = beginings4.ToList();
            for (int j = 1; j < beginings4.Count(); j += 2) beginings41.RemoveAt(j);


            var components = beginings1.Concat(beginings2).Concat(beginings3).Concat(beginings41).ToList();

            foreach (var com in components)
            {
                com.Add("");
                int i = Int32.Parse(com[0]);
                string name = com[1];
                if (name == "BlockLatex") find_end(lines, com, i + 1, @"^\$\$(.*)");
                else if (name == "Header" || name == "YouTube") find_end(lines, com, i, @"/>");
                else if (name == "Note" || name == "Warning" || name == "Info" || name == "DeepDive")
                    find_end(lines, com, i, string.Format(@"</{0}>", name));
                else if(name != "InlineLatex") find_end(lines, com, i + 1, "^```");
            }

            SortedSet<int> lines_found = new SortedSet<int>(); 
            foreach(var com in components){
                for(int i = Int32.Parse(com[0]); i <= Int32.Parse(com[2]); i++){
                    lines_found.Add(i);
                }

            }
            for(int i=0; i < lines.Count(); i++) {
                if(!lines_found.Contains(i)){
                    int beginning = i;
                    int end = i;
                    while(!lines_found.Contains(i) && i+1 < lines.Count()){
                        end = i;
                        i++;
                    }
                    components.Add(new List<string>{beginning.ToString(), "Markdown", end.ToString()});
                }
            }

            components.Sort((x, y) => Int32.Parse(x[0]).CompareTo(Int32.Parse(y[0])));
            return components;
        }

        /// <summary>
        /// Replace component types with their content
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        /// 
        public static List<List<string>> return_components(string path, List<List<string>> info)
        {
            List<string> lines = File.ReadAllLines(path).ToList();
            List<List<string>> components = new List<List<string>>();

            for (int i = 0; i < info.Count; i++)
            {
                components.Add(new List<string>());
                string content = "";
                for (int j = Int32.Parse(info[i][0]); j <= Int32.Parse(info[i][2]); j++)
                {
                    content += lines[j];
                    content += "\n";
                }
                content = content.Remove(content.Length - 1);
                components[i].Add(info[i][0]);
                components[i].Add(content);
            }
            return components;
        }
        public static void get_feedback(string path, List<List<string>> info)
        {
            List<string> lines = File.ReadAllLines(path).ToList();

            for (int i = 0; i < info.Count; i++)
            {
                ComponentType component;
                if(Enum.IsDefined(typeof(ComponentType), info[i][1])){
                    Enum.TryParse(info[i][1], out component);
                }
                else component = ComponentType.Markdown;

                string content = "";
                for (int j = Int32.Parse(info[i][0]); j <= Int32.Parse(info[i][2]); j++)
                {
                    content += lines[j];
                    content += "\n";
                }
                content = content.Remove(content.Length - 1);

                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Component type: " + info[i][1]);
                Console.WriteLine("Beginning at line: " + info[i][0]);
                Console.WriteLine("Message:");
                // if(component != ComponentType.InlineLatex && component != ComponentType.BlockLatex) 
                Console.WriteLine(Linter.Lint(content, component));
                Console.WriteLine("-------------------------------------------");
            }
        }
    }
}
