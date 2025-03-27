using CID_Tester.ViewModel;
using CID_Tester.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Store;

public class DocumentStore
{
    public IEnumerable<BaseViewModel> Documents { get; private set; }
    public object ActiveDocument { get; set; } = null!;
    public IDocument? PreviousDocument { get; set; }
    public IEnumerable<BaseViewModel> Anchorables { get; private set; } = [];


    public event Action<BaseViewModel>? OnDocumentOpenned;
    public event Action<BaseViewModel>? OnDocumentClosed;
    public event Action<BaseViewModel>? OnActiveDocumentChanged;
    public event Action<IEnumerable<BaseViewModel>>? OnAnchorableAdded;
    public event Action<IEnumerable<BaseViewModel>>? OnAnchorableRemoved;

    public DocumentStore(IEnumerable<BaseViewModel> documents)
    {
        Documents = documents;
    }

    public void AddDocument<T>(BaseViewModel document)
    {
        if (document is not IDocument)
        {
            throw new ArgumentException("Document must implement IDocument interface");
        }
        ICollection<BaseViewModel> DocumentCollection = Documents.ToList(); 
        BaseViewModel? activeDocument = DocumentCollection.FirstOrDefault(d =>
        {
            return ((IDocument)d).Title == ((IDocument)document).Title;
        });

        if (activeDocument == null)
        {
            DocumentCollection.Add(document);
            Documents = DocumentCollection;
            OnDocumentOpenned?.Invoke(document);
            setActiveDocument(document);
            ClearAnchorables();
        }
        if (ActiveDocument != activeDocument && activeDocument != null)
        {
            setActiveDocument(activeDocument);
            ClearAnchorables();
        }
    }

    public void setActiveDocument(BaseViewModel document)
    {
        PreviousDocument = (IDocument)ActiveDocument;
        ActiveDocument = document;
        OnActiveDocumentChanged?.Invoke(document);
    }



    public void RemoveDocument(BaseViewModel document)
    {
        ICollection<BaseViewModel> DocumentCollection = Documents.ToList();
        DocumentCollection.Remove(document);
        Documents = DocumentCollection;
        OnDocumentClosed?.Invoke(document);
        if (document == ActiveDocument) ClearAnchorables();
    }

    public void AddAnchorables(BaseViewModel anchorable)
    {
        ICollection<BaseViewModel> AnchorableCollection = Anchorables.ToList();
        BaseViewModel? ancorableExist = AnchorableCollection.FirstOrDefault(d => d == anchorable);

        if (ancorableExist == null)
        {
            AnchorableCollection.Add(anchorable);
            Anchorables = AnchorableCollection;
            OnAnchorableAdded?.Invoke(Anchorables);
        }
    }

    public void RemoveAnchorable(BaseViewModel anchorable)
    {
        ICollection<BaseViewModel> AnchorableCollection = Anchorables.ToList();
        AnchorableCollection.Remove(anchorable);
        Anchorables = AnchorableCollection;
        OnAnchorableRemoved?.Invoke(Anchorables);
    }

    public void ClearAnchorables()
    {
        Anchorables = [];
        OnAnchorableRemoved?.Invoke(Anchorables);
    }
}
