﻿using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Plugins.FSharp.ProjectModelBase;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.FSharp.Impl.Cache2.Declarations;
using JetBrains.ReSharper.Psi.FSharp.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.PersistentMap;

namespace JetBrains.ReSharper.Psi.FSharp.Impl.Cache2
{
  public class FSharpCacheProvider : ILanguageCacheProvider
  {
    private const int CacheVersion = 2;

    public void BuildCache(IFile file, ICacheBuilder builder)
    {
      if (file.GetProjectFileType().Equals(FSharpScriptProjectFileType.Instance))
        return;

      var declarationProcessor = new FSharpCacheDeclarationProcessor(builder, CacheVersion, GetFSharpFileKind(file));
      (file as IFSharpFile)?.Accept(declarationProcessor);
    }

    private static FSharpFileKind GetFSharpFileKind(IFile file)
    {
      if (file is IFSharpImplFile)
        return FSharpFileKind.ImplFile;
      if (file is IFSharpSigFile)
        return FSharpFileKind.SigFile;

      throw new ArgumentOutOfRangeException();
    }

    public ProjectFilePart LoadProjectFilePart(IPsiSourceFile sourceFile, ProjectFilePartsTree tree, IReader reader)
    {
      var part = new FSharpProjectFilePart(sourceFile, reader);
      if (part.CacheVersion != CacheVersion)
        tree.ForceDirty();

      return part;
    }

    public Part ReadPart(byte tag, IReader reader)
    {
      switch ((FSharpPartKind) tag)
      {
        case FSharpPartKind.DeclaredNamespace:
          return new DeclaredNamespacePart(reader);
        case FSharpPartKind.QualifiedNamespace:
          return new QualifiedNamespacePart(reader);
        case FSharpPartKind.TopLevelModule:
          return new TopLevelModulePart(reader);
        case FSharpPartKind.NestedModule:
          return new NestedModulePart(reader);
        case FSharpPartKind.Exception:
          return new ExceptionPart(reader);
        case FSharpPartKind.Enum:
          return new EnumPart(reader);
        case FSharpPartKind.Record:
          return new RecordPart(reader);
        case FSharpPartKind.Union:
          return new UnionPart(reader);
        case FSharpPartKind.UnionCase:
          return new UnionCasePart(reader);
        case FSharpPartKind.HiddenType:
          return new HiddenTypePart(reader);
        case FSharpPartKind.Class:
          return new ClassPart(reader);
        case FSharpPartKind.Interface:
          return new InterfacePart(reader);
        case FSharpPartKind.Struct:
          return new StructPart(reader);
        default:
          throw new SerializationError("Unknown tag:" + tag);
      }
    }

    public bool NeedCacheUpdate(ITreeNode elementContainingChanges, PsiChangedElementType type)
    {
      return true;
    }

    public bool IsCaseSensitive(IPsiModule module)
    {
      return true;
    }

    public IEnumerable<IPsiSourceFile> GetAffectedOnPsiModulePropertiesChange(IPsiModule module)
    {
      // todo: check this
      return EmptyList<IPsiSourceFile>.InstanceList;
    }

    public bool IsCachableTreeNode(ITreeNode treeNode)
    {
      return false;
    }

    public NodeType GetNodeType(int index)
    {
      return FSharpNodeTypeIndexer.Instance.GetNodeType(index);
    }

    public string PersistentTreeNodeCacheUniqueId => null;
    public string TokenCacheUniqueId => null;
    public string BufferCacheUniqueId => null;
    public string StubTreeNodeCacheUniqueId => null;

    public void SerializeMetadata(IFile file, UnsafeWriter bufferWriter)
    {
    }

    public void RestoreMetadata(IFile file, UnsafeReader bufferReader)
    {
    }

    public bool IsInternableToken(TokenNodeType tokenNodeType)
    {
      throw new NotImplementedException();
    }

    public void BuildCache(ISandBox sandBox, ICacheBuilder builder)
    {
      throw new NotImplementedException();
    }

    public bool IsCacheableInClosedForm(IChameleonNode node)
    {
      throw new NotImplementedException();
    }

    public TreeElement CreateChameleonNode(NodeType nodeType, TreeOffset startOffset, TreeOffset endOffset)
    {
      throw new NotImplementedException();
    }
  }
}