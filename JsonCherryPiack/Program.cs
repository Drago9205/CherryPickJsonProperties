using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace JsonCherryPiack
{
    class Program
    {
        static void Main(string[] args)
        {
            var jTokenInput = JToken.Parse(InputHelper.Input);
            Traverse(jTokenInput, InputHelper.Fields);
            Console.WriteLine($"{Environment.NewLine}FinalJSON: {jTokenInput}");
        }
        
        public static void Traverse(JToken token, string[] fields)
        {
            JContainer container = token as JContainer;
            //Get current json parent element as property and keep it if contained in the list of fields to select
            var tokenProperty = token as JProperty;
            if (container == null || (tokenProperty != null && fields.Contains(tokenProperty.Name)))
            {
                return;
            }

            foreach (JToken el in container.Children().ToArray())
            {
                Traverse(el, fields);

                var prop = el as JProperty;
                if (prop != null)
                {
                    if (!fields.Contains(prop.Name))
                    {
                        if (!prop.Value.HasValues)
                        {
                            prop.Remove();
                        }

                        //Remove arrays with empty objects, when no properties from the array have been selected
                        if (prop.Value.Type == JTokenType.Array && prop.Parent != null)
                        {
                            if (prop.Value.All(x => !x.HasValues))
                            {
                                prop.Remove();
                            }
                        }
                    }
                    else
                    {
                        if (prop.Value.IsNullOrEmpty())
                        {
                            prop.Remove();
                        }
                    }
                }
            }
        }

        [Obsolete]
        private static void removeFields(JToken token, string[] fields)
        {
            JContainer container = token as JContainer;
            if (container == null) return;

            List<JToken> removeList = new List<JToken>();
            foreach (JToken el in container.Children())
            {
                JProperty p = el as JProperty;
                if (p != null && !fields.Contains(p.Name))
                {
                    removeList.Add(el);
                }

                removeFields(el, fields);
            }

            foreach (JToken el in removeList)
            {
                Console.WriteLine($"Removing: {el}");
                el.Remove();
            }
        }
    }

}
