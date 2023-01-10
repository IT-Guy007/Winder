using System.Collections.ObjectModel;

namespace DataModel;

public class ChatCollection: ObservableCollection<ChatMessage >{
    
    public ChatCollection() : base() {
        
    }
}