﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Transpiler
{
    public class Transpiler
    {
        private readonly IFileWriter fileWriter;
        private readonly Configuration config;

        public Transpiler(Configuration config)
        {
            this.config = config;
            this.fileWriter = new DefaultFileWriter();
        }

        public Transpiler(Configuration config, IFileWriter fileWriter)
        {
            this.config = config;
            this.fileWriter = fileWriter;
        }

        public void Run(IEnumerable<Type> types)
        {
            this.fileWriter.CreateDirectory(this.config.TargetDirectory);

            var tsTypes = this.CreateTsTypes(types);

            foreach (var tsType in tsTypes)
            {
                var imports = new List<string>();
                var body = new List<string>();

                if (this.config.PrintGeneratedFileText)
                {
                    imports.Add("// This file was generated by the CS2TS-Transpiler");
                    imports.Add(string.Empty);
                }

                // Sturcture Line
                var genericAddon = tsType.GenericArguments.Any()
                    ? $"<{string.Join(", ", tsType.GenericArguments)}>"
                    : string.Empty;

                var extentionAddon = string.Empty;


                // if (tsType.HasBaseClass)
                // {
                // var knownBaseClass = tsTypes.SingleOrDefault(t => t.Id == tsType.Type.BaseType.FullName);
                // if (knownBaseClass != null)
                // {
                //     var relPath = this.CreateRelativeDirectoryPath(tsType.Directory, knownBaseClass.Directory);
                //     imports.Add($"import {{ {knownBaseClass.Name} }} from \"{relPath + knownBaseClass.Name}\";");

                //     extentionAddon = " extends " + knownBaseClass.Name;
                // }
                // }

                var interfaces = tsType.Type.GetInterfaces().ToList();
                if (tsType.HasBaseClass) interfaces.Insert(0, tsType.Type.BaseType);

                var tsInterfaces = interfaces
                    .Select(inf => tsTypes.SingleOrDefault(t => t.Id == inf.FullName
                        || (inf.IsGenericType && t.Id.Split('`')[0] == inf.FullName.Split('`')[0])))
                    .Where(x => x != null);
                if (tsInterfaces.Any())
                {
                    foreach (var tsInf in tsInterfaces)
                    {
                        var relPath = this.CreateRelativeDirectoryPath(tsType.Directory, tsInf.Directory);
                        imports.Add($"import {{ {tsInf.Name} }} from \"{relPath + tsInf.Name}\";");

                        var infName = tsInf.Name;
                        // generic interface like Bla<int>
                        if (tsInf.GenericArguments.Any())
                        {
                            var genericArgumentsTypeNames = new List<string>();
                            var inf = interfaces.Single(i => tsInf.Id.Split('`')[0] == i.FullName.Split('`')[0]);
                            foreach (var gta in inf.GenericTypeArguments)
                            {
                                var otherTsType = tsTypes.SingleOrDefault(t => t.Id == gta.FullName);
                                if (otherTsType == null)
                                {
                                    // maybe a primitive?
                                    var tsTypeName = this.TsTypeName(gta);

                                    genericArgumentsTypeNames.Add(tsTypeName);
                                    continue;
                                }

                                genericArgumentsTypeNames.Add(otherTsType.Name);
                            }

                            infName += $"<{string.Join(", ", genericArgumentsTypeNames)}>";
                        }

                        extentionAddon += string.IsNullOrEmpty(extentionAddon)
                            ? " extends " + infName
                            : ", " + infName;
                    }
                }

                body.Add($"export interface {tsType.Name}{genericAddon}{extentionAddon} {{");

                // Properties
                var properties = tsType.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var prop in properties)
                {
                    this.CreatePropertyLines(prop, tsType, tsTypes, tsInterfaces, imports, body);
                }

                body.Add("}");

                // Write File
                var filePath = Path.Combine(tsType.Directory, $"{tsType.Name}.ts");
                this.fileWriter.CreateFile(filePath, this.CombineLines(imports, body));
            }
        }

        IEnumerable<TsType> CreateTsTypes(IEnumerable<Type> types)
        {
            foreach (var t in types)
            {
                var subFolders = this.config.UseNamespacesAsFolders
                    ? this.config.MapNamespace(t.Namespace).Replace(".", "\\")
                    : string.Empty;

                yield return new TsType
                {
                    Id = t.FullName,
                    Name = this.GetTypeName(t),
                    Directory = Path.Combine(this.config.TargetDirectory, subFolders.Trim('\\')),
                    GenericArguments = t.GetGenericArguments().Select(x => x.Name).ToList(),
                    HasBaseClass = t.BaseType != null && t.BaseType != typeof(Object),
                    Type = t
                };
            }
        }

        void CreatePropertyLines(PropertyInfo property, TsType tsType, IEnumerable<TsType> tsTypes, IEnumerable<TsType> tsInterfaces,
            List<string> imports, List<string> body)
        {
            var isInterfaceProperty = tsInterfaces.SelectMany(tsi => tsi.Type.GetMember(property.Name)).Any();
            if (isInterfaceProperty) return;

            var propertyType = property.PropertyType;

            var isEnumerable = propertyType != typeof(string)
                && typeof(IEnumerable).IsAssignableFrom(propertyType);

            if (isEnumerable)
                propertyType = propertyType.GetGenericArguments().First();

            if (propertyType.IsGenericParameter)
            {
                body.Add($"    {property.Name}: {propertyType.Name}{(isEnumerable ? "[]" : string.Empty)};");

                return;
            }

            // possible TsType
            var otherTsType = tsTypes.SingleOrDefault(t => t.Id == propertyType.FullName);

            if (otherTsType == null)
            {
                body.Add($"    {property.Name}: {this.TsTypeName(propertyType)}{(isEnumerable ? "[]" : string.Empty)};");
            }
            else
            {
                var relPath = this.CreateRelativeDirectoryPath(tsType.Directory, otherTsType.Directory);
                imports.Add($"import {{ {otherTsType.Name} }} from \"{relPath + otherTsType.Name}\";");
                body.Add($"    {property.Name}: {otherTsType.Name}{(isEnumerable ? "[]" : string.Empty)};");
            }
        }

        List<string> CombineLines(List<string> imports, List<string> body)
        {
            var fileLines = new List<string>();
            fileLines.AddRange(imports);
            if (imports.Any() && !string.IsNullOrWhiteSpace(imports.Last())) fileLines.Add(string.Empty);
            fileLines.AddRange(body);

            return fileLines;
        }

        string CreateRelativeDirectoryPath(string fromDirectoryPath, string toDirectoryPath)
        {
            if (!fromDirectoryPath.EndsWith("\\")) fromDirectoryPath += "\\";
            var fromUri = new Uri(fromDirectoryPath);
            if (!toDirectoryPath.EndsWith("\\")) toDirectoryPath += "\\";
            var toUri = new Uri(toDirectoryPath);

            var relativePath = fromUri.MakeRelativeUri(toUri).ToString();

            return relativePath.StartsWith("..") ? relativePath : "./" + relativePath;
        }

        string GetTypeName(Type type)
        {
            if (!type.IsGenericType) return type.Name;

            return type.Name.Split('`')[0];
        }

        string TsTypeName(Type propType)
        {
            var tsTypeName = "any";

            var isNullable = propType.IsValueType && Nullable.GetUnderlyingType(propType) != null;
            if (isNullable)
                propType = Nullable.GetUnderlyingType(propType);

            if (propType.Name == typeof(Boolean).Name) tsTypeName = "boolean";
            if (propType.Name == typeof(Byte).Name) tsTypeName = "number";
            if (propType.Name == typeof(SByte).Name) tsTypeName = "number";
            if (propType.Name == typeof(Decimal).Name) tsTypeName = "number";
            if (propType.Name == typeof(Double).Name) tsTypeName = "number";
            if (propType.Name == typeof(Single).Name) tsTypeName = "number";
            if (propType.Name == typeof(Int32).Name) tsTypeName = "number";
            if (propType.Name == typeof(UInt32).Name) tsTypeName = "number";
            if (propType.Name == typeof(Int64).Name) tsTypeName = "number";
            if (propType.Name == typeof(UInt64).Name) tsTypeName = "number";
            if (propType.Name == typeof(Int16).Name) tsTypeName = "number";
            if (propType.Name == typeof(UInt16).Name) tsTypeName = "number";
            if (propType.Name == typeof(Char).Name) tsTypeName = "string";
            if (propType.Name == typeof(String).Name) tsTypeName = "string";
            if (propType.Name == typeof(Guid).Name) tsTypeName = "string";
            if (propType.Name == typeof(DateTime).Name) tsTypeName = "Date";
            if (propType.Name == typeof(DateTimeOffset).Name) tsTypeName = "Date";

            if (isNullable)
                tsTypeName = tsTypeName + " | null";

            return tsTypeName;
        }
    }
}
