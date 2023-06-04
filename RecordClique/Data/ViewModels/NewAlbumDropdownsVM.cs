using RecordClique.Models;


namespace RecordClique.Data.ViewModels
{
    public class NewAlbumDropdownsVM
    {

    public NewAlbumDropdownsVM() {
            Labels = new List<Label>();          
            Artists = new List<Artist>();
        }
        public List<Label> Labels { get; set; }
        public List<Artist> Artists { get; set; }
    }
}
