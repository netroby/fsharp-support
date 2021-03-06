﻿namespace JetBrains.ReSharper.Plugins.FSharp.Psi.Parsing
{
  public class ParserMessages
  {
    public const string IDS_F_SHARP_DECLARATION = "";
    public const string IDS_MODULE_LIKE_DECLARATION = "";
    public const string IDS_TOP_LEVEL_MODULE_OR_NAMESPACE_DECLARATION = "";
    public const string IDS_MODULE_MEMBER = "";
    public const string IDS_MODULE_MEMBER_DECLARATION = "";
    public const string IDS_MODULE_MEMBER_STATEMENT = "";
    public const string IDS_F_SHARP_TYPE_DECLARATION = "";
    public const string IDS_F_SHARP_TYPE_PARAMETERS_OWNER_DECLARATION = "";
    public const string IDS_UNION_DECLARATION = "";
    public const string IDS_F_SHARP_UNION_CASE_DECLARATION = "";
    public const string IDS_F_SHARP_TYPE_MEMBER_DECLARATION = "";
    public const string IDS_OBJECT_MODEL_TYPE_DECLARATION = "";
    public const string IDS_MODIFIER = "";
    public const string IDS_NOT_COMPILED_TYPE_DECLARATION = "";
    public const string IDS_SIMPLE_TYPE_DECLARATION = "";
    public const string IDS_TYPE_EXPRESSION = "";

    public static string GetString(string id)
    {
      return id;
    }

    public static string GetExpectedMessage(string expectedSymbol)
    {
      return string.Empty;
    }

    public static string GetExpectedMessage(string firstExpectedSymbol, string secondExpectedSymbol)
    {
      return string.Empty;
    }
  }
}