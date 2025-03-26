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
    public IEnumerable<BaseViewModel> Anchorables { get; private set; } = [];


    public event Action<IEnumerable<BaseViewModel>>? OnDocumentOpenned;
    public event Action<IEnumerable<BaseViewModel>>? OnDocumentClosed;
    public event Action<BaseViewModel>? OnActiveDocumentChanged;
    public event Action<IEnumerable<BaseViewModel>>? OnAnchorableAdded;
    public event Action<IEnumerable<BaseViewModel>>? OnAnchorableRemoved;

    public DocumentStore(IEnumerable<BaseViewModel> documents)
    {
        Documents = documents;
    }

    public void AddDocument<T>(BaseViewModel document)
    {
        ICollection<BaseViewModel> DocumentCollection = Documents.ToList(); 
        BaseViewModel? activeDocument = DocumentCollection.FirstOrDefault(d =>
        {
            return ((IDocument)d).Title == ((IDocument)document).Title;
        });

        if (activeDocument == null)
        {
            DocumentCollection.Add(document);
            Documents = DocumentCollection;
            OnDocumentOpenned?.Invoke(Documents);
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
        ActiveDocument = document;
        OnActiveDocumentChanged?.Invoke(document);
    }



    public void RemoveDocument(BaseViewModel document)
    {
        ICollection<BaseViewModel> DocumentCollection = Documents.ToList();
        DocumentCollection.Remove(document);
        Documents = DocumentCollection;
        OnDocumentClosed?.Invoke(Documents);
        ClearAnchorables();
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
