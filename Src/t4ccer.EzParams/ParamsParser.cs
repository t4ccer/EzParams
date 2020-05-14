using System;
using System.Collections.Generic;
using System.Linq;

namespace t4ccer.EzParams
{
    public class ParamsParser
    {
        Dictionary<string, string> splittedParams = new Dictionary<string, string>();
        List<Parameter> parameters = new List<Parameter>();
        string programName;

        public ParamsParser(string argsString, string programName = "PROGRAM")
        {
            this.programName = programName;
            ParseString(argsString);
        }

        /// <summary>
        /// Parses parameters using finite state machine
        /// </summary>
        /// <param name="argsString"></param>
        private void ParseString(string argsString)
        {
            string key = "";
            string value = null;
            var mode = ParsingMode.KeyStart;

            for (int i = 0; i < argsString.Length;)
            {
                switch (mode)
                {
                    case ParsingMode.KeyStart:
                        if (argsString[i] == '-')
                        {
                            key += argsString[i];
                            mode = ParsingMode.Key;
                        }
                        i++;
                        break;
                    case ParsingMode.Key:
                        if (argsString[i] == ' ')
                            mode = ParsingMode.KeyEnd;
                        else
                            key += argsString[i];
                        i++;
                        break;
                    case ParsingMode.KeyEnd:
                        if (argsString[i] == '-')
                        {
                            splittedParams[key] = value;
                            key = "";
                            mode = ParsingMode.KeyStart;
                        }
                        else
                        {
                            mode = ParsingMode.ValueStart;
                            value = "";
                        }
                        break;
                    case ParsingMode.ValueStart:
                        if (argsString[i] == ' ')
                        { }
                        else if (argsString[i] == '"')
                            mode = ParsingMode.QuotString;
                        else
                        {
                            value += argsString[i];
                            mode = ParsingMode.Value;
                        }
                        i++;
                        break;
                    case ParsingMode.Value:
                        if (argsString[i] == ' ')
                        {
                            splittedParams[key] = value;
                            key = "";
                            value = null;
                            mode = ParsingMode.KeyStart;
                        }
                        else
                            value += argsString[i];
                        i++;
                        break;
                    case ParsingMode.QuotString:
                        if (argsString[i] == '\\' && argsString[i + 1] == '"')
                        {
                            value += '"';
                            i++;
                        }
                        else if (argsString[i] == '"')
                            mode = ParsingMode.Value;
                        else
                            value += argsString[i];
                        i++;
                        break;
                    default:
                        //Should not happen
                        throw new Exception();
                }
            }
            if (key != "")
                splittedParams[key] = value;
        }

        /// <summary>
        /// Add string parameter to parser
        /// </summary>
        /// <param name="content">Parameter value</param>
        /// <param name="name">Parameter name</param>
        /// <param name="alias">Parameter alias (can be null)</param>
        /// <param name="desc">Parameter description</param>
        /// <param name="defaultContent">Default parameter value. Content will be set to whis value if parameter is not provided. Null by default</param>
        /// <returns></returns>
        public ParamsParser WithParameter(out string content, string name, string alias, string desc="", string defaultContent = null)
        {
            parameters.Add(new Parameter(name, alias, defaultContent, desc));

            if (splittedParams.ContainsKey(name))
                content = splittedParams[name];
            else if (alias != null && splittedParams.ContainsKey(alias))
                content = splittedParams[alias];
            else
                content = defaultContent;

            return this;
        }
        /// <summary>
        /// Add flag parameter to parser
        /// </summary>
        /// <param name="isPresent">Will be set to true if flag provided</param>
        /// <param name="name">Parameter name</param>
        /// <param name="alias">Parameter alias (can be null)</param>
        /// <param name="desc">Parameter description</param>
        /// <returns></returns>
        public ParamsParser WithParameter(out bool isPresent, string name, string alias, string desc="")
        {
            parameters.Add(new Parameter(name, alias, "false", desc));

            isPresent = splittedParams.ContainsKey(name) || splittedParams.ContainsKey(alias);

            return this;
        }
        /// <summary>
        /// Add default usage info. Remember to add at the end.
        /// </summary>
        /// <param name="isPresent">Will be set to true if flag provided</param>
        /// <param name="name">Parameter name</param>
        /// <param name="alias">Parameter alias (can be null)</param>
        /// <param name="desc">Parameter description</param>
        /// <returns></returns>
        public ParamsParser WithDefaultHelpParameter(out bool isPresent, string name = "--help", string alias = "-h", string desc = "Shows this info")
        {
            parameters.Add(new Parameter(name, alias, "false", desc));

            isPresent = splittedParams.ContainsKey(name) || splittedParams.ContainsKey(alias);

            if(isPresent)
            {
                Console.WriteLine($"Usage: {programName} [OPTIONS]\nOPIONS:");
                int longestName = parameters.Select(x => x.name.Length).Max();
                int longestAlias = parameters.Where(x => x.alias != null).Select(x => x.alias.Length).Max();
                //int longestSum = parameters.Select(x => x.name.Length + x.alias.Length).Max();
                foreach (var param in parameters)
                {
                    Console.WriteLine($"\t{param.name.PadRight(longestName)} " +
                        $"{((param.alias != null) ? param.alias.PadRight(longestAlias) : "".PadRight(longestAlias))} " +
                        $"(default : {(param.defValue == null ? "null" : param.defValue)}) " +
                        $"{param.description}");
                }
            }

            return this;
        }
    }
}
