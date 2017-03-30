using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ACE.Client.Common;

namespace ACE.Client.Model.CodeGenerators
{
    public class XAMLGenerator
    {
        public static Assembly ModelAssembly { get; private set; }
        public static Assembly CommonAssembly { get; private set; }
        public static string ResourcesDirectory { get; private set; }

        public static Func<string,string> CamelCaseSpacer => (orig) => Regex.Replace(orig + ":", "(\\B[A-Z])", " $1");

        private static string nl = Environment.NewLine;
        public  XAMLGenerator()
        {
            var templ = typeof(DataTemplateAttribute); // just to guarantee that this assembly will be loaded
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            ModelAssembly = assemblies.First(a => a.GetTypes().Contains(typeof(ClientModel)));
            CommonAssembly = assemblies.First(a => a.GetTypes().Contains(typeof(DataTemplateAttribute)));

            //AssemblyPath = AppDomain.CurrentDomain.BaseDirectory;
            ResourcesDirectory = Properties.Resources.CodeGeneratedDirectory;
        }

        //TODO: handle inconsistent input
        //TODO: Use VS Extensibilities
        public void Generate(ClientModel model)
        {
            var modelEntityTypes = ModelAssembly.GetTypes().Where(t => t.BaseType != null && t.BaseType.IsGenericType
            && t.BaseType.GetGenericTypeDefinition().Name.StartsWith("ModelEntity"));
            foreach (var entityType in modelEntityTypes)
            {
                var annotatedProperties = new List<PropertyInfo>();
                var properties = entityType.GetProperties();
                annotatedProperties.AddRange(from propertyInfo in properties from customAttributeData in propertyInfo.CustomAttributes
                        where customAttributeData.AttributeType == typeof (DataTemplateAttribute) select propertyInfo);
                if (annotatedProperties.Any()) GenerateGroup(annotatedProperties);
            }
        }

        public void GenerateGroup(IEnumerable<PropertyInfo> annotatedProperties)
        {
            //var propertyAttributeList = annotatedProperties.Select(p => new { Property= p, Attribute = p.GetCustomAttributes(typeof(DataTemplateAttribute)) as DataTemplateAttribute });
            var propertyAttributeList = annotatedProperties.Select(p => new { Property = p, Attribute = p.GetCustomAttribute(typeof(DataTemplateAttribute)) as DataTemplateAttribute });
            var groupedProperties = propertyAttributeList.GroupBy(pa => pa.Attribute.Group + "." + pa.Attribute.Type).OrderBy(pg => pg.Key)
                .Select(pg => new { Key = pg.Key, PropAttrList = pg });

            var declaringType = groupedProperties.First().PropAttrList.Select(pa => pa.Property.DeclaringType).First();
            var path = ResourcesDirectory + $"\\{declaringType.Name}.Resources.xaml";
            if (File.Exists(path)) File.Delete(path);
            using (FileStream fs = File.Create(path))
            {
                Write(fs, WS(0) + @"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""" + nl);
                Write(fs, WS(0) + @"                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""" + nl);
                Write(fs, WS(0) + @"                    xmlns:local=""clr -namespace:ACE.Client.Resources"" >" + nl);
                foreach (var gp in groupedProperties)
                {
                    var templGroup = gp.PropAttrList.First().Attribute.Group;
                    var templType = gp.PropAttrList.First().Attribute.Type;
                    var templId = $"{declaringType.Name}.{templGroup}.{templType}";
                    var orderedProperties = gp.PropAttrList.OrderBy(p => p.Attribute.Order).Select(pa => pa.Property);

                    Write(fs, WS(3) + @"<DataTemplate x:Key=""" + templId + @""" >" + nl);
                    if (templType == GenConstants.TypePanel) GeneratePanel(fs, templGroup, templType, orderedProperties);
                    if (templType == GenConstants.TypeColumn) GenerateColumn(fs, templGroup, templType, orderedProperties);
                    Write(fs, WS(3) + @"</DataTemplate>" + nl);
                }
                Write(fs, WS(0) + @"</ResourceDictionary>");
            }
        }

        public void GeneratePanel(FileStream fs, string templateGroup, string Templatetype, IEnumerable<PropertyInfo> annotatedProperties)
        {
            Write(fs, WS(6) + @"<StackPanel Orientation=""Horizontal"" >" + nl);

            foreach (var annotprop in annotatedProperties)
            {
                var lblText = CamelCaseSpacer(annotprop.Name);
                int lblWidth = ((lblText.ToCharArray().Count() )* (70 - 12) / 12) + 22;
                Write(fs, WS(9) + @"<StackPanel Orientation=""Horizontal"" Margin=""4, 2"" >" + nl);
                Write(fs, WS(12) + $@"<TextBlock Text=""{lblText}"" Width=""{lblWidth}"" FontWeight=""DemiBold"" Foreground=""Blue"" />" + nl);
                if (annotprop.CanWrite)
                    Write(fs, WS(12) + @"<TextBox Text=""{Binding EditingEntity." + annotprop.Name + @"}"" Width=""80"" />" + nl);
                else
                    Write(fs, WS(12) + @"<TextBox Text=""{Binding EditingEntity." + annotprop.Name + @", Mode=OneWay}"" Width=""100"" IsReadOnly=""True"" />" + nl);

                Write(fs, WS(9) + @"</StackPanel>" + nl);
            }

            Write(fs, WS(6) + @"</StackPanel>" + nl);
        }

        public void GenerateColumn(FileStream fs, string templateGroup, string Templatetype, IEnumerable<PropertyInfo> annotatedProperties)
        {
            Write(fs, WS(6) + @"<StackPanel Orientation=""Vertical"" >" + nl);

            //find max width
            var widestText = annotatedProperties.Select(p => CamelCaseSpacer(p.Name).ToCharArray().Count()).Max();
            var widestWidth = widestText * ((70 - 12) / 12) + 22;

            foreach (var annotprop in annotatedProperties)
            {
                var lblText = CamelCaseSpacer(annotprop.Name);
                //int lblWidth = ((lblText.ToCharArray().Count()) * (70 - 12) / 12) + 22;
                Write(fs, WS(9) + @"<StackPanel Orientation=""Horizontal"" Margin=""4, 2"" >" + nl);
                Write(fs, WS(12) + $@"<TextBlock Text=""{lblText}"" Width=""{widestWidth}"" FontWeight=""DemiBold"" Foreground=""Blue"" />" + nl);
                if (annotprop.CanWrite)
                    Write(fs, WS(12) + @"<TextBox Text=""{Binding EditingEntity." + annotprop.Name + @"}"" Width=""80"" />" + nl);
                else
                    Write(fs, WS(12) + @"<TextBox Text=""{Binding EditingEntity." + annotprop.Name + @", Mode=OneWay}"" Width=""100"" IsReadOnly=""True"" />" + nl);

                Write(fs, WS(9) + @"</StackPanel>" + nl);
            }

            Write(fs, WS(6) + @"</StackPanel>" + nl);
        }

        private static void Write(FileStream fs, string text)
        {
            var line = new UTF8Encoding(true).GetBytes(text);
            fs.Write(line, 0, line.Length);
        }

        private static string WS(int numberOfSpaces)
        {
            return new string(' ', numberOfSpaces);
        }
    }
}
#region left overs
//foreach (var entityType in modelEntityTypes)
//{
//    var properties = entityType.GetProperties();
//    foreach (var propertyInfo in properties)
//    {
//        foreach (var customAttributeData in propertyInfo.CustomAttributes)
//        {
//            if (customAttributeData.AttributeType == typeof(DataTemplateAttribute)) annotatedProperties.Add(propertyInfo);

//        }
//    }
//}
#endregion