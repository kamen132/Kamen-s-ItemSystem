/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:48
* @Modify:
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KamenGameFramewrok.Editor
{
    public class ViewCodeGenerator
    {
        public class StringGenerator
        {
            public static readonly string LineEnding = "\n";
            private StringBuilder mBuilder;

            public StringGenerator(int capacity)
            {
                mBuilder = new StringBuilder(capacity);
            }

            public StringGenerator()
            {
                mBuilder = new StringBuilder();
            }

            public void Append(string content)
            {
                mBuilder.Append(content);
            }

            public void AppendLine(string line)
            {
                mBuilder.Append(line);
                mBuilder.Append(LineEnding);
            }

            public void AppendLine()
            {
                mBuilder.Append(LineEnding);
            }

            public override string ToString()
            {
                return mBuilder.ToString();
            }
        }

        private ViewCodeTemplate mTemplate;
        private ViewCodeAssetLoader mLoader;

        private class ViewCodeGeneratorConfig
        {
            public string TemplatePath { get; private set; }
            public string TargetRootDir { get; private set; }

            public ViewCodeGeneratorConfig(string templatePath, string targetRootDir)
            {
                TemplatePath = templatePath;
                TargetRootDir = targetRootDir;
            }
        }

        private static Dictionary<string, ViewCodeGeneratorConfig> STagBasedGeneratorConfig =
            new Dictionary<string, ViewCodeGeneratorConfig>()
            {
                {
                    "uiitem",
                    new ViewCodeGeneratorConfig("Assets/Scripts/Kamen/Editor/ViewCodeGenerator/Template/ItemTemplate.txt",
                        "Assets/Scripts/Game/UI/GenViewCode/Items/")
                },
                {
                    "uiwindow",
                    new ViewCodeGeneratorConfig("Assets/Scripts/Kamen/Editor/ViewCodeGenerator/Template/ViewTemplate.txt",
                        "Assets/Scripts/Game/UI/GenViewCode/Views/")
                },
                {
                    "uiview",
                    new ViewCodeGeneratorConfig("Assets/Scripts/Kamen/Editor/ViewCodeGenerator/Template/ViewTemplate.txt",
                        "Assets/Scripts/Game/UI/GenViewCode/Views/")
                },
                {
                    "uiactionbarbaseview",
                    new ViewCodeGeneratorConfig(
                        "Assets/Scripts/Kamen/Editor/ViewCodeGenerator/Template/ActionBarViewTemplate.txt",
                        "Assets/Scripts/Game/UI/GenViewCode/ActionBarViews/")
                },
                {
                    "uicustomcontrol",
                    new ViewCodeGeneratorConfig(
                        "Assets/Scripts/Kamen/Editor/ViewCodeGenerator/Template/UICustomControlTemplate.txt",
                        "Assets/Scripts/Game/UI/GenViewCode/CustomControls/")
                },
            };

        private static ViewCodeGeneratorConfig GetCodeGeneratorConfigByTagName(string tagName)
        {
            tagName = tagName.Trim().ToLower();
            ViewCodeGeneratorConfig ret = null;
            if (STagBasedGeneratorConfig.TryGetValue(tagName, out ret))
            {

            }

            return ret;
        }


        /// <summary>
        /// 文件是否需要重新生成
        /// </summary>
        private static bool IsFileNeedRegenerate(string sourceFilePath, string targetFilePath)
        {
            if (!File.Exists(sourceFilePath))
            {
                //源文件不存在，无需生成
                return false;
            }

            if (!File.Exists(targetFilePath))
            {
                //目标文件不存在，需要生成新文件
                return true;
            }

            //源文件和目标文件均存在，进行时间戳比较
            DateTime sourceDt = File.GetLastWriteTime(sourceFilePath);
            DateTime targetDt = File.GetLastWriteTime(targetFilePath);

            if (sourceDt > targetDt)
            {
                //源文件时间比目标文件时间晚，需要重新生成文件
                return true;
            }

            return false;
        }

        public static void SmartGenerate(string rootDir, string assetBundleName, GameObject target, string prefabPath)
        {
            // 资源加载器
            ViewCodeAssetLoader loader = new ViewCodeAssetLoader();
            loader.Load(assetBundleName, target, prefabPath);

            string generatorConfigTag = string.Empty;
            // 区分是item还是view
            if (loader.IsLoaded)
            {
                generatorConfigTag = loader.Target.tag;
            }

            ViewCodeGeneratorConfig generatorConfig = GetCodeGeneratorConfigByTagName(generatorConfigTag);

            if (null == generatorConfig)
            {
                // 碰到不需要生成的脚本时，将加载的go也回收掉
                loader.Unload();
                return;
            }

            ViewCodeTemplate codeTemplate = new ViewCodeTemplate();
            codeTemplate.Load(generatorConfig.TemplatePath);

            // 生成脚本
            ViewCodeGenerator viewCodeGenerator = new ViewCodeGenerator();
            string targetRootDir = Path.Combine(rootDir, generatorConfig.TargetRootDir);
            viewCodeGenerator.Generate(codeTemplate, loader, targetRootDir, assetBundleName, false);
            loader?.Dispose();
        }

        public static void Generate(string rootDir, string assetBundleName, GameObject target, string assetPath)
        {
            SmartGenerate(rootDir, assetBundleName, target, assetPath);
        }

        public static string GetGeneratedName(string targetDir, string className)
        {

            string targetPath = Path.Combine(targetDir, $"{className}.gen.cs");
            targetPath = targetPath.Replace("/", @"\");
            return targetPath;
        }

        private void Generate(ViewCodeTemplate template, ViewCodeAssetLoader assetLoader, string targetDir, string assetPath, bool gameObjectWithExportTagOnly = true)
        {

            if (template == null)
            {
                Debug.Log($"<color=red>模板=null</color>");
                return;
            }

            if (!template.IsLoaded)
            {
                Debug.Log($"<color=red>模板没加载</color>");
                return;
            }

            if (assetLoader == null)
            {
                Debug.Log($"<color=red>资源加载器=null</color>");
                return;
            }

            if (!assetLoader.IsLoaded)
            {
                Debug.Log($"<color=red>资源加载器没加载</color>");
                return;
            }

            mTemplate = template;
            mLoader = assetLoader;

            mLoader.Export(gameObjectWithExportTagOnly);

            {
                mTemplate.ReplaceBegin();

                GenerateFileHeader();
                // 生成方法
                GenerateClass();
                GenerateVariables();
                GenerateVariableAssigners();
                GenerateCustomControl();

                GenerateControls();

                string targetPath = GetGeneratedName(targetDir, mLoader.AssetName);
                mTemplate.Export(targetPath);
            }

            mLoader.Unload();
        }

        private void GenerateCustomControl()
        {
            StringGenerator controlVariables = new StringGenerator();
            StringGenerator controlVariableAssigners = new StringGenerator();

            controlVariables.AppendLine();
            controlVariableAssigners.AppendLine();

            this.mTemplate.Append(ViewCodeTemplate.PlaceHolder.__CUSTOM_CONTROL_DEFINES__, controlVariables.ToString());
            this.mTemplate.Append(ViewCodeTemplate.PlaceHolder.__CUSTOM_CONTROL_ASSIGNERS__, controlVariableAssigners.ToString());
        }

        private void GenerateControls()
        {
        }

        private void AppendAllControlVariable(StringGenerator variables, KeyValuePair<string, List<GameObject>> iter, int subPairIndex)
        {
            if (ExportViewAsVariable("UIItem", variables, iter, subPairIndex, string.Empty))
            {
                return;
            }

            if (ExportViewAsVariable("UIView", variables, iter, subPairIndex, string.Empty))
            {
                return;
            }

            ExportComponentAsVariable("Button", variables, iter, subPairIndex, "Btn");
            ExportComponentAsVariable("Image", variables, iter, subPairIndex, "Img");
            ExportComponentAsVariable("ScrollRect", variables, iter, subPairIndex, "Scroll");
            ExportComponentAsVariable("Slider", variables, iter, subPairIndex, "Slider");
            ExportComponentAsVariable("Text", variables, iter, subPairIndex, "Txt");
            ExportComponentAsVariable("TextMeshProUGUI", variables, iter, subPairIndex, "TxtPro");
        }

        private void AppendVariableAssigners(StringGenerator variables
            , KeyValuePair<string, List<GameObject>> iter
            , int subPairIndex)
        {
            if (ExportViewAsVariableAssigner("UIItem", variables, iter, subPairIndex, string.Empty))
            {
                return;
            }

            if (ExportViewAsVariableAssigner("UIView", variables, iter, subPairIndex, string.Empty))
            {
                return;
            }

            ExportComponentAsVariableAssigner("Button", variables, iter, subPairIndex, "Btn");
            ExportComponentAsVariableAssigner("Image", variables, iter, subPairIndex, "Img");
            ExportComponentAsVariableAssigner("ScrollRect", variables, iter, subPairIndex, "Scroll");
            ExportComponentAsVariableAssigner("Slider", variables, iter, subPairIndex, "Slider");
            ExportComponentAsVariableAssigner("TextMeshProUGUI", variables, iter, subPairIndex, "TxtPro");
            ExportComponentAsVariableAssigner("Text", variables, iter, subPairIndex, "Txt");
        }

        private string mNamespaceInclude = "using KamenGameFramewrok;\n" +
                                           "using UnityEngine;\n" +
                                           "using UnityEngine.UI;\n"+
                                           "using TMPro;\n";

        private void GenerateFileHeader()
        {
            this.mTemplate.Replace(ViewCodeTemplate.PlaceHolder.__FILE_HEADER__, $"// 自动生成代码，请勿手动修改");
        }


        private void GenerateClass()
        {

            {
                this.mTemplate.Replace(ViewCodeTemplate.PlaceHolder.__NAMESPACE_INCLUDE__, mNamespaceInclude);
            }

            {
                string strNamespace = "Game.UI.Code";
                this.mTemplate.Replace(ViewCodeTemplate.PlaceHolder.__NAMESPACE__, strNamespace);
            }

            {
                this.mTemplate.Replace(ViewCodeTemplate.PlaceHolder.__CLASS_NAME__, this.mLoader.AssetName);
            }

            {
                this.mTemplate.Replace(ViewCodeTemplate.PlaceHolder.__ASSET_PATH__, this.mLoader.PrefabAssetPath);
            }

            {
                this.mTemplate.Replace(ViewCodeTemplate.PlaceHolder.__PREFAB_PATH__, this.mLoader.PrefabAssetPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GenerateVariables()
        {
            StringGenerator variables = new StringGenerator();

            foreach (var iter in this.mLoader.NamedGameObjects)
            {
                string goName = iter.Key;
                int goCount = iter.Value.Count;
                GameObject curGo = iter.Value[0];

                variables.AppendLine("\t\t/// <summary>");
                foreach (Component component in curGo.GetComponents<Component>())
                {
                    if (null == component
                        || component is Transform
                        || component is CanvasRenderer)
                    {
                        continue;
                    }

                    variables.AppendLine($"\t\t/// {component.GetType().Name}");
                }

                variables.AppendLine("\t\t/// </summary>");

                if (goCount == 1)
                {
                    variables.AppendLine($"\t\tprotected GameObject {GetGoVariableName(iter, -1)};");
                    AppendAllControlVariable(variables, iter, -1);
                }
                else
                {
                    //这里需要判断path是否相同，如果path相同，需要另外一种find方式
                    Dictionary<string, List<GameObject>> gameObjectsPathDict = new Dictionary<string, List<GameObject>>();
                    for (int i = 0; i < iter.Value.Count; ++i)
                    {
                        GameObject iterValue = iter.Value[i];
                        string path = mLoader.GetGameObjectPath(iterValue);
                        List<GameObject> brothers = null;
                        if (!gameObjectsPathDict.TryGetValue(path, out brothers))
                        {
                            brothers = new List<GameObject>();
                            gameObjectsPathDict.Add(path, brothers);
                        }

                        brothers.Add(iterValue);
                    }

                    if (gameObjectsPathDict.Count == iter.Value.Count)
                    {
                        variables.AppendLine($"\t\tprotected GameObject[] {GetGoVariableName(iter)} = new GameObject[{goCount}];");
                        AppendAllControlVariable(variables, iter, -1);
                    }
                    else
                    {
                        int sameNameIndex = 0;
                        foreach (var pair in gameObjectsPathDict)
                        {
                            List<GameObject> brothers = pair.Value;
                            variables.AppendLine($"\t\tprotected GameObject[] {GetGoVariableName(iter, sameNameIndex)} = new GameObject[{brothers.Count}];");
                            AppendAllControlVariable(variables, iter, sameNameIndex);
                            sameNameIndex++;
                        }
                    }
                }

                variables.AppendLine();
            }

            this.mTemplate.Replace(ViewCodeTemplate.PlaceHolder.__CONTROL_DEFINES__, variables.ToString());
        }


        private string GetGoVariableName(KeyValuePair<string, List<GameObject>> pair, int subIndex = -1)
        {
            if (subIndex == -1)
            {
                //没有subIndex
                if (pair.Value.Count == 1)
                {
                    return pair.Key;
                }
                else
                {
                    return $"_arr{pair.Key}";
                }
            }
            else
            {
                return $"_arr{pair.Key}{(subIndex == 0 ? "" : subIndex.ToString())}";
            }
        }

        private string GetControlVariableName(KeyValuePair<string, List<GameObject>> pair, string prefix, int subIndex = -1)
        {
            string originalName = pair.Value[0].name;
            originalName = originalName.Replace("_", "");
            originalName = originalName.Replace(" ", "");
            if (originalName.StartsWith("m"))
            {
                originalName = originalName.Substring(1);
            }

            originalName = originalName[0].ToString().ToUpper() + originalName.Substring(1);
            if (subIndex == -1)
            {
                if (pair.Value.Count == 1)
                {
                    return $"m{originalName}{prefix}";
                }
                else
                {
                    return $"mArr{originalName}{prefix}";
                }
            }
            else
            {
                return $"mArr{originalName}{prefix}{(subIndex == 0 ? "" : subIndex.ToString())}";
            }
        }

        private bool ExportViewAsVariable(
            string targetTagName, StringGenerator variables, KeyValuePair<string, List<GameObject>> pair, int subPairIndex, string prefix)
        {
            //只需要导出一个component即可满足需求
            GameObject gameObject = pair.Value[0];

            if (gameObject.CompareTag(targetTagName)
                && UnityEditor.PrefabUtility.GetPrefabAssetType(gameObject) == UnityEditor.PrefabAssetType.Regular)
            {
                // 找到源头的类型
                var hehe = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
                if (hehe == null)
                {
                    var target = mLoader.Target;
                    Debug.LogException(new Exception($"{target.name}预设引用异常了，快看看{gameObject.name}里的item是否有断链的情况."), target);
                }

                string targetTypeName = hehe.name;

                if (pair.Value.Count == 1)
                {
                    variables.AppendLine($"\t\tprotected {targetTypeName} {GetControlVariableName(pair, prefix, subPairIndex)};");
                }
                else
                {
                    variables.AppendLine($"\t\tprotected {targetTypeName}[] {GetControlVariableName(pair, prefix, subPairIndex)} = new {targetTypeName}[{pair.Value.Count}];");
                }

                return true;
            }

            return false;
        }

        private void ExportComponentAsVariable(string targetTypeName, StringGenerator variables, KeyValuePair<string, List<GameObject>> pair, int subPairIndex, string prefix)
        {
            //只需要导出一个component即可满足需求
            GameObject gameObject = pair.Value[0];

            Component component = gameObject.GetComponent(targetTypeName);

            if (component != null && component.GetType().Name == targetTypeName)
            {
                if (pair.Value.Count == 1)
                {
                    variables.AppendLine($"\t\tprotected {targetTypeName} {GetControlVariableName(pair, prefix, subPairIndex)};");
                }
                else
                {
                    variables.AppendLine($"\t\tprotected {targetTypeName}[] {GetControlVariableName(pair, prefix, subPairIndex)} = new {targetTypeName}[{pair.Value.Count}];");
                }
            }
        }

        private void ExportElementAsVariable(string targetTypeName, StringGenerator variables, KeyValuePair<string, List<GameObject>> pair, int subPairIndex, string prefix)
        {
            //只需要导出一个component即可满足需求
            GameObject gameObject = pair.Value[0];
            Component component = gameObject.GetComponent(targetTypeName);
            if (component != null)
            {
                if (pair.Value.Count == 1)
                {
                    variables.AppendLine($"\t\tprotected {targetTypeName} {GetControlVariableName(pair, prefix, subPairIndex)};");
                }
                else
                {
                    variables.AppendLine($"\t\tprotected {targetTypeName}[] {GetControlVariableName(pair, prefix, subPairIndex)} = new {targetTypeName}[{pair.Value.Count}];");
                }
            }
        }

        private void GenerateVariableAssigners()
        {
            StringGenerator variables = new StringGenerator();

            foreach (var iter in this.mLoader.NamedGameObjects)
            {
                string goName = iter.Key;
                int goCount = iter.Value.Count;

                if (goCount == 1)
                {
                    string goPath = this.mLoader.GetGameObjectPath(iter.Value[0]);
                    variables.AppendLine($"\t\t\t{goName} = this.Root.transform.Find(\"{goPath}\")?.gameObject;");
                    //顺便把一些自定义控件也导出一下
                    AppendVariableAssigners(variables, iter, -1);
                }
                else
                {
                    //这里需要判断path是否相同，如果path相同，需要另外一种find方式
                    //bool gameObjectsAreBrothers = false;
                    Dictionary<string, List<GameObject>> gameObjectsPathDict = new Dictionary<string, List<GameObject>>();
                    for (int i = 0; i < iter.Value.Count; ++i)
                    {
                        GameObject curGo = iter.Value[i];
                        string path = mLoader.GetGameObjectPath(curGo);
                        List<GameObject> brothers = null;
                        if (!gameObjectsPathDict.TryGetValue(path, out brothers))
                        {
                            brothers = new List<GameObject>();
                            gameObjectsPathDict.Add(path, brothers);
                        }

                        brothers.Add(curGo);
                    }

                    if (gameObjectsPathDict.Count == iter.Value.Count)
                    {
                        //所有同名的gameobject都不是兄弟，可以直接find
                        for (int i = 0; i < iter.Value.Count; ++i)
                        {
                            GameObject curGo = iter.Value[i];
                            string goPath = this.mLoader.GetGameObjectPath(curGo);
                            variables.AppendLine($"\t\t\t_arr{goName}[{i}] = this.Root.transform.Find(\"{goPath}\")?.gameObject;");
                            //顺便把一些自定义控件也导出一下
                        }

                        AppendVariableAssigners(variables, iter, -1);
                    }
                    else
                    {
                        int sameNameIndex = 0;
                        foreach (var pair in gameObjectsPathDict)
                        {
                            List<GameObject> brothers = pair.Value;
                            variables.AppendLine($"\t\t\t{{");
                            string parentPath = mLoader.GetGameObjectPath(brothers[0].transform.parent.gameObject);
                            string parentName = brothers[0].transform.parent.name;
                            variables.AppendLine($"\t\t\t\tTransform {parentName} = this.Root.transform.Find(\"{parentPath}\");");
                            variables.AppendLine($"\t\t\t\tint currentIndex = 0;");
                            variables.AppendLine($"\t\t\t\tfor (int i = 0; i < {parentName}.childCount; i++)");
                            variables.AppendLine($"\t\t\t\t{{");
                            variables.AppendLine($"\t\t\t\t\tTransform child = {parentName}.GetChild(i);");
                            variables.AppendLine($"\t\t\t\t\tif (child.name == \"{brothers[0].name}\")");
                            variables.AppendLine($"\t\t\t\t\t\t{{");
                            variables.AppendLine($"\t\t\t\t\t\t\t{GetGoVariableName(iter, sameNameIndex)}[currentIndex++] = child.gameObject;");
                            variables.AppendLine($"\t\t\t\t\t\t}}");
                            variables.AppendLine($"\t\t\t\t\t}}");
                            variables.AppendLine($"\t\t\t}}");

                            AppendVariableAssigners(variables, iter, sameNameIndex);

                            sameNameIndex++;
                        }
                    }
                }

                variables.AppendLine();
            }

            this.mTemplate.Replace(ViewCodeTemplate.PlaceHolder.__CONTROL_ASSIGNERS__, variables.ToString());
        }



        private void ExportComponentAsVariableAssigner(string targetTypeName, StringGenerator variables, KeyValuePair<string, List<GameObject>> pair, int subPairIndex, string prefix)
        {
            //只需要导出一个component即可满足需求
            GameObject gameObject = pair.Value[0];

            Component component = gameObject.GetComponent(targetTypeName);

            if (component != null && component.GetType().Name == targetTypeName)
            {
                //第一个字母大写,culture类好像不大好用
                if (pair.Value.Count == 1)
                {
                    variables.AppendLine($"\t\t\t{GetControlVariableName(pair, prefix, subPairIndex)} = {GetGoVariableName(pair, subPairIndex)}.GetComponent<{targetTypeName}>();");
                }
                else
                {
                    variables.AppendLine($"\t\t\tfor (int i = 0; i < {GetGoVariableName(pair, subPairIndex)}.Length; i++)");
                    variables.AppendLine($"\t\t\t{{");
                    variables.AppendLine($"\t\t\t\t{GetControlVariableName(pair, prefix, subPairIndex)}[i] = {GetGoVariableName(pair, subPairIndex)}[i].GetComponent<{targetTypeName}>();");
                    variables.AppendLine($"\t\t\t}}");
                }
            }
        }

        /// <summary>
        /// 导出UI元素
        /// </summary>
        private void ExportElememtAsVariableAssigner(string targetTypeName, StringGenerator variables, KeyValuePair<string, List<GameObject>> pair, int subPairIndex, string prefix)
        {
            //只需要导出一个component即可满足需求
            GameObject gameObject = pair.Value[0];
            Component component = gameObject.GetComponent(targetTypeName);
            if (component != null)
            {
                //第一个字母大写,culture类好像不大好用
                if (pair.Value.Count == 1)
                {
                    string varName = GetControlVariableName(pair, prefix, subPairIndex);
                    variables.AppendLine($"\t\t\t{varName} = new {targetTypeName}({GetGoVariableName(pair, subPairIndex)});");
                    variables.AppendLine($"\t\t\tAddSubitem({varName}, Root);");
                }
                else
                {
                    variables.AppendLine($"\t\t\tfor (int i = 0; i < {GetGoVariableName(pair, subPairIndex)}.Length; i++)");
                    variables.AppendLine($"\t\t\t{{");

                    string varName = GetControlVariableName(pair, prefix, subPairIndex);
                    variables.AppendLine($"\t\t\t{varName}[i] = new {targetTypeName}({GetGoVariableName(pair, subPairIndex)}));");
                    variables.AppendLine($"\t\t\tAddSubitem({varName}, Root);");

                    variables.AppendLine($"\t\t\t}}");
                }
            }
        }


        private bool ExportViewAsVariableAssigner(string targetTagName, StringGenerator variables, KeyValuePair<string, List<GameObject>> pair, int subPairIndex, string prefix)
        {
            //只需要导出一个component即可满足需求
            GameObject gameObject = pair.Value[0];

            if (gameObject.CompareTag(targetTagName)
                && UnityEditor.PrefabUtility.GetPrefabAssetType(gameObject) == UnityEditor.PrefabAssetType.Regular)
            {
                // 找到源头的类型
                string targetTypeName = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(gameObject).name;
                //第一个字母大写,culture类好像不大好用
                if (pair.Value.Count == 1)
                {
                    string varName = GetControlVariableName(pair, prefix, subPairIndex);
                    variables.AppendLine($"\t\t\t{varName} = new {targetTypeName}({GetGoVariableName(pair, subPairIndex)});");
                    variables.AppendLine($"\t\t\tAddSubitem({varName}, Root);");
                }
                else
                {
                    variables.AppendLine($"\t\t\tfor (int i = 0; i < {GetGoVariableName(pair, subPairIndex)}.Length; i++)");
                    variables.AppendLine($"\t\t\t{{");

                    string varName = GetControlVariableName(pair, prefix, subPairIndex);
                    variables.AppendLine($"\t\t\t{varName}[i] = new {targetTypeName}({GetGoVariableName(pair, subPairIndex)}[i]);");
                    variables.AppendLine($"\t\t\tAddSubitem({varName}[i], Root);");

                    variables.AppendLine($"\t\t\t}}");
                }

                return true;
            }

            return false;
        }
    }
}