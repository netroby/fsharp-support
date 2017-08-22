﻿using System;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Impl;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Plugins.FSharp.Psi.Parsing;
using JetBrains.ReSharper.Plugins.FSharp.Psi.Tree;
using JetBrains.ReSharper.Plugins.FSharp.Psi.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using Microsoft.FSharp.Compiler.SourceCodeServices;
using Microsoft.VisualStudio.FSharp.LanguageService;

namespace JetBrains.ReSharper.Plugins.FSharp.Services.Cs.CodeCompletion
{
  [IntellisensePart]
  public class FSharpCodeCompletionContextProvider : CodeCompletionContextProviderBase
  {
    public override bool IsApplicable(CodeCompletionContext context)
    {
      return context.File is IFSharpFile;
    }

    public override ISpecificCodeCompletionContext GetCompletionContext(CodeCompletionContext context)
    {
      var file = (IFSharpFile) context.File;
      var parseResults = file.ParseResults;

      var caretTreeOffset = context.CaretTreeOffset;
      var caretOffset = caretTreeOffset.Offset;

      var token = file.FindTokenAt(caretTreeOffset);
      var tokenBefore = file.FindTokenAt(caretTreeOffset - 1);
      var tokenBeforeType = tokenBefore?.GetTokenType();

      if (parseResults == null ||
          tokenBeforeType == FSharpTokenType.LINE_COMMENT ||
          tokenBeforeType == FSharpTokenType.DEAD_CODE || token?.GetTokenType() == FSharpTokenType.DEAD_CODE ||
          token == tokenBefore && tokenBeforeType != null &&
          (tokenBeforeType.IsComment || tokenBeforeType.IsStringLiteral ||
           tokenBeforeType.IsConstantLiteral) ||
          context.SelectedRange.TextRange.Length > 0 ||
          tokenBefore.GetTreeEndOffset() < caretTreeOffset || token.GetTreeEndOffset() < caretTreeOffset)
      {
        var selectedRange = context.SelectedRange;
        return new FSharpCodeCompletionContext(context, new TextLookupRanges(selectedRange, selectedRange),
          TreeOffset.InvalidOffset, DocumentCoords.Empty, null, null, null, null, null, null, false);
      }

      var document = context.Document;
      var completedRangeStartOffset =
        tokenBeforeType != null && (tokenBeforeType == FSharpTokenType.IDENTIFIER || tokenBeforeType.IsKeyword)
          ? tokenBefore.GetTreeStartOffset().Offset
          : caretOffset;
      var completedRange = new DocumentRange(document, new TextRange(completedRangeStartOffset, caretOffset));
      var defaultRanges = GetTextLookupRanges(context, completedRange);

      var ranges = ShouldReplace(token)
        ? defaultRanges.WithReplaceRange(new DocumentRange(document,new TextRange(caretOffset,
          Math.Max(token.GetTreeEndOffset().Offset, caretOffset))))
        : defaultRanges;

      var coords = document.GetCoordsByOffset(caretOffset);
      var lineText = document.GetLineText(coords.Line);
      var names = QuickParse.GetPartialLongNameEx(lineText, (int) coords.Column - 1);

      var fsCompletionContext = UntypedParseImpl.TryGetCompletionContext(coords.GetPos(), parseResults, lineText); // todo: create issue about implicit FSOpt

      return new FSharpCodeCompletionContext(context, ranges, caretTreeOffset, coords, names, tokenBefore, token,
        lineText, fsCompletionContext?.Value, parseResults);
    }

    private static bool ShouldReplace([CanBeNull] ITreeNode token)
    {
      var tokenType = token?.GetTokenType();
      return tokenType != null && (tokenType == FSharpTokenType.IDENTIFIER || tokenType.IsKeyword);
    }
  }
}